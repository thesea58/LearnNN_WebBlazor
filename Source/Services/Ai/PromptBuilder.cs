using System.Security.Cryptography;
using System.Text;
using LearnNN_WebBlazor.Models.Ai;
using LearnNN_WebBlazor.Models.Study;

namespace LearnNN_WebBlazor.Services.Ai;

/// <summary>
/// Implements standardized prompt generation with strict JSON schema constraints and pedagogical Vietnamese guidance.
/// <para>VN: Triển khai sinh prompt chuẩn hóa với ràng buộc schema JSON chặt chẽ và hướng dẫn sư phạm bằng tiếng Việt.</para>
/// </summary>
public class PromptBuilder : IPromptBuilder
{
    private const string DefaultRole = "VAI TRÒ: Bạn là một chuyên gia giảng dạy tiếng Anh và luyện thi TOEIC xuất sắc dành cho người Việt Nam. Phong cách giải thích ngắn gọn, súc tích, bóc tách bản chất ngữ pháp/từ vựng và chỉ ra các bẫy đề thi kinh điển.";

    #region IPromptBuilder Implementation

    /// <inheritdoc/>
    public RenderedAiPrompt Render(AiPromptDefinition def)
    {
        if (string.IsNullOrWhiteSpace(def.RequestId))
            def.RequestId = GenerateRequestId(def.Kind);

        var sb = new StringBuilder();
        sb.AppendLine($"[LearnNN-AI] request_id: {def.RequestId} | kind: {def.Kind} | version: {def.PromptVersion}");
        sb.AppendLine();
        sb.AppendLine(string.IsNullOrWhiteSpace(def.SystemRole) ? DefaultRole : def.SystemRole);
        sb.AppendLine();
        sb.AppendLine("NHIỆM VỤ:");
        sb.AppendLine(def.Instructions);
        sb.AppendLine();
        sb.AppendLine("DỮ LIỆU ĐẦU VÀO:");
        sb.AppendLine(def.ContextData);
        sb.AppendLine();
        sb.AppendLine("ĐỊNH DẠNG ĐẦU RA BẮT BUỘC:");
        sb.AppendLine("Chỉ trả về DUY NHẤT MỘT khối ```json hợp lệ, không viết bất kỳ lời chào hay giải thích nào bên ngoài.");
        sb.AppendLine($"Khối JSON bắt buộc phải chứa trường \"request_id\": \"{def.RequestId}\".");
        sb.AppendLine();
        sb.AppendLine(def.OutputSchemaSpec);
        sb.AppendLine();
        if (!string.IsNullOrWhiteSpace(def.ExampleJson))
        {
            sb.AppendLine("VÍ DỤ CẤU TRÚC JSON MẪU:");
            sb.AppendLine("```json");
            sb.AppendLine(def.ExampleJson.Trim());
            sb.AppendLine("```");
        }

        string fullText = sb.ToString();
        string inputHash = ComputeHash($"{def.Kind}:{def.ContextData}");

        return new RenderedAiPrompt
        {
            RequestId = def.RequestId,
            Kind = def.Kind,
            PromptVersion = def.PromptVersion,
            FullPromptText = fullText,
            InputHash = inputHash
        };
    }

    /// <inheritdoc/>
    public RenderedAiPrompt BuildQuizExplanationPrompt(
        string questionStem,
        IReadOnlyList<string> options,
        int correctIndex,
        int chosenIndex,
        string? relatedTerm = null)
    {
        string requestId = GenerateRequestId("EXPLAIN");

        var optSb = new StringBuilder();
        for (int i = 0; i < options.Count; i++)
        {
            char letter = (char)('A' + i);
            optSb.AppendLine($"  {letter}. {options[i]}");
        }

        char correctLetter = (char)('A' + correctIndex);
        char chosenLetter = (char)('A' + chosenIndex);
        bool isCorrect = (correctIndex == chosenIndex);

        var contextSb = new StringBuilder();
        contextSb.AppendLine($"- Câu hỏi: \"{questionStem}\"");
        contextSb.AppendLine("- Các lựa chọn:");
        contextSb.Append(optSb);
        contextSb.AppendLine($"- Đáp án ĐÚNG: {correctLetter}. {options[correctIndex]}");
        contextSb.AppendLine($"- Người học ĐÃ CHỌN: {chosenLetter}. {options[chosenIndex]} ({(isCorrect ? "ĐÚNG" : "SAI")})");
        if (!string.IsNullOrWhiteSpace(relatedTerm))
        {
            contextSb.AppendLine($"- Từ vựng trọng tâm liên quan: \"{relatedTerm}\"");
        }

        string instructions = isCorrect
            ? "Người học đã trả lời ĐÚNG. Hãy giải thích ngắn gọn tại sao đáp án này chuẩn xác, các dấu hiệu nhận biết nhanh trong bài thi TOEIC và phân tích sơ lược các đáp án còn lại."
            : "Người học đã trả lời SAI. Hãy bóc tách cụ thể: (1) Tại sao đáp án người học chọn là sai / bẫy gì, (2) Tại sao đáp án chuẩn mới chính xác, (3) Quy tắc ngữ pháp hoặc cấu trúc cần nhớ.";

        string schemaSpec =
@"{
  ""request_id"": """ + requestId + @""",
  ""is_correct"": " + (isCorrect ? "true" : "false") + @",
  ""correct_answer_reason_vi"": ""<Giải thích tại sao đáp án đúng lại chính xác>"",
  ""trap_type"": ""<Loại bẫy trong câu hỏi: vd: Từ loại, Thì động từ, Từ đồng âm, Dịch nghĩa nhầm, Đại từ quan hệ, hoặc 'Không có bẫy'>"",
  ""why_distractors_are_wrong_vi"": ""<Lý do các đáp án nhiễu khác sai>"",
  ""grammar_rule_ref"": ""<Tên điểm ngữ pháp / cấu trúc cốt lõi>"",
  ""advice_for_learner_vi"": ""<Lời khuyên ngắn gọn để không bao giờ sai lại lỗi này>""
}";

