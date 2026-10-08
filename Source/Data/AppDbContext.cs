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
    public DbSet<LearnerProfile> LearnerProfiles => Set<LearnerProfile>();
    public DbSet<SkillTag> SkillTags => Set<SkillTag>();
    public DbSet<TagMastery> TagMasteries => Set<TagMastery>();
    public DbSet<WordProgress> WordProgresses => Set<WordProgress>();
    public DbSet<AnswerLog> AnswerLogs => Set<AnswerLog>();
    public DbSet<AiRequest> AiRequests => Set<AiRequest>();

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

            // One-to-one relationship with WordProgress
            entity.HasOne(w => w.WordProgress)
                  .WithOne(wp => wp.Word)
                  .HasForeignKey<WordProgress>(wp => wp.WordId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        #endregion

        #region LearnerProfile Configuration
        modelBuilder.Entity<LearnerProfile>(entity =>
        {
            entity.Property(p => p.CreatedAt).HasDefaultValueSql("datetime('now')");
        });
        #endregion

        #region SkillTag Configuration
        modelBuilder.Entity<SkillTag>(entity =>
        {
            entity.HasIndex(s => s.Code).IsUnique();
            entity.HasIndex(s => s.Category);
            entity.HasOne(s => s.ParentTag)
                  .WithMany(p => p.ChildTags)
                  .HasForeignKey(s => s.ParentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
        #endregion

        #region TagMastery Configuration
        modelBuilder.Entity<TagMastery>(entity =>
        {
            entity.HasIndex(tm => tm.SkillTagId);
            entity.HasOne(tm => tm.SkillTag)
                  .WithMany(st => st.TagMasteries)
                  .HasForeignKey(tm => tm.SkillTagId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        #endregion

        #region WordProgress Configuration
        modelBuilder.Entity<WordProgress>(entity =>
        {
            entity.HasIndex(wp => wp.WordId).IsUnique();
            entity.HasIndex(wp => wp.DueDate);
            entity.Property(wp => wp.EaseFactor).HasDefaultValue(2.5);
            entity.Property(wp => wp.IntervalDays).HasDefaultValue(1);
            entity.Property(wp => wp.Repetitions).HasDefaultValue(0);
            entity.Property(wp => wp.Lapses).HasDefaultValue(0);
        });
        #endregion

        #region AnswerLog Configuration
        modelBuilder.Entity<AnswerLog>(entity =>
        {
            entity.HasIndex(al => al.SessionId);
            entity.HasIndex(al => al.SkillTagId);
            entity.HasIndex(al => al.CreatedAt);
            entity.HasIndex(al => new { al.ItemType, al.ItemId });
            entity.Property(al => al.CreatedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(al => al.SkillTag)
                  .WithMany(st => st.AnswerLogs)
                  .HasForeignKey(al => al.SkillTagId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
        #endregion

        #region AiRequest Configuration
        modelBuilder.Entity<AiRequest>(entity =>
        {
            entity.HasIndex(ar => ar.RequestId).IsUnique();
            entity.HasIndex(ar => ar.InputHash);
            entity.HasIndex(ar => ar.Status);
            entity.HasIndex(ar => ar.CreatedAt);
            entity.Property(ar => ar.CreatedAt).HasDefaultValueSql("datetime('now')");
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
                IsMastered = false,
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
                IsMastered = false,
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

        #region Seed LearnerProfile
        modelBuilder.Entity<LearnerProfile>().HasData(
            new LearnerProfile
            {
                Id = 1,
                TargetScore = 650,
                DailyGoalMinutes = 30,
                CurrentStage = "S1",
                PreferredAccent = "en-US",
                CreatedAt = seedDate
            }
        );
        #endregion

        #region Seed SkillTags
        modelBuilder.Entity<SkillTag>().HasData(
            // High-level roots
            new SkillTag { Id = 1, Code = "VOC.ROOT", Name = "Từ vựng tổng quát", Category = "Vocabulary", ParentId = null },
            new SkillTag { Id = 2, Code = "GRAM.ROOT", Name = "Ngữ pháp tổng quát", Category = "Grammar", ParentId = null },
            new SkillTag { Id = 3, Code = "LIS.ROOT", Name = "Kỹ năng nghe", Category = "Listening", ParentId = null },
            new SkillTag { Id = 4, Code = "READ.ROOT", Name = "Kỹ năng đọc", Category = "Reading", ParentId = null },

            // Vocabulary sub-tags
            new SkillTag { Id = 5, Code = "VOC.TOEIC_600", Name = "Từ vựng TOEIC 600 Essential Words", Category = "Vocabulary", ParentId = 1 },
            new SkillTag { Id = 6, Code = "VOC.WORD_FORM", Name = "Cấu tạo từ & Từ loại (Word Form)", Category = "Vocabulary", ParentId = 1 },
            new SkillTag { Id = 7, Code = "VOC.COLLOCATION", Name = "Cụm từ đi kèm (Collocation)", Category = "Vocabulary", ParentId = 1 },

            // Grammar sub-tags
            new SkillTag { Id = 8, Code = "GRAM.TENSE", Name = "Các thì trong tiếng Anh", Category = "Grammar", ParentId = 2 },
            new SkillTag { Id = 9, Code = "GRAM.PARTS_OF_SPEECH", Name = "Từ loại & Vị trí trong câu", Category = "Grammar", ParentId = 2 },
            new SkillTag { Id = 10, Code = "GRAM.PASSIVE_VOICE", Name = "Câu bị động", Category = "Grammar", ParentId = 2 },
            new SkillTag { Id = 11, Code = "GRAM.RELATIVE_CLAUSE", Name = "Mệnh đề quan hệ", Category = "Grammar", ParentId = 2 },

            // Listening sub-tags
            new SkillTag { Id = 12, Code = "LIS.PART1_PHOTO", Name = "Part 1 - Mô tả hình ảnh", Category = "Listening", ParentId = 3 },
            new SkillTag { Id = 13, Code = "LIS.PART2_QA", Name = "Part 2 - Hỏi đáp", Category = "Listening", ParentId = 3 },
            new SkillTag { Id = 14, Code = "LIS.PART3_CONV", Name = "Part 3 - Đoạn hội thoại", Category = "Listening", ParentId = 3 },
            new SkillTag { Id = 15, Code = "LIS.PART4_TALK", Name = "Part 4 - Bài nói ngắn", Category = "Listening", ParentId = 3 },

            // Reading sub-tags
            new SkillTag { Id = 16, Code = "READ.PART5_INCOMPLETE", Name = "Part 5 - Điền câu", Category = "Reading", ParentId = 4 },
            new SkillTag { Id = 17, Code = "READ.PART6_TEXT_COMPLETION", Name = "Part 6 - Hoàn thành đoạn văn", Category = "Reading", ParentId = 4 },
            new SkillTag { Id = 18, Code = "READ.PART7_SINGLE_PASSAGE", Name = "Part 7 - Đoạn đơn", Category = "Reading", ParentId = 4 }
        );
        #endregion
    }
}
