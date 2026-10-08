using LearnNN_WebBlazor.Data;
using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models.Personalization;
using Microsoft.EntityFrameworkCore;

namespace LearnNN_WebBlazor.Services;

/// <summary>
/// Implements answer logging, response timing analysis, and dampened accuracy skill mastery calculations.
/// <para>VN: Triển khai ghi nhận nhật ký trả lời, phân tích thời gian phản hồi và tính toán độ thành thạo kỹ năng theo tỷ lệ đúng có trọng số ngưỡng tối thiểu.</para>
/// </summary>
public class MasteryTrackingService : IMasteryTrackingService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public MasteryTrackingService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    #region Tracking Operations

    /// <inheritdoc />
    public async Task<AnswerLog> LogAnswerAsync(
        Guid sessionId,
        string itemType,
        int itemId,
        int? skillTagId,
        bool isCorrect,
        long responseTimeMs,
        string? selectedAnswer = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        // A. Resolve default skill tag for vocabulary words if not provided
        int? effectiveTagId = skillTagId;
        if (!effectiveTagId.HasValue && itemType.Equals("Word", StringComparison.OrdinalIgnoreCase))
        {
            var defaultTag = await db.SkillTags
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Code == "VOC.TOEIC_600" || st.Code == "VOC.ROOT");
            effectiveTagId = defaultTag?.Id;
        }

        // B. Create and persist answer log entry
        var log = new AnswerLog
        {
            SessionId = sessionId,
            ItemType = itemType,
            ItemId = itemId,
            SkillTagId = effectiveTagId,
            IsCorrect = isCorrect,
            ResponseTimeMs = Math.Max(0, responseTimeMs),
            SelectedAnswer = selectedAnswer,
            CreatedAt = DateTime.UtcNow
        };

        db.AnswerLogs.Add(log);
        await db.SaveChangesAsync();

        // C. Update cumulative tag mastery if a valid skill tag is attached
        if (effectiveTagId.HasValue)
        {
            await UpdateTagMasteryInternalAsync(db, effectiveTagId.Value, isCorrect);
        }

        return log;
    }

    /// <inheritdoc />
    public async Task UpdateTagMasteryAsync(int skillTagId, bool isCorrect)
    {
        await using var db = await _factory.CreateDbContextAsync();
        await UpdateTagMasteryInternalAsync(db, skillTagId, isCorrect);
    }

    /// <inheritdoc />
    public async Task<List<SkillRadarDto>> GetSkillRadarAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();

        // A. Load all categories from SkillTags and join with existing TagMasteries
        var tagsWithMastery = await db.SkillTags
            .AsNoTracking()
            .Include(st => st.TagMasteries)
            .ToListAsync();

        var categories = new[] { "Vocabulary", "Grammar", "Listening", "Reading" };
        var result = new List<SkillRadarDto>();

        foreach (var category in categories)
        {
            var categoryTags = tagsWithMastery.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            var masteries = categoryTags.SelectMany(t => t.TagMasteries).ToList();

            int totalAttempts = masteries.Sum(m => m.TotalAttempts);
            int correctAttempts = masteries.Sum(m => m.CorrectAttempts);

            // Dampened accuracy average across category
            double avgScore = 0.0;
            if (totalAttempts > 0)
            {
                avgScore = Math.Round(((double)correctAttempts / totalAttempts) * Math.Min(1.0, totalAttempts / 5.0), 3);
            }

            result.Add(new SkillRadarDto
            {
                Category = category,
                MasteryScore = avgScore,
                TotalAttempts = totalAttempts,
                CorrectAttempts = correctAttempts
            });
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<int> GetDefaultVocabSkillTagIdAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        var tag = await db.SkillTags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Code == "VOC.TOEIC_600" || t.Code == "VOC.ROOT");

        return tag?.Id ?? 1;
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Updates or creates a <see cref="TagMastery"/> entry using the dampened accuracy formula.
    /// Dampened formula: Score = (Correct / Total) * min(1.0, Total / 5.0).
    /// </summary>
    private static async Task UpdateTagMasteryInternalAsync(AppDbContext db, int skillTagId, bool isCorrect)
    {
        var mastery = await db.TagMasteries.FirstOrDefaultAsync(tm => tm.SkillTagId == skillTagId);

        if (mastery == null)
        {
            mastery = new TagMastery
            {
                SkillTagId = skillTagId,
                TotalAttempts = 1,
                CorrectAttempts = isCorrect ? 1 : 0,
                LastPracticedAt = DateTime.UtcNow
            };
            mastery.MasteryScore = Math.Round(((double)mastery.CorrectAttempts / mastery.TotalAttempts) * Math.Min(1.0, mastery.TotalAttempts / 5.0), 3);
            db.TagMasteries.Add(mastery);
        }
        else
        {
            mastery.TotalAttempts++;
            if (isCorrect)
            {
                mastery.CorrectAttempts++;
            }

            mastery.MasteryScore = Math.Round(((double)mastery.CorrectAttempts / mastery.TotalAttempts) * Math.Min(1.0, mastery.TotalAttempts / 5.0), 3);
            mastery.LastPracticedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync();
    }

    #endregion
}
