using LearnNN_WebBlazor.Models.Ai;

namespace LearnNN_WebBlazor.Services.Ai;

/// <summary>
/// Encapsulates the execution result returned by a channel-specific executor.
/// <para>VN: Đóng gói kết quả thực thi trả về từ executor của từng kênh cụ thể.</para>
/// </summary>
public class AiExecutionResult
{
    public bool IsSuccess { get; set; }
    public string? RawResponseText { get; set; }
    public string? ModelUsed { get; set; }
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Indicates whether the failure was caused by rate limiting or quota exhaustion (HTTP 429).
    /// </summary>
    public bool IsQuotaExhausted { get; set; }

    public static AiExecutionResult Success(string rawResponse, string model) => new()
    {
        IsSuccess = true,
        RawResponseText = rawResponse,
        ModelUsed = model
    };

    public static AiExecutionResult ManualPending() => new()
    {
        IsSuccess = true,
        RawResponseText = null,
        ModelUsed = "manual-web"
    };

    public static AiExecutionResult Failure(string errorMessage, bool isQuota = false) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage,
        IsQuotaExhausted = isQuota
    };
}

/// <summary>
/// Defines the dispatch contract for executing an AI prompt through a concrete channel (API or Manual).
/// <para>VN: Định nghĩa hợp đồng điều phối thực thi prompt qua một kênh cụ thể (API hoặc Thủ công).</para>
/// </summary>
public interface IAiExecutor
{
    /// <summary>
    /// The channel represented by this executor.
    /// </summary>
    AiChannel Channel { get; }

    /// <summary>
    /// Executes or schedules the rendered prompt.
    /// </summary>
    Task<AiExecutionResult> ExecuteAsync(
        RenderedAiPrompt prompt,
        string? modelOverride = null,
        CancellationToken cancellationToken = default);
}
