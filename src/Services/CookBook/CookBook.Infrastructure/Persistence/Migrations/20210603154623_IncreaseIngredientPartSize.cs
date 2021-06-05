using Microsoft.EntityFrameworkCore.Migrations;

namespace Culina.CookBook.Infrastructure.Persistence.Migrations
{
    public partial class IncreaseIngredientPartSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Part",
                schema: "CookBook",
                table: "RecipeIngredients",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Part",
                schema: "CookBook",
                table: "RecipeIngredients",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);
        }
    }
}
