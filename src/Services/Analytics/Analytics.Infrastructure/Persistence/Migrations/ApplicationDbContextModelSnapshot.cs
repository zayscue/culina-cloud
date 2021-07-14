﻿// <auto-generated />
using System;
using CulinaCloud.Analytics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CulinaCloud.Analytics.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Analytics")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CulinaCloud.Analytics.Domain.Entities.RecipePopularity", b =>
                {
                    b.Property<Guid>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("RecipeId");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("Created");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("CreatedBy");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("LastModified");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("LastModifiedBy");

                    b.Property<decimal>("RatingAverage")
                        .HasColumnType("numeric(7,5)")
                        .HasColumnName("RatingAverage");

                    b.Property<int>("RatingCount")
                        .HasColumnType("integer")
                        .HasColumnName("RatingCount");

                    b.Property<int>("RatingSum")
                        .HasColumnType("integer")
                        .HasColumnName("RatingSum");

                    b.Property<decimal>("RatingWeightedAverage")
                        .HasColumnType("numeric(7,5)")
                        .HasColumnName("RatingWeightedAverage");

                    b.Property<string>("Submitted")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)")
                        .HasColumnName("Submitted");

                    b.HasKey("RecipeId");

                    b.ToTable("RecipePopularity");
                });

            modelBuilder.Entity("CulinaCloud.Analytics.Domain.Entities.RecipeSimilarity", b =>
                {
                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uuid")
                        .HasColumnName("RecipeId");

                    b.Property<Guid>("SimilarRecipeId")
                        .HasColumnType("uuid")
                        .HasColumnName("SimilarRecipeId");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("Created");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("CreatedBy");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("LastModified");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("LastModifiedBy");

                    b.Property<decimal>("SimilarityScore")
                        .HasColumnType("numeric(7,5)")
                        .HasColumnName("SimilarityScore");

                    b.Property<string>("SimilarityType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("SimilarityType");

                    b.HasKey("RecipeId", "SimilarRecipeId");

                    b.ToTable("RecipeSimilarity");
                });
#pragma warning restore 612, 618
        }
    }
}
