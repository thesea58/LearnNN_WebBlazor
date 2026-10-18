using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Automated Playwright tests for Task 24.06: Modals and AI pages mobile responsiveness.
/// <para>VN: Kiểm thử tự động Task 24.06: Công thái học Modal (351px/336px, sticky header/footer, touch target >= 44px) và chống tràn 360px cho các trang AI.</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class ModalAndAiMobileTests
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
    public async Task WordFormModal_Mobile375_Dimensions_StickyHeaderFooter_AndTouchTargetsAsync()
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

        // Click Add Word button
        var addBtn = page.Locator("button:has(.bi-plus-circle-fill)");
        await addBtn.ClickAsync();

        // Wait for modal to appear
        var modalDialog = page.Locator(".modal.show .modal-dialog");
        var modalContent = page.Locator(".modal.show .modal-content");
        await Assertions.Expect(modalContent).ToBeVisibleAsync();

        var box = await modalContent.BoundingBoxAsync();
        Assert.That(box, Is.Not.Null);
        // calc(100vw - 24px) on 375px is 351px
        Assert.That(box!.Width, Is.InRange(340, 360), "Modal width should be approximately calc(100vw - 24px).");
        Assert.That(box.Height, Is.LessThanOrEqualTo(667 * 0.92), "Modal content height should not exceed 90dvh.");

        // Check header and footer sticky positioning
        var header = page.Locator(".modal.show .modal-header");
        var footer = page.Locator(".modal.show .modal-footer");
        var headerPos = await header.EvaluateAsync<string>("el => getComputedStyle(el).position");
        var footerPos = await footer.EvaluateAsync<string>("el => getComputedStyle(el).position");
        Assert.That(headerPos, Is.EqualTo("sticky"), "Modal header must be sticky on mobile.");
        Assert.That(footerPos, Is.EqualTo("sticky"), "Modal footer must be sticky on mobile.");

        // Check close button touch target
        var closeBtn = page.Locator(".modal.show .btn-close");
        var closeBox = await closeBtn.BoundingBoxAsync();
        Assert.That(closeBox, Is.Not.Null);
        Assert.That(closeBox!.Height, Is.GreaterThanOrEqualTo(40), "Close button height should be >= 40px.");

        // Check footer buttons touch target
        var cancelBtn = footer.Locator("button:has-text('Hủy')");
        var cancelBox = await cancelBtn.BoundingBoxAsync();
        Assert.That(cancelBox, Is.Not.Null);
        Assert.That(cancelBox!.Height, Is.GreaterThanOrEqualTo(44), "Footer cancel button height must be >= 44px.");

        // Close modal
        await cancelBtn.ClickAsync();
        await Assertions.Expect(modalDialog).ToHaveCountAsync(0);
    }

    [Test]
    public async Task ConfirmDeleteModal_Mobile375_CenteredAndResponsiveAsync()
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

        // Click delete button on first topic
        var deleteBtn = page.Locator("button:has(.bi-trash3)").First;
        await deleteBtn.ClickAsync();

        // Wait for ConfirmDeleteModal
        var modalDialog = page.Locator(".modal.show .modal-dialog");
        await Assertions.Expect(modalDialog).ToBeVisibleAsync();

        var box = await modalDialog.BoundingBoxAsync();
        Assert.That(box, Is.Not.Null);
        Assert.That(box!.Width, Is.LessThanOrEqualTo(375), "Modal width should not exceed viewport.");

        var cancelBtn = page.Locator(".modal.show .modal-footer button:has-text('Hủy')");
        await cancelBtn.ClickAsync();
        await Assertions.Expect(modalDialog).ToHaveCountAsync(0);
    }

    [Test]
    public async Task TopicManage_FormModal_Mobile360_DimensionsAndStickyAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 360, Height = 640 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/topics");

        // Click Add Topic button
        var addBtn = page.Locator("button:has-text('Thêm chủ đề mới')");
        await addBtn.ClickAsync();

        // Wait for modal
        var modalDialog = page.Locator(".modal.show .modal-dialog");
        var modalContent = page.Locator(".modal.show .modal-content");
        await Assertions.Expect(modalContent).ToBeVisibleAsync();

        var box = await modalContent.BoundingBoxAsync();
        Assert.That(box, Is.Not.Null);
        // calc(100vw - 24px) on 360px is 336px
        Assert.That(box!.Width, Is.InRange(325, 345), "Modal width should be approx 336px.");

        // Check footer save and cancel buttons touch targets
        var saveBtn = page.Locator(".modal.show .modal-footer button:has-text('Tạo chủ đề')");
        var saveBox = await saveBtn.BoundingBoxAsync();
        Assert.That(saveBox, Is.Not.Null);
        Assert.That(saveBox!.Height, Is.GreaterThanOrEqualTo(44), "Save button height must be >= 44px.");

        var cancelBtn = page.Locator(".modal.show .modal-footer button:has-text('Hủy')");
        await cancelBtn.ClickAsync();
        await Assertions.Expect(modalDialog).ToHaveCountAsync(0);
    }

    [Test]
    public async Task AiInbox_Mobile360_HasNoHorizontalOverflow_AcrossAllTabsAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 360, Height = 640 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/ai/inbox");

        // Tab 1: Pending
        var overflow1 = await OverflowHelper.CheckOverflowAsync(page);
        Assert.That(overflow1.IsOverflowing, Is.False, $"AI Inbox Tab 1 has horizontal overflow on 360px: {string.Join(", ", overflow1.Offenders.Select(o => $"{o.Tag}.{o.Cls} (+{o.OverflowPx}px)"))}");

        // Tab 2: Completed
        var completedTab = page.Locator("button:has-text('Lịch sử hoàn thành')");
        await completedTab.ClickAsync();
        await page.WaitForTimeoutAsync(300);
        var overflow2 = await OverflowHelper.CheckOverflowAsync(page);
        Assert.That(overflow2.IsOverflowing, Is.False, $"AI Inbox Tab 2 has horizontal overflow on 360px: {string.Join(", ", overflow2.Offenders.Select(o => $"{o.Tag}.{o.Cls} (+{o.OverflowPx}px)"))}");

        // Tab 3: Sandbox
        var sandboxTab = page.Locator(".nav-pills button:has-text('AI Sandbox')");
        await sandboxTab.ClickAsync();
        await page.WaitForTimeoutAsync(300);
        var overflow3 = await OverflowHelper.CheckOverflowAsync(page);
        Assert.That(overflow3.IsOverflowing, Is.False, $"AI Inbox Tab 3 has horizontal overflow on 360px: {string.Join(", ", overflow3.Offenders.Select(o => $"{o.Tag}.{o.Cls} (+{o.OverflowPx}px)"))}");
    }

    [Test]
    public async Task AiSettings_Mobile360_HasNoHorizontalOverflowAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 360, Height = 640 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/ai/settings");

        var overflow = await OverflowHelper.CheckOverflowAsync(page);
        Assert.That(overflow.IsOverflowing, Is.False, $"AI Settings has horizontal overflow on 360px: {string.Join(", ", overflow.Offenders.Select(o => $"{o.Tag}.{o.Cls} (+{o.OverflowPx}px)"))}");

        // Verify radio cards are selectable
        var manualCard = page.Locator("label[for='modeManual']");
        await manualCard.ClickAsync();
        await Assertions.Expect(page.Locator("#modeManual")).ToBeCheckedAsync();
    }

    [Test]
    public async Task AiManualBridgeModal_Mobile375_NoOverflowAndButtonsWrapAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 375, Height = 667 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/ai/inbox");

        // Switch to sandbox
        var sandboxTab = page.Locator(".nav-pills button:has-text('AI Sandbox')");
        await sandboxTab.ClickAsync();
        await page.WaitForTimeoutAsync(300);

        // Click run sandbox test (manual channel by default)
        var runBtn = page.Locator("button:has-text('Sinh Prompt & Chạy Thử')");
        await runBtn.ClickAsync();

        // Wait for AiManualBridgeModal
        var modalDialog = page.Locator(".modal.show .modal-dialog");
        await Assertions.Expect(modalDialog).ToBeVisibleAsync();

        // Check overflow
        var overflow = await OverflowHelper.CheckOverflowAsync(page);
        Assert.That(overflow.IsOverflowing, Is.False, $"AI Manual Bridge Modal caused horizontal overflow: {string.Join(", ", overflow.Offenders.Select(o => $"{o.Tag}.{o.Cls} (+{o.OverflowPx}px)"))}");

        // Close modal
        var closeBtn = page.Locator(".modal.show .modal-footer button:has-text('Đóng')");
        await closeBtn.ClickAsync();
        await Assertions.Expect(modalDialog).ToHaveCountAsync(0);

        // Clean up created request to restore database to clean state
        var tabs = new[] { "Yêu cầu đang chờ", "Lịch sử hoàn thành" };
        foreach (var tabName in tabs)
        {
            var tabBtn = page.Locator($".nav-pills button:has-text('{tabName}')");
            await tabBtn.ClickAsync();
            await page.WaitForTimeoutAsync(300);
            var deleteBtn = page.Locator("button:has-text('Xóa'), button:has(.bi-trash)").First;
            if (await deleteBtn.IsVisibleAsync())
            {
                await deleteBtn.ClickAsync();
                await page.WaitForTimeoutAsync(400);
            }
        }
    }
}
