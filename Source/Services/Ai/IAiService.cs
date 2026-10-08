using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models.Ai;

namespace LearnNN_WebBlazor.Services.Ai;

/// <summary>
/// Primary application entry point for AI interactions supporting transparent caching, multi-channel dispatch,
/// automatic degradation (Hybrid -> Manual), and manual response completion.
/// <para>VN: Điểm đầu mối chính cho các tương tác AI hỗ trợ cache trong suốt, điều phối đa kênh,
/// tự động hạ cấp (Hybrid sang Manual), và hoàn thành phản hồi thủ công.</para>
/// </summary>
public interface IAiService
{
    /// <summary>
    /// Processes an AI prompt across configured modes (Auto, Manual, Hybrid) with automatic cache evaluation.
    /// </summary>
    Task<AiServiceResponse<T>> RequestAsync<T>(
        RenderedAiPrompt prompt,
        string? modelOverride = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates and records a user-submitted JSON payload for an outstanding manual bridge request.
    /// </summary>
    Task<AiValidationResult<T>> CompleteManualRequestAsync<T>(
        string requestId,
        string rawPastedText,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the current count of pending AI requests requiring user input.
    /// </summary>
    Task<int> GetPendingRequestCountAsync();

    /// <summary>
    /// Retrieves recent AI requests for history review or pending fulfillment.
    /// </summary>
    Task<List<AiRequest>> GetRecentRequestsAsync(int limit = 20, AiRequestStatus? statusFilter = null);

    /// <summary>
    /// Retrieves a specific request by its correlation identifier.
    /// </summary>
    Task<AiRequest?> GetRequestByIdAsync(string requestId);

    /// <summary>
    /// Deletes a specific AI request record.
    /// </summary>
    Task<bool> DeleteRequestAsync(int id);
}
