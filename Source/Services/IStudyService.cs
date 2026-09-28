using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models.Study;

namespace LearnNN_WebBlazor.Services;

/// <summary>
/// Defines the contract for vocabulary study activities, games generation, quizzes, and mastery tracking.
/// <para>VN: Định nghĩa hợp đồng cho các hoạt động học từ vựng, tạo game, trắc nghiệm và theo dõi độ ghi nhớ.</para>
/// </summary>
public interface IStudyService
{
    #region Study Operations

    /// <summary>
    /// Retrieves a randomized list of words tailored for study sessions based on the provided filter options.
    /// </summary>
    /// <param name="options">Options specifying topic, mastery filter, and requested item count.</param>
    /// <returns>A randomized list of words matching criteria.</returns>
    Task<List<Word>> GetWordsForStudyAsync(StudySessionOptions options);

    /// <summary>
    /// Generates a set of multiple-choice quiz questions with 4 choices per question (1 correct + 3 random distractors).
    /// </summary>
    /// <param name="options">Session configuration options.</param>
    /// <returns>A list of generated quiz questions.</returns>
    Task<List<QuizQuestionDto>> GenerateQuizQuestionsAsync(StudySessionOptions options);

    /// <summary>
    /// Generates shuffled cards for the Word Matching game (pairs of English terms and Vietnamese meanings).
    /// </summary>
    /// <param name="options">Session configuration options.</param>
    /// <param name="pairCount">Number of pairs to generate (default is 6 pairs, yielding 12 cards).</param>
    /// <returns>A shuffled list of matching cards.</returns>
    Task<List<MatchCardDto>> GenerateMatchCardsAsync(StudySessionOptions options, int pairCount = 6);

    /// <summary>
    /// Sets the mastery status of a specific word.
    /// </summary>
    /// <param name="wordId">The word identifier.</param>
    /// <param name="isMastered">The new mastery status value.</param>
    Task SetWordMasteryAsync(int wordId, bool isMastered);

    /// <summary>
    /// Updates the mastery status for a batch of word identifiers.
    /// </summary>
    /// <param name="wordIds">Collection of word identifiers to update.</param>
    /// <param name="isMastered">The new mastery status value.</param>
    Task SetBatchWordsMasteryAsync(IEnumerable<int> wordIds, bool isMastered);

    /// <summary>
    /// Calculates the total count and unmastered count of words for a topic or globally.
    /// </summary>
    /// <param name="topicId">Optional topic identifier (null for all topics).</param>
    /// <returns>A tuple containing total count and unmastered count.</returns>
    Task<(int TotalCount, int UnmasteredCount)> GetStudyWordCountsAsync(int? topicId);

    #endregion
}
