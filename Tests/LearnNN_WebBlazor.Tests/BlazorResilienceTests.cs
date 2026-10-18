using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;
using System.IO;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Automated tests for Task 24.10: Blazor Server Resilience (Mobile-friendly reconnect modal, auto-reconnect on visibility/online/focus, offline fallback, and circuit retention).
/// <para>VN: Kiểm thử tự động Task 24.10: Khả năng phục hồi Blazor Server (Modal reconnect mobile, tự reconnect khi đổi mạng/tab, trang offline fallback và bảo lưu circuit).</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class BlazorResilienceTests
{
    private IPlaywright _playwright = null!;
    private static readonly string WwwRootDir = Path.Combine(TestServerFixture.FindSolutionRoot(), "Source", "wwwroot");
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
    /// Verifies that offline.html exists, has viewport-fit=cover, and auto-reloads on network restoration.
    /// </summary>
    [Test]
    public void OfflineHtml_ExistsAndConfiguredProperly()
    {
        var offlinePath = Path.Combine(WwwRootDir, "offline.html");
        Assert.That(File.Exists(offlinePath), Is.True, "Source/wwwroot/offline.html must exist.");

        var content = File.ReadAllText(offlinePath);
        Assert.That(content, Does.Contain("viewport-fit=cover"), "offline.html must include viewport-fit=cover for notch support.");
        Assert.That(content, Does.Contain("addEventListener('online'"), "offline.html must automatically reload when the device comes back online.");
        Assert.That(content, Does.Contain("btn-retry"), "offline.html must include a retry button.");
    }

    /// <summary>
    /// Verifies that service-worker.js precaches offline.html and uses it as fallback for navigate mode.
    /// </summary>
    [Test]
    public void ServiceWorker_PrecachesAndFallsBackToOfflineHtml()
    {
        var swPath = Path.Combine(WwwRootDir, "service-worker.js");
        Assert.That(File.Exists(swPath), Is.True, "service-worker.js must exist.");

        var content = File.ReadAllText(swPath);
        Assert.That(content, Does.Contain("/offline.html"), "service-worker.js must precache /offline.html.");
        Assert.That(content, Does.Contain("caches.match('/offline.html')"), "service-worker.js must fallback to /offline.html on navigate failures.");
    }

    /// <summary>
    /// Verifies that Program.cs configures extended circuit retention to protect mobile sessions.
    /// </summary>
    [Test]
    public void Program_ConfiguresExtendedCircuitRetention()
    {
        var programPath = Path.Combine(SourceDir, "Program.cs");
        Assert.That(File.Exists(programPath), Is.True, "Program.cs must exist.");

        var content = File.ReadAllText(programPath);
        Assert.That(content, Does.Contain("DisconnectedCircuitRetentionPeriod"), "Program.cs must configure DisconnectedCircuitRetentionPeriod.");
        Assert.That(content, Does.Contain("DisconnectedCircuitMaxRetained"), "Program.cs must configure DisconnectedCircuitMaxRetained.");
    }

    /// <summary>
    /// Verifies that ReconnectModal.razor.js attaches auto-reconnect listeners on online, visibilitychange, and focus.
    /// </summary>
    [Test]
    public void ReconnectModal_JsHasAutoReconnectListeners()
    {
        var jsPath = Path.Combine(SourceDir, "Components", "Layout", "ReconnectModal.razor.js");
        Assert.That(File.Exists(jsPath), Is.True, "ReconnectModal.razor.js must exist.");

        var content = File.ReadAllText(jsPath);
        Assert.That(content, Does.Contain("window.addEventListener(\"online\""), "ReconnectModal.razor.js must listen for online event.");
        Assert.That(content, Does.Contain("document.addEventListener(\"visibilitychange\""), "ReconnectModal.razor.js must listen for visibilitychange event.");
        Assert.That(content, Does.Contain("window.addEventListener(\"focus\""), "ReconnectModal.razor.js must listen for focus event.");
        Assert.That(content, Does.Contain("isRetrying"), "ReconnectModal.razor.js must have retry concurrency guard.");
    }

    /// <summary>
    /// Verifies in Playwright that ReconnectModal exists in the DOM and retry button meets touch target requirements (>= 44px).
    /// </summary>
    [Test]
    public async Task ReconnectModal_DOMAndTouchTargetAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new()
        {
            ViewportSize = new() { Width = 390, Height = 844 } // iPhone 14 mobile viewport
        });
        var page = await context.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/");

        // Verify dialog element exists in DOM
        var modalDialog = page.Locator("#components-reconnect-modal");
        await Assertions.Expect(modalDialog).ToHaveCountAsync(1);

        // Verify retry button exists in DOM
        var retryButton = page.Locator("#components-reconnect-button");
        await Assertions.Expect(retryButton).ToHaveCountAsync(1);

        // Verify retry button CSS min-height is at least 44px
        var minHeight = await retryButton.EvaluateAsync<string>("el => getComputedStyle(el).minHeight");
        Assert.That(minHeight, Does.Contain("44px"), "Retry button must have min-height: 44px for touch ergonomics.");

        // Verify modal container responsive width
        var modalCssWidth = await modalDialog.EvaluateAsync<string>("el => getComputedStyle(el).width");
        Assert.That(modalCssWidth, Is.Not.Null.And.Not.Empty);
    }
}
