namespace LearnNN_WebBlazor.Models.Personalization;

/// <summary>
/// Data transfer object containing the computed result after applying the SuperMemo-2 (SM-2) algorithm.
/// <para>VN: Đối tượng truyền dữ liệu chứa kết quả tính toán sau khi áp dụng thuật toán SuperMemo-2 (SM-2).</para>
/// </summary>
public class SrsReviewResultDto
{
    public int WordId { get; set; }

    public int QualityRating { get; set; }

    public int IntervalDays { get; set; }

    public double EaseFactor { get; set; }

    public int Repetitions { get; set; }

    public DateTime NextDueDate { get; set; }

    public bool IsMastered { get; set; }
}
