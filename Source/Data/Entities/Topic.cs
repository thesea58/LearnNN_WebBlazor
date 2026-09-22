using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN_WebBlazor.Data.Entities;

[Table("Topics")]
public class Topic
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên chủ đề không được để trống")]
    [MaxLength(100, ErrorMessage = "Tên chủ đề tối đa 100 ký tự")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public ICollection<Word> Words { get; set; } = new List<Word>();
}
