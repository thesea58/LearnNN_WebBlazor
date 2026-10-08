namespace LearnNN_WebBlazor.Models.Personalization;

/// <summary>
/// Data transfer object holding next interval previews for each evaluation button on flashcards.
/// <para>VN: Đối tượng truyền dữ liệu chứa số ngày ôn tập dự kiến cho từng nút đánh giá trên thẻ flashcard.</para>
/// </summary>
public class SrsIntervalPreviewDto
{
    /// <summary>
    /// Next interval if rated Blackout (Rating 0).
    /// </summary>
    public int BlackoutDays { get; set; } = 1;

    /// <summary>
    /// Next interval if rated Difficult (Rating 3).
    /// </summary>
    public int DifficultDays { get; set; } = 1;

    /// <summary>
    /// Next interval if rated Good (Rating 4).
    /// </summary>
    public int GoodDays { get; set; } = 1;

    /// <summary>
    /// Next interval if rated Easy (Rating 5).
    /// </summary>
    public int EasyDays { get; set; } = 1;
}
