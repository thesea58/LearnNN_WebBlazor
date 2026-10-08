namespace LearnNN_WebBlazor.Models.Personalization;

/// <summary>
/// Defines standard quality response ratings for the SuperMemo-2 (SM-2) spaced repetition algorithm.
/// <para>VN: Định nghĩa các mức đánh giá chất lượng phản hồi chuẩn cho thuật toán lặp lại ngắt quãng SuperMemo-2 (SM-2).</para>
/// </summary>
public enum SrsRating
{
    /// <summary>
    /// Complete blackout; forgot the term completely. Resets repetitions to 0 and interval to 1 day.
    /// <para>VN: Quên hoàn toàn từ vựng; đặt lại số lần lặp về 0 và khoảng cách về 1 ngày.</para>
    /// </summary>
    Blackout = 0,

    /// <summary>
    /// Correct response recalled with serious difficulty.
    /// <para>VN: Nhớ đúng nhưng rất khó khăn, tốn nhiều thời gian suy nghĩ.</para>
    /// </summary>
    Difficult = 3,

    /// <summary>
    /// Correct response after brief hesitation; standard successful recall.
    /// <para>VN: Nhớ đúng sau một chút đắn đo; mức nhớ thành công tiêu chuẩn.</para>
    /// </summary>
    Good = 4,

    /// <summary>
    /// Perfect response with instant recall; increases ease factor and interval rapidly.
    /// <para>VN: Nhớ ngay lập tức và hoàn hảo; tăng nhanh khoảng cách ngày và hệ số dễ.</para>
    /// </summary>
    Easy = 5
}
