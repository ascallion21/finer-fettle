﻿// <auto-generated />
using System;
using FinerFettle.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinerFettle.Web.Migrations
{
    [DbContext(typeof(CoreContext))]
    [Migration("20220717233414_ReSaveNewsletters")]
    partial class ReSaveNewsletters
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ExerciseType")
                        .HasColumnType("integer");

                    b.Property<int>("Muscles")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Exercise");

                    b.HasComment("Exercises listed on the website");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Variation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Equipment")
                        .HasColumnType("integer");

                    b.Property<int?>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<string>("Instruction")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MuscleContractions")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ProficiencyReps")
                        .HasColumnType("integer");

                    b.Property<int?>("ProficiencySecs")
                        .HasColumnType("integer");

                    b.Property<int?>("ProficiencySets")
                        .HasColumnType("integer");

                    b.Property<int?>("Progression")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("Variation");

                    b.HasComment("Progressions of an exercise");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Footnotes.Footnote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Footnote");

                    b.HasComment("Sage advice");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Newsletter.Newsletter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("Equipment")
                        .HasColumnType("integer");

                    b.Property<int>("ExerciseType")
                        .HasColumnType("integer");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Newsletter");

                    b.HasComment("A day's workout routine");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.User.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Equipment")
                        .HasColumnType("integer");

                    b.Property<bool>("NeedsRest")
                        .HasColumnType("boolean");

                    b.Property<int?>("Progression")
                        .HasColumnType("integer");

                    b.Property<bool>("WantsStrength")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasComment("User who signed up for the newsletter");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Variation", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.Exercise.Exercise", null)
                        .WithMany("Variations")
                        .HasForeignKey("ExerciseId");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Newsletter.Newsletter", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Exercise", b =>
                {
                    b.Navigation("Variations");
                });
#pragma warning restore 612, 618
        }
    }
}
