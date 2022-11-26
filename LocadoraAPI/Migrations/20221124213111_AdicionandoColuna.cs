using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocadoraAPI.Migrations
{
    public partial class AdicionandoColuna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Devolvido",
                table: "Locacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Devolvido",
                table: "Locacoes");
        }
    }
}
