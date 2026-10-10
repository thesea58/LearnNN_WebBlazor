using System.Text.Json.Serialization;

namespace LearnNN_WebBlazor.Models.Ai;

/// <summary>
/// Structured DTO representing the AI explanation of a quiz question and trap analysis.
/// <para>VN: DTO cấu trúc biểu diễn lời giải thích câu hỏi quiz và phân tích bẫy từ AI.</para>
/// </summary>
public class QuizExplanationDto
{
    [JsonPropertyName("request_id")]
    public string RequestId { get; set; } = string.Empty;

    [JsonPropertyName("is_correct")]
    public bool IsCorrect { get; set; }

    [JsonPropertyName("correct_answer_reason_vi")]
    public string CorrectAnswerReasonVi { get; set; } = string.Empty;

    [JsonPropertyName("trap_type")]
    public string TrapType { get; set; } = string.Empty;

    [JsonPropertyName("why_distractors_are_wrong_vi")]
    public string WhyDistractorsAreWrongVi { get; set; } = string.Empty;

    [JsonPropertyName("grammar_rule_ref")]
    public string GrammarRuleRef { get; set; } = string.Empty;

    [JsonPropertyName("advice_for_learner_vi")]
    public string AdviceForLearnerVi { get; set; } = string.Empty;
}

/// <summary>
/// Structured DTO representing enriched lexical details for a vocabulary term.
/// <para>VN: DTO cấu trúc biểu diễn dữ liệu làm giàu từ vựng (ngữ cảnh công sở, cụm từ, mẹo nhớ tiếng Việt).</para>
/// </summary>
public class WordEnrichmentDto
{
    [JsonPropertyName("request_id")]
    public string RequestId { get; set; } = string.Empty;

    [JsonPropertyName("term")]
    public string Term { get; set; } = string.Empty;

    [JsonPropertyName("phonetic")]
    public string Phonetic { get; set; } = string.Empty;

    [JsonPropertyName("part_of_speech")]
    public string PartOfSpeech { get; set; } = string.Empty;

    [JsonPropertyName("meaning_vi")]
    public string MeaningVi { get; set; } = string.Empty;

    [JsonPropertyName("business_example_en")]
    public string BusinessExampleEn { get; set; } = string.Empty;

    [JsonPropertyName("business_example_vi")]
    public string BusinessExampleVi { get; set; } = string.Empty;

    [JsonPropertyName("collocations")]
    public List<string> Collocations { get; set; } = new();

    [JsonPropertyName("word_family")]
    public List<string> WordFamily { get; set; } = new();

    [JsonPropertyName("vietnamese_mnemonic")]
    public string VietnameseMnemonic { get; set; } = string.Empty;
}

/// <summary>
/// Structured DTO representing the AI batch explanation of multiple quiz questions.
/// <para>VN: DTO cấu trúc biểu diễn kết quả giải thích và phân tích bẫy hàng loạt cho nhiều câu hỏi quiz.</para>
/// </summary>
public class BatchQuizExplanationDto
{
    [JsonPropertyName("request_id")]
    public string RequestId { get; set; } = string.Empty;

    [JsonPropertyName("items")]
    public List<BatchQuizItemExplanationDto> Items { get; set; } = new();
}

/// <summary>
/// Structured item within a batch AI quiz explanation response.
/// <para>VN: DTO chi tiết cho từng câu trong gói giải thích hàng loạt từ AI.</para>
/// </summary>
public class BatchQuizItemExplanationDto
{
    [JsonPropertyName("word_id")]
    public int WordId { get; set; }

    [JsonPropertyName("term")]
    public string Term { get; set; } = string.Empty;

    [JsonPropertyName("is_correct")]
    public bool IsCorrect { get; set; }

    [JsonPropertyName("correct_answer_reason_vi")]
    public string CorrectAnswerReasonVi { get; set; } = string.Empty;

    [JsonPropertyName("trap_type")]
    public string TrapType { get; set; } = string.Empty;

    [JsonPropertyName("why_distractors_are_wrong_vi")]
    public string WhyDistractorsAreWrongVi { get; set; } = string.Empty;

    [JsonPropertyName("grammar_rule_ref")]
    public string GrammarRuleRef { get; set; } = string.Empty;

    [JsonPropertyName("advice_for_learner_vi")]
    public string AdviceForLearnerVi { get; set; } = string.Empty;

    /// <summary>
    /// Converts this item to a standalone QuizExplanationDto.
    /// </summary>
    public QuizExplanationDto ToQuizExplanationDto(string requestId)
    {
        return new QuizExplanationDto
        {
            RequestId = requestId,
            IsCorrect = IsCorrect,
            CorrectAnswerReasonVi = CorrectAnswerReasonVi,
            TrapType = TrapType,
            WhyDistractorsAreWrongVi = WhyDistractorsAreWrongVi,
            GrammarRuleRef = GrammarRuleRef,
            AdviceForLearnerVi = AdviceForLearnerVi
        };
    }
}

