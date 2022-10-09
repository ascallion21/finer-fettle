﻿// <auto-generated />
using System;
using FinerFettle.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinerFettle.Web.Migrations
{
    [DbContext(typeof(CoreContext))]
    partial class CoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EquipmentEquipmentGroup", b =>
                {
                    b.Property<int>("EquipmentGroupsId")
                        .HasColumnType("integer");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.HasKey("EquipmentGroupsId", "EquipmentId");

                    b.HasIndex("EquipmentId");

                    b.ToTable("equipment_group_equipment", (string)null);
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("equipment");

                    b.HasComment("Equipment used in an exercise");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.EquipmentGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Instruction")
                        .HasColumnType("text");

                    b.Property<bool>("IsWeight")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Required")
                        .HasColumnType("boolean");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VariationId");

                    b.ToTable("equipment_group");

                    b.HasComment("Equipment that can be switched out for one another");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PrimaryMuscles")
                        .HasColumnType("integer");

                    b.Property<int>("Proficiency")
                        .HasColumnType("integer");

                    b.Property<int>("SecondaryMuscles")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("exercise");

                    b.HasComment("Exercises listed on the website");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.ExercisePrerequisite", b =>
                {
                    b.Property<int>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<int>("PrerequisiteExerciseId")
                        .HasColumnType("integer");

                    b.HasKey("ExerciseId", "PrerequisiteExerciseId");

                    b.HasIndex("PrerequisiteExerciseId");

                    b.ToTable("exercise_prerequisite");

                    b.HasComment("Pre-requisite exercises for other exercises");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Intensity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("IntensityLevel")
                        .HasColumnType("integer");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VariationId");

                    b.ToTable("intensity");

                    b.HasComment("Intensity level of an exercise variation per user's strengthing preference");
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

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<int>("ExerciseType")
                        .HasColumnType("integer");

                    b.Property<int>("MuscleContractions")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SportsFocus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("variation");

                    b.HasComment("Variations of exercises");
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

                    b.ToTable("footnote");

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

                    b.Property<bool>("IsDeloadWeek")
                        .HasColumnType("boolean");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("newsletter");

                    b.HasComment("A day's workout routine");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.User.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("AcceptedTerms")
                        .HasColumnType("boolean");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EmailVerbosity")
                        .HasColumnType("integer");

                    b.Property<bool>("PrefersWeights")
                        .HasColumnType("boolean");

                    b.Property<int>("RecoveryMuscle")
                        .HasColumnType("integer");

                    b.Property<int>("RestDays")
                        .HasColumnType("integer");

                    b.Property<int>("SportsFocus")
                        .HasColumnType("integer");

                    b.Property<int>("StrengtheningPreference")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("user");

                    b.HasComment("User who signed up for the newsletter");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.User.UserEquipment", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "EquipmentId");

                    b.HasIndex("EquipmentId");

                    b.ToTable("user_equipment");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.User.UserExercise", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<bool>("Ignore")
                        .HasColumnType("boolean");

                    b.Property<DateOnly>("LastSeen")
                        .HasColumnType("date");

                    b.Property<int>("Progression")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "ExerciseId");

                    b.HasIndex("ExerciseId");

                    b.ToTable("user_exercise");

                    b.HasComment("User's progression level of an exercise");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.User.UserVariation", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("LastSeen")
                        .HasColumnType("date");

                    b.HasKey("UserId", "VariationId");

                    b.HasIndex("VariationId");

                    b.ToTable("user_variation");

                    b.HasComment("User's intensity stats");
                });

            modelBuilder.Entity("EquipmentEquipmentGroup", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.Exercise.EquipmentGroup", null)
                        .WithMany()
                        .HasForeignKey("EquipmentGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinerFettle.Web.Models.Exercise.Equipment", null)
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.EquipmentGroup", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.Exercise.Variation", "Variation")
                        .WithMany("EquipmentGroups")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.ExercisePrerequisite", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.Exercise.Exercise", "Exercise")
                        .WithMany("Prerequisites")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinerFettle.Web.Models.Exercise.Exercise", "PrerequisiteExercise")
                        .WithMany("Exercises")
                        .HasForeignKey("PrerequisiteExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("PrerequisiteExercise");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Intensity", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.Exercise.Variation", "Variation")
                        .WithMany("Intensities")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("FinerFettle.Web.Models.Exercise.Proficiency", "Proficiency", b1 =>
                        {
                            b1.Property<int>("IntensityId")
                                .HasColumnType("integer");

                            b1.Property<int?>("MaxReps")
                                .HasColumnType("integer");

                            b1.Property<int?>("MinReps")
                                .HasColumnType("integer");

                            b1.Property<int?>("Secs")
                                .HasColumnType("integer");

                            b1.Property<int>("Sets")
                                .HasColumnType("integer");

                            b1.HasKey("IntensityId");

                            b1.ToTable("intensity");

                            b1.WithOwner()
                                .HasForeignKey("IntensityId");
                        });

                    b.Navigation("Proficiency")
                        .IsRequired();

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Variation", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.Exercise.Exercise", "Exercise")
                        .WithMany("Variations")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("FinerFettle.Web.Models.Exercise.Progression", "Progression", b1 =>
                        {
                            b1.Property<int>("VariationId")
                                .HasColumnType("integer");

                            b1.Property<int?>("Max")
                                .HasColumnType("integer");

                            b1.Property<int?>("Min")
                                .HasColumnType("integer");

                            b1.HasKey("VariationId");

                            b1.ToTable("variation");

                            b1.WithOwner()
                                .HasForeignKey("VariationId");
                        });

                    b.Navigation("Exercise");

                    b.Navigation("Progression")
                        .IsRequired();
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Newsletter.Newsletter", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.OwnsOne("FinerFettle.Web.Models.Exercise.ExerciseRotaion", "ExerciseRotation", b1 =>
                        {
                            b1.Property<int>("NewsletterId")
                                .HasColumnType("integer");

                            b1.Property<int>("ExerciseType")
                                .HasColumnType("integer");

                            b1.Property<int>("MuscleGroups")
                                .HasColumnType("integer");

                            b1.Property<int>("id")
                                .HasColumnType("integer");

                            b1.HasKey("NewsletterId");

                            b1.ToTable("newsletter");

                            b1.WithOwner()
                                .HasForeignKey("NewsletterId");
                        });

                    b.Navigation("ExerciseRotation")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.User.UserEquipment", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.Exercise.Equipment", "Equipment")
                        .WithMany("UserEquipments")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinerFettle.Web.Models.User.User", "User")
                        .WithMany("UserEquipments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.User.UserExercise", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.Exercise.Exercise", "Exercise")
                        .WithMany("UserExercises")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinerFettle.Web.Models.User.User", "User")
                        .WithMany("UserExercises")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.User.UserVariation", b =>
                {
                    b.HasOne("FinerFettle.Web.Models.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinerFettle.Web.Models.Exercise.Variation", "Variation")
                        .WithMany("UserVariations")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Equipment", b =>
                {
                    b.Navigation("UserEquipments");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Exercise", b =>
                {
                    b.Navigation("Exercises");

                    b.Navigation("Prerequisites");

                    b.Navigation("UserExercises");

                    b.Navigation("Variations");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.Exercise.Variation", b =>
                {
                    b.Navigation("EquipmentGroups");

                    b.Navigation("Intensities");

                    b.Navigation("UserVariations");
                });

            modelBuilder.Entity("FinerFettle.Web.Models.User.User", b =>
                {
                    b.Navigation("UserEquipments");

                    b.Navigation("UserExercises");
                });
#pragma warning restore 612, 618
        }
    }
}
