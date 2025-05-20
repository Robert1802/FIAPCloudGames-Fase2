using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoEntidadePromocao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PromocaoId",
                table: "Jogo",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Promocoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    DataFim = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocoes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_PromocaoId",
                table: "Jogo",
                column: "PromocaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Promocoes_PromocaoId",
                table: "Jogo",
                column: "PromocaoId",
                principalTable: "Promocoes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Promocoes_PromocaoId",
                table: "Jogo");

            migrationBuilder.DropTable(
                name: "Promocoes");

            migrationBuilder.DropIndex(
                name: "IX_Jogo_PromocaoId",
                table: "Jogo");

            migrationBuilder.DropColumn(
                name: "PromocaoId",
                table: "Jogo");
        }
    }
}
