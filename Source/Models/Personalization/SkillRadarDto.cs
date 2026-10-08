namespace LearnNN_WebBlazor.Models.Personalization;

/// <summary>
/// Data transfer object representing mastery statistics for a specific skill category or tag for radar chart visualization.
/// <para>VN: Đối tượng truyền dữ liệu đại diện cho thống kê độ thành thạo của một danh mục hoặc thẻ kỹ năng để vẽ biểu đồ radar.</para>
/// </summary>
public class SkillRadarDto
{
    /// <summary>
    /// Category or tag name (e.g. "Vocabulary", "Grammar", "Listening", "Reading").
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Cumulative mastery score from 0.0 to 1.0 (dampened accuracy).
    /// </summary>
    public double MasteryScore { get; set; }

    /// <summary>
    /// Total interaction attempts under this category.
    /// </summary>
    public int TotalAttempts { get; set; }

    /// <summary>
    /// Total correct attempts under this category.
    /// </summary>
    public int CorrectAttempts { get; set; }

    /// <summary>
    /// Raw accuracy percentage (Correct / Total * 100).
    /// </summary>
    public int AccuracyPercentage => TotalAttempts > 0 ? (int)Math.Round((double)CorrectAttempts / TotalAttempts * 100) : 0;
}
