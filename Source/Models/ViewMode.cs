namespace LearnNN_WebBlazor.Models;

/// <summary>
/// Specifies the presentation view mode for data lists and tables.
/// <para>VN: Định nghĩa các chế độ trình bày dữ liệu dạng bảng (dòng) hoặc dạng thẻ icon (card grid).</para>
/// </summary>
public enum ViewMode
{
    /// <summary>
    /// Traditional table row view mode with high data density (optimized for desktop scanning).
    /// </summary>
    Table = 0,

    /// <summary>
    /// Visual card / tile icon grid view mode (optimized for mobile touch, audio, and visual learning).
    /// </summary>
    Card = 1
}
