namespace LearnNN_WebBlazor.Models.Ai;

/// <summary>
/// Defines the operating mode of the AI integration system.
/// <para>VN: Định nghĩa chế độ vận hành của hệ thống tích hợp AI.</para>
/// </summary>
public enum AiMode
{
    /// <summary>
    /// Manual AI Bridge: No API key required; copy prompt to web chatbot and paste JSON back.
    /// </summary>
    Manual = 0,

    /// <summary>
    /// Direct Gemini API: Requires Google AI Studio API key for automated execution.
    /// </summary>
    Auto = 1,

    /// <summary>
    /// Hybrid Mode: Uses Gemini API, automatically degrades to Manual Bridge on quota limit or network issues.
    /// </summary>
    Hybrid = 2
}

/// <summary>
/// Specifies the channel through which an AI request was or will be fulfilled.
/// <para>VN: Xác định kênh mà yêu cầu AI được xử lý (thủ công qua web chatbot hay qua API).</para>
/// </summary>
public enum AiChannel
{
    /// <summary>
    /// Manual bridge copy/paste via user web browser.
    /// </summary>
    Manual = 0,

    /// <summary>
    /// Direct automated API communication.
    /// </summary>
    Api = 1
}

/// <summary>
/// Represents the lifecycle status of an AI interaction request.
/// <para>VN: Trạng thái vòng đời của một yêu cầu tương tác AI.</para>
/// </summary>
public enum AiRequestStatus
{
    /// <summary>
    /// Awaiting user response paste (in Manual mode) or queued for execution.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Successfully processed, validated, and cached.
    /// </summary>
    Completed = 1,

    /// <summary>
    /// API call or execution failed due to an error.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// The pasted JSON or response content failed schema validation.
    /// </summary>
    Invalid = 3
}
