using CsvHelper.Configuration.Attributes;

namespace LearnNN_WebBlazor.Models;

/// <summary>
/// DTO representing a row in the CSV file for importing and exporting words.
/// <para>VN: DTO đại diện cho một dòng trong file CSV dùng để import và export từ vựng.</para>
/// </summary>
public class WordCsvRecord
{
    [Name("TopicName")]
    public string TopicName { get; set; } = string.Empty;

    [Name("Term")]
    public string Term { get; set; } = string.Empty;

    [Name("Meaning")]
    public string Meaning { get; set; } = string.Empty;

    [Name("Pronunciation")]
    public string? Phonetic { get; set; }

    [Name("PartOfSpeech")]
    public string? PartOfSpeech { get; set; }

    [Name("Example")]
    public string? ExampleSentence { get; set; }

    [Name("ExampleTranslation")]
    public string? ExampleTranslation { get; set; }

    [Name("IsMastered")]
    [BooleanTrueValues("yes", "true", "1", "true")]
    [BooleanFalseValues("no", "false", "0", "false", "")]
    public bool IsMastered { get; set; }
}
