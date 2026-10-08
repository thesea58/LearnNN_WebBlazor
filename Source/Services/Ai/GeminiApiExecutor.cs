using System.Net;
using System.Text;
using System.Text.Json;
using LearnNN_WebBlazor.Models.Ai;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LearnNN_WebBlazor.Services.Ai;

/// <summary>
/// Executor communicating directly with the Google AI Studio Gemini REST API with JSON mode enabled.
/// <para>VN: Bộ thực thi giao tiếp trực tiếp với REST API của Google AI Studio Gemini với chế độ JSON được kích hoạt.</para>
/// </summary>
public class GeminiApiExecutor : IAiExecutor
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<AiOptions> _options;
    private readonly ILogger<GeminiApiExecutor> _logger;

    public AiChannel Channel => AiChannel.Api;

    public GeminiApiExecutor(
        HttpClient httpClient,
        IOptions<AiOptions> options,
        ILogger<GeminiApiExecutor> logger)
    {
        _httpClient = httpClient;
        _options = options;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AiExecutionResult> ExecuteAsync(
        RenderedAiPrompt prompt,
        string? modelOverride = null,
        CancellationToken cancellationToken = default)
    {
        var config = _options.Value;
        string? apiKey = config.ApiKey;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return AiExecutionResult.Failure(
                "Chưa cấu hình Google AI Studio API Key. Vui lòng thiết lập qua User Secrets, biến môi trường, hoặc chuyển sang chế độ Manual Bridge."
            );
        }

        string model = !string.IsNullOrWhiteSpace(modelOverride)
            ? modelOverride
            : config.FastModel;

        string requestUri = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt.FullPromptText }
                    }
                }
            },
            generationConfig = new
            {
                responseMimeType = "application/json"
            }
        };

        string jsonPayload = JsonSerializer.Serialize(requestBody);
        using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(requestUri, content, cancellationToken);

            if (response.StatusCode == HttpStatusCode.TooManyRequests) // 429
            {
                _logger.LogWarning("Gemini API rate limit or quota exceeded (429) for model {Model}", model);
                return AiExecutionResult.Failure(
                    "Google AI Studio đã chạm giới hạn tần suất hoặc hạn ngạch sử dụng (HTTP 429 Too Many Requests).",
                    isQuota: true
                );
            }

            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Gemini API error (Status {StatusCode}): {Response}", response.StatusCode, responseContent);
                string friendlyMessage = ExtractErrorMessage(responseContent) ?? $"Gọi API Gemini thất bại (Mã lỗi {response.StatusCode}).";
                return AiExecutionResult.Failure(friendlyMessage);
            }

            // Extract candidate text
            using var doc = JsonDocument.Parse(responseContent);
            var root = doc.RootElement;

            if (root.TryGetProperty("candidates", out var candidates) &&
                candidates.GetArrayLength() > 0 &&
                candidates[0].TryGetProperty("content", out var cContent) &&
                cContent.TryGetProperty("parts", out var parts) &&
                parts.GetArrayLength() > 0 &&
                parts[0].TryGetProperty("text", out var textElem))
            {
                string text = textElem.GetString() ?? string.Empty;
                return AiExecutionResult.Success(text, model);
            }

            return AiExecutionResult.Failure("API Gemini trả về kết quả rỗng hoặc không chứa phần văn bản hợp lệ.");
        }
        catch (TaskCanceledException)
        {
            return AiExecutionResult.Failure("Yêu cầu gửi tới Gemini API đã bị quá thời gian chờ (timeout).");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error communicating with Gemini API");
            return AiExecutionResult.Failure($"Lỗi kết nối tới Gemini API: {ex.Message}");
        }
    }

    private static string? ExtractErrorMessage(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("error", out var errorElem) &&
                errorElem.TryGetProperty("message", out var msgElem))
            {
                return msgElem.GetString();
            }
        }
        catch
        {
            // Ignore parse errors on raw error strings
        }
        return null;
    }
}
