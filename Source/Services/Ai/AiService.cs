using LearnNN_WebBlazor.Data;
using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models.Ai;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LearnNN_WebBlazor.Services.Ai;

/// <summary>
/// Core orchestrator for AI operations providing intelligent caching, multi-channel dispatch,
/// graceful fallback from API to Manual Bridge, and manual request reconciliation.
/// <para>VN: Bộ điều phối cốt lõi cho các hoạt động AI, cung cấp cơ chế cache thông minh, điều phối đa kênh,
/// tự động hạ cấp từ API sang Manual Bridge khi lỗi quota, và tiếp nhận kết quả thủ công.</para>
/// </summary>
public class AiService : IAiService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly IOptions<AiOptions> _options;
    private readonly ILenientJsonParser _lenientParser;
    private readonly GeminiApiExecutor _apiExecutor;
    private readonly ManualBridgeExecutor _manualExecutor;
    private readonly ILogger<AiService> _logger;

    public AiService(
        IDbContextFactory<AppDbContext> dbFactory,
        IOptions<AiOptions> options,
        ILenientJsonParser lenientParser,
        GeminiApiExecutor apiExecutor,
        ManualBridgeExecutor manualExecutor,
        ILogger<AiService> logger)
    {
        _dbFactory = dbFactory;
        _options = options;
        _lenientParser = lenientParser;
        _apiExecutor = apiExecutor;
        _manualExecutor = manualExecutor;
        _logger = logger;
    }

    #region IAiService Implementation

    /// <inheritdoc/>
    public async Task<AiServiceResponse<T>> RequestAsync<T>(
        RenderedAiPrompt prompt,
        string? modelOverride = null,
        CancellationToken cancellationToken = default)
    {
        using var context = await _dbFactory.CreateDbContextAsync(cancellationToken);

        // 1. Transparent Cache Evaluation: Check if an identical prompt was already completed
        if (!string.IsNullOrWhiteSpace(prompt.InputHash))
        {
            var cached = await context.AiRequests
                .AsNoTracking()
                .Where(r => r.InputHash == prompt.InputHash && r.Status == AiRequestStatus.Completed && !string.IsNullOrEmpty(r.ResponseJson))
                .OrderByDescending(r => r.CompletedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (cached != null && !string.IsNullOrWhiteSpace(cached.ResponseJson))
            {
                _logger.LogInformation("AI Request Cache Hit for hash {Hash} (RequestId: {RequestId})", prompt.InputHash, cached.RequestId);
                var cachedValidation = _lenientParser.ParseAndValidate<T>(cached.ResponseJson, cached.RequestId);
                if (cachedValidation.IsValid && cachedValidation.Data != null)
                {
                    return AiServiceResponse<T>.Completed(cachedValidation.Data, cached.RequestId, isCached: true);
                }
            }
        }

        var currentMode = _options.Value.Mode;

        // 2. Dispatch based on current operating mode
        switch (currentMode)
        {
            case AiMode.Manual:
                return await HandleManualDispatchAsync<T>(context, prompt, cancellationToken);

            case AiMode.Auto:
                return await HandleAutoDispatchAsync<T>(context, prompt, modelOverride, cancellationToken);

            case AiMode.Hybrid:
            default:
                return await HandleHybridDispatchAsync<T>(context, prompt, modelOverride, cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task<AiValidationResult<T>> CompleteManualRequestAsync<T>(
        string requestId,
        string rawPastedText,
        CancellationToken cancellationToken = default)
    {
        using var context = await _dbFactory.CreateDbContextAsync(cancellationToken);
        var entity = await context.AiRequests.FirstOrDefaultAsync(r => r.RequestId == requestId, cancellationToken);

        if (entity == null)
        {
            return AiValidationResult<T>.Failure($"Không tìm thấy yêu cầu AI có mã \"{requestId}\".");
        }

        var validation = _lenientParser.ParseAndValidate<T>(rawPastedText, requestId);

        if (validation.IsValid)
        {
            entity.Status = AiRequestStatus.Completed;
            entity.ResponseJson = validation.NormalizedJson;
            entity.CompletedAt = DateTime.UtcNow;
            entity.ErrorMessage = null;
        }
        else
        {
            entity.Status = AiRequestStatus.Invalid;
            entity.ErrorMessage = validation.ErrorMessage;
        }

        await context.SaveChangesAsync(cancellationToken);
        return validation;
    }

    /// <inheritdoc/>
    public async Task<int> GetPendingRequestCountAsync()
    {
        using var context = await _dbFactory.CreateDbContextAsync();
        return await context.AiRequests.CountAsync(r => r.Status == AiRequestStatus.Pending);
    }

    /// <inheritdoc/>
    public async Task<List<AiRequest>> GetRecentRequestsAsync(int limit = 20, AiRequestStatus? statusFilter = null)
    {
        using var context = await _dbFactory.CreateDbContextAsync();
        var query = context.AiRequests.AsNoTracking();

        if (statusFilter.HasValue)
        {
            query = query.Where(r => r.Status == statusFilter.Value);
        }

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<AiRequest?> GetRequestByIdAsync(string requestId)
    {
        using var context = await _dbFactory.CreateDbContextAsync();
        return await context.AiRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RequestId == requestId);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteRequestAsync(int id)
    {
        using var context = await _dbFactory.CreateDbContextAsync();
        var entity = await context.AiRequests.FindAsync(id);
        if (entity == null) return false;

        context.AiRequests.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Dispatch Handlers

    private async Task<AiServiceResponse<T>> HandleManualDispatchAsync<T>(
        AppDbContext context,
        RenderedAiPrompt prompt,
        CancellationToken cancellationToken)
    {
        var record = new AiRequest
        {
            RequestId = prompt.RequestId,
            Kind = prompt.Kind,
            PromptVersion = prompt.PromptVersion,
            InputHash = prompt.InputHash,
            PromptText = prompt.FullPromptText,
            Channel = AiChannel.Manual,
            Status = AiRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        context.AiRequests.Add(record);
        await context.SaveChangesAsync(cancellationToken);

        return AiServiceResponse<T>.PendingManual(prompt.RequestId, prompt.FullPromptText);
    }

    private async Task<AiServiceResponse<T>> HandleAutoDispatchAsync<T>(
        AppDbContext context,
        RenderedAiPrompt prompt,
        string? modelOverride,
        CancellationToken cancellationToken)
    {
        var record = new AiRequest
        {
            RequestId = prompt.RequestId,
            Kind = prompt.Kind,
            PromptVersion = prompt.PromptVersion,
            InputHash = prompt.InputHash,
            PromptText = prompt.FullPromptText,
            Channel = AiChannel.Api,
            Status = AiRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        context.AiRequests.Add(record);
        await context.SaveChangesAsync(cancellationToken);

        var exec = await _apiExecutor.ExecuteAsync(prompt, modelOverride, cancellationToken);
        record.ModelLabel = exec.ModelUsed;

        if (exec.IsSuccess && !string.IsNullOrWhiteSpace(exec.RawResponseText))
        {
            var val = _lenientParser.ParseAndValidate<T>(exec.RawResponseText, prompt.RequestId);
            if (val.IsValid && val.Data != null)
            {
                record.Status = AiRequestStatus.Completed;
                record.ResponseJson = val.NormalizedJson;
                record.CompletedAt = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);

                return AiServiceResponse<T>.Completed(val.Data, prompt.RequestId);
            }

            record.Status = AiRequestStatus.Invalid;
            record.ErrorMessage = val.ErrorMessage;
            await context.SaveChangesAsync(cancellationToken);

            var resp = AiServiceResponse<T>.Failed(prompt.RequestId, val.ErrorMessage ?? "Dữ liệu JSON không đúng schema.");
            resp.FixPrompt = val.FixPrompt;
            return resp;
        }

        record.Status = AiRequestStatus.Failed;
        record.ErrorMessage = exec.ErrorMessage;
        await context.SaveChangesAsync(cancellationToken);

        return AiServiceResponse<T>.Failed(prompt.RequestId, exec.ErrorMessage ?? "Gọi Gemini API không thành công.");
    }

    private async Task<AiServiceResponse<T>> HandleHybridDispatchAsync<T>(
        AppDbContext context,
        RenderedAiPrompt prompt,
        string? modelOverride,
        CancellationToken cancellationToken)
    {
        // If API key is not configured, silently degrade to Manual Bridge
        if (!_options.Value.HasApiKey)
        {
            _logger.LogInformation("Hybrid mode: No API key configured. Degraded to Manual Bridge for RequestId {RequestId}", prompt.RequestId);
            var manualResp = await HandleManualDispatchAsync<T>(context, prompt, cancellationToken);
            manualResp.NoticeMessage = "Hệ thống đang chạy ở chế độ Cầu nối thủ công (Manual Bridge) do chưa thiết lập API Key.";
            return manualResp;
        }

        // Try calling the Gemini API first
        var exec = await _apiExecutor.ExecuteAsync(prompt, modelOverride, cancellationToken);

        if (exec.IsSuccess && !string.IsNullOrWhiteSpace(exec.RawResponseText))
        {
            var record = new AiRequest
            {
                RequestId = prompt.RequestId,
                Kind = prompt.Kind,
                PromptVersion = prompt.PromptVersion,
                InputHash = prompt.InputHash,
                PromptText = prompt.FullPromptText,
                Channel = AiChannel.Api,
                Status = AiRequestStatus.Pending,
                ModelLabel = exec.ModelUsed,
                CreatedAt = DateTime.UtcNow
            };

            var val = _lenientParser.ParseAndValidate<T>(exec.RawResponseText, prompt.RequestId);
            if (val.IsValid && val.Data != null)
            {
                record.Status = AiRequestStatus.Completed;
                record.ResponseJson = val.NormalizedJson;
                record.CompletedAt = DateTime.UtcNow;
                context.AiRequests.Add(record);
                await context.SaveChangesAsync(cancellationToken);

                return AiServiceResponse<T>.Completed(val.Data, prompt.RequestId);
            }

            record.Status = AiRequestStatus.Invalid;
            record.ErrorMessage = val.ErrorMessage;
            context.AiRequests.Add(record);
            await context.SaveChangesAsync(cancellationToken);

            var resp = AiServiceResponse<T>.Failed(prompt.RequestId, val.ErrorMessage ?? "Dữ liệu JSON không đúng schema.");
            resp.FixPrompt = val.FixPrompt;
            return resp;
        }

        // If rate limited (429) or connection error, degrade to Manual Bridge
        if (exec.IsQuotaExhausted || !exec.IsSuccess)
        {
            _logger.LogWarning("Hybrid mode fallback to Manual Bridge for {RequestId}. Reason: {Reason}", prompt.RequestId, exec.ErrorMessage);

            var manualResp = await HandleManualDispatchAsync<T>(context, prompt, cancellationToken);
            manualResp.NoticeMessage = exec.IsQuotaExhausted
                ? "API Gemini đã chạm giới hạn quota (HTTP 429). Hệ thống đã tự động chuyển sang chế độ Cầu nối thủ công (Manual Bridge)."
                : $"API gặp sự cố ({exec.ErrorMessage}). Hệ thống đã tự động chuyển sang chế độ Cầu nối thủ công.";
            return manualResp;
        }

        return AiServiceResponse<T>.Failed(prompt.RequestId, exec.ErrorMessage ?? "Không thể hoàn thành yêu cầu.");
    }

    #endregion
}
