using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LearnNN_WebBlazor.Models.Ai;

namespace LearnNN_WebBlazor.Data.Entities;

/// <summary>
/// Persists AI request metadata, prompt templates, and execution responses across both automated API and manual bridge channels.
/// <para>VN: Lưu trữ thông tin yêu cầu AI, mẫu prompt và kết quả xử lý qua cả hai kênh API tự động và cầu nối thủ công.</para>
/// </summary>
[Table("AiRequests")]
public class AiRequest
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Unique correlation identifier (e.g., "REQ-20261009-XXXX") returned by the chatbot to prevent mismatched pastes.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string RequestId { get; set; } = string.Empty;

    /// <summary>
    /// Functional category of the AI interaction (e.g., "QuizExplanation", "WordEnrichment", "SentenceGrading", "SandboxTest").
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Kind { get; set; } = string.Empty;

    /// <summary>
    /// Version tag of the prompt template employed for tracking response consistency.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string PromptVersion { get; set; } = "v1.0";

    /// <summary>
    /// SHA256 hash digest of the normalized input payload for idempotency and transparent response caching.
    /// </summary>
    [MaxLength(64)]
    public string? InputHash { get; set; }

    /// <summary>
    /// Full prompt instructions and JSON schema definition sent to the AI service or copied by the user.
    /// </summary>
    [Required]
    public string PromptText { get; set; } = string.Empty;

    /// <summary>
    /// Execution channel employed for this interaction (Manual or Api).
    /// </summary>
    public AiChannel Channel { get; set; } = AiChannel.Manual;

    /// <summary>
    /// Current lifecycle state of the request (Pending, Completed, Failed, Invalid).
    /// </summary>
    public AiRequestStatus Status { get; set; } = AiRequestStatus.Pending;

    /// <summary>
    /// Structured JSON payload returned by the AI provider or submitted via the manual bridge.
    /// </summary>
    public string? ResponseJson { get; set; }

    /// <summary>
    /// Specific AI model label or provider identifier (e.g., "gemini-2.5-flash-lite", "manual-web-gemini").
    /// </summary>
    [MaxLength(100)]
    public string? ModelLabel { get; set; }

    /// <summary>
    /// Detailed diagnostic message if schema validation or external communication fails.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Timestamp when this request was created and queued.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when response processing and validation finished successfully.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
}
