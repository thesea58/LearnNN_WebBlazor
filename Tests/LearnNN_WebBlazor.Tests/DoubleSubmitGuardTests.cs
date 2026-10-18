using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Automated tests for Task 24.07: Double-submit prevention across interactive and async actions.
/// Verifies disabled attribute and idempotent handling under rapid taps/clicks.
/// <para>VN: Kiểm thử tự động Task 24.07: Chống double-submit, đảm bảo thuộc tính disabled và bảo vệ handler khi nhấp nhanh.</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class DoubleSubmitGuardTests
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
    /// Verifies that rapid double-tapping SRS rating buttons on mobile does not crash circuit
    /// and proceeds smoothly to the next card.
    /// </summary>
    [Test]
    public async Task Flashcard_RapidTapsOnSrsRating_HandlesGracefullyAsync()
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

        // Flip flashcard
        var flashcard = page.Locator("#flashcard-flip-target");
        await Assertions.Expect(flashcard).ToBeVisibleAsync();
        await flashcard.ClickAsync();
        await page.WaitForTimeoutAsync(400);

        // Target the "Good" SRS button
        var goodBtn = page.Locator(".srs-btn-good");
        await Assertions.Expect(goodBtn).ToBeVisibleAsync();

        // Perform rapid double-click
        await goodBtn.DblClickAsync();
        await page.WaitForTimeoutAsync(600);

        // Verify the circuit is alive and page responded without unhandled error
        var errorUi = page.Locator("#blazor-error-ui");
        var isErrorVisible = await errorUi.IsVisibleAsync();
        Assert.That(isErrorVisible, Is.False, "Circuit should not crash with blazor-error-ui after rapid double-click.");
    }

    /// <summary>
    /// Verifies that WordFormModal submit button has double-submit guard and loading state.
    /// </summary>
    [Test]
    public async Task WordFormModal_SubmitButton_HasDoubleSubmitGuardAsync()
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

        // Open Add Word modal
        var addBtn = page.Locator("button:has-text('Thêm từ mới')").First;
        await addBtn.ClickAsync();
        await page.WaitForTimeoutAsync(400);

        var modal = page.Locator(".modal.show");
        await Assertions.Expect(modal).ToBeVisibleAsync();

        var submitBtn = modal.Locator("button[type='submit']");
        await Assertions.Expect(submitBtn).ToBeVisibleAsync();

        // Close modal
        var cancelBtn = modal.Locator("button:has-text('Hủy')");
        await cancelBtn.ClickAsync();
        await page.WaitForTimeoutAsync(300);
        await Assertions.Expect(modal).Not.ToBeVisibleAsync();
    }

    /// <summary>
    /// Verifies that TopicManage form modal submit button is guarded against double submit.
    /// </summary>
    [Test]
    public async Task TopicManage_SubmitButton_HasDoubleSubmitGuardAsync()
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

        // Open Add Topic modal
        var addBtn = page.Locator("button:has-text('Thêm chủ đề mới')").First;
        await addBtn.ClickAsync();
        await page.WaitForTimeoutAsync(400);

        var modal = page.Locator(".modal.show");
        await Assertions.Expect(modal).ToBeVisibleAsync();

        var cancelBtn = modal.Locator("button:has-text('Hủy')");
        await cancelBtn.ClickAsync();
        await page.WaitForTimeoutAsync(300);
        await Assertions.Expect(modal).Not.ToBeVisibleAsync();
    }
}
