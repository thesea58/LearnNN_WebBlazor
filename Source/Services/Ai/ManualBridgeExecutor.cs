using LearnNN_WebBlazor.Models.Ai;

namespace LearnNN_WebBlazor.Services.Ai;

/// <summary>
/// Executor handling the manual AI bridge workflow where prompts are copied to a web chatbot and responses pasted back.
/// <para>VN: Bộ thực thi xử lý luồng cầu nối thủ công: sao chép prompt sang chatbot web và dán kết quả trở lại.</para>
/// </summary>
public class ManualBridgeExecutor : IAiExecutor
{
    public AiChannel Channel => AiChannel.Manual;

    /// <inheritdoc/>
    public Task<AiExecutionResult> ExecuteAsync(
        RenderedAiPrompt prompt,
        string? modelOverride = null,
        CancellationToken cancellationToken = default)
    {
        // For Manual Bridge, prompt dispatch simply prepares the request for user copy/paste
        return Task.FromResult(AiExecutionResult.ManualPending());
    }
}
