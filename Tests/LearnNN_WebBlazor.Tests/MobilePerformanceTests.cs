using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;
using System.IO;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Automated tests for Task 24.11: Mobile Performance (Brotli/Gzip compression, static asset cache headers, LCP < 2.5s, CLS < 0.1 on core mobile routes).
/// <para>VN: Kiểm thử tự động Task 24.11: Hiệu năng mobile (Nén Brotli/Gzip, Cache header tài nguyên tĩnh, LCP < 2.5s, CLS < 0.1 trên các route chính).</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class MobilePerformanceTests
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
    /// Verifies that Program.cs registers ResponseCompression with Brotli and Gzip providers.
    /// </summary>
    [Test]
    public void Program_RegistersBrotliAndGzipCompression()
    {
        var programPath = Path.Combine(SourceDir, "Program.cs");
        Assert.That(File.Exists(programPath), Is.True, "Program.cs must exist.");

        var content = File.ReadAllText(programPath);
        Assert.That(content, Does.Contain("AddResponseCompression"), "Program.cs must register AddResponseCompression.");
        Assert.That(content, Does.Contain("BrotliCompressionProvider"), "Program.cs must add BrotliCompressionProvider.");
        Assert.That(content, Does.Contain("GzipCompressionProvider"), "Program.cs must add GzipCompressionProvider.");
        Assert.That(content, Does.Contain("UseResponseCompression"), "Program.cs must call UseResponseCompression in pipeline.");
    }

    /// <summary>
    /// Verifies that static assets have immutable/public cache headers while service-worker.js has no-cache.
    /// </summary>
    [Test]
    public async Task StaticAssets_HaveAppropriateCacheControlHeadersAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();

        // Check service-worker.js header: must not be cached aggressively so updates are immediate
        var swResponse = await page.APIRequest.GetAsync($"{TestServerFixture.BaseUrl}/service-worker.js");
        Assert.That(swResponse.Ok, Is.True, "Fetching /service-worker.js should succeed.");
        var swCacheControl = swResponse.Headers.TryGetValue("cache-control", out var swCc) ? swCc : string.Empty;
        Assert.That(swCacheControl, Does.Contain("no-cache").Or.Contain("no-store"), "service-worker.js should have no-cache / no-store header.");

        // Check icon or favicon header: should be cached
        var iconResponse = await page.APIRequest.GetAsync($"{TestServerFixture.BaseUrl}/icons/icon-192.png");
        Assert.That(iconResponse.Ok, Is.True, "Fetching /icons/icon-192.png should succeed.");
        var iconCacheControl = iconResponse.Headers.TryGetValue("cache-control", out var iconCc) ? iconCc : string.Empty;
        Assert.That(iconCacheControl, Does.Contain("public").And.Contain("max-age"), "Static icons should have public caching headers.");
    }

    /// <summary>
    /// Measures Cumulative Layout Shift (CLS < 0.1) and Largest Contentful Paint (LCP < 2.5s) on Home ("/").
    /// </summary>
    [Test]
    public async Task Route_Home_MobileWebVitalsWithinBudgetsAsync()
    {
        await AssertMobileWebVitalsAsync("/");
    }

    /// <summary>
    /// Measures Cumulative Layout Shift (CLS < 0.1) and Largest Contentful Paint (LCP < 2.5s) on Words ("/words").
    /// </summary>
    [Test]
    public async Task Route_Words_MobileWebVitalsWithinBudgetsAsync()
    {
        await AssertMobileWebVitalsAsync("/words");
    }

    /// <summary>
    /// Measures Cumulative Layout Shift (CLS < 0.1) and Largest Contentful Paint (LCP < 2.5s) on Study Hub ("/study").
    /// </summary>
    [Test]
    public async Task Route_Study_MobileWebVitalsWithinBudgetsAsync()
    {
        await AssertMobileWebVitalsAsync("/study");
    }

    private async Task AssertMobileWebVitalsAsync(string route)
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var context = await browser.NewContextAsync(new()
        {
            ViewportSize = new() { Width = 390, Height = 844 }, // Mobile iPhone 14
            IsMobile = true,
            HasTouch = true
        });
        var page = await context.NewPageAsync();

        // Inject performance observer before navigation
        await page.AddInitScriptAsync(@"
            window.__clsScore = 0;
            try {
                const po = new PerformanceObserver((entryList) => {
                    for (const entry of entryList.getEntries()) {
                        if (!entry.hadRecentInput) {
                            window.__clsScore += entry.value;
                        }
                    }
                });
                po.observe({ type: 'layout-shift', buffered: true });
            } catch (e) {}
        ");

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, route);

        // Evaluate CLS score
        var cls = await page.EvaluateAsync<double>("() => window.__clsScore || 0");
        Assert.That(cls, Is.LessThan(0.1), $"Route {route} CLS score ({cls:F4}) exceeded budget of 0.1.");

        // Evaluate LCP or navigation timing
        var lcp = await page.EvaluateAsync<double>(@"() => {
            const entries = performance.getEntriesByType('largest-contentful-paint');
            if (entries && entries.length > 0) {
                return entries[entries.length - 1].startTime;
            }
            const nav = performance.getEntriesByType('navigation')[0];
            return nav ? nav.domContentLoadedEventEnd : 0;
        }");

        Assert.That(lcp, Is.LessThan(2500), $"Route {route} LCP / Load time ({lcp:F0}ms) exceeded budget of 2500ms.");
    }
}
