using Microsoft.Playwright;

namespace LearnNN_WebBlazor.Tests.Helpers;

/// <summary>
/// Helper utilities for Blazor Server page navigation and circuit readiness synchronization.
/// <para>VN: Các tiện ích hỗ trợ điều hướng và đồng bộ hóa trạng thái sẵn sàng của Blazor Server Circuit.</para>
/// </summary>
public static class BlazorTestHelper
{
    #region Constants and Route Metadata

    /// <summary>
    /// The 10 mandatory routes defined in task specification.
    /// </summary>
    public static readonly string[] RequiredRoutes =
    [
        "/",
        "/words",
        "/topics",
        "/study",
        "/study/flashcards",
        "/study/match",
        "/study/scramble",
        "/study/quiz",
        "/ai/inbox",
        "/ai/settings"
    ];

    #endregion

    #region Synchronization Methods

    /// <summary>
    /// Navigates to a relative route on the test server and waits for full Blazor Server circuit stability.
    /// </summary>
    /// <param name="page">Playwright page instance.</param>
    /// <param name="relativeRoute">Target relative path (e.g. "/words").</param>
    /// <param name="waitForSelector">Optional specific CSS selector to wait for on page.</param>
    public static async Task NavigateAndStabilizeAsync(IPage page, string relativeRoute, string? waitForSelector = null)
    {
        string fullUrl = $"{TestServerFixture.BaseUrl}{relativeRoute}";

        // A. Navigate to URL and wait for DOMContentLoaded
        await page.GotoAsync(fullUrl, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = 30000
        });

        // B. Wait for network to quiet down
        try
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle, new() { Timeout = 5000 });
        }
        catch (TimeoutException)
        {
            // NetworkIdle timeout is non-fatal for persistent WebSocket connections in Blazor Server
        }

        // C. Wait for loading spinners to disappear
        try
        {
            await page.WaitForSelectorAsync(".spinner-border", new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Detached,
                Timeout = 5000
            });
        }
        catch (TimeoutException)
        {
            // Spinner already detached or not present
        }

        // D. Wait for custom selector if specified
        if (!string.IsNullOrEmpty(waitForSelector))
        {
            await page.WaitForSelectorAsync(waitForSelector, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 8000
            });
        }

        // E. Stabilize circuit and rendering tree (allow CSS transitions to settle)
        await page.WaitForTimeoutAsync(500);
    }

    /// <summary>
    /// Normalizes a route path into a safe filesystem filename slug.
    /// </summary>
    /// <param name="route">Relative route string.</param>
    /// <returns>Clean file slug, e.g. "home", "study-flashcards".</returns>
    public static string ToRouteSlug(string route)
    {
        var cleaned = route.Trim('/').Replace('/', '-');
        return string.IsNullOrEmpty(cleaned) ? "home" : cleaned;
    }

    #endregion
}
