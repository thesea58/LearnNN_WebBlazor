using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearnNN_WebBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonalizationCoreAndSrs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearnerProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TargetScore = table.Column<int>(type: "INTEGER", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DailyGoalMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentStage = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PreferredAccent = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnerProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillTags_SkillTags_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SkillTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WordProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WordId = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IntervalDays = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    EaseFactor = table.Column<double>(type: "REAL", nullable: false, defaultValue: 2.5),
                    Repetitions = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Lapses = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    LastStudiedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordProgresses_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ItemType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillTagId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    ResponseTimeMs = table.Column<long>(type: "INTEGER", nullable: false),
                    SelectedAnswer = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerLogs_SkillTags_SkillTagId",
                        column: x => x.SkillTagId,
                        principalTable: "SkillTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TagMasteries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SkillTagId = table.Column<int>(type: "INTEGER", nullable: false),
                    MasteryScore = table.Column<double>(type: "REAL", nullable: false),
                    TotalAttempts = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectAttempts = table.Column<int>(type: "INTEGER", nullable: false),
                    LastPracticedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagMasteries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagMasteries_SkillTags_SkillTagId",
                        column: x => x.SkillTagId,
                        principalTable: "SkillTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LearnerProfiles",
                columns: new[] { "Id", "CreatedAt", "CurrentStage", "DailyGoalMinutes", "ExamDate", "PreferredAccent", "TargetScore", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "S1", 30, null, "en-US", 650, null });

            migrationBuilder.InsertData(
                table: "SkillTags",
                columns: new[] { "Id", "Category", "Code", "Name", "ParentId" },
                values: new object[,]
                {
                    { 1, "Vocabulary", "VOC.ROOT", "Từ vựng tổng quát", null },
                    { 2, "Grammar", "GRAM.ROOT", "Ngữ pháp tổng quát", null },
                    { 3, "Listening", "LIS.ROOT", "Kỹ năng nghe", null },
                    { 4, "Reading", "READ.ROOT", "Kỹ năng đọc", null }
                });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsMastered",
                value: false);

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsMastered",
                value: false);

            migrationBuilder.InsertData(
                table: "SkillTags",
                columns: new[] { "Id", "Category", "Code", "Name", "ParentId" },
                values: new object[,]
                {
                    { 5, "Vocabulary", "VOC.TOEIC_600", "Từ vựng TOEIC 600 Essential Words", 1 },
                    { 6, "Vocabulary", "VOC.WORD_FORM", "Cấu tạo từ & Từ loại (Word Form)", 1 },
                    { 7, "Vocabulary", "VOC.COLLOCATION", "Cụm từ đi kèm (Collocation)", 1 },
                    { 8, "Grammar", "GRAM.TENSE", "Các thì trong tiếng Anh", 2 },
                    { 9, "Grammar", "GRAM.PARTS_OF_SPEECH", "Từ loại & Vị trí trong câu", 2 },
                    { 10, "Grammar", "GRAM.PASSIVE_VOICE", "Câu bị động", 2 },
                    { 11, "Grammar", "GRAM.RELATIVE_CLAUSE", "Mệnh đề quan hệ", 2 },
                    { 12, "Listening", "LIS.PART1_PHOTO", "Part 1 - Mô tả hình ảnh", 3 },
                    { 13, "Listening", "LIS.PART2_QA", "Part 2 - Hỏi đáp", 3 },
                    { 14, "Listening", "LIS.PART3_CONV", "Part 3 - Đoạn hội thoại", 3 },
                    { 15, "Listening", "LIS.PART4_TALK", "Part 4 - Bài nói ngắn", 3 },
                    { 16, "Reading", "READ.PART5_INCOMPLETE", "Part 5 - Điền câu", 4 },
                    { 17, "Reading", "READ.PART6_TEXT_COMPLETION", "Part 6 - Hoàn thành đoạn văn", 4 },
                    { 18, "Reading", "READ.PART7_SINGLE_PASSAGE", "Part 7 - Đoạn đơn", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLogs_CreatedAt",
                table: "AnswerLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLogs_ItemType_ItemId",
                table: "AnswerLogs",
                columns: new[] { "ItemType", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLogs_SessionId",
                table: "AnswerLogs",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLogs_SkillTagId",
                table: "AnswerLogs",
                column: "SkillTagId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillTags_Category",
                table: "SkillTags",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_SkillTags_Code",
                table: "SkillTags",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillTags_ParentId",
                table: "SkillTags",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TagMasteries_SkillTagId",
                table: "TagMasteries",
                column: "SkillTagId");

            migrationBuilder.CreateIndex(
                name: "IX_WordProgresses_DueDate",
                table: "WordProgresses",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_WordProgresses_WordId",
                table: "WordProgresses",
                column: "WordId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerLogs");

            migrationBuilder.DropTable(
                name: "LearnerProfiles");

            migrationBuilder.DropTable(
                name: "TagMasteries");

            migrationBuilder.DropTable(
                name: "WordProgresses");

            migrationBuilder.DropTable(
                name: "SkillTags");

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsMastered",
                value: true);

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsMastered",
                value: true);
        }
    }
}
