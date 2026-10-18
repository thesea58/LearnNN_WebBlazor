using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// End-to-end tests for Task 24.03: Mobile Bottom Navigation and study session visibility.
/// Verifies:
/// 1. Mobile bottom nav is completely hidden on PC (&gt;= 1024px), sidebar is visible.
/// 2. Mobile bottom nav is visible on mobile (&lt; 1024px) for non-study routes with 5 items and touch targets &gt;= 44px.
/// 3. Mobile bottom nav is hidden on study session routes (/study/flashcards, /study/match, /study/scramble, /study/quiz) and mobile exit button appears.
/// <para>VN: Kiểm thử tự động E2E cho Task 24.03: Thanh điều hướng MobileBottomNav và cơ chế ẩn/hiện phiên học.</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class MobileNavigationTests
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
    public async Task Desktop_HidesBottomNav_AndShowsSidebarAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1366, Height = 768 }
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/");

        // Verify sidebar is visible on PC
        var sidebar = page.Locator(".sidebar");
        await Assertions.Expect(sidebar).ToBeVisibleAsync();

        // Verify bottom nav is hidden on PC
        var bottomNav = page.Locator(".mobile-bottom-nav");
        var count = await bottomNav.CountAsync();
        if (count > 0)
        {
            await Assertions.Expect(bottomNav).ToBeHiddenAsync();
        }
    }

    [Test]
    public async Task Mobile_ShowsBottomNav_With5Items_AndHidesSidebarAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 390, Height = 844 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/");

        // Verify sidebar is hidden on Mobile
        var sidebar = page.Locator(".sidebar");
        await Assertions.Expect(sidebar).ToBeHiddenAsync();

        // Verify bottom nav is visible
        var bottomNav = page.Locator(".mobile-bottom-nav");
        await Assertions.Expect(bottomNav).ToBeVisibleAsync();

        // Verify 5 nav items exist
        var navItems = bottomNav.Locator(".bottom-nav-item");
        var itemCount = await navItems.CountAsync();
        Assert.That(itemCount, Is.EqualTo(5), "Mobile bottom nav must have exactly 5 items (Home, Study, Words, Topics, AI Inbox).");

        // Verify touch target height >= 44px
        var firstItem = navItems.First;
        var box = await firstItem.BoundingBoxAsync();
        Assert.That(box, Is.Not.Null);
        Assert.That(box!.Height, Is.GreaterThanOrEqualTo(44.0), "Touch target height must be >= 44px.");

        // Verify home item is active
        var activeItem = bottomNav.Locator(".bottom-nav-item.active");
        await Assertions.Expect(activeItem).ToBeVisibleAsync();
    }

    private static readonly string[] StudySessionRoutes =
    [
        "/study/flashcards",
        "/study/match",
        "/study/scramble",
        "/study/quiz"
    ];

    [TestCaseSource(nameof(StudySessionRoutes))]
    public async Task Mobile_StudySession_HidesBottomNav_AndShowsExitButtonAsync(string route)
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 390, Height = 844 },
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, route);

        // Verify bottom nav is hidden/removed in study sessions
        var bottomNav = page.Locator(".mobile-bottom-nav");
        var count = await bottomNav.CountAsync();
        if (count > 0)
        {
            await Assertions.Expect(bottomNav).ToBeHiddenAsync();
        }

        // Verify mobile exit button is visible in header
        var exitBtn = page.Locator("header.top-row a[href='/study']");
        await Assertions.Expect(exitBtn).ToBeVisibleAsync();
    }
}
