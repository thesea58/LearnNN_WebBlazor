using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;
using System.IO;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Automated tests for Task 24.12: Mobile Accessibility (HTML lang="vi", focus-visible outline, aria-labels for icon buttons, screen reader classes).
/// <para>VN: Kiểm thử tự động Task 24.12: Trợ năng mobile (Thẻ HTML lang="vi", chỉ báo focus-visible, aria-label cho các nút icon, class screen reader).</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class MobileAccessibilityTests
{
    private IPlaywright _playwright = null!;
    private static readonly string SourceDir = Path.Combine(TestServerFixture.FindSolutionRoot(), "Source");

    [OneTimeSetUp]
    public async Task InitAsync()
    {
        _playwright = await Playwright.CreateAsync();
    }

    [OneTimeTearDown]
    public void DisposePlaywright()
    {
        _playwright.Dispose();
    }

    /// <summary>
    /// Verifies that App.razor specifies lang="vi" on the root html element.
    /// </summary>
    [Test]
    public void AppRazor_SpecifiesLangVi()
    {
        var appRazorPath = Path.Combine(SourceDir, "Components", "App.razor");
        Assert.That(File.Exists(appRazorPath), Is.True, "App.razor must exist.");

        var content = File.ReadAllText(appRazorPath);
        Assert.That(content, Does.Contain("<html lang=\"vi\">"), "App.razor must declare <html lang=\"vi\"> for Vietnamese accessibility.");
    }

    /// <summary>
    /// Verifies that app.css defines :focus-visible outlines and screen reader utility classes (.sr-only / .visually-hidden).
    /// </summary>
    [Test]
    public void AppCss_HasFocusVisibleAndScreenReaderClasses()
    {
        var appCssPath = Path.Combine(SourceDir, "wwwroot", "app.css");
        Assert.That(File.Exists(appCssPath), Is.True, "app.css must exist.");

        var content = File.ReadAllText(appCssPath);
        Assert.That(content, Does.Contain(":focus-visible"), "app.css must define :focus-visible rules.");
        Assert.That(content, Does.Contain("outline: 2px solid"), "app.css focus-visible must provide clear outline.");
        Assert.That(content, Does.Contain(".visually-hidden").Or.Contain(".sr-only"), "app.css must define screen reader utility classes.");
    }

    /// <summary>
    /// Verifies that Razor components define aria-label attributes for icon-only and interactive elements.
    /// </summary>
    [Test]
    public void RazorComponents_ContainAriaLabelsForAccessibility()
    {
        var sharedDir = Path.Combine(SourceDir, "Components", "Shared");
        var pagesDir = Path.Combine(SourceDir, "Components", "Pages");
        var layoutDir = Path.Combine(SourceDir, "Components", "Layout");

        // ViewModeSwitcher
        var switcherPath = Path.Combine(sharedDir, "ViewModeSwitcher.razor");
        var switcherContent = File.ReadAllText(switcherPath);
        Assert.That(switcherContent, Does.Contain("aria-label=\"Kiểu dòng (Bảng chi tiết)\""));
        Assert.That(switcherContent, Does.Contain("aria-label=\"Kiểu thẻ icon (Trực quan)\""));

        // WordList
        var wordListPath = Path.Combine(pagesDir, "Words", "WordList.razor");
        var wordListContent = File.ReadAllText(wordListPath);
        Assert.That(wordListContent, Does.Contain("aria-label=\"Xóa tìm kiếm\""));
        Assert.That(wordListContent, Does.Contain("aria-label=\"Đặt lại tất cả bộ lọc\""));
        Assert.That(wordListContent, Does.Contain("aria-label=\"Import từ vựng CSV\""));

        // TopicManage
        var topicManagePath = Path.Combine(pagesDir, "Topics", "TopicManage.razor");
        var topicManageContent = File.ReadAllText(topicManagePath);
        Assert.That(topicManageContent, Does.Contain("aria-label=\"Đóng\""));

        // Modals
        var deleteModalContent = File.ReadAllText(Path.Combine(sharedDir, "ConfirmDeleteModal.razor"));
        Assert.That(deleteModalContent, Does.Contain("aria-label=\"Đóng\""));

        var wordFormModalContent = File.ReadAllText(Path.Combine(pagesDir, "Words", "WordFormModal.razor"));
        Assert.That(wordFormModalContent, Does.Contain("aria-label=\"Đóng\""));

        var aiBridgeModalContent = File.ReadAllText(Path.Combine(sharedDir, "AiManualBridgeModal.razor"));
        Assert.That(aiBridgeModalContent, Does.Contain("aria-label=\"Đóng\""));

        // NavMenu & MobileBottomNav
        var navMenuContent = File.ReadAllText(Path.Combine(layoutDir, "NavMenu.razor"));
        Assert.That(navMenuContent, Does.Contain("aria-label=\"Thu gọn hoặc mở rộng menu điều hướng\""));

        var bottomNavContent = File.ReadAllText(Path.Combine(layoutDir, "MobileBottomNav.razor"));
        Assert.That(bottomNavContent, Does.Contain("aria-label=\"Điều hướng chính trên di động\""));
    }

    /// <summary>
    /// Navigates live mobile browser across main routes to check lang attribute and button accessibility.
    /// </summary>
    [Test]
    public async Task LiveBrowser_AccessibilityAuditOnMobileAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new()
        {
            ViewportSize = new() { Width = 360, Height = 780 },
            IsMobile = true,
            HasTouch = true
        });

        var page = await context.NewPageAsync();

        string[] routes = ["/", "/words", "/topics", "/study"];
        foreach (var route in routes)
        {
            await page.GotoAsync($"{TestServerFixture.BaseUrl}{route}", new() { WaitUntil = WaitUntilState.NetworkIdle });

            // 1. Check html lang attribute
            var langAttr = await page.Locator("html").GetAttributeAsync("lang");
            Assert.That(langAttr, Is.EqualTo("vi"), $"Route '{route}' must have <html lang=\"vi\">.");

            // 2. Audit all visible buttons for accessible name (text, aria-label, or title)
            var buttons = await page.Locator("button:visible, a.btn:visible").AllAsync();
            foreach (var btn in buttons)
            {
                var text = (await btn.InnerTextAsync()).Trim();
                var ariaLabel = (await btn.GetAttributeAsync("aria-label") ?? string.Empty).Trim();
                var title = (await btn.GetAttributeAsync("title") ?? string.Empty).Trim();

                bool hasAccessibleName = !string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(ariaLabel) || !string.IsNullOrEmpty(title);
                Assert.That(hasAccessibleName, Is.True, $"Visible button on route '{route}' must have an accessible name (text, aria-label, or title).");
            }
        }
    }
}
