using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models.Personalization;

namespace LearnNN_WebBlazor.Services;

/// <summary>
/// Defines the contract for the SuperMemo-2 (SM-2) Spaced Repetition System (SRS) engine,
/// handling due review queues, interval calculations, and retention statistics.
/// <para>VN: Định nghĩa hợp đồng cho công cụ Lặp lại ngắt quãng (SRS) SuperMemo-2 (SM-2), xử lý hàng đợi ôn tập đến hạn, tính khoảng cách ngày và thống kê ghi nhớ.</para>
/// </summary>
public interface ISrsEngineService
{
    #region SRS Operations

    /// <summary>
    /// Retrieves words that are currently due for review (DueDate &lt;= Today), ordered by most overdue.
    /// </summary>
    /// <param name="limit">Maximum number of due words to fetch.</param>
    /// <param name="topicId">Optional topic filter.</param>
    /// <returns>List of vocabulary words requiring review today.</returns>
    Task<List<Word>> GetDueWordsAsync(int limit = 50, int? topicId = null);

    /// <summary>
    /// Applies the SuperMemo-2 algorithm to a word review, updating its progress and syncing mastery flags.
    /// </summary>
    /// <param name="wordId">Word identifier being reviewed.</param>
    /// <param name="qualityRating">Recall quality rating (0 to 5).</param>
    /// <param name="sessionId">Optional study session GUID for interaction logging.</param>
    /// <param name="responseTimeMs">Latency in milliseconds taken to review.</param>
    /// <returns>Computed review results including next due date and interval.</returns>
    Task<SrsReviewResultDto> RecordReviewResultAsync(
        int wordId,
        int qualityRating,
        Guid? sessionId = null,
        long responseTimeMs = 0);

    /// <summary>
    /// Aggregates system-wide or topic-specific SRS statistics including Due Today, Learning, and Mastered counts.
    /// </summary>
    /// <param name="topicId">Optional topic filter.</param>
    /// <returns>Aggregated SRS statistics DTO.</returns>
    Task<SrsStatisticsDto> GetSrsStatisticsAsync(int? topicId = null);

    /// <summary>
    /// Computes the prospective next interval in days for each rating level (Quên, Khó, Tốt, Dễ) for UI button badges.
    /// </summary>
    /// <param name="wordId">Target word identifier.</param>
    /// <returns>Preview of intervals in days for all 4 rating choices.</returns>
    Task<SrsIntervalPreviewDto> GetIntervalPreviewAsync(int wordId);

    #endregion
}
