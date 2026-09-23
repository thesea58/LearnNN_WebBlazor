namespace LearnNN_WebBlazor.Models;

/// <summary>
/// Filter options for the mastery status of words.
/// <para>VN: Tùy chọn lọc theo trạng thái ghi nhớ của từ vựng.</para>
/// </summary>
public enum MasteredFilter
{
    All = 0,
    NotMastered = 1,
    Mastered = 2
}

/// <summary>
/// Encapsulates filter criteria for querying words, including search, topic, mastery status, and pagination.
/// <para>VN: Đóng gói các tiêu chí lọc khi truy vấn từ vựng, bao gồm tìm kiếm, chủ đề, trạng thái ghi nhớ và phân trang.</para>
/// </summary>
public class WordFilterModel
{
    public string? SearchTerm { get; set; }
    public int? TopicId { get; set; }
    public MasteredFilter MasteredFilter { get; set; } = MasteredFilter.All;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
