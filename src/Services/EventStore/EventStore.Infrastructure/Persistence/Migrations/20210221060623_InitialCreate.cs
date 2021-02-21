using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CulinaCloud.EventStore.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "EventStore");

            migrationBuilder.CreateTable(
                name: "Aggregates",
                schema: "EventStore",
                columns: table => new
                {
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    AggregateType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aggregates", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "EventStore",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    Occurred = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RaisedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Details = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aggregates",
                schema: "EventStore");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "EventStore");
        }
    }
}
