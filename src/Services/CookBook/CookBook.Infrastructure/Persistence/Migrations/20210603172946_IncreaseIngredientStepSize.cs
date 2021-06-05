using Microsoft.EntityFrameworkCore.Migrations;

namespace Culina.CookBook.Infrastructure.Persistence.Migrations
{
    public partial class IncreaseIngredientStepSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Instruction",
                schema: "CookBook",
                table: "RecipeSteps",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Instruction",
                schema: "CookBook",
                table: "RecipeSteps",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);
        }
    }
}
