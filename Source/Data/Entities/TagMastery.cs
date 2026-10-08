using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN_WebBlazor.Data.Entities;

/// <summary>
/// Tracks cumulative mastery metrics and evaluation scores for a specific <see cref="SkillTag"/>.
/// <para>VN: Theo dõi chỉ số thành thạo tích lũy và điểm số đánh giá cho một thẻ kỹ năng cụ thể.</para>
/// </summary>
[Table("TagMasteries")]
public class TagMastery
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key referencing the associated skill tag.
    /// </summary>
    [Required]
    [ForeignKey(nameof(SkillTag))]
    public int SkillTagId { get; set; }

    /// <summary>
    /// Cumulative mastery score ranging from 0.0 (novice) to 1.0 (mastered).
    /// </summary>
    public double MasteryScore { get; set; } = 0.0;

    /// <summary>
    /// Total number of questions or review attempts under this skill tag.
    /// </summary>
    public int TotalAttempts { get; set; } = 0;

    /// <summary>
    /// Total count of correct answers under this skill tag.
    /// </summary>
    public int CorrectAttempts { get; set; } = 0;

    /// <summary>
    /// Timestamp of the most recent practice activity under this skill tag.
    /// </summary>
    public DateTime? LastPracticedAt { get; set; }

    /// <summary>
    /// Navigation property to the tracked skill tag.
    /// </summary>
    public SkillTag SkillTag { get; set; } = null!;
}
