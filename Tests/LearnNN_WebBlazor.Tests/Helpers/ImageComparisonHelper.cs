using SkiaSharp;

namespace LearnNN_WebBlazor.Tests.Helpers;

/// <summary>
/// Pixel-by-pixel image comparison helper using SkiaSharp with configurable difference tolerance.
/// <para>VN: Tiện ích so sánh ảnh từng pixel sử dụng SkiaSharp với ngưỡng sai số tùy chỉnh.</para>
/// </summary>
public static class ImageComparisonHelper
{
    #region Nested Types

    /// <summary>
    /// Result record containing image comparison outcome and metrics.
    /// <para>VN: Bản ghi kết quả so sánh ảnh bao gồm trạng thái khớp, tỉ lệ phần trăm và chi tiết.</para>
    /// </summary>
    /// <param name="IsMatch">True if diff percentage is below or equal to the allowed threshold.</param>
    /// <param name="DiffPercentage">The percentage of differing pixels (0.0 to 100.0).</param>
    /// <param name="DiffPixels">Total count of differing pixels.</param>
    /// <param name="TotalPixels">Total count of image pixels.</param>
    /// <param name="Message">Descriptive message of the comparison result.</param>
    public record DiffResult(bool IsMatch, double DiffPercentage, int DiffPixels, int TotalPixels, string Message);

    #endregion

    #region Public Comparison Methods

    /// <summary>
    /// Compares an in-memory screenshot against a baseline image on disk.
    /// If baseline does not exist, it saves the screenshot as the new baseline.
    /// </summary>
    /// <param name="baselinePath">Absolute path to baseline PNG file.</param>
    /// <param name="actualBytes">In-memory PNG bytes of actual screenshot.</param>
    /// <param name="diffOutputPath">Optional path to write diff PNG if threshold is exceeded.</param>
    /// <param name="maxAllowedDiffPercent">Maximum permitted difference percentage (defaults to 0.1%).</param>
    /// <returns>A <see cref="DiffResult"/> evaluating the comparison.</returns>
    public static DiffResult Compare(
        string baselinePath,
        byte[] actualBytes,
        string? diffOutputPath = null,
        double maxAllowedDiffPercent = 0.1)
    {
        // A. Baseline initialization check
        if (!File.Exists(baselinePath))
        {
            var baselineDir = Path.GetDirectoryName(baselinePath);
            if (!string.IsNullOrEmpty(baselineDir) && !Directory.Exists(baselineDir))
            {
                Directory.CreateDirectory(baselineDir);
            }

            File.WriteAllBytes(baselinePath, actualBytes);
            return new DiffResult(true, 0.0, 0, 0, $"Baseline created at: {baselinePath}");
        }

        // B. Load baseline and actual bitmaps
        using var baselineBitmap = SKBitmap.Decode(baselinePath);
        using var actualBitmap = SKBitmap.Decode(actualBytes);

        if (baselineBitmap == null || actualBitmap == null)
        {
            return new DiffResult(false, 100.0, 0, 0, "Failed to decode PNG images for comparison.");
        }

        // C. Check dimension equality
        if (baselineBitmap.Width != actualBitmap.Width || baselineBitmap.Height != actualBitmap.Height)
        {
            return new DiffResult(
                false,
                100.0,
                baselineBitmap.Width * baselineBitmap.Height,
                baselineBitmap.Width * baselineBitmap.Height,
                $"Dimensions differ: Baseline ({baselineBitmap.Width}x{baselineBitmap.Height}) vs Actual ({actualBitmap.Width}x{actualBitmap.Height})"
            );
        }

        int width = baselineBitmap.Width;
        int height = baselineBitmap.Height;
        int totalPixels = width * height;
        int diffPixels = 0;

        using var diffBitmap = new SKBitmap(width, height);

        // D. Scan each pixel and calculate color delta
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var p1 = baselineBitmap.GetPixel(x, y);
                var p2 = actualBitmap.GetPixel(x, y);

                int dr = Math.Abs(p1.Red - p2.Red);
                int dg = Math.Abs(p1.Green - p2.Green);
                int db = Math.Abs(p1.Blue - p2.Blue);
                int da = Math.Abs(p1.Alpha - p2.Alpha);

                // Per-channel tolerance to handle minor font antialiasing / GPU subpixel jitter
                if (dr > 15 || dg > 15 || db > 15 || da > 15)
                {
                    diffPixels++;
                    // Highlight difference with vibrant red
                    diffBitmap.SetPixel(x, y, new SKColor(255, 0, 0, 255));
                }
                else
                {
                    // Dim unchanged context pixels
                    diffBitmap.SetPixel(x, y, new SKColor((byte)(p2.Red / 4), (byte)(p2.Green / 4), (byte)(p2.Blue / 4), 200));
                }
            }
        }

        // E. Compute percentage and determine pass/fail
        double diffPercent = (double)diffPixels / totalPixels * 100.0;
        bool isMatch = diffPercent <= maxAllowedDiffPercent;

        if (!isMatch && !string.IsNullOrEmpty(diffOutputPath))
        {
            var diffDir = Path.GetDirectoryName(diffOutputPath);
            if (!string.IsNullOrEmpty(diffDir) && !Directory.Exists(diffDir))
            {
                Directory.CreateDirectory(diffDir);
            }

            using var image = SKImage.FromBitmap(diffBitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(diffOutputPath);
            data.SaveTo(stream);
        }

        return new DiffResult(
            isMatch,
            diffPercent,
            diffPixels,
            totalPixels,
            $"Diff: {diffPercent:F3}% ({diffPixels}/{totalPixels} px), allowed: {maxAllowedDiffPercent:F3}%"
        );
    }

    #endregion
}
