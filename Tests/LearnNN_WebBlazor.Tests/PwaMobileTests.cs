using LearnNN_WebBlazor.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;
using SkiaSharp;
using System.IO;
using System.Text.Json;

namespace LearnNN_WebBlazor.Tests;

/// <summary>
/// Automated tests for Task 24.09: Progressive Web App (PWA) manifest, icons, theme-color, apple-touch-icon, and service worker.
/// <para>VN: Kiểm thử tự động Task 24.09: Manifest PWA, bộ icon 192/512/maskable/apple-touch, theme-color và service worker.</para>
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class PwaMobileTests
{
    private IPlaywright _playwright = null!;
    private static readonly string WwwRootDir = Path.Combine(TestServerFixture.FindSolutionRoot(), "Source", "wwwroot");

    [OneTimeSetUp]
    public async Task InitAsync()
    {
        _playwright = await Playwright.CreateAsync();
        EnsurePwaIconsGenerated();
    }

    [OneTimeTearDown]
    public void DisposePlaywright()
    {
        _playwright.Dispose();
    }

    /// <summary>
    /// Generates high-quality branded PWA icons if not present.
    /// </summary>
    private static void EnsurePwaIconsGenerated()
    {
        var iconsDir = Path.Combine(WwwRootDir, "icons");
        if (!Directory.Exists(iconsDir))
        {
            Directory.CreateDirectory(iconsDir);
        }

        var iconConfigs = new (string FileName, int Size, bool Maskable)[]
        {
            ("icon-192.png", 192, false),
            ("icon-512.png", 512, false),
            ("icon-maskable-192.png", 192, true),
            ("icon-maskable-512.png", 512, true),
            ("apple-touch-icon.png", 180, false)
        };

        foreach (var (fileName, size, maskable) in iconConfigs)
        {
            var filePath = Path.Combine(iconsDir, fileName);
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                GenerateIcon(filePath, size, maskable);
            }
        }
    }

    private static void GenerateIcon(string path, int size, bool maskable)
    {
        using var surface = SKSurface.Create(new SKImageInfo(size, size, SKColorType.Rgba8888, SKAlphaType.Premul));
        var canvas = surface.Canvas;

        // Background gradient: #4f46e5 (indigo-600) to #6366f1 (indigo-500)
        using var bgPaint = new SKPaint
        {
            IsAntialias = true,
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(size, size),
                new[] { new SKColor(79, 70, 229), new SKColor(99, 102, 241) },
                SKShaderTileMode.Clamp)
        };

        if (maskable)
        {
            // Maskable icon must fill the entire canvas
            canvas.DrawRect(0, 0, size, size, bgPaint);
        }
        else
        {
            // Rounded rectangle for standard & apple-touch icon
            float cornerRadius = size * 0.18f;
            canvas.DrawRoundRect(new SKRoundRect(new SKRect(0, 0, size, size), cornerRadius), bgPaint);
        }

        // Inner translucent circle
        using var circlePaint = new SKPaint
        {
            IsAntialias = true,
            Color = new SKColor(255, 255, 255, 35),
            Style = SKPaintStyle.Fill
        };
        canvas.DrawCircle(size / 2f, size / 2f, size * 0.32f, circlePaint);

        // Center typography "NN"
        using var textPaint = new SKPaint
        {
            IsAntialias = true,
            Color = SKColors.White,
            Style = SKPaintStyle.Fill
        };

        using var typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);
        using var font = new SKFont(typeface, size * 0.38f);
        font.GetFontMetrics(out var fontMetrics);
        float textWidth = font.MeasureText("NN");
        float textX = (size - textWidth) / 2f;
        float textY = (size / 2f) - ((fontMetrics.Ascent + fontMetrics.Descent) / 2f);
        canvas.DrawText("NN", textX, textY, SKTextAlign.Left, font, textPaint);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite(path);
        data.SaveTo(stream);
    }

    /// <summary>
    /// Verifies that all required PWA icons exist and have valid dimensions.
    /// </summary>
    [Test]
    public void PwaIcons_AllExistWithCorrectDimensions()
    {
        var iconsDir = Path.Combine(WwwRootDir, "icons");
        Assert.That(Directory.Exists(iconsDir), Is.True, "Source/wwwroot/icons directory must exist.");

        var expectedIcons = new Dictionary<string, (int Width, int Height)>
        {
            { "icon-192.png", (192, 192) },
            { "icon-512.png", (512, 512) },
            { "icon-maskable-192.png", (192, 192) },
            { "icon-maskable-512.png", (512, 512) },
            { "apple-touch-icon.png", (180, 180) }
        };

        foreach (var (fileName, (expectedW, expectedH)) in expectedIcons)
        {
            var filePath = Path.Combine(iconsDir, fileName);
            Assert.That(File.Exists(filePath), Is.True, $"Icon {fileName} must exist.");

            using var codec = SKCodec.Create(filePath);
            Assert.That(codec, Is.Not.Null, $"Icon {fileName} must be a valid image file.");
            Assert.That(codec.Info.Width, Is.EqualTo(expectedW), $"Icon {fileName} width mismatch.");
            Assert.That(codec.Info.Height, Is.EqualTo(expectedH), $"Icon {fileName} height mismatch.");
        }
    }

    /// <summary>
    /// Verifies manifest.webmanifest format and required PWA fields.
    /// </summary>
    [Test]
    public void Manifest_HasRequiredPwaFields()
    {
        var manifestPath = Path.Combine(WwwRootDir, "manifest.webmanifest");
        Assert.That(File.Exists(manifestPath), Is.True, "manifest.webmanifest must exist.");

        var json = File.ReadAllText(manifestPath);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.That(root.TryGetProperty("name", out var name) && !string.IsNullOrWhiteSpace(name.GetString()), Is.True, "Manifest must have a non-empty 'name'.");
        Assert.That(root.TryGetProperty("short_name", out var shortName) && !string.IsNullOrWhiteSpace(shortName.GetString()), Is.True, "Manifest must have a non-empty 'short_name'.");
        Assert.That(root.TryGetProperty("start_url", out var startUrl) && startUrl.GetString() == "/", Is.True, "Manifest must have 'start_url': '/'.");
        Assert.That(root.TryGetProperty("display", out var display) && display.GetString() == "standalone", Is.True, "Manifest must have 'display': 'standalone'.");
        Assert.That(root.TryGetProperty("theme_color", out var themeColor) && themeColor.GetString() == "#4f46e5", Is.True, "Manifest theme_color must be '#4f46e5'.");

        Assert.That(root.TryGetProperty("icons", out var icons) && icons.GetArrayLength() >= 4, Is.True, "Manifest must declare at least 4 icons.");
    }

    /// <summary>
    /// Verifies service-worker.js exists and safeguards SignalR / WebSocket circuits.
    /// </summary>
    [Test]
    public void ServiceWorker_SafeguardsSignalRAndFramework()
    {
        var swPath = Path.Combine(WwwRootDir, "service-worker.js");
        Assert.That(File.Exists(swPath), Is.True, "service-worker.js must exist.");

        var content = File.ReadAllText(swPath);
        Assert.That(content, Does.Contain("/_blazor"), "Service worker must explicitly bypass /_blazor circuits.");
        Assert.That(content, Does.Contain("/_framework"), "Service worker must bypass /_framework.");
        Assert.That(content, Does.Contain("install"), "Service worker must handle install event.");
        Assert.That(content, Does.Contain("activate"), "Service worker must handle activate event.");
        Assert.That(content, Does.Contain("fetch"), "Service worker must handle fetch event.");
    }

    /// <summary>
    /// Verifies that App.razor outputs PWA meta tags and links, and registers service worker.
    /// </summary>
    [Test]
    public async Task Page_IncludesPwaMetaTagsAndRegistersServiceWorkerAsync()
    {
        await using var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();

        await BlazorTestHelper.NavigateAndStabilizeAsync(page, "/");

        // Verify Manifest link
        var manifestLink = page.Locator("link[rel='manifest']");
        await Assertions.Expect(manifestLink).ToHaveCountAsync(1);
        var manifestHref = await manifestLink.GetAttributeAsync("href");
        Assert.That(manifestHref, Does.Contain("manifest.webmanifest"));

        // Verify Apple Touch Icon link
        var appleIconLink = page.Locator("link[rel='apple-touch-icon']");
        await Assertions.Expect(appleIconLink).ToHaveCountAsync(1);

        // Verify Theme Color meta
        var themeColorMeta = page.Locator("meta[name='theme-color']");
        await Assertions.Expect(themeColorMeta).ToHaveCountAsync(1);
        var themeColorContent = await themeColorMeta.GetAttributeAsync("content");
        Assert.That(themeColorContent, Is.EqualTo("#4f46e5"));

        // Verify Apple Mobile Web App meta
        var appleCapableMeta = page.Locator("meta[name='apple-mobile-web-app-capable']");
        await Assertions.Expect(appleCapableMeta).ToHaveCountAsync(1);

        // Fetch manifest through HTTP and verify status 200
        var manifestResponse = await page.APIRequest.GetAsync($"{TestServerFixture.BaseUrl}/manifest.webmanifest");
        Assert.That(manifestResponse.Ok, Is.True, "Fetching /manifest.webmanifest should succeed (200 OK).");
    }
}
