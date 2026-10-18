using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Visual regression tests comparing PC viewports (1366x768 and 1920x1080) against committed baselines.
/// Guarantees that desktop layout and appearance remain strictly locked (≤ 0.1% diff).
/// Dynamic in-game elements (randomized words/timers) are masked to ensure deterministic visual validation.
/// <para>VN: Bộ kiểm thử hồi quy thị giác PC ở các độ phân giải 1366x768 và 1920x1080 so với ảnh chuẩn baseline.</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class PcBaselineTests : PageTest
{
    #region Viewport Definitions

    private static readonly (int Width, int Height, string Label)[] PcViewports =
    [
        (1366, 768, "1366x768"),
        (1920, 1080, "1920x1080")
    ];

    #endregion

    #region Test Source Generation

    public static IEnumerable<TestCaseData> GetBaselineTestCases()
    {
        foreach (var viewport in PcViewports)
        {
            foreach (var route in BlazorTestHelper.RequiredRoutes)
            {
                var slug = BlazorTestHelper.ToRouteSlug(route);
                yield return new TestCaseData(route, viewport.Width, viewport.Height, viewport.Label, slug)
                    .SetName($"PC_Baseline_{viewport.Label}_{slug}");
            }
        }
    }

    #endregion

    #region Baseline Visual Regression Tests

    [TestCaseSource(nameof(GetBaselineTestCases))]
    public async Task ValidatePcBaselineAsync(string route, int width, int height, string viewportLabel, string slug)
    {
        // A. Setup viewport size & console error tracking
        await Page.SetViewportSizeAsync(width, height);

        var consoleErrors = new List<string>();
        Page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                consoleErrors.Add($"[Console Error] {msg.Text}");
            }
        };
        Page.PageError += (_, error) =>
        {
            consoleErrors.Add($"[Page Error] {error}");
        };

        // B. Navigate and wait for Blazor Server circuit stability
        await BlazorTestHelper.NavigateAndStabilizeAsync(Page, route);

        // C. Configure masks for dynamic randomized content in game/study modules
        var screenshotOptions = new PageScreenshotOptions
        {
            FullPage = false,
            Animations = ScreenshotAnimations.Disabled
        };

        var maskLocators = new List<ILocator>();
        switch (slug)
        {
            case "study-flashcards":
                maskLocators.Add(Page.Locator(".flashcard-scene"));
                maskLocators.Add(Page.Locator(".srs-interval-badge"));
                break;
            case "study-match":
                maskLocators.Add(Page.Locator(".matching-grid"));
                maskLocators.Add(Page.Locator(".badge:has(.bi-stopwatch)"));
                break;
            case "study-scramble":
                maskLocators.Add(Page.Locator(".card:has(.scramble-slots)"));
                break;
            case "study-quiz":
                maskLocators.Add(Page.Locator(".card:has(.quiz-option-btn)"));
                maskLocators.Add(Page.Locator(".badge:has(.bi-patch-question)"));
                break;
        }

        if (maskLocators.Count > 0)
        {
            screenshotOptions.Mask = maskLocators;
        }

        // D. Take viewport screenshot
        byte[] actualScreenshot = await Page.ScreenshotAsync(screenshotOptions);

        // E. Resolve baseline and diff paths
        string rootDir = TestServerFixture.FindSolutionRoot();
        string baselineDir = Path.Combine(rootDir, "Tests", "LearnNN_WebBlazor.Tests", "Baselines", viewportLabel);
        string baselinePath = Path.Combine(baselineDir, $"{slug}.png");
        string diffDir = Path.Combine(rootDir, "Tests", "LearnNN_WebBlazor.Tests", "Diffs", viewportLabel);
        string diffPath = Path.Combine(diffDir, $"{slug}_diff.png");

        // F. Perform pixel-by-pixel comparison with 0.1% tolerance
        var diffResult = ImageComparisonHelper.Compare(
            baselinePath,
            actualScreenshot,
            diffPath,
            maxAllowedDiffPercent: 0.1
        );

        // G. Assertions
        Assert.Multiple(() =>
        {
            Assert.That(diffResult.IsMatch, Is.True,
                $"Visual regression detected on {route} ({viewportLabel})! {diffResult.Message}. Diff saved to: {diffPath}");

            Assert.That(consoleErrors, Is.Empty,
                $"Console errors detected on {route} ({viewportLabel}): {string.Join(" | ", consoleErrors)}");
        });
    }

    #endregion
}
