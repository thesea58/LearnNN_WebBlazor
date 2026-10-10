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

    /// <summary>
    /// Records the result of a quiz answer attempt, logging user interaction and updating spaced repetition state.
    /// </summary>
    /// <param name="sessionId">The quiz session identifier.</param>
    /// <param name="wordId">The word being tested.</param>
    /// <param name="isCorrect">Whether the selected option was correct.</param>
    /// <param name="responseTimeMs">Learner response time in milliseconds.</param>
    /// <param name="selectedOption">Text of the option selected by user.</param>
    Task RecordQuizAnswerAsync(Guid sessionId, int wordId, bool isCorrect, long responseTimeMs, string? selectedOption = null);

    /// <summary>
    /// Records the result of a card matching attempt in the Word Match game.
    /// </summary>
    /// <param name="sessionId">The matching game session identifier.</param>
    /// <param name="wordId">The word tested in the card match.</param>
    /// <param name="isCorrect">Whether the pair matched correctly.</param>
    /// <param name="responseTimeMs">Time taken to make the match attempt in milliseconds.</param>
    Task RecordMatchAnswerAsync(Guid sessionId, int wordId, bool isCorrect, long responseTimeMs);

    /// <summary>
    /// Records the result of a word scramble spelling attempt.
    /// </summary>
    /// <param name="sessionId">The word scramble game session identifier.</param>
    /// <param name="wordId">The word tested in the scramble.</param>
    /// <param name="isCorrect">Whether the scrambled word was solved correctly.</param>
    /// <param name="responseTimeMs">Time taken to submit in milliseconds.</param>
    Task RecordScrambleAnswerAsync(Guid sessionId, int wordId, bool isCorrect, long responseTimeMs);

    /// <summary>
    /// Updates the AI-classified trap type in the learner's answer log for a question in a quiz session.
    /// </summary>
    /// <param name="sessionId">The quiz session identifier.</param>
    /// <param name="wordId">The word being tested.</param>
    /// <param name="trapType">The classified trap type returned by the AI.</param>
    Task UpdateAnswerLogTrapTypeAsync(Guid sessionId, int wordId, string trapType);

    #endregion
}
