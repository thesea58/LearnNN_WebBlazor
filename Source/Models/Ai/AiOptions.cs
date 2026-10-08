namespace LearnNN_WebBlazor.Models.Ai;

/// <summary>
/// Configuration options binding for the AI subsystem (API keys, models, modes, and web chatbot targets).
/// <para>VN: Lớp cấu hình ràng buộc cho hệ thống AI (API key, model, chế độ và URL chatbot web).</para>
/// </summary>
public class AiOptions
{
    public const string SectionName = "Ai";

    /// <summary>
    /// Current operating mode: Manual, Auto, or Hybrid. Defaults to Manual.
    /// </summary>
    public AiMode Mode { get; set; } = AiMode.Manual;

    /// <summary>
    /// Google AI Studio API Key. Configured securely via User Secrets or Environment Variables.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Model used for lightweight/fast tasks such as quiz explanations and word enrichment (e.g. "gemini-2.5-flash-lite").
    /// </summary>
    public string FastModel { get; set; } = "gemini-2.5-flash-lite";

    /// <summary>
    /// Model used for complex reasoning tasks such as study plan generation and writing feedback (e.g. "gemini-2.5-flash").
    /// </summary>
    public string SmartModel { get; set; } = "gemini-2.5-flash";

    /// <summary>
    /// Web chatbot URL opened when the user clicks 'Open Chatbot' in the Manual Bridge modal.
    /// </summary>
    public string PreferredChatbotUrl { get; set; } = "https://gemini.google.com/";

    /// <summary>
    /// Maximum allowed automated API calls per day to avoid unexpected quota exhaustion.
    /// </summary>
    public int DailyApiLimit { get; set; } = 100;

    /// <summary>
    /// Returns true if an API key has been supplied.
    /// </summary>
    public bool HasApiKey => !string.IsNullOrWhiteSpace(ApiKey);
}
