using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN_WebBlazor.Data.Entities;

/// <summary>
/// Records individual study interaction logs, answer choices, and response times for learning analytics and AI diagnosis.
/// <para>VN: Ghi lại nhật ký tương tác học tập từng câu, lựa chọn đáp án và thời gian phản hồi phục vụ phân tích học tập và AI chẩn đoán.</para>
/// </summary>
[Table("AnswerLogs")]
public class AnswerLog
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Identifier grouping interaction logs belonging to the same study or game session.
    /// </summary>
    [Required]
    public Guid SessionId { get; set; }

    /// <summary>
    /// Classification of item practiced ("Word" or "Question").
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string ItemType { get; set; } = "Word";

    /// <summary>
    /// Identifier of the specific word or question attempted.
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// Optional foreign key to the associated skill tag classification.
    /// </summary>
    [ForeignKey(nameof(SkillTag))]
    public int? SkillTagId { get; set; }

    /// <summary>
    /// Indicates whether the attempt was answered correctly.
    /// </summary>
    public bool IsCorrect { get; set; }

    /// <summary>
    /// Elapsed response time in milliseconds taken by the learner to answer.
    /// </summary>
    public long ResponseTimeMs { get; set; }

    /// <summary>
    /// String representation of the user's chosen option or submitted text.
    /// </summary>
    [MaxLength(500)]
    public string? SelectedAnswer { get; set; }

    /// <summary>
    /// Timestamp when this answer was recorded.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property to the categorized skill tag.
    /// </summary>
    public SkillTag? SkillTag { get; set; }
}
