using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearnNN_WebBlazor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    Term = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phonetic = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PartOfSpeech = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Meaning = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ExampleSentence = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ExampleTranslation = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsMastered = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Words_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Từ vựng về công nghệ thông tin, lập trình, phần mềm", "Công nghệ", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Từ vựng giao tiếp hàng ngày, xã giao, văn phòng", "Giao tiếp", null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Từ vựng học thuật dùng trong kỳ thi IELTS", "IELTS Academic", null }
                });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "CreatedAt", "ExampleSentence", "ExampleTranslation", "Meaning", "PartOfSpeech", "Phonetic", "Term", "TopicId", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The sorting algorithm runs in O(n log n) time.", "Thuật toán sắp xếp này chạy trong thời gian O(n log n).", "Thuật toán – tập hợp các bước xử lý để giải quyết bài toán", "noun", "/ˈæl.ɡə.rɪ.ðəm/", "algorithm", 1, null });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "CreatedAt", "ExampleSentence", "ExampleTranslation", "IsMastered", "Meaning", "PartOfSpeech", "Phonetic", "Term", "TopicId", "UpdatedAt" },
                values: new object[] { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), ".NET is a powerful framework for building web applications.", ".NET là một framework mạnh mẽ để xây dựng ứng dụng web.", true, "Khung phần mềm – bộ thư viện/công cụ hỗ trợ xây dựng ứng dụng", "noun", "/ˈfreɪm.wɜːk/", "framework", 1, null });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "CreatedAt", "ExampleSentence", "ExampleTranslation", "Meaning", "PartOfSpeech", "Phonetic", "Term", "TopicId", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Please push your code to the Git repository.", "Hãy đẩy code của bạn lên kho Git.", "Kho lưu trữ mã nguồn", "noun", "/rɪˈpɒz.ɪ.tər.i/", "repository", 1, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "I apologize for the delay in responding.", "Tôi xin lỗi vì đã trả lời trễ.", "Xin lỗi, tạ lỗi", "verb", "/əˈpɒl.ə.dʒaɪz/", "apologize", 2, null }
                });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "CreatedAt", "ExampleSentence", "ExampleTranslation", "IsMastered", "Meaning", "PartOfSpeech", "Phonetic", "Term", "TopicId", "UpdatedAt" },
                values: new object[] { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Could you clarify what you mean by that?", "Bạn có thể làm rõ ý bạn muốn nói không?", true, "Làm rõ, giải thích rõ hơn", "verb", "/ˈklær.ɪ.faɪ/", "clarify", 2, null });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "CreatedAt", "ExampleSentence", "ExampleTranslation", "Meaning", "PartOfSpeech", "Phonetic", "Term", "TopicId", "UpdatedAt" },
                values: new object[,]
                {
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "There has been a substantial increase in online learning.", "Đã có sự gia tăng đáng kể trong việc học trực tuyến.", "Đáng kể, quan trọng, lớn về quy mô", "adjective", "/səbˈstæn.ʃəl/", "substantial", 3, null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The task was difficult; nonetheless, she completed it.", "Nhiệm vụ rất khó; dẫu vậy, cô ấy đã hoàn thành nó.", "Tuy nhiên, dẫu vậy, mặc dù thế", "adverb", "/ˌnʌn.ðəˈles/", "nonetheless", 3, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topics_Name",
                table: "Topics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Words_IsMastered",
                table: "Words",
                column: "IsMastered");

            migrationBuilder.CreateIndex(
                name: "IX_Words_Term",
                table: "Words",
                column: "Term");

            migrationBuilder.CreateIndex(
                name: "IX_Words_TopicId",
                table: "Words",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Topics");
        }
    }
}
