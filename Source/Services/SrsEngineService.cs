using LearnNN_WebBlazor.Data;
using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models.Personalization;
using Microsoft.EntityFrameworkCore;

namespace LearnNN_WebBlazor.Services;

/// <summary>
/// Implements the SuperMemo-2 (SM-2) Spaced Repetition System algorithm, review scheduling,
/// and backward-compatible mastery synchronization.
/// <para>VN: Triển khai thuật toán Lặp lại ngắt quãng SuperMemo-2 (SM-2), lập lịch ôn tập và đồng bộ trạng thái thành thạo tương thích ngược.</para>
/// </summary>
public class SrsEngineService : ISrsEngineService
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly IMasteryTrackingService _masteryService;

    public SrsEngineService(
        IDbContextFactory<AppDbContext> factory,
        IMasteryTrackingService masteryService)
    {
        _factory = factory;
        _masteryService = masteryService;
    }

    #region SRS Operations

    /// <inheritdoc />
    public async Task<List<Word>> GetDueWordsAsync(int limit = 50, int? topicId = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var endOfToday = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

        // A. Build query filtering by due date and optional topic
        var query = db.WordProgresses
            .AsNoTracking()
            .Include(wp => wp.Word)
                .ThenInclude(w => w.Topic)
            .Where(wp => wp.DueDate <= endOfToday);

        if (topicId.HasValue && topicId.Value > 0)
        {
            query = query.Where(wp => wp.Word.TopicId == topicId.Value);
        }

        // B. Fetch overdue words prioritized by most overdue first
        var dueWords = await query
            .OrderBy(wp => wp.DueDate)
            .Take(limit)
            .Select(wp => wp.Word)
            .ToListAsync();

        return dueWords;
    }

    /// <inheritdoc />
    public async Task<SrsReviewResultDto> RecordReviewResultAsync(
        int wordId,
        int qualityRating,
        Guid? sessionId = null,
        long responseTimeMs = 0)
    {
        await using var db = await _factory.CreateDbContextAsync();

        // A. Fetch word and its associated WordProgress record
        var word = await db.Words
            .Include(w => w.WordProgress)
            .FirstOrDefaultAsync(w => w.Id == wordId);

        if (word == null)
        {
            throw new ArgumentException($"Word with ID {wordId} not found.", nameof(wordId));
        }

        // B. Initialize WordProgress if reviewing for the very first time
        var progress = word.WordProgress;
        if (progress == null)
        {
            progress = new WordProgress
            {
                WordId = word.Id,
                IntervalDays = 1,
                EaseFactor = 2.5,
                Repetitions = 0,
                Lapses = 0,
                DueDate = DateTime.UtcNow
            };
            db.WordProgresses.Add(progress);
            word.WordProgress = progress;
        }

        // C. Calculate next interval and ease factor via SM-2 logic
        int rating = Math.Clamp(qualityRating, 0, 5);
        ApplySm2Algorithm(progress, rating);

        // D. Sync Word.IsMastered flag based on IntervalDays >= 21
        word.IsMastered = progress.IntervalDays >= 21;
        word.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        // E. Log answer trace asynchronously for analytics
        var effectiveSessionId = sessionId ?? Guid.NewGuid();
        bool isSuccess = rating >= 3;
        await _masteryService.LogAnswerAsync(
            effectiveSessionId,
            "Word",
            word.Id,
            null,
            isSuccess,
            responseTimeMs,
            $"SM2 Rating {rating}");

        return new SrsReviewResultDto
        {
            WordId = word.Id,
            QualityRating = rating,
            IntervalDays = progress.IntervalDays,
            EaseFactor = progress.EaseFactor,
            Repetitions = progress.Repetitions,
            NextDueDate = progress.DueDate,
            IsMastered = word.IsMastered
        };
    }

    /// <inheritdoc />
    public async Task<SrsStatisticsDto> GetSrsStatisticsAsync(int? topicId = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var endOfToday = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

        // A. Base words query
        var wordQuery = db.Words.AsNoTracking().AsQueryable();
        if (topicId.HasValue && topicId.Value > 0)
        {
            wordQuery = wordQuery.Where(w => w.TopicId == topicId.Value);
        }
        int totalWords = await wordQuery.CountAsync();

        // B. Word progress query
        var progressQuery = db.WordProgresses.AsNoTracking().Include(wp => wp.Word).AsQueryable();
        if (topicId.HasValue && topicId.Value > 0)
        {
            progressQuery = progressQuery.Where(wp => wp.Word.TopicId == topicId.Value);
        }

        int dueToday = await progressQuery.CountAsync(wp => wp.DueDate <= endOfToday);
        int mastered = await progressQuery.CountAsync(wp => wp.IntervalDays >= 21);
        int learning = await progressQuery.CountAsync(wp => wp.IntervalDays < 21);
        int newWords = Math.Max(0, totalWords - (learning + mastered));

        return new SrsStatisticsDto
        {
            TotalWordsCount = totalWords,
            DueTodayCount = dueToday,
            MasteredCount = mastered,
            LearningCount = learning,
            NewWordsCount = newWords
        };
    }

    /// <inheritdoc />
    public async Task<SrsIntervalPreviewDto> GetIntervalPreviewAsync(int wordId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var progress = await db.WordProgresses.AsNoTracking().FirstOrDefaultAsync(wp => wp.WordId == wordId);

        return new SrsIntervalPreviewDto
        {
            BlackoutDays = CalculateProspectiveInterval(progress, 0),
            DifficultDays = CalculateProspectiveInterval(progress, 3),
            GoodDays = CalculateProspectiveInterval(progress, 4),
            EasyDays = CalculateProspectiveInterval(progress, 5)
        };
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Computes the prospective next interval in days for a specific rating without modifying the database.
    /// </summary>
    private static int CalculateProspectiveInterval(WordProgress? existingProgress, int testRating)
    {
        var tempProgress = new WordProgress
        {
            IntervalDays = existingProgress?.IntervalDays ?? 1,
            EaseFactor = existingProgress?.EaseFactor ?? 2.5,
            Repetitions = existingProgress?.Repetitions ?? 0,
            Lapses = existingProgress?.Lapses ?? 0
        };

        ApplySm2Algorithm(tempProgress, testRating);
        return tempProgress.IntervalDays;
    }

    /// <summary>
    /// Executes core SM-2 interval and ease factor calculations in-place.
    /// Supports a soft penalty (Rating 2) for game answers as agreed in design review.
    /// </summary>
    private static void ApplySm2Algorithm(WordProgress progress, int rating)
    {
        // Special case: Rating 2 is a soft penalty from games (halves interval rather than hard reset to 1)
        if (rating == 2)
        {
            progress.IntervalDays = Math.Max(1, progress.IntervalDays / 2);
            progress.Repetitions = Math.Max(0, progress.Repetitions - 1);
            progress.Lapses++;
            progress.EaseFactor = Math.Max(1.3, Math.Round(progress.EaseFactor - 0.16, 2));
        }
        else if (rating < 3)
        {
            // Standard SM-2 recall failure: Reset repetition sequence to day 1
            progress.Repetitions = 0;
            progress.IntervalDays = 1;
            progress.Lapses++;
            progress.EaseFactor = Math.Max(1.3, Math.Round(progress.EaseFactor + (0.1 - (5 - rating) * (0.08 + (5 - rating) * 0.02)), 2));
        }
        else
        {
            // Standard SM-2 recall success
            if (progress.Repetitions == 0)
            {
                progress.IntervalDays = 1;
            }
            else if (progress.Repetitions == 1)
            {
                progress.IntervalDays = 6;
            }
            else
            {
                progress.IntervalDays = Math.Max(1, (int)Math.Round(progress.IntervalDays * progress.EaseFactor));
            }

            progress.Repetitions++;
            progress.EaseFactor = Math.Max(1.3, Math.Round(progress.EaseFactor + (0.1 - (5 - rating) * (0.08 + (5 - rating) * 0.02)), 2));
        }

        progress.DueDate = DateTime.UtcNow.Date.AddDays(progress.IntervalDays);
        progress.LastStudiedAt = DateTime.UtcNow;
    }

    #endregion
}
