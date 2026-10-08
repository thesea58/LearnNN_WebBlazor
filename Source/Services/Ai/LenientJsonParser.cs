using System.Text.Json;
using System.Text.RegularExpressions;
using LearnNN_WebBlazor.Models.Ai;

namespace LearnNN_WebBlazor.Services.Ai;

/// <summary>
/// Robust and tolerant JSON parser capable of extracting JSON payloads from conversational chatbot responses,
/// repairing common syntax deviations (smart quotes, trailing commas), verifying correlation request IDs,
/// and synthesizing correction prompts upon failure.
/// <para>VN: Bộ phân tích JSON khoan dung có khả năng bóc tách payload JSON từ hội thoại chatbot,
/// tự sửa các lỗi cú pháp phổ biến (smart quotes, dấu phẩy thừa), kiểm tra mã request_id và tự sinh prompt sửa lỗi.</para>
/// </summary>
public class LenientJsonParser : ILenientJsonParser
{
    private const int MaxAllowedLength = 100_000; // 100 KB limit to prevent payload flooding / DOS
    private static readonly Regex MarkdownJsonBlockRegex = new(@"```(?:json)?\s*([\s\S]*?)\s*```", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex TrailingCommaRegex = new(@",\s*([\]}])", RegexOptions.Compiled);

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    #region ILenientJsonParser Implementation

    /// <inheritdoc/>
    public AiValidationResult<T> ParseAndValidate<T>(string? rawInput, string expectedRequestId)
    {
        if (string.IsNullOrWhiteSpace(rawInput))
        {
            return AiValidationResult<T>.Failure(
                "Nội dung dán vào đang trống. Vui lòng dán phản hồi JSON từ chatbot.",
                BuildFixPrompt(expectedRequestId, "Bạn chưa cung cấp nội dung JSON nào.")
            );
        }

        if (rawInput.Length > MaxAllowedLength)
        {
            return AiValidationResult<T>.Failure(
                $"Nội dung quá lớn ({rawInput.Length:N0} ký tự). Giới hạn tối đa là {MaxAllowedLength:N0} ký tự để bảo đảm an toàn hệ thống.",
                null
            );
        }

        string cleanedJson;
        try
        {
            cleanedJson = CleanRawJson(rawInput);
        }
        catch (Exception ex)
        {
            return AiValidationResult<T>.Failure(
                $"Không thể trích xuất JSON: {ex.Message}",
                BuildFixPrompt(expectedRequestId, "Không tìm thấy cấu trúc JSON hợp lệ trong câu trả lời.")
            );
        }

        // Validate JSON syntax and inspect request_id
        try
        {
            using var doc = JsonDocument.Parse(cleanedJson);
            var root = doc.RootElement;

            // Check request_id correlation if present at root or in items
            string? foundRequestId = ExtractRequestId(root);
            if (!string.IsNullOrWhiteSpace(foundRequestId) &&
                !string.Equals(foundRequestId.Trim(), expectedRequestId.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return AiValidationResult<T>.Failure(
                    $"Sai mã Request ID! Bạn vừa dán kết quả của yêu cầu '{foundRequestId}', nhưng hệ thống đang chờ '{expectedRequestId}'. Hãy kiểm tra lại lượt chat trên web.",
                    BuildFixPrompt(expectedRequestId, $"Bạn đã trả về sai request_id. Mã chính xác phải là: \"{expectedRequestId}\"."),
                    cleanedJson
                );
            }

            // Strongly typed deserialization
            var data = JsonSerializer.Deserialize<T>(cleanedJson, _jsonOptions);
            if (data == null)
            {
                return AiValidationResult<T>.Failure(
                    "Không thể ánh xạ dữ liệu JSON sang cấu trúc dữ liệu mong đợi.",
                    BuildFixPrompt(expectedRequestId, "Cấu trúc dữ liệu JSON không khớp với schema yêu cầu."),
                    cleanedJson
                );
            }

            return AiValidationResult<T>.Success(data, cleanedJson, expectedRequestId);
        }
        catch (JsonException jEx)
        {
            string errorDesc = $"Lỗi cú pháp JSON ở dòng {jEx.LineNumber}, vị trí {jEx.BytePositionInLine}: {jEx.Message}";
            return AiValidationResult<T>.Failure(
                errorDesc,
                BuildFixPrompt(expectedRequestId, errorDesc),
                cleanedJson
            );
        }
        catch (Exception ex)
        {
            return AiValidationResult<T>.Failure(
                $"Lỗi xử lý kết quả: {ex.Message}",
                BuildFixPrompt(expectedRequestId, ex.Message),
                cleanedJson
            );
        }
    }

    /// <inheritdoc/>
    public string CleanRawJson(string rawInput)
    {
        if (string.IsNullOrWhiteSpace(rawInput))
            return string.Empty;

        string text = rawInput.Trim();

        // 1. Look for markdown code block ```json ... ```
        var match = MarkdownJsonBlockRegex.Match(text);
        if (match.Success && match.Groups.Count > 1)
        {
            text = match.Groups[1].Value.Trim();
        }
        else
        {
            // 2. If no code block, slice between the first '{' or '[' and the last '}' or ']'
            int firstBrace = text.IndexOf('{');
            int firstBracket = text.IndexOf('[');
            int start = -1;

            if (firstBrace >= 0 && firstBracket >= 0)
                start = Math.Min(firstBrace, firstBracket);
            else if (firstBrace >= 0)
                start = firstBrace;
            else if (firstBracket >= 0)
                start = firstBracket;

            int lastBrace = text.LastIndexOf('}');
            int lastBracket = text.LastIndexOf(']');
            int end = Math.Max(lastBrace, lastBracket);

            if (start >= 0 && end > start)
            {
                text = text.Substring(start, end - start + 1).Trim();
            }
        }

        // 3. Normalize smart quotes to standard ASCII quotes
        text = text.Replace('“', '"')
                   .Replace('”', '"')
                   .Replace('‘', '\'')
                   .Replace('’', '\'');

        // 4. Clean trailing commas in objects or arrays: e.g. ", }" -> " }"
        text = TrailingCommaRegex.Replace(text, "$1");

        return text;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Searches for a "request_id" or "requestId" property in the JSON element.
    /// </summary>
    private static string? ExtractRequestId(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.Object)
        {
            if (root.TryGetProperty("request_id", out var p1) && p1.ValueKind == JsonValueKind.String)
                return p1.GetString();
            if (root.TryGetProperty("requestId", out var p2) && p2.ValueKind == JsonValueKind.String)
                return p2.GetString();
        }
        else if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
        {
            var first = root[0];
            if (first.ValueKind == JsonValueKind.Object)
            {
                if (first.TryGetProperty("request_id", out var p1) && p1.ValueKind == JsonValueKind.String)
                    return p1.GetString();
                if (first.TryGetProperty("requestId", out var p2) && p2.ValueKind == JsonValueKind.String)
                    return p2.GetString();
            }
        }

        return null;
    }

    /// <summary>
    /// Constructs a standardized, polite correction prompt that the learner can copy and paste into the chatbot.
    /// </summary>
    private static string BuildFixPrompt(string requestId, string errorDetail)
    {
        return $"[LearnNN-AI SỬA LỖI] request_id: {requestId}\n\n" +
               $"Kết quả trước bạn vừa trả về gặp vấn đề kỹ thuật sau:\n" +
               $"- Chi tiết lỗi: {errorDetail}\n\n" +
               $"Vui lòng sửa lại và CHỈ trả về duy nhất MỘT khối ```json hợp lệ, đúng cấu trúc và chứa đúng request_id: \"{requestId}\". Không thêm bất kỳ lời chào hay giải thích nào bên ngoài khối json.";
    }

    #endregion
}
