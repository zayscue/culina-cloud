﻿// <auto-generated />
using System;
using System.Text.Json;
using CulinaCloud.EventStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CulinaCloud.EventStore.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("EventStore")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CulinaCloud.EventStore.Domain.Entities.Aggregate", b =>
                {
                    b.Property<Guid>("AggregateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("AggregateId");

                    b.Property<string>("AggregateType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("AggregateType");

                    b.Property<int>("Version")
                        .HasColumnType("integer")
                        .HasColumnName("Version");

                    b.HasKey("AggregateId");

                    b.ToTable("Aggregates");
                });

            modelBuilder.Entity("CulinaCloud.EventStore.Domain.Entities.Event", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("EventId");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uuid")
                        .HasColumnName("AggregateId");

                    b.Property<JsonDocument>("Data")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("Data");

                    b.Property<string>("Details")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Details");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("EventName");

                    b.Property<DateTimeOffset>("Occurred")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("Occurred");

                    b.Property<string>("RaisedBy")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("RaisedBy");

                    b.Property<int>("Version")
                        .HasColumnType("integer")
                        .HasColumnName("Version");

                    b.HasKey("EventId");

                    b.HasIndex("AggregateId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("CulinaCloud.EventStore.Domain.Entities.Event", b =>
                {
                    b.HasOne("CulinaCloud.EventStore.Domain.Entities.Aggregate", "Aggregate")
                        .WithMany("Events")
                        .HasForeignKey("AggregateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aggregate");
                });

            modelBuilder.Entity("CulinaCloud.EventStore.Domain.Entities.Aggregate", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
