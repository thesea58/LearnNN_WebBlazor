using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN_WebBlazor.Data.Entities;

[Table("Words")]
public class Word
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Topic))]
    public int TopicId { get; set; }

    [Required(ErrorMessage = "Từ vựng không được để trống")]
    [MaxLength(100, ErrorMessage = "Từ/cụm từ tối đa 100 ký tự")]
    public string Term { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Phonetic { get; set; }

    [MaxLength(50)]
    public string? PartOfSpeech { get; set; }

    [Required(ErrorMessage = "Nghĩa của từ không được để trống")]
    [MaxLength(500, ErrorMessage = "Nghĩa của từ tối đa 500 ký tự")]
    public string Meaning { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ExampleSentence { get; set; }

    [MaxLength(500)]
    public string? ExampleTranslation { get; set; }

    public bool IsMastered { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public Topic Topic { get; set; } = null!;
}
