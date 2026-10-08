using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models.Personalization;

namespace LearnNN_WebBlazor.Services;

/// <summary>
/// Defines the contract for recording interaction logs, evaluating learner responses, and computing skill mastery.
/// <para>VN: Định nghĩa hợp đồng cho việc ghi nhận nhật ký tương tác, đánh giá phản hồi người học và tính toán độ thành thạo kỹ năng.</para>
/// </summary>
public interface IMasteryTrackingService
{
    #region Tracking Operations

    /// <summary>
    /// Logs an individual study interaction event and updates associated skill tag mastery metrics.
    /// </summary>
    /// <param name="sessionId">The unique study/game session identifier.</param>
    /// <param name="itemType">Item classification ("Word" or "Question").</param>
    /// <param name="itemId">Primary key of the attempted word or question.</param>
    /// <param name="skillTagId">Optional skill tag identifier.</param>
    /// <param name="isCorrect">Whether the response was correct.</param>
    /// <param name="responseTimeMs">Learner response latency in milliseconds.</param>
    /// <param name="selectedAnswer">Text or identifier of chosen option.</param>
    /// <returns>The created answer log entity.</returns>
    Task<AnswerLog> LogAnswerAsync(
        Guid sessionId,
        string itemType,
        int itemId,
        int? skillTagId,
        bool isCorrect,
        long responseTimeMs,
        string? selectedAnswer = null);

    /// <summary>
    /// Updates cumulative mastery scores and attempt counts for a specified skill tag.
    /// </summary>
    /// <param name="skillTagId">Target skill tag identifier.</param>
    /// <param name="isCorrect">Whether the latest attempt was correct.</param>
    Task UpdateTagMasteryAsync(int skillTagId, bool isCorrect);

    /// <summary>
    /// Retrieves aggregated mastery scores and attempt statistics grouped across high-level skill categories.
    /// </summary>
    /// <returns>List of category mastery radar DTOs.</returns>
    Task<List<SkillRadarDto>> GetSkillRadarAsync();

    /// <summary>
    /// Retrieves the default skill tag ID for general vocabulary (e.g. VOC.TOEIC_600).
    /// </summary>
    /// <returns>Skill tag identifier.</returns>
    Task<int> GetDefaultVocabSkillTagIdAsync();

    #endregion
}
