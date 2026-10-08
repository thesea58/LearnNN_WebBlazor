using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN_WebBlazor.Data.Entities;

/// <summary>
/// Represents a standardized skill classification tag in the knowledge taxonomy,
/// supporting a hierarchical tree structure across Vocabulary, Grammar, Listening, and Reading.
/// <para>VN: Đại diện cho thẻ phân loại kỹ năng chuẩn hóa trong cây kiến thức, hỗ trợ cấu trúc phân cấp qua Từ vựng, Ngữ pháp, Nghe và Đọc.</para>
/// </summary>
[Table("SkillTags")]
public class SkillTag
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Unique classification code (e.g., "VOC.TOEIC_600", "VOC.WORD_FORM", "GRAM.TENSE.PAST").
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable skill name in Vietnamese or English.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// High-level skill category (Vocabulary, Grammar, Listening, Reading).
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Optional parent skill identifier for hierarchical skill relationships.
    /// </summary>
    [ForeignKey(nameof(ParentTag))]
    public int? ParentId { get; set; }

    /// <summary>
    /// Navigation property to parent skill tag.
    /// </summary>
    public SkillTag? ParentTag { get; set; }

    /// <summary>
    /// Child skill tags categorized under this parent tag.
    /// </summary>
    public ICollection<SkillTag> ChildTags { get; set; } = new List<SkillTag>();

    /// <summary>
    /// Mastery records tracked for this skill tag.
    /// </summary>
    public ICollection<TagMastery> TagMasteries { get; set; } = new List<TagMastery>();

    /// <summary>
    /// Answer logs associated with this skill tag.
    /// </summary>
    public ICollection<AnswerLog> AnswerLogs { get; set; } = new List<AnswerLog>();
}
