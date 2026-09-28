namespace LearnNN_WebBlazor.Models.Study;

/// <summary>
/// Encapsulates the evaluation summary of a completed quiz session.
/// <para>VN: Đóng gói bảng tóm tắt kết quả đánh giá của một bài kiểm tra trắc nghiệm hoàn thành.</para>
/// </summary>
public class QuizResultDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the total number of questions in the quiz.
    /// </summary>
    public int TotalQuestions { get; set; }

    /// <summary>
    /// Gets or sets the count of questions answered correctly.
    /// </summary>
    public int CorrectAnswersCount { get; set; }

    /// <summary>
    /// Gets or sets the count of questions answered incorrectly.
    /// </summary>
    public int IncorrectAnswersCount => TotalQuestions - CorrectAnswersCount;

    /// <summary>
    /// Gets the score percentage rounded to the nearest integer.
    /// </summary>
    public int ScorePercentage => TotalQuestions > 0 ? (int)Math.Round((double)CorrectAnswersCount / TotalQuestions * 100) : 0;

    /// <summary>
    /// Gets or sets the elapsed time spent completing the quiz.
    /// </summary>
    public TimeSpan TimeElapsed { get; set; }

    /// <summary>
    /// Gets or sets the full question list with user answers for detailed review.
    /// </summary>
    public List<QuizQuestionDto> Questions { get; set; } = new();

    #endregion
}
