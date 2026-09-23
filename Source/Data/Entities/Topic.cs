using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN_WebBlazor.Data.Entities;

/// <summary>
/// Represents a vocabulary topic that groups related words for organized learning.
/// <para>VN: Đại diện cho một chủ đề từ vựng, dùng để nhóm các từ liên quan phục vụ việc học có tổ chức.</para>
/// </summary>
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

    /// <summary>
    /// Words belonging to this topic. Cascade-deleted when the topic is removed.
    /// </summary>
    public ICollection<Word> Words { get; set; } = new List<Word>();
}
