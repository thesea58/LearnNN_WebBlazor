using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnNN_WebBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddAiInfrastructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AiRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RequestId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Kind = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PromptVersion = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    InputHash = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    PromptText = table.Column<string>(type: "TEXT", nullable: false),
                    Channel = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ResponseJson = table.Column<string>(type: "TEXT", nullable: true),
                    ModelLabel = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiRequests_CreatedAt",
                table: "AiRequests",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AiRequests_InputHash",
                table: "AiRequests",
                column: "InputHash");

            migrationBuilder.CreateIndex(
                name: "IX_AiRequests_RequestId",
                table: "AiRequests",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiRequests_Status",
                table: "AiRequests",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiRequests");
        }
    }
}
