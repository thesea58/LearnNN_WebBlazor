using LearnNN_WebBlazor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnNN_WebBlazor.Data;

/// <summary>
/// EF Core database context for the LearnNN application.
/// Configures entity mappings, indexes, relationships, and seed data via Fluent API.
/// <para>VN: DbContext của ứng dụng LearnNN, cấu hình ánh xạ entity, index, quan hệ và dữ liệu mẫu qua Fluent API.</para>
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Word> Words => Set<Word>();

    /// <summary>
    /// Configures entity schemas, indexes, default values, and relationships using Fluent API.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Topic Configuration
        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasIndex(t => t.Name).IsUnique();
            entity.Property(t => t.CreatedAt).HasDefaultValueSql("datetime('now')");
        });
        #endregion

        #region Word Configuration
        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasIndex(w => w.TopicId);
            entity.HasIndex(w => w.Term);
            entity.HasIndex(w => w.IsMastered);
            entity.Property(w => w.IsMastered).HasDefaultValue(false);
            entity.Property(w => w.CreatedAt).HasDefaultValueSql("datetime('now')");

            // Cascade delete: removing a topic also removes all its words
            entity.HasOne(w => w.Topic)
                  .WithMany(t => t.Words)
                  .HasForeignKey(w => w.TopicId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        #endregion

        SeedData(modelBuilder);
    }

    /// <summary>
    /// Populates the database with initial sample topics and words for development/demo purposes.
    /// </summary>
    private static void SeedData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Topic>().HasData(
            new Topic
            {
                Id = 1,
                Name = "Công nghệ",
                Description = "Từ vựng về công nghệ thông tin, lập trình, phần mềm",
                CreatedAt = seedDate
            },
            new Topic
            {
                Id = 2,
                Name = "Giao tiếp",
                Description = "Từ vựng giao tiếp hàng ngày, xã giao, văn phòng",
                CreatedAt = seedDate
            },
            new Topic
            {
                Id = 3,
                Name = "IELTS Academic",
                Description = "Từ vựng học thuật dùng trong kỳ thi IELTS",
                CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<Word>().HasData(
            new Word
            {
                Id = 1,
                TopicId = 1,
                Term = "algorithm",
                Phonetic = "/ˈæl.ɡə.rɪ.ðəm/",
                PartOfSpeech = "noun",
                Meaning = "Thuật toán – tập hợp các bước xử lý để giải quyết bài toán",
                ExampleSentence = "The sorting algorithm runs in O(n log n) time.",
                ExampleTranslation = "Thuật toán sắp xếp này chạy trong thời gian O(n log n).",
                IsMastered = false,
                CreatedAt = seedDate
            },
            new Word
            {
                Id = 2,
                TopicId = 1,
                Term = "framework",
                Phonetic = "/ˈfreɪm.wɜːk/",
                PartOfSpeech = "noun",
                Meaning = "Khung phần mềm – bộ thư viện/công cụ hỗ trợ xây dựng ứng dụng",
                ExampleSentence = ".NET is a powerful framework for building web applications.",
                ExampleTranslation = ".NET là một framework mạnh mẽ để xây dựng ứng dụng web.",
                IsMastered = true,
                CreatedAt = seedDate
            },
            new Word
            {
                Id = 3,
                TopicId = 1,
                Term = "repository",
                Phonetic = "/rɪˈpɒz.ɪ.tər.i/",
                PartOfSpeech = "noun",
                Meaning = "Kho lưu trữ mã nguồn",
                ExampleSentence = "Please push your code to the Git repository.",
                ExampleTranslation = "Hãy đẩy code của bạn lên kho Git.",
                IsMastered = false,
                CreatedAt = seedDate
            },
            new Word
            {
                Id = 4,
                TopicId = 2,
                Term = "apologize",
                Phonetic = "/əˈpɒl.ə.dʒaɪz/",
                PartOfSpeech = "verb",
                Meaning = "Xin lỗi, tạ lỗi",
                ExampleSentence = "I apologize for the delay in responding.",
                ExampleTranslation = "Tôi xin lỗi vì đã trả lời trễ.",
                IsMastered = false,
                CreatedAt = seedDate
            },
            new Word
            {
                Id = 5,
                TopicId = 2,
                Term = "clarify",
                Phonetic = "/ˈklær.ɪ.faɪ/",
                PartOfSpeech = "verb",
                Meaning = "Làm rõ, giải thích rõ hơn",
                ExampleSentence = "Could you clarify what you mean by that?",
                ExampleTranslation = "Bạn có thể làm rõ ý bạn muốn nói không?",
                IsMastered = true,
                CreatedAt = seedDate
            },
            new Word
            {
                Id = 6,
                TopicId = 3,
                Term = "substantial",
                Phonetic = "/səbˈstæn.ʃəl/",
                PartOfSpeech = "adjective",
                Meaning = "Đáng kể, quan trọng, lớn về quy mô",
                ExampleSentence = "There has been a substantial increase in online learning.",
                ExampleTranslation = "Đã có sự gia tăng đáng kể trong việc học trực tuyến.",
                IsMastered = false,
                CreatedAt = seedDate
            },
            new Word
            {
                Id = 7,
                TopicId = 3,
                Term = "nonetheless",
                Phonetic = "/ˌnʌn.ðəˈles/",
                PartOfSpeech = "adverb",
                Meaning = "Tuy nhiên, dẫu vậy, mặc dù thế",
                ExampleSentence = "The task was difficult; nonetheless, she completed it.",
                ExampleTranslation = "Nhiệm vụ rất khó; dẫu vậy, cô ấy đã hoàn thành nó.",
                IsMastered = false,
                CreatedAt = seedDate
            }
        );
    }
}
