using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CulinaCloud.Interactions.Infrastructure.Persistence.Migrations
{
    public partial class Interactions_InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Interactions");

            migrationBuilder.CreateTable(
                name: "EventOutbox",
                schema: "Interactions",
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
                name: "Reviews",
                schema: "Interactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_RecipeId",
                schema: "Interactions",
                table: "Reviews",
                columns: new[] { "UserId", "RecipeId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventOutbox",
                schema: "Interactions");

            migrationBuilder.DropTable(
                name: "Reviews",
                schema: "Interactions");
        }
    }
}
