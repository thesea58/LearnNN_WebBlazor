namespace LearnNN_WebBlazor.Models.Study;

/// <summary>
/// Specifies whether a matching game card displays an English term or a Vietnamese meaning.
/// <para>VN: Xác định loại thẻ trong game nối từ là thuật ngữ tiếng Anh hay nghĩa tiếng Việt.</para>
/// </summary>
public enum MatchCardType
{
    Term,
    Meaning
}

/// <summary>
/// Represents a single interactive card in the vocabulary matching game.
/// <para>VN: Đại diện cho một ô thẻ tương tác trong trò chơi ghép đôi nối từ vựng.</para>
/// </summary>
public class MatchCardDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier of the card in the current game board.
    /// </summary>
    public string CardId { get; set; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets or sets the database Word ID this card belongs to (used to verify pairs).
    /// </summary>
    public int WordId { get; set; }

    /// <summary>
    /// Gets or sets the card display text (either English term or Vietnamese meaning).
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the card type.
    /// </summary>
    public MatchCardType CardType { get; set; }

    /// <summary>
    /// Gets or sets whether this card is currently selected by the player.
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets whether this card has been successfully matched and solved.
    /// </summary>
    public bool IsMatched { get; set; }

    /// <summary>
    /// Gets or sets whether the card is in a temporary wrong state (triggers shake animation).
    /// </summary>
    public bool IsWrong { get; set; }

    #endregion
}
