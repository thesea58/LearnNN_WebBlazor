namespace LearnNN_WebBlazor.Models;

public enum MasteredFilter
{
    All = 0,
    NotMastered = 1,
    Mastered = 2
}

public class WordFilterModel
{
    public string? SearchTerm { get; set; }
    public int? TopicId { get; set; }
    public MasteredFilter MasteredFilter { get; set; } = MasteredFilter.All;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
