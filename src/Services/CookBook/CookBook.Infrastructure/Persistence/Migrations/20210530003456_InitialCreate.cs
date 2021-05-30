using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Culina.CookBook.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CookBook");

            migrationBuilder.CreateTable(
                name: "EventOutbox",
                schema: "CookBook",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsStored = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    EventName = table.Column<string>(type: "text", nullable: false),
                    Occurred = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AggregateType = table.Column<string>(type: "text", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    RaisedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Details = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Data = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventOutbox", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                schema: "CookBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                schema: "CookBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IngredientName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                schema: "CookBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    EstimatedMinutes = table.Column<int>(type: "integer", nullable: false),
                    Serves = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Yield = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    NumberOfSteps = table.Column<int>(type: "integer", nullable: false),
                    NumberOfIngredients = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "CookBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TagName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeImages",
                schema: "CookBook",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeImages", x => new { x.RecipeId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_RecipeImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "CookBook",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeImages_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "CookBook",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                schema: "CookBook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IngredientId = table.Column<Guid>(type: "uuid", nullable: true),
                    Quantity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Part = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => new { x.RecipeId, x.Id });
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalSchema: "CookBook",
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "CookBook",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeMetadata",
                schema: "CookBook",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeMetadata", x => new { x.RecipeId, x.Type });
                    table.ForeignKey(
                        name: "FK_RecipeMetadata_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "CookBook",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeNutrition",
                schema: "CookBook",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServingSize = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ServingsPerRecipe = table.Column<int>(type: "integer", nullable: false),
                    Calories = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CaloriesFromFat = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CaloriesFromFatPdv = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    TotalFat = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    TotalFatPdv = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    SaturatedFat = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    decimal52 = table.Column<decimal>(name: "decimal(5, 2)", type: "numeric", nullable: false),
                    Cholesterol = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CholesterolPdv = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    DietaryFiber = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    DietaryFiberPdv = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Sugar = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    SugarPdv = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Sodium = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    SodiumPdv = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Protein = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    ProteinPdv = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    TotalCarbohydrates = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    TotalCarbohydratesPdv = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeNutrition", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_RecipeNutrition_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "CookBook",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeSteps",
                schema: "CookBook",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Instruction = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSteps", x => new { x.RecipeId, x.Order });
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "CookBook",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeTags",
                schema: "CookBook",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTags", x => new { x.RecipeId, x.TagId });
                    table.ForeignKey(
                        name: "FK_RecipeTags_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "CookBook",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "CookBook",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_Url",
                schema: "CookBook",
                table: "Images",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientName",
                schema: "CookBook",
                table: "Ingredients",
                column: "IngredientName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImages_ImageId",
                schema: "CookBook",
                table: "RecipeImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                schema: "CookBook",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_TagId",
                schema: "CookBook",
                table: "RecipeTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagName",
                schema: "CookBook",
                table: "Tags",
                column: "TagName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventOutbox",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "RecipeImages",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "RecipeIngredients",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "RecipeMetadata",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "RecipeNutrition",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "RecipeSteps",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "RecipeTags",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "Images",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "Ingredients",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "Recipes",
                schema: "CookBook");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "CookBook");
        }
    }
}
