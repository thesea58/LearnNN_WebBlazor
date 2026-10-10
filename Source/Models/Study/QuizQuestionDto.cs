using LearnNN_WebBlazor.Models.Ai;

namespace LearnNN_WebBlazor.Models.Study;

/// <summary>
/// Represents a multiple-choice question item for vocabulary quizzes with 4 answer choices.
/// <para>VN: Đại diện cho một câu hỏi trắc nghiệm kiểm tra từ vựng với 4 phương án lựa chọn.</para>
/// </summary>
public class QuizQuestionDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the primary word identifier this question is testing.
    /// </summary>
    public int WordId { get; set; }

    /// <summary>
    /// Gets or sets the question prompt (e.g. English term or Vietnamese meaning).
    /// </summary>
    public string QuestionText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phonetic pronunciation of the word (if available).
    /// </summary>
    public string? Phonetic { get; set; }

    /// <summary>
    /// Gets or sets the part of speech (noun, verb, etc.).
    /// </summary>
    public string? PartOfSpeech { get; set; }

    /// <summary>
    /// Gets or sets the example sentence containing the word.
    /// </summary>
    public string? ExampleSentence { get; set; }

    /// <summary>
    /// Gets or sets the list of answer options (typically 4 choices).
    /// </summary>
    public List<string> Options { get; set; } = new();

    /// <summary>
    /// Gets or sets the 0-based index of the correct answer within <see cref="Options"/>.
    /// </summary>
    public int CorrectOptionIndex { get; set; }

    /// <summary>
    /// Gets or sets the 0-based index of the user's selected choice (-1 if not yet answered).
    /// </summary>
    public int SelectedOptionIndex { get; set; } = -1;

    /// <summary>
    /// Gets a value indicating whether the question has been answered by the user.
    /// </summary>
    public bool IsAnswered => SelectedOptionIndex >= 0;

    /// <summary>
    /// Gets a value indicating whether the user answered correctly.
    /// </summary>
    public bool IsCorrect => SelectedOptionIndex == CorrectOptionIndex;

    /// <summary>
    /// Gets or sets the target word's English term for reference.
    /// </summary>
    public string Term { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target word's Vietnamese meaning for reference.
    /// </summary>
    public string Meaning { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the AI-generated explanation and trap detection analysis for this question.
    /// </summary>
    public QuizExplanationDto? Explanation { get; set; }

    /// <summary>
    /// Gets or sets whether the AI explanation card is expanded in the UI.
    /// </summary>
    public bool IsExplanationExpanded { get; set; } = true;

    /// <summary>
    /// Gets or sets whether an AI explanation request is currently loading for this question.
    /// </summary>
    public bool IsExplaining { get; set; }

    #endregion
}

