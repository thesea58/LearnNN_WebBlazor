using LearnNN_WebBlazor.Data;
using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnNN_WebBlazor.Services;

/// <summary>
/// Implements vocabulary management business logic using EF Core with DbContextFactory
/// for Blazor Server concurrency safety.
/// <para>VN: Triển khai logic nghiệp vụ quản lý từ vựng sử dụng EF Core với DbContextFactory
/// để đảm bảo an toàn đồng thời trên Blazor Server.</para>
/// </summary>
public class VocabularyService : IVocabularyService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public VocabularyService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    // ─── TOPICS ──────────────────────────────────────────

    /// <inheritdoc />
    public async Task<List<Topic>> GetAllTopicsAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Topics
            .Include(t => t.Words)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Topic?> GetTopicByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Topics
            .Include(t => t.Words)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc />
    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        await using var db = await _factory.CreateDbContextAsync();
        topic.CreatedAt = DateTime.UtcNow;
        topic.UpdatedAt = null;

        db.Topics.Add(topic);
        await db.SaveChangesAsync();
        return topic;
    }

    /// <inheritdoc />
    public async Task<Topic> UpdateTopicAsync(Topic topic)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var existing = await db.Topics.FindAsync(topic.Id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Topic with ID {topic.Id} not found.");
        }

        existing.Name = topic.Name;
        existing.Description = topic.Description;
        existing.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return existing;
    }

    /// <inheritdoc />
    public async Task DeleteTopicAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var topic = await db.Topics.FindAsync(id);
        if (topic != null)
        {
            db.Topics.Remove(topic);
            await db.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<bool> TopicNameExistsAsync(string name, int? excludeId = null)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var trimmedName = name.Trim().ToLower();
        var query = db.Topics.Where(t => t.Name.ToLower() == trimmedName);

        if (excludeId.HasValue)
        {
            query = query.Where(t => t.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    // ─── WORDS ───────────────────────────────────────────

    /// <inheritdoc />
    public async Task<List<Word>> GetWordsAsync(WordFilterModel filter)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var query = BuildWordFilterQuery(db, filter);

        query = query.OrderByDescending(w => w.CreatedAt);

        var page = filter.Page > 0 ? filter.Page : 1;
        var pageSize = filter.PageSize > 0 ? filter.PageSize : 20;

        return await query
            .Include(w => w.Topic)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<int> GetWordCountAsync(WordFilterModel filter)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var query = BuildWordFilterQuery(db, filter);
        return await query.CountAsync();
    }

    /// <inheritdoc />
    public async Task<Word?> GetWordByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Words
            .Include(w => w.Topic)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    /// <inheritdoc />
    public async Task<Word> CreateWordAsync(Word word)
    {
        await using var db = await _factory.CreateDbContextAsync();
        word.CreatedAt = DateTime.UtcNow;
        word.UpdatedAt = null;

        db.Words.Add(word);
        await db.SaveChangesAsync();

        // Reload topic navigation so the caller gets complete data
        await db.Entry(word).Reference(w => w.Topic).LoadAsync();
        return word;
    }

    /// <inheritdoc />
    public async Task<Word> UpdateWordAsync(Word word)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var existing = await db.Words.FindAsync(word.Id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Word with ID {word.Id} not found.");
        }

        existing.TopicId = word.TopicId;
        existing.Term = word.Term;
        existing.Phonetic = word.Phonetic;
        existing.PartOfSpeech = word.PartOfSpeech;
        existing.Meaning = word.Meaning;
        existing.ExampleSentence = word.ExampleSentence;
        existing.ExampleTranslation = word.ExampleTranslation;
        existing.IsMastered = word.IsMastered;
        existing.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        // Reload topic navigation so the caller gets complete data
        await db.Entry(existing).Reference(w => w.Topic).LoadAsync();
        return existing;
    }

    /// <inheritdoc />
    public async Task DeleteWordAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var word = await db.Words.FindAsync(id);
        if (word != null)
        {
            db.Words.Remove(word);
            await db.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task ToggleMasteredAsync(int wordId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var word = await db.Words.FindAsync(wordId);
        if (word != null)
        {
            word.IsMastered = !word.IsMastered;
            word.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }

    // ─── PRIVATE HELPERS ─────────────────────────────────

    /// <summary>
    /// Builds a composable IQueryable applying search, topic, and mastery filters from the given model.
    /// </summary>
    private static IQueryable<Word> BuildWordFilterQuery(AppDbContext db, WordFilterModel filter)
    {
        var query = db.Words.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.Trim().ToLower();
            query = query.Where(w => w.Term.ToLower().Contains(term) || w.Meaning.ToLower().Contains(term));
        }

        if (filter.TopicId.HasValue && filter.TopicId.Value > 0)
        {
            query = query.Where(w => w.TopicId == filter.TopicId.Value);
        }

        if (filter.MasteredFilter == MasteredFilter.NotMastered)
        {
            query = query.Where(w => !w.IsMastered);
        }
        else if (filter.MasteredFilter == MasteredFilter.Mastered)
        {
            query = query.Where(w => w.IsMastered);
        }

        return query;
    }
}
