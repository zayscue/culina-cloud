using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CulinaCloud.Analytics.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Analytics");

            migrationBuilder.CreateTable(
                name: "RecipePopularity",
                schema: "Analytics",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Submitted = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    RatingCount = table.Column<int>(type: "integer", nullable: false),
                    RatingSum = table.Column<int>(type: "integer", nullable: false),
                    RatingAverage = table.Column<decimal>(type: "numeric(7,5)", nullable: false),
                    RatingWeightedAverage = table.Column<decimal>(type: "numeric(7,5)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipePopularity", x => x.RecipeId);
                });

            migrationBuilder.CreateTable(
                name: "RecipeSimilarity",
                schema: "Analytics",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SimilarRecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SimilarityType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SimilarityScore = table.Column<decimal>(type: "numeric(7,5)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSimilarity", x => new { x.RecipeId, x.SimilarRecipeId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipePopularity",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "RecipeSimilarity",
                schema: "Analytics");
        }
    }
}
