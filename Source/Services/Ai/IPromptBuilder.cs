using LearnNN_WebBlazor.Models.Ai;
using LearnNN_WebBlazor.Models.Study;

namespace LearnNN_WebBlazor.Services.Ai;

/// <summary>
/// Defines contracts for constructing structured, pedagogically sound AI prompt templates.
/// <para>VN: Định nghĩa hợp đồng cho việc xây dựng các mẫu prompt AI sư phạm, chuẩn hóa cấu trúc.</para>
/// </summary>
public interface IPromptBuilder
{
    /// <summary>
    /// Renders a generic prompt from a defined specification into full markdown text.
    /// </summary>
    RenderedAiPrompt Render(AiPromptDefinition definition);

    /// <summary>
    /// Constructs a standardized prompt for analyzing and explaining a TOEIC quiz question and identifying traps.
    /// </summary>
    RenderedAiPrompt BuildQuizExplanationPrompt(
        string questionStem,
        IReadOnlyList<string> options,
        int correctIndex,
        int chosenIndex,
        string? relatedTerm = null);

    /// <summary>
    /// Constructs a standardized batch prompt for explaining multiple quiz questions and analyzing distractors/traps in a single interaction.
    /// </summary>
    RenderedAiPrompt BuildBatchQuizExplanationPrompt(
        IReadOnlyList<QuizQuestionDto> questions);

    /// <summary>
    /// Constructs a standardized prompt for lexical enrichment (collocations, word family, business context, mnemonic).
    /// </summary>
    RenderedAiPrompt BuildWordEnrichmentPrompt(
        string term,
        string? partOfSpeech = null,
        string? currentMeaning = null);

    /// <summary>
    /// Computes a deterministic SHA256 hash string for caching and deduplication.
    /// </summary>
    string ComputeHash(string input);
}
