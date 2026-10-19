using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Automated tests for Task 24.05: Study mobile touch, tap-flip flashcard,
/// sticky SRS buttons, optional swipe toggle, and 360px viewport responsiveness.
/// <para>VN: Kiểm thử tự động Task 24.05: Flashcard cảm ứng, 4 nút SRS dính đáy, nút toggle vuốt và chống tràn 360px.</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class StudyMobileTests
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
    public async Task Flashcard_TapFlip_AndStickySrsButtons_OnMobileAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 375, Height = 667 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/study/flashcards");

        // Verify flashcard is present
        var flashcard = page.Locator("#flashcard-flip-target");
        await Assertions.Expect(flashcard).ToBeVisibleAsync();

        // Tap/click card to flip
        await flashcard.ClickAsync();
        await page.WaitForTimeoutAsync(500);

        // Verify flipped state class
        await Assertions.Expect(flashcard).ToHaveClassAsync(new System.Text.RegularExpressions.Regex("is-flipped"));

        // Verify 4 SRS rating buttons are visible
        var srsButtons = page.Locator(".srs-rating-btn");
        var count = await srsButtons.CountAsync();
        Assert.That(count, Is.EqualTo(4), "Must have 4 SRS rating buttons when flipped.");

        // Verify touch target height >= 44px
        var firstBtn = srsButtons.First;
        var box = await firstBtn.BoundingBoxAsync();
        Assert.That(box, Is.Not.Null);
        Assert.That(box!.Height, Is.GreaterThanOrEqualTo(44.0), "SRS button height must be >= 44px.");

        // Verify controls container sits naturally below flashcard (not sticky, no overlap)
        var controls = page.Locator(".flashcard-controls-container");
        var position = await controls.EvaluateAsync<string>("el => window.getComputedStyle(el).position");
        Assert.That(position, Is.EqualTo("static"), "Controls container must sit naturally below the card on mobile.");

        var cardBox = await flashcard.BoundingBoxAsync();
        var controlsBox = await controls.BoundingBoxAsync();
        Assert.That(cardBox, Is.Not.Null);
        Assert.That(controlsBox, Is.Not.Null);
        Assert.That(controlsBox!.Y, Is.GreaterThanOrEqualTo(cardBox!.Y + cardBox.Height - 2.0),
            "Controls container must sit below the flashcard without overlapping.");
    }

    [Test]
    public async Task Flashcard_SwipeToggle_DefaultsToOff_AndCanToggleAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 375, Height = 667 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/study/flashcards");

        // Verify swipe setting button defaults to OFF
        var swipeBtn = page.Locator("button[title*='Vuốt màn hình']");
        await Assertions.Expect(swipeBtn).ToBeVisibleAsync();
        var initialTitle = await swipeBtn.GetAttributeAsync("title");
        Assert.That(initialTitle, Does.Contain("TẮT"), "Swipe setting must default to OFF.");

        // Click to toggle ON
        await swipeBtn.ClickAsync();
        await page.WaitForTimeoutAsync(300);

        var toggledTitle = await swipeBtn.GetAttributeAsync("title");
        Assert.That(toggledTitle, Does.Contain("BẬT"), "Clicking swipe button must toggle it ON.");
    }

    private static readonly string[] StudyGameRoutes =
    [
        "/study/flashcards",
        "/study/match",
        "/study/scramble",
        "/study/quiz"
    ];

    [TestCaseSource(nameof(StudyGameRoutes))]
    public async Task StudyGames_NoHorizontalOverflow_At360pxViewportAsync(string route)
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 360, Height = 640 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, route);

        // Check horizontal overflow using standard script
        var overflowResult = await OverflowHelper.CheckOverflowAsync(page);
        Assert.That(overflowResult.IsOverflowing, Is.False,
            $"Horizontal overflow detected on {route} at 360px: {string.Join(", ", overflowResult.Offenders.Select(o => $"{o.Tag}.{o.Cls} (+{o.OverflowPx}px)"))}");
    }
}
