using LearnNN_WebBlazor.Models.Ai;

namespace LearnNN_WebBlazor.Services.Ai;

/// <summary>
/// Provides tolerant extraction, syntax normalization, and schema validation for AI responses.
/// <para>VN: Cung cấp khả năng bóc tách khoan dung, chuẩn hóa cú pháp và kiểm định schema cho phản hồi AI.</para>
/// </summary>
public interface ILenientJsonParser
{
    /// <summary>
    /// Extracts, cleans, validates, and deserializes raw response text into the specified strongly-typed DTO.
    /// </summary>
    /// <typeparam name="T">Expected target DTO structure.</typeparam>
    /// <param name="rawInput">Raw text pasted from web chatbot or returned by API.</param>
    /// <param name="expectedRequestId">Expected RequestId for correlation guard.</param>
    AiValidationResult<T> ParseAndValidate<T>(string? rawInput, string expectedRequestId);

    /// <summary>
    /// Normalizes raw text by extracting markdown blocks, stripping smart quotes, and fixing trailing commas.
    /// </summary>
    string CleanRawJson(string rawInput);
}
