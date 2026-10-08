using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN_WebBlazor.Data.Entities;

/// <summary>
/// Maintains scientific spaced repetition (SRS) state and SuperMemo-2 scheduling parameters for a <see cref="Word"/>.
/// <para>VN: Quản lý trạng thái lặp lại ngắt quãng (SRS) và các tham số lịch trình SuperMemo-2 cho một từ vựng.</para>
/// </summary>
[Table("WordProgresses")]
public class WordProgress
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key referencing the parent vocabulary word entry.
    /// </summary>
    [Required]
    [ForeignKey(nameof(Word))]
    public int WordId { get; set; }

    /// <summary>
    /// Next scheduled review timestamp in UTC.
    /// </summary>
    [Required]
    public DateTime DueDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Current repetition interval in days before next review.
    /// </summary>
    public int IntervalDays { get; set; } = 1;

    /// <summary>
    /// SuperMemo-2 ease factor determining interval growth multiplier (default: 2.5, minimum: 1.3).
    /// </summary>
    public double EaseFactor { get; set; } = 2.5;

    /// <summary>
    /// Count of consecutive successful reviews since last lapse.
    /// </summary>
    public int Repetitions { get; set; } = 0;

    /// <summary>
    /// Total count of recall failures (times forgotten) throughout learning history.
    /// </summary>
    public int Lapses { get; set; } = 0;

    /// <summary>
    /// Timestamp when this word was last reviewed or practiced.
    /// </summary>
    public DateTime? LastStudiedAt { get; set; }

    /// <summary>
    /// Navigation property to the target vocabulary word.
    /// </summary>
    public Word Word { get; set; } = null!;
}
