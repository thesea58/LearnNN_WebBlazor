namespace LearnNN_WebBlazor.Models.Ai;

/// <summary>
/// Encapsulates the evaluation outcome of raw AI responses against expected schema specifications.
/// <para>VN: Đóng gói kết quả đánh giá phản hồi thô từ AI so với cấu trúc schema kỳ vọng.</para>
/// </summary>
/// <typeparam name="T">Expected strongly-typed output payload.</typeparam>
public class AiValidationResult<T>
{
    /// <summary>
    /// Indicates whether parsing and schema conformity succeeded.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Deserialized typed payload when validation succeeds.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Cleaned, normalized JSON string extracted from markdown code blocks.
    /// </summary>
    public string? NormalizedJson { get; set; }

    /// <summary>
    /// Extracted or validated request identifier from the response.
    /// </summary>
    public string? ResponseRequestId { get; set; }

    /// <summary>
    /// Detailed diagnostic message describing parsing errors or schema mismatches.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Readily-formatted correction prompt that the learner can copy and paste into the chatbot to fix formatting mistakes.
    /// </summary>
    public string? FixPrompt { get; set; }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static AiValidationResult<T> Success(T data, string normalizedJson, string requestId)
    {
        return new AiValidationResult<T>
        {
            IsValid = true,
            Data = data,
            NormalizedJson = normalizedJson,
            ResponseRequestId = requestId
        };
    }

    /// <summary>
    /// Creates a failed validation result with diagnostic details and auto-generated correction prompt.
    /// </summary>
    public static AiValidationResult<T> Failure(string errorMessage, string? fixPrompt = null, string? rawExtracted = null)
    {
        return new AiValidationResult<T>
        {
            IsValid = false,
            ErrorMessage = errorMessage,
            FixPrompt = fixPrompt,
            NormalizedJson = rawExtracted
        };
    }
}
