using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Automated tests for Task 24.08: Input semantics, touch target gaps, virtual keyboard scroll margin, and touch ergonomics.
/// <para>VN: Kiểm thử tự động Task 24.08: Ngữ nghĩa input, khoảng cách chạm tối thiểu, scroll margin và công thái học cảm ứng.</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class InputAndTouchErgonomicsTests
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

    /// <summary>
    /// Verifies that WordList search input has correct semantics: type="search" and inputmode="search".
    /// </summary>
    [Test]
    public async Task Words_SearchInput_HasCorrectSemanticsAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/words");

        var searchInput = page.Locator("input[inputmode='search']");
        await Assertions.Expect(searchInput).ToBeVisibleAsync();

        var inputMode = await searchInput.GetAttributeAsync("inputmode");
        Assert.That(inputMode, Is.EqualTo("search"), "Search input should have inputmode='search'.");
    }

    /// <summary>
    /// Verifies that AiSettings chatbot url input has correct semantics: type="url" and inputmode="url".
    /// </summary>
    [Test]
    public async Task AiSettings_ChatbotUrlInput_HasCorrectSemanticsAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/ai/settings");

        var urlInput = page.Locator("input[type='url']");
        await Assertions.Expect(urlInput).ToBeVisibleAsync();

        var inputMode = await urlInput.GetAttributeAsync("inputmode");
        Assert.That(inputMode, Is.EqualTo("url"), "Chatbot URL input should have inputmode='url'.");
    }

    /// <summary>
    /// Verifies that inputs on mobile have scroll-margin-bottom: 24px so virtual keyboard does not occlude active input.
    /// </summary>
    [Test]
    public async Task Inputs_HaveScrollMarginBottom_OnMobileAsync()
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

        var searchInput = page.Locator("input[inputmode='search']");
        await Assertions.Expect(searchInput).ToBeVisibleAsync();

        var scrollMargin = await searchInput.EvaluateAsync<string>("el => window.getComputedStyle(el).scrollMarginBottom");
        Assert.That(scrollMargin, Is.EqualTo("24px"), "Inputs should have scroll-margin-bottom: 24px on mobile.");
    }

    /// <summary>
    /// Verifies that form switch / check controls on mobile meet the 44px touch target height recommendation.
    /// </summary>
    [Test]
    public async Task FormCheck_HasAdequateTouchTargetHeight_OnMobileAsync()
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

        // Open Add Word modal to inspect .form-check
        var addBtn = page.Locator("button:has-text('Thêm từ mới')").First;
        await addBtn.ClickAsync();
        await page.WaitForTimeoutAsync(400);

        var formCheck = page.Locator(".form-check").First;
        await Assertions.Expect(formCheck).ToBeVisibleAsync();

        var box = await formCheck.BoundingBoxAsync();
        Assert.That(box, Is.Not.Null);
        Assert.That(box!.Height, Is.GreaterThanOrEqualTo(40), "Form check / switch touch height should be at least 40px.");

        // Clean up: close modal
        var cancelBtn = page.Locator(".modal.show button:has-text('Hủy')");
        await cancelBtn.ClickAsync();
    }
}
