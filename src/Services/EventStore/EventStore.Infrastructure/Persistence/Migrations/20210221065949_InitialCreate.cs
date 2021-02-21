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

            const string addEventStoredProcedure = @"
                create or replace procedure ""EventStore"".""AddEvent""(
                    ""eventId"" uuid,
                    ""data"" json,
                    ""occurred"" timestamp,
                    ""raisedBy"" character varying,
                    ""details"" character varying,
                    ""aggregateId"" uuid,
                    ""aggregateType"" character varying
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

                    insert into ""EventStore"".""Events""(
                        ""EventId"",
                        ""AggregateId"",
                        ""Version"",
                        ""Data"",
                        ""Occurred"",
                        ""RaisedBy"",
                        ""Details""
                    ) values(
                        ""eventId"",
                        ""aggregateId"",
                        ""version"",
                        ""data"",
                        ""occurred"",
                        ""raisedBy"",
                        ""details""
                    );

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

                    commit;
                end;
                $$;
            ";
            migrationBuilder.Sql(addEventStoredProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string dropAddEventStoredProcedure = @"
                drop procedure ""EventStore"".""AddEvent"";
            ";
            migrationBuilder.Sql(dropAddEventStoredProcedure);

            migrationBuilder.DropTable(
                name: "Aggregates",
                schema: "EventStore");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "EventStore");
        }
    }
}
