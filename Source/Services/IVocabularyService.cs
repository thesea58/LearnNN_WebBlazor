using LearnNN_WebBlazor.Data.Entities;
using LearnNN_WebBlazor.Models;

namespace LearnNN_WebBlazor.Services;

/// <summary>
/// Defines the contract for vocabulary management operations including CRUD for topics and words.
/// <para>VN: Định nghĩa hợp đồng cho các thao tác quản lý từ vựng bao gồm CRUD chủ đề và từ.</para>
/// </summary>
public interface IVocabularyService
{
    // ─── TOPICS ──────────────────────────────────────────

    /// <summary>
    /// Retrieves all topics ordered by name, including their associated words.
    /// </summary>
    Task<List<Topic>> GetAllTopicsAsync();

    /// <summary>
    /// Retrieves a single topic by its identifier, including associated words.
    /// </summary>
    /// <param name="id">The topic identifier.</param>
    /// <returns>The matching topic, or <c>null</c> if not found.</returns>
    Task<Topic?> GetTopicByIdAsync(int id);

    /// <summary>
    /// Creates a new topic with auto-generated timestamps.
    /// </summary>
    /// <param name="topic">The topic entity to create.</param>
    /// <returns>The created topic with its assigned identifier.</returns>
    Task<Topic> CreateTopicAsync(Topic topic);

    /// <summary>
    /// Updates an existing topic's name and description.
    /// </summary>
    /// <param name="topic">The topic entity containing updated values.</param>
    /// <returns>The updated topic entity.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the topic ID does not exist.</exception>
    Task<Topic> UpdateTopicAsync(Topic topic);

    /// <summary>
    /// Deletes a topic and all its associated words (cascade).
    /// </summary>
    /// <param name="id">The topic identifier to delete.</param>
    Task DeleteTopicAsync(int id);

    /// <summary>
    /// Checks whether a topic name already exists in the database (case-insensitive).
    /// </summary>
    /// <param name="name">The topic name to check.</param>
    /// <param name="excludeId">Optional topic ID to exclude (used during edit to allow the current name).</param>
    /// <returns><c>true</c> if a duplicate name exists; otherwise <c>false</c>.</returns>
    Task<bool> TopicNameExistsAsync(string name, int? excludeId = null);

    // ─── WORDS ───────────────────────────────────────────

    /// <summary>
    /// Retrieves a paginated list of words matching the given filter criteria.
    /// </summary>
    /// <param name="filter">Filter containing search term, topic, mastery status, and pagination.</param>
    Task<List<Word>> GetWordsAsync(WordFilterModel filter);

    /// <summary>
    /// Returns the total count of words matching the given filter (for pagination calculation).
    /// </summary>
    /// <param name="filter">Filter criteria to apply.</param>
    Task<int> GetWordCountAsync(WordFilterModel filter);

    /// <summary>
    /// Retrieves a single word by its identifier, including its parent topic.
    /// </summary>
    /// <param name="id">The word identifier.</param>
    /// <returns>The matching word, or <c>null</c> if not found.</returns>
    Task<Word?> GetWordByIdAsync(int id);

    /// <summary>
    /// Creates a new word entry with auto-generated timestamps.
    /// </summary>
    /// <param name="word">The word entity to create.</param>
    /// <returns>The created word with its topic navigation loaded.</returns>
    Task<Word> CreateWordAsync(Word word);

    /// <summary>
    /// Updates all editable fields of an existing word.
    /// </summary>
    /// <param name="word">The word entity containing updated values.</param>
    /// <returns>The updated word with its topic navigation loaded.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the word ID does not exist.</exception>
    Task<Word> UpdateWordAsync(Word word);

    /// <summary>
    /// Deletes a word by its identifier.
    /// </summary>
    /// <param name="id">The word identifier to delete.</param>
    Task DeleteWordAsync(int id);

    /// <summary>
    /// Toggles the mastery status of a word (mastered ↔ not mastered).
    /// </summary>
    /// <param name="wordId">The word identifier to toggle.</param>
    Task ToggleMasteredAsync(int wordId);
}
