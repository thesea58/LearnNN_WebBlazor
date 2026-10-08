namespace LearnNN_WebBlazor.Models.Ai;

/// <summary>
/// Defines standard template inputs and schema constraints used by prompt builders.
/// <para>VN: Định nghĩa các tham số đầu vào và cấu trúc schema chuẩn dùng cho các bộ sinh prompt.</para>
/// </summary>
public class AiPromptDefinition
{
    /// <summary>
    /// Unique correlation identifier (e.g., "REQ-20261009-XXXX").
    /// </summary>
    public string RequestId { get; set; } = string.Empty;

    /// <summary>
    /// Category tag describing the functional intent.
    /// </summary>
    public string Kind { get; set; } = string.Empty;

    /// <summary>
    /// Version label of this prompt structure.
    /// </summary>
    public string PromptVersion { get; set; } = "v1.0";

    /// <summary>
    /// Pedagogical role assumed by the model (e.g., TOEIC Vietnamese tutor).
    /// </summary>
    public string SystemRole { get; set; } = string.Empty;

    /// <summary>
    /// Step-by-step instructions directing the model on how to solve the problem.
    /// </summary>
    public string Instructions { get; set; } = string.Empty;

    /// <summary>
    /// Contextual data payload containing questions, words, or learner submissions.
    /// </summary>
    public string ContextData { get; set; } = string.Empty;

    /// <summary>
    /// JSON output schema instructions and markdown codeblock requirements.
    /// </summary>
    public string OutputSchemaSpec { get; set; } = string.Empty;

    /// <summary>
    /// Example JSON output demonstrating the required structure.
    /// </summary>
    public string ExampleJson { get; set; } = string.Empty;

    /// <summary>
    /// Number of items bundled in this prompt when batching is used.
    /// </summary>
    public int BatchCount { get; set; } = 1;
}

/// <summary>
/// Encapsulates the complete rendered prompt text ready for copy or API dispatch.
/// <para>VN: Đóng gói chuỗi văn bản prompt hoàn chỉnh sẵn sàng để sao chép hoặc gửi qua API.</para>
/// </summary>
public class RenderedAiPrompt
{
    public string RequestId { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string PromptVersion { get; set; } = "v1.0";
    public string FullPromptText { get; set; } = string.Empty;
    public string InputHash { get; set; } = string.Empty;
}