        var def = new AiPromptDefinition
        {
            RequestId = requestId,
            Kind = "QuizExplanation",
            PromptVersion = "v1.0",
            SystemRole = DefaultRole,
            Instructions = instructions,
            ContextData = contextSb.ToString(),
            OutputSchemaSpec = "Cấu trúc JSON yêu cầu:",
            ExampleJson = schemaSpec
        };

        return Render(def);
    }

    /// <inheritdoc/>
    public RenderedAiPrompt BuildBatchQuizExplanationPrompt(
        IReadOnlyList<QuizQuestionDto> questions)
    {
        string requestId = GenerateRequestId("BATCH-QUIZ");

        var contextSb = new StringBuilder();
        contextSb.AppendLine($"Danh sách {questions.Count} câu hỏi trắc nghiệm cần phân tích và giải thích:");
        contextSb.AppendLine();

        for (int qIdx = 0; qIdx < questions.Count; qIdx++)
        {
            var q = questions[qIdx];
            char correctLetter = (char)('A' + q.CorrectOptionIndex);
            char chosenLetter = q.SelectedOptionIndex >= 0 && q.SelectedOptionIndex < q.Options.Count
                ? (char)('A' + q.SelectedOptionIndex)
                : '?';
            bool isCorrect = q.IsCorrect;

            contextSb.AppendLine($"### CÂU HỎI {qIdx + 1} (word_id: {q.WordId}):");
            contextSb.AppendLine($"- Từ vựng / Thuật ngữ: \"{q.Term}\"");
            if (!string.IsNullOrWhiteSpace(q.PartOfSpeech)) contextSb.AppendLine($"- Từ loại: {q.PartOfSpeech}");
            if (!string.IsNullOrWhiteSpace(q.ExampleSentence)) contextSb.AppendLine($"- Câu ví dụ ngữ cảnh: \"{q.ExampleSentence}\"");
            contextSb.AppendLine("- Các phương án:");
            for (int i = 0; i < q.Options.Count; i++)
            {
                char letter = (char)('A' + i);
                contextSb.AppendLine($"    {letter}. {q.Options[i]}");
            }
            contextSb.AppendLine($"- Đáp án ĐÚNG: {correctLetter}. {(q.CorrectOptionIndex >= 0 && q.CorrectOptionIndex < q.Options.Count ? q.Options[q.CorrectOptionIndex] : q.Meaning)}");
            contextSb.AppendLine($"- Người học ĐÃ CHỌN: {chosenLetter}. {(q.SelectedOptionIndex >= 0 && q.SelectedOptionIndex < q.Options.Count ? q.Options[q.SelectedOptionIndex] : "Chưa chọn")} ({(isCorrect ? "ĐÚNG" : "SAI")})");
            contextSb.AppendLine();
        }

        string instructions =
            "Hãy đóng vai trò một chuyên gia luyện thi TOEIC giàu kinh nghiệm, phân tích toàn bộ danh sách câu hỏi trên. Với mỗi câu hỏi, hãy chỉ ra: " +
            "(1) Tại sao đáp án đúng lại chính xác, (2) Loại bẫy của câu hỏi (VD: Từ loại, Từ đồng âm, Dịch nghĩa nhầm, Ngữ cảnh công sở, hoặc 'Không có bẫy'), " +
            "(3) Phân tích lý do các phương án nhiễu khác sai, (4) Điểm ngữ pháp / cấu trúc cốt lõi, (5) Lời khuyên sư phạm ngắn gọn cho người học.";

        string schemaSpec =
@"{
  ""request_id"": """ + requestId + @""",
  ""items"": [
    {
      ""word_id"": <word_id tương ứng của câu>,
      ""term"": ""<từ vựng của câu>"",
      ""is_correct"": <true|false>,
      ""correct_answer_reason_vi"": ""<Giải thích tại sao đáp án đúng lại chính xác>"",
      ""trap_type"": ""<Loại bẫy: Từ loại, Từ đồng âm, Dịch nghĩa nhầm, Cấu trúc, v.v.>"",
      ""why_distractors_are_wrong_vi"": ""<Lý do các đáp án nhiễu khác sai>"",
      ""grammar_rule_ref"": ""<Tên điểm ngữ pháp / cấu trúc cốt lõi>"",
      ""advice_for_learner_vi"": ""<Lời khuyên ngắn gọn để không bao giờ sai lại lỗi này>""
    }
  ]
}";

        var def = new AiPromptDefinition
        {
            RequestId = requestId,
            Kind = "BatchQuizExplanation",
            PromptVersion = "v1.0",
            SystemRole = DefaultRole,
            Instructions = instructions,
            ContextData = contextSb.ToString(),
            OutputSchemaSpec = "Cấu trúc JSON yêu cầu (bắt buộc trả về đúng mảng items tương ứng):",
            ExampleJson = schemaSpec
        };

        return Render(def);
    }

    /// <inheritdoc/>
    public RenderedAiPrompt BuildWordEnrichmentPrompt(
        string term,
        string? partOfSpeech = null,
        string? currentMeaning = null)
    {
        string requestId = GenerateRequestId("ENRICH");

        var contextSb = new StringBuilder();
        contextSb.AppendLine($"- Từ vựng: \"{term}\"");
        if (!string.IsNullOrWhiteSpace(partOfSpeech)) contextSb.AppendLine($"- Từ loại hiện tại: {partOfSpeech}");
        if (!string.IsNullOrWhiteSpace(currentMeaning)) contextSb.AppendLine($"- Nghĩa hiện tại: {currentMeaning}");

        string instructions =
            "Hãy làm giàu thông tin cho từ vựng này phục vụ kỳ thi TOEIC: điền phiên âm quốc tế chuẩn IPA, giải nghĩa súc tích tiếng Việt, " +
            "cung cấp 1 câu ví dụ ngữ cảnh công sở/thương mại (song ngữ Anh - Việt), liệt kê 2-4 collocations thông dụng, " +
            "các từ cùng họ (word family) và 1 mẹo liên tưởng/ghi nhớ bằng tiếng Việt (mnemonic).";

        string schemaSpec =
@"{
  ""request_id"": """ + requestId + @""",
  ""term"": """ + term + @""",
  ""phonetic"": ""/ˈ.../"",
  ""part_of_speech"": ""noun / verb / adjective / adverb"",
  ""meaning_vi"": ""<Nghĩa tiếng Việt rõ ràng, cô đọng>"",
  ""business_example_en"": ""<Ví dụ tiếng Anh ngữ cảnh văn phòng, hợp đồng hoặc báo cáo>"",
  ""business_example_vi"": ""<Bản dịch tiếng Việt của câu ví dụ trên>"",
  ""collocations"": [""cụm 1"", ""cụm 2""],
  ""word_family"": [""từ loại khác 1"", ""từ loại khác 2""],
  ""vietnamese_mnemonic"": ""<Mẹo liên tưởng âm thanh hoặc hình ảnh vui bằng tiếng Việt giúp nhớ nhanh>""
}";

        var def = new AiPromptDefinition
        {
            RequestId = requestId,
            Kind = "WordEnrichment",
            PromptVersion = "v1.0",
            SystemRole = DefaultRole,
            Instructions = instructions,
            ContextData = contextSb.ToString(),
            OutputSchemaSpec = "Cấu trúc JSON yêu cầu:",
            ExampleJson = schemaSpec
        };

        return Render(def);
    }

    /// <inheritdoc/>
    public string ComputeHash(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input.Trim()));
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Produces a human-readable, unique correlation identifier.
    /// </summary>
    private static string GenerateRequestId(string prefix)
    {
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string randomPart = Guid.NewGuid().ToString("N")[..6].ToUpper();
        return $"{prefix.ToUpper()}-{timestamp}-{randomPart}";
    }

    #endregion
}
