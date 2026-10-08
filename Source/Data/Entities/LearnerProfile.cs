using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN_WebBlazor.Data.Entities;

/// <summary>
/// Represents the individual learner profile, storing target scores, exam target dates,
/// daily commitment goals, and current learning stage.
/// <para>VN: Đại diện cho hồ sơ người học cá nhân, lưu trữ mục tiêu điểm số, ngày thi, thời gian học mỗi ngày và chặng học hiện tại.</para>
/// </summary>
[Table("LearnerProfiles")]
public class LearnerProfile
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Target TOEIC or proficiency score (e.g., 650).
    /// </summary>
    public int TargetScore { get; set; } = 650;

    /// <summary>
    /// Planned target exam date.
    /// </summary>
    public DateTime? ExamDate { get; set; }

    /// <summary>
    /// Daily study goal in minutes (e.g., 30 minutes).
    /// </summary>
    public int DailyGoalMinutes { get; set; } = 30;

    /// <summary>
    /// Current learning stage in the curriculum (e.g., S1: Foundation, S2: Bridge, S3: Part Practice, S4: Sprint).
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string CurrentStage { get; set; } = "S1";

    /// <summary>
    /// Preferred English audio accent for pronunciation (e.g., "en-US", "en-GB").
    /// </summary>
    [MaxLength(20)]
    public string PreferredAccent { get; set; } = "en-US";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
