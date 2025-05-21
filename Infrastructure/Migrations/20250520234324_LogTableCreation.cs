using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LogTableCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Level = table.Column<string>(type: "NVARCHAR(128)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Exception = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Properties = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
