using System.Text.Json.Serialization;
using Microsoft.Playwright;

namespace LearnNN_WebBlazor.Tests.Helpers;

/// <summary>
/// Helper to detect unwanted horizontal overflow on web pages according to project rules.
/// <para>VN: Tiện ích kiểm tra lỗi tràn ngang trên trang theo quy định tại mobile-desktop-lock.md.</para>
/// </summary>
public static class OverflowHelper
{
    #region Nested Models

    /// <summary>
    /// Model capturing the result of the JavaScript overflow detection script.
    /// <para>VN: Model chứa kết quả kiểm tra tràn ngang trả về từ script JavaScript.</para>
    /// </summary>
    public class OverflowResult
    {
        [JsonPropertyName("isOverflowing")]
        public bool IsOverflowing { get; set; }

        [JsonPropertyName("offenders")]
        public List<OverflowOffender> Offenders { get; set; } = [];
    }

    /// <summary>
    /// Model capturing individual DOM element causing horizontal overflow.
    /// <para>VN: Thông tin phần tử DOM gây ra hiện tượng tràn màn hình ngang.</para>
    /// </summary>
    public class OverflowOffender
    {
        [JsonPropertyName("tag")]
        public string Tag { get; set; } = string.Empty;

        [JsonPropertyName("cls")]
        public string Cls { get; set; } = string.Empty;

        [JsonPropertyName("overflowPx")]
        public double OverflowPx { get; set; }
    }

    #endregion

    #region Script Definition

    /// <summary>
    /// Standard JavaScript script from mobile-desktop-lock.md section 5.
    /// </summary>
    public const string CheckOverflowScript = """
    (() => {
      const doc = document.documentElement, w = doc.clientWidth, offenders = [];
      const skip = el => el.closest('[aria-hidden="true"], .modal:not(.show)') ||
        getComputedStyle(el).position === 'fixed' ||
        [...(function*(n){ while ((n = n.parentElement)) yield n; })(el)]
          .some(p => ['auto','scroll'].includes(getComputedStyle(p).overflowX) && p.scrollWidth > p.clientWidth);
      document.querySelectorAll('body *').forEach(el => {
        const r = el.getBoundingClientRect();
        if ((r.right > w + 1 || r.left < -1) && r.width > 0 && !skip(el))
          offenders.push({ tag: el.tagName, cls: String(el.className), overflowPx: Math.round(r.right - w) });
      });
      return { isOverflowing: doc.scrollWidth > w + 1, offenders };
    })();
    """;

    #endregion

    #region Public Execution Methods

    /// <summary>
    /// Evaluates overflow script on the provided Playwright page instance.
    /// </summary>
    /// <param name="page">Playwright page to inspect.</param>
    /// <returns>A structured <see cref="OverflowResult"/>.</returns>
    public static async Task<OverflowResult> CheckOverflowAsync(IPage page)
    {
        var result = await page.EvaluateAsync<OverflowResult>(CheckOverflowScript);
        return result ?? new OverflowResult();
    }

    #endregion
}
