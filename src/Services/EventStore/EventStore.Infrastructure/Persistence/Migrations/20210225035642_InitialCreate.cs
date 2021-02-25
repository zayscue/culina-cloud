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
                    EventName = table.Column<string>(type: "text", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    Occurred = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RaisedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Details = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_Aggregates_AggregateId",
                        column: x => x.AggregateId,
                        principalSchema: "EventStore",
                        principalTable: "Aggregates",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_AggregateId",
                schema: "EventStore",
                table: "Events",
                column: "AggregateId");

            const string storeEventStoredProcedure = @"
                create or replace procedure ""EventStore"".""StoreEvent""(
                    ""eventId"" uuid,
                    ""eventName"" text,
                    ""data"" jsonb,
                    ""occurred"" timestamptz,
                    ""raisedBy"" text,
                    ""details"" text,
                    ""aggregateId"" uuid,
                    ""aggregateType"" text
                )
                language plpgsql
                as
                $$
                declare
                    version integer;
                begin
                    if ""aggregateId"" is not null then
                        select ""Version""
                        into version
                        from ""EventStore"".""Aggregates""
                        where ""EventStore"".""Aggregates"".""AggregateId"" = ""aggregateId"";

                        if version is null then
                            version := 0;
                        end if;

                        version:= version + 1;
                    else
                        version:= 0;
                    end if;

                    if version = 1 then
                        insert into ""EventStore"".""Aggregates""(
                            ""AggregateId"",
                            ""AggregateType"",
                            ""Version""
                        ) values(
                            ""aggregateId"",
                            ""aggregateType"",
                            version
                        );
                    end if;

                    if version > 1 then
                        update ""EventStore"".""Aggregates""
                        set ""Version"" = version
                        where ""AggregateId"" = ""aggregateId"";
                    end if;

                    insert into ""EventStore"".""Events""(
                        ""EventId"",
                        ""EventName"",
                        ""AggregateId"",
                        ""Version"",
                        ""Data"",
                        ""Occurred"",
                        ""RaisedBy"",
                        ""Details""
                    ) values(
                        ""eventId"",
                        ""eventName"",
                        ""aggregateId"",
                        version,
                        ""data"",
                        ""occurred"",
                        ""raisedBy"",
                        ""details""
                    );

                    commit;
                end;
                $$;
            ";
            migrationBuilder.Sql(storeEventStoredProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string dropStoreEventStoredProcedure = @"
                drop procedure ""EventStore"".""StoreEvent"";
            ";

            migrationBuilder.Sql(dropStoreEventStoredProcedure);

            migrationBuilder.DropTable(
                name: "Events",
                schema: "EventStore");

            migrationBuilder.DropTable(
                name: "Aggregates",
                schema: "EventStore");
        }
    }
}
