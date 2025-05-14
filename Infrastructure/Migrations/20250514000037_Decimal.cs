using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Decimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Jogo",
                type: "DECIMAL(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(10,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Desconto",
                table: "Jogo",
                type: "DECIMAL(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(3,0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Jogo",
                type: "DECIMAL(10,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Desconto",
                table: "Jogo",
                type: "DECIMAL(3,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(3,2)");
        }
    }
}
