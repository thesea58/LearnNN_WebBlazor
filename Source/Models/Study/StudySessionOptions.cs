namespace LearnNN_WebBlazor.Models.Study;

/// <summary>
/// Encapsulates filtering and session options when launching a vocabulary study or game activity.
/// <para>VN: Đóng gói các tùy chọn bộ lọc và cấu hình phiên khi khởi chạy hoạt động học hoặc trò chơi từ vựng.</para>
/// </summary>
public class StudySessionOptions
{
    #region Properties

    /// <summary>
    /// Gets or sets the target topic identifier (null indicates all topics).
    /// </summary>
    public int? TopicId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether only unmastered words (IsMastered == false) should be selected.
    /// </summary>
    public bool OnlyUnmastered { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum number of words or question items for the study session.
    /// </summary>
    public int ItemCount { get; set; } = 20;

    #endregion
}
