using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIAPCloudGames.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LogFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exception",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "MessageTemplate",
                table: "Logs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Exception",
                table: "Logs",
                type: "NVARCHAR(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageTemplate",
                table: "Logs",
                type: "NVARCHAR(MAX)",
                nullable: true);
        }
    }
}
