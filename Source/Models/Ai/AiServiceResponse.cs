namespace LearnNN_WebBlazor.Models.Ai;

/// <summary>
/// Unified response returned by the AI service facade across Auto, Manual, and Hybrid execution paths.
/// <para>VN: Phản hồi thống nhất trả về từ AI service facade qua cả ba luồng thực thi Auto, Manual và Hybrid.</para>
/// </summary>
/// <typeparam name="T">Expected typed payload.</typeparam>
public class AiServiceResponse<T>
{
    /// <summary>
    /// Indicates whether the request is completed successfully with valid data.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Indicates whether the returned payload was retrieved from database cache without calling the AI provider.
    /// </summary>
    public bool IsCached { get; set; }

    /// <summary>
    /// True if the request requires the learner to copy the prompt and paste the JSON via the Manual Bridge modal.
    /// </summary>
    public bool RequiresManualInput { get; set; }

    /// <summary>
    /// Correlation identifier of this AI request.
    /// </summary>
    public string RequestId { get; set; } = string.Empty;

    /// <summary>
    /// Full prompt text for display or clipboard copying in manual mode.
    /// </summary>
    public string? PromptText { get; set; }

    /// <summary>
    /// Lifecycle state of this request.
    /// </summary>
    public AiRequestStatus Status { get; set; }

    /// <summary>
    /// Strongly typed data object when completed and valid.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Error message if an error occurred.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Informational message (e.g., notice about automatic degradation from Hybrid to Manual).
    /// </summary>
    public string? NoticeMessage { get; set; }

    /// <summary>
    /// Correction prompt generated if pasted JSON was invalid.
    /// </summary>
    public string? FixPrompt { get; set; }

    public static AiServiceResponse<T> Completed(T data, string requestId, bool isCached = false) => new()
    {
        IsSuccess = true,
        IsCached = isCached,
        RequiresManualInput = false,
        Status = AiRequestStatus.Completed,
        RequestId = requestId,
        Data = data
    };

    public static AiServiceResponse<T> PendingManual(string requestId, string promptText, string? notice = null) => new()
    {
        IsSuccess = false,
        RequiresManualInput = true,
        Status = AiRequestStatus.Pending,
        RequestId = requestId,
        PromptText = promptText,
        NoticeMessage = notice
    };

    public static AiServiceResponse<T> Failed(string requestId, string errorMessage) => new()
    {
        IsSuccess = false,
        RequiresManualInput = false,
        Status = AiRequestStatus.Failed,
        RequestId = requestId,
        ErrorMessage = errorMessage
    };
}
