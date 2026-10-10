using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnNN_WebBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddTrapTypeToAnswerLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrapType",
                table: "AnswerLogs",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrapType",
                table: "AnswerLogs");
        }
    }
}
