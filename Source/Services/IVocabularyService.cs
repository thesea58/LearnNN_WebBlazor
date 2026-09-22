using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models;

namespace LearnNN_WebBlazor.Services;

public interface IVocabularyService
{
    // ─── TOPICS ──────────────────────────────────────────
    Task<List<Topic>> GetAllTopicsAsync();
    Task<Topic?> GetTopicByIdAsync(int id);
    Task<Topic> CreateTopicAsync(Topic topic);
    Task<Topic> UpdateTopicAsync(Topic topic);
    Task DeleteTopicAsync(int id);
    Task<bool> TopicNameExistsAsync(string name, int? excludeId = null);

    // ─── WORDS ───────────────────────────────────────────
    Task<List<Word>> GetWordsAsync(WordFilterModel filter);
    Task<int> GetWordCountAsync(WordFilterModel filter);
    Task<Word?> GetWordByIdAsync(int id);
    Task<Word> CreateWordAsync(Word word);
    Task<Word> UpdateWordAsync(Word word);
    Task DeleteWordAsync(int id);
    Task ToggleMasteredAsync(int wordId);
}
