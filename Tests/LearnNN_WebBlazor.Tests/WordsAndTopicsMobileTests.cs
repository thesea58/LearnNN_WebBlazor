using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Automated tests for Task 24.04: Words &amp; Topics mobile responsiveness,
/// card view default, table data-label stack pattern, and pagination touch targets.
/// <para>VN: Kiểm thử tự động Task 24.04: Giao diện mobile Words &amp; Topics, Card view mặc định, bảng data-label và touch target phân trang.</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class WordsAndTopicsMobileTests
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

    [Test]
    public async Task Words_DefaultsToCardView_OnMobileViewportAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 390, Height = 844 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/words");

        // Verify word cards are rendered by default
        var wordCards = page.Locator(".word-card");
        var count = await wordCards.CountAsync();
        Assert.That(count, Is.GreaterThan(0), "Words page should default to Card view on mobile viewport.");
    }

    [Test]
    public async Task Words_TableMode_HasDataLabels_AndHidesThead_OnMobileAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 375, Height = 667 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/words");

        // Switch to Table mode via ViewModeSwitcher
        var tableModeBtn = page.Locator("button[title*='dòng' i], button[title*='Table' i], button:has(.bi-table)");
        await tableModeBtn.ClickAsync();
        await page.WaitForTimeoutAsync(500);

        // Verify table rendered with table-responsive-stack
        var table = page.Locator("table.table-responsive-stack");
        await Assertions.Expect(table).ToBeVisibleAsync();

        // Verify thead is hidden on <= 767.98px
        var thead = table.Locator("thead");
        await Assertions.Expect(thead).ToBeHiddenAsync();

        // Verify td elements have data-label attribute
        var termTd = table.Locator("td[data-label='Từ vựng']").First;
        await Assertions.Expect(termTd).ToBeVisibleAsync();
    }

    [Test]
    public async Task Topics_TableMode_HasDataLabels_OnMobileAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 375, Height = 667 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/topics");

        // Switch to Table mode via ViewModeSwitcher
        var tableModeBtn = page.Locator("button[title*='dòng' i], button[title*='Table' i], button:has(.bi-list-ul)");
        if (await tableModeBtn.CountAsync() > 0)
        {
            await tableModeBtn.ClickAsync();
            await page.WaitForTimeoutAsync(500);
        }

        // Verify table has data-label on topic name
        var table = page.Locator("table.table-responsive-stack");
        await Assertions.Expect(table).ToBeVisibleAsync();

        var topicTd = table.Locator("td[data-label='Tên chủ đề']").First;
        await Assertions.Expect(topicTd).ToBeVisibleAsync();
    }

    [Test]
    public async Task Words_InputFontSize_Is16px_OnMobile_ToPreventAutoZoomAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 375, Height = 667 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/words");

        var searchInput = page.Locator("input[placeholder*='Tìm']");
        await Assertions.Expect(searchInput).ToBeVisibleAsync();

        var fontSize = await searchInput.EvaluateAsync<string>("el => window.getComputedStyle(el).fontSize");
        Assert.That(fontSize, Is.EqualTo("16px"), "Search input font size must be 16px on mobile to avoid iOS Safari auto-zoom.");
    }

    [Test]
    public async Task Words_Pagination_TouchTarget_IsAtLeast44px_OnMobileAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 390, Height = 844 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/words");

        var pageLinks = page.Locator(".pagination .page-link");
        var count = await pageLinks.CountAsync();
        if (count > 0)
        {
            var firstLink = pageLinks.First;
            var box = await firstLink.BoundingBoxAsync();
            Assert.That(box, Is.Not.Null);
            Assert.That(box!.Height, Is.GreaterThanOrEqualTo(44.0), "Pagination link height must be >= 44px.");
            Assert.That(box!.Width, Is.GreaterThanOrEqualTo(44.0), "Pagination link width must be >= 44px.");
        }
    }
}
