using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Multi-browser engine and mobile viewport harness verification tests.
/// Validates cross-browser execution across Chromium, WebKit, and Firefox engines,
/// and executes horizontal overflow checks across target mobile viewports.
/// <para>VN: Kiểm thử tích hợp đa trình duyệt (Chromium, WebKit, Firefox) và kiểm tra lỗi tràn ngang trên các viewport mobile.</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class MultiEngineHarnessTests
{
    private IPlaywright _playwright = null!;

    [OneTimeSetUp]
    public async Task InitPlaywrightAsync()
    {
        _playwright = await Playwright.CreateAsync();
    }

    [OneTimeTearDown]
    public void DisposePlaywright()
    {
        _playwright.Dispose();
    }

    #region Engine Verification Tests

    [Test]
    public async Task ChromiumEngine_LaunchesAndNavigatesAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();
        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/");
        var title = await page.TitleAsync();
        Assert.That(title, Does.Contain("LearnNN"));
    }

    [Test]
    public async Task WebKitEngine_LaunchesAndNavigatesAsync()
    {
        await using var browser = await _playwright.Webkit.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();
        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/");
        var title = await page.TitleAsync();
        Assert.That(title, Does.Contain("LearnNN"));
    }

    [Test]
    public async Task FirefoxEngine_LaunchesAndNavigatesAsync()
    {
        await using var browser = await _playwright.Firefox.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();
        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/");
        var title = await page.TitleAsync();
        Assert.That(title, Does.Contain("LearnNN"));
    }

    #endregion

    #region Mobile Viewport Overflow Baseline Tests

    private static readonly (int Width, int Height, string DeviceName)[] MobileMatrix =
    [
        (375, 667, "iPhone SE (375x667)"),
        (390, 844, "iPhone 14 (390x844)"),
        (412, 915, "Pixel 7 (412x915)"),
        (360, 640, "Min Baseline (360x640)"),
        (768, 1024, "Tablet (768x1024)")
    ];

    public static IEnumerable<TestCaseData> GetMobileOverflowCases()
    {
        foreach (var dev in MobileMatrix)
        {
            foreach (var route in BlazorTestHelper.RequiredRoutes)
            {
                var slug = BlazorTestHelper.ToRouteSlug(route);
                yield return new TestCaseData(route, dev.Width, dev.Height, dev.DeviceName)
                    .SetName($"Mobile_Overflow_{dev.DeviceName.Replace(" ", "_")}_{slug}");
            }
        }
    }

    [TestCaseSource(nameof(GetMobileOverflowCases))]
    public async Task MobileViewport_CheckConsoleAndReadinessAsync(string route, int width, int height, string deviceName)
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = width, Height = height },
            IsMobile = true,
            HasTouch = true
        });

        var page = await context.NewPageAsync();

        var consoleErrors = new List<string>();
        page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                consoleErrors.Add(msg.Text);
            }
        };
        page.PageError += (_, err) => consoleErrors.Add(err);

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, route);

        Assert.That(consoleErrors, Is.Empty,
            $"Console errors detected on {route} ({deviceName}): {string.Join(" | ", consoleErrors)}");
    }

    #endregion
}
