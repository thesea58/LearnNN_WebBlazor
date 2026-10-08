namespace LearnNN_WebBlazor.Models.Personalization;

/// <summary>
/// Data transfer object holding aggregated Spaced Repetition System (SRS) statistics for dashboard display.
/// <para>VN: Đối tượng truyền dữ liệu chứa các thống kê tổng hợp của hệ thống lặp lại ngắt quãng (SRS) hiển thị trên bảng điều khiển.</para>
/// </summary>
public class SrsStatisticsDto
{
    /// <summary>
    /// Total count of words currently due for review today (DueDate &lt;= Today).
    /// </summary>
    public int DueTodayCount { get; set; }

    /// <summary>
    /// Words currently in learning cycle (IntervalDays &lt; 21).
    /// </summary>
    public int LearningCount { get; set; }

    /// <summary>
    /// Words considered mastered via spaced repetition (IntervalDays &gt;= 21).
    /// </summary>
    public int MasteredCount { get; set; }

    /// <summary>
    /// Fresh words that haven't been studied yet (no WordProgress record).
    /// </summary>
    public int NewWordsCount { get; set; }

    /// <summary>
    /// Total words available in the selected topic or system.
    /// </summary>
    public int TotalWordsCount { get; set; }

    /// <summary>
    /// Mastery percentage based on interval criteria.
    /// </summary>
    public int MasteredPercent => TotalWordsCount > 0 ? (int)Math.Round((double)MasteredCount / TotalWordsCount * 100) : 0;
}
