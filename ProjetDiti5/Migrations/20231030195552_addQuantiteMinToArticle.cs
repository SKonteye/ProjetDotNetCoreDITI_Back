using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetDiti5.Migrations
{
    /// <inheritdoc />
    public partial class addQuantiteMinToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantiteMin",
                table: "Articles",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantiteMin",
                table: "Articles");
        }
    }
}
