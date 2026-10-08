using System.Runtime.InteropServices;
using LearnNN_WebBlazor.Data;
using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models.Study;
using Microsoft.EntityFrameworkCore;

namespace LearnNN_WebBlazor.Services;

/// <summary>
/// Implements vocabulary study workflows, gamified activities, quiz generation, and mastery state synchronization.
/// <para>VN: Triển khai luồng học từ vựng, các hoạt động game, tạo bài trắc nghiệm và đồng bộ trạng thái ghi nhớ.</para>
/// </summary>
public class StudyService : IStudyService
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly ISrsEngineService _srsEngineService;
    private readonly IMasteryTrackingService _masteryService;

    public StudyService(
        IDbContextFactory<AppDbContext> factory,
        ISrsEngineService srsEngineService,
        IMasteryTrackingService masteryService)
    {
        _factory = factory;
        _srsEngineService = srsEngineService;
        _masteryService = masteryService;
    }

    #region Study Operations

    /// <inheritdoc />
    public async Task<List<Word>> GetWordsForStudyAsync(StudySessionOptions options)
    {
        // When SRS Due-only mode is selected, load exclusively words due for spaced review
        if (options.DueSrsOnly)
        {
            var dueWords = await _srsEngineService.GetDueWordsAsync(options.ItemCount, options.TopicId);
            ShuffleList(dueWords);
            return dueWords;
        }

        await using var db = await _factory.CreateDbContextAsync();

        // A. Build base query with topic and mastery filters
        var query = db.Words.AsNoTracking().Include(w => w.Topic).AsQueryable();

        if (options.TopicId.HasValue && options.TopicId.Value > 0)
        {
            query = query.Where(w => w.TopicId == options.TopicId.Value);
        }

        if (options.OnlyUnmastered)
        {
            query = query.Where(w => !w.IsMastered);
        }

        // B. Fetch matching candidate words
        var words = await query.ToListAsync();

        // If not enough unmastered words found, fallback to include mastered words if requested
        if (words.Count < options.ItemCount && options.OnlyUnmastered)
        {
            var fallbackQuery = db.Words.AsNoTracking().Include(w => w.Topic).AsQueryable();
            if (options.TopicId.HasValue && options.TopicId.Value > 0)
            {
                fallbackQuery = fallbackQuery.Where(w => w.TopicId == options.TopicId.Value);
            }

            var extraWords = await fallbackQuery
                .Where(w => w.IsMastered)
                .ToListAsync();

            words.AddRange(extraWords);
            words = words.DistinctBy(w => w.Id).ToList();
        }

        // C. Randomize list order using in-memory shuffle
        ShuffleList(words);

        return words.Take(options.ItemCount).ToList();
    }

    /// <inheritdoc />
    public async Task<List<QuizQuestionDto>> GenerateQuizQuestionsAsync(StudySessionOptions options)
    {
        await using var db = await _factory.CreateDbContextAsync();

        // A. Retrieve target words for the quiz questions
        var targetWords = await GetWordsForStudyAsync(options);
        if (targetWords.Count == 0)
        {
            return new List<QuizQuestionDto>();
        }

        // B. Fetch a diverse pool of meanings from database for multiple choice distractors
        var distractorPool = await db.Words
            .AsNoTracking()
            .Select(w => new { w.Id, w.Meaning, w.Term })
            .ToListAsync();

        var questions = new List<QuizQuestionDto>();

        // C. Build each quiz question with 1 correct answer and 3 random distractors
        foreach (var word in targetWords)
        {
            var otherDistractors = distractorPool
                .Where(d => d.Id != word.Id && !string.IsNullOrWhiteSpace(d.Meaning) && d.Meaning != word.Meaning)
                .Select(d => d.Meaning)
                .Distinct()
                .ToList();

            ShuffleList(otherDistractors);

            var optionsList = new List<string> { word.Meaning };
            optionsList.AddRange(otherDistractors.Take(3));

            // In case of very small database, fill placeholder options if needed
            while (optionsList.Count < 4)
            {
                optionsList.Add($"Ý nghĩa khác {optionsList.Count}");
            }

            ShuffleList(optionsList);
            int correctIndex = optionsList.IndexOf(word.Meaning);

            questions.Add(new QuizQuestionDto
            {
                WordId = word.Id,
                Term = word.Term,
                Meaning = word.Meaning,
                Phonetic = word.Phonetic,
                PartOfSpeech = word.PartOfSpeech,
                ExampleSentence = word.ExampleSentence,
                QuestionText = word.Term,
                Options = optionsList,
                CorrectOptionIndex = correctIndex
            });
        }

        return questions;
    }

    /// <inheritdoc />
    public async Task<List<MatchCardDto>> GenerateMatchCardsAsync(StudySessionOptions options, int pairCount = 6)
    {
        // A. Ensure we request enough words for the requested pair count
        var queryOptions = new StudySessionOptions
        {
            TopicId = options.TopicId,
            OnlyUnmastered = options.OnlyUnmastered,
            ItemCount = Math.Max(options.ItemCount, pairCount)
        };

        var words = await GetWordsForStudyAsync(queryOptions);
        var selectedWords = words.Take(pairCount).ToList();

        // B. Create twin cards (English Term and Vietnamese Meaning) for each word
        var cards = new List<MatchCardDto>();

        foreach (var word in selectedWords)
        {
            cards.Add(new MatchCardDto
            {
                WordId = word.Id,
                Content = word.Term,
                CardType = MatchCardType.Term
            });

            cards.Add(new MatchCardDto
            {
                WordId = word.Id,
                Content = word.Meaning,
                CardType = MatchCardType.Meaning
            });
        }

        // C. Shuffle all cards thoroughly on the game board
        ShuffleList(cards);

        return cards;
    }

    /// <inheritdoc />
    public async Task SetWordMasteryAsync(int wordId, bool isMastered)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var word = await db.Words.FindAsync(wordId);
        if (word != null)
        {
            word.IsMastered = isMastered;
            word.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task SetBatchWordsMasteryAsync(IEnumerable<int> wordIds, bool isMastered)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var idList = wordIds.Distinct().ToList();
        var words = await db.Words.Where(w => idList.Contains(w.Id)).ToListAsync();

        foreach (var word in words)
        {
            word.IsMastered = isMastered;
            word.UpdatedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<(int TotalCount, int UnmasteredCount)> GetStudyWordCountsAsync(int? topicId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var query = db.Words.AsNoTracking().AsQueryable();

        if (topicId.HasValue && topicId.Value > 0)
        {
            query = query.Where(w => w.TopicId == topicId.Value);
        }

        int total = await query.CountAsync();
        int unmastered = await query.CountAsync(w => !w.IsMastered);

        return (total, unmastered);
    }

    /// <inheritdoc />
    public async Task RecordQuizAnswerAsync(Guid sessionId, int wordId, bool isCorrect, long responseTimeMs, string? selectedOption = null)
    {
        // A. Map game answer to SM-2 rating: 4 for correct, 2 for mistake (soft penalty)
        int rating = isCorrect ? 4 : 2;
        await _srsEngineService.RecordReviewResultAsync(wordId, rating, sessionId, responseTimeMs);
    }

    /// <inheritdoc />
    public async Task RecordMatchAnswerAsync(Guid sessionId, int wordId, bool isCorrect, long responseTimeMs)
    {
        // A. Map matching game answer to SM-2 rating: 4 for correct, 2 for mistake
        int rating = isCorrect ? 4 : 2;
        await _srsEngineService.RecordReviewResultAsync(wordId, rating, sessionId, responseTimeMs);
    }

    /// <inheritdoc />
    public async Task RecordScrambleAnswerAsync(Guid sessionId, int wordId, bool isCorrect, long responseTimeMs)
    {
        // A. Map word scramble answer to SM-2 rating: 4 for correct, 2 for mistake
        int rating = isCorrect ? 4 : 2;
        await _srsEngineService.RecordReviewResultAsync(wordId, rating, sessionId, responseTimeMs);
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Shuffles elements of a list in-place using the Fisher-Yates algorithm.
    /// </summary>
    private static void ShuffleList<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Shared.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    #endregion
}
