using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                    Submitted = table.Column<DateOnly>(type: "DATE", nullable: false),
                    RatingCount = table.Column<int>(type: "integer", nullable: false),
                    RatingSum = table.Column<int>(type: "integer", nullable: false),
                    RatingAverage = table.Column<decimal>(type: "numeric(18,16)", nullable: false),
                    RatingWeightedAverage = table.Column<decimal>(type: "numeric(18,16)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    SimilarityScore = table.Column<decimal>(type: "numeric(18,16)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSimilarity", x => new { x.RecipeId, x.SimilarRecipeId, x.SimilarityType });
                });
            const string createCreateRecipePopularityFunction = @"
                create or replace function ""Analytics"".""CreateRecipePopularity""(
                    recipeId uuid,
                    submitted date,
                    count int,
                    sum int,
                    average decimal,
                    createdBy varchar,
                    created timestamp without time zone default(now() at time zone 'utc')
                ) returns decimal
                language plpgsql
                as
                $$
                declare
                    mean decimal;
                    minimumCount decimal;
                    weightedAverage decimal;
                begin
                    with c as
                    (
                        select
                            ""RecipeId"",
                            ""RatingCount"",
                            rank() over(order by ""RatingCount"") as rk,
                            count(*) over() as nr
                       from ""Analytics"".""RecipePopularity""
                    ),
                    s as
                    (
                        select (1 - (10000 / cast(count(""RatingCount"") as decimal))) as SumOfRatingCount
                        from ""Analytics"".""RecipePopularity""
                    )
                    select a.""RatingCount"" into minimumCount
                    from(
                        select *
                        from(select ""RecipeId"",
                                     ""RatingCount"",
                                     (1.0 * (cast(rk as decimal) - 1) / (cast(nr as decimal) - 1)) as PercentileRank,
                                     ((cast(rk as decimal) - 1) / (cast(nr as decimal) / 5) + 1) as Quintile
                              from c) as b
                        where b.PercentileRank >= (select SumOfRatingCount from s)
                        order by Quintile desc
                    ) as a
                    order by a.Quintile
                    limit 1;

                    select cast(sum(""RatingSum"") as decimal)/ cast(sum(""RatingCount"") as decimal) into mean
                    from ""Analytics"".""RecipePopularity"";

                    select(((count * average) + (mean * minimumCount)) / (count + minimumCount)) into weightedAverage;

                    insert into ""Analytics"".""RecipePopularity""(""RecipeId"", ""Submitted"", ""RatingCount"", ""RatingSum"", ""RatingAverage"", ""RatingWeightedAverage"", ""Created"", ""CreatedBy"")
                    values(recipeId, submitted, count, sum, average, weightedAverage, created, createdBy);

                    return weightedAverage;
                end
                $$;
            ";
            migrationBuilder.Sql(createCreateRecipePopularityFunction);

            const string createUpdateRecipePopularityFunction = @"
                create or replace function ""Analytics"".""UpdateRecipePopularity""(
                    recipeId uuid,
                    rating int,
                    lastModifiedBy varchar,
                    lastModified timestamp without time zone default(now() at time zone 'utc')
                ) returns decimal
                language plpgsql
                as
                $$
                declare
                    count int;
                    sum int;
                    average decimal;
                    mean decimal;
                    minimumCount decimal;
                    weightedAverage decimal;
                begin
                    with c as
                    (
                        select
                            ""RecipeId"",
                            ""RatingCount"",
                            rank() over(order by ""RatingCount"") as rk,
                            count(*) over() as nr
                       from ""Analytics"".""RecipePopularity""
                    ),
                    s as
                    (
                        select (1 - (10000 / cast(count(""RatingCount"") as decimal))) as SumOfRatingCount
                        from ""Analytics"".""RecipePopularity""
                    )
                    select a.""RatingCount"" into minimumCount
                    from(
                        select *
                        from(select ""RecipeId"",
                                     ""RatingCount"",
                                     (1.0 * (cast(rk as decimal) - 1) / (cast(nr as decimal) - 1)) as PercentileRank,
                                     ((cast(rk as decimal) - 1) / (cast(nr as decimal) / 5) + 1) as Quintile
                              from c) as b
                        where b.PercentileRank >= (select SumOfRatingCount from s)
                        order by Quintile desc
                    ) as a
                    order by a.Quintile
                    limit 1;

                    select cast(sum(""RatingSum"") as decimal)/ cast(sum(""RatingCount"") as decimal) into mean
                    from ""Analytics"".""RecipePopularity"";

                    select into sum, count, average
                        (""RatingSum"" + rating),
                        (""RatingCount"" + 1),
                        cast((""RatingSum"" + rating) as decimal) / cast((""RatingCount"" + 1) as decimal)
                    from ""Analytics"".""RecipePopularity""
                    where ""RecipeId"" = recipeId;

                    select(((cast(count as decimal) * average) + (mean * minimumCount)) / (cast(count as decimal) + minimumCount))
                    into weightedAverage;

                    update ""Analytics"".""RecipePopularity""
                    set ""RatingCount"" = count,
                        ""RatingSum"" = sum,
                        ""RatingAverage"" = average,
                        ""RatingWeightedAverage"" = weightedAverage,
                        ""LastModified"" = lastModified,
                        ""LastModifiedBy"" = lastModifiedBy
                    where ""RecipeId"" = recipeId;

                    return weightedAverage;
                end
                $$;
            ";
            migrationBuilder.Sql(createUpdateRecipePopularityFunction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string dropCreateRecipePopularityFunction = @"
                drop function if exists ""Analytics"".""CreateRecipePopularity"";
            ";

            migrationBuilder.Sql(dropCreateRecipePopularityFunction);

            const string dropUpdateRecipePopularityFunction = @"
                drop function if exists ""Analytics"".""UpdateRecipePopularity"";
            ";

            migrationBuilder.Sql(dropUpdateRecipePopularityFunction);

            migrationBuilder.DropTable(
                name: "RecipePopularity",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "RecipeSimilarity",
                schema: "Analytics");
        }
    }
}
