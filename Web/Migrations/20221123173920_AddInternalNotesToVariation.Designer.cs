﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web.Data;

#nullable disable

namespace Web.Migrations
{
    [DbContext(typeof(CoreContext))]
    [Migration("20221123173920_AddInternalNotesToVariation")]
    partial class AddInternalNotesToVariation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
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

            modelBuilder.Entity("Web.Entities.Equipment.Equipment", b =>
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

                    b.ToTable("equipment", t =>
                        {
                            t.HasComment("Equipment used in an exercise");
                        });
                });

            modelBuilder.Entity("Web.Entities.Equipment.EquipmentGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<string>("Instruction")
                        .HasColumnType("text");

                    b.Property<bool>("IsWeight")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("VariationId");

                    b.ToTable("equipment_group", t =>
                        {
                            t.HasComment("Equipment that can be switched out for one another");
                        });
                });

            modelBuilder.Entity("Web.Entities.Exercise.Exercise", b =>
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

                    b.Property<int>("Proficiency")
                        .HasColumnType("integer");

                    b.Property<int>("RecoveryMuscle")
                        .HasColumnType("integer");

                    b.Property<int>("SportsFocus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("exercise", t =>
                        {
                            t.HasComment("Exercises listed on the website");
                        });
                });

            modelBuilder.Entity("Web.Entities.Exercise.ExercisePrerequisite", b =>
                {
                    b.Property<int>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<int>("PrerequisiteExerciseId")
                        .HasColumnType("integer");

                    b.HasKey("ExerciseId", "PrerequisiteExerciseId");

                    b.HasIndex("PrerequisiteExerciseId");

                    b.ToTable("exercise_prerequisite", t =>
                        {
                            t.HasComment("Pre-requisite exercises for other exercises");
                        });
                });

            modelBuilder.Entity("Web.Entities.Exercise.ExerciseVariation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<int>("ExerciseType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsBonus")
                        .HasColumnType("boolean");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VariationId");

                    b.HasIndex("ExerciseId", "VariationId")
                        .IsUnique();

                    b.ToTable("exercise_variation", t =>
                        {
                            t.HasComment("Variation progressions for an exercise track");
                        });
                });

            modelBuilder.Entity("Web.Entities.Exercise.Intensity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<int>("IntensityLevel")
                        .HasColumnType("integer");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VariationId");

                    b.ToTable("intensity", t =>
                        {
                            t.HasComment("Intensity level of an exercise variation per user's strengthing preference");
                        });
                });

            modelBuilder.Entity("Web.Entities.Exercise.Variation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("AntiGravity")
                        .HasColumnType("boolean");

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<string>("ImageCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MovementPattern")
                        .HasColumnType("integer");

                    b.Property<int>("MuscleContractions")
                        .HasColumnType("integer");

                    b.Property<int>("MuscleMovement")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<int>("StabilityMuscles")
                        .HasColumnType("integer");

                    b.Property<int>("StrengthMuscles")
                        .HasColumnType("integer");

                    b.Property<int>("StretchMuscles")
                        .HasColumnType("integer");

                    b.Property<bool>("Unilateral")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("variation", t =>
                        {
                            t.HasComment("Variations of exercises");
                        });
                });

            modelBuilder.Entity("Web.Entities.Footnote.Footnote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("footnote", t =>
                        {
                            t.HasComment("Sage advice");
                        });
                });

            modelBuilder.Entity("Web.Entities.Newsletter.Newsletter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("Frequency")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeloadWeek")
                        .HasColumnType("boolean");

                    b.Property<int>("StrengtheningPreference")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("newsletter", t =>
                        {
                            t.HasComment("A day's workout routine");
                        });
                });

            modelBuilder.Entity("Web.Entities.Newsletter.NewsletterVariation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("NewsletterId")
                        .HasColumnType("integer");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("NewsletterId");

                    b.HasIndex("VariationId");

                    b.ToTable("newsletter_variation", t =>
                        {
                            t.HasComment("A day's workout routine");
                        });
                });

            modelBuilder.Entity("Web.Entities.User.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("AcceptedTerms")
                        .HasColumnType("boolean");

                    b.Property<DateOnly>("CreatedDate")
                        .HasColumnType("date");

                    b.Property<int>("DeloadAfterEveryXWeeks")
                        .HasColumnType("integer");

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EmailVerbosity")
                        .HasColumnType("integer");

                    b.Property<int>("Frequency")
                        .HasColumnType("integer");

                    b.Property<bool>("IncludeBonus")
                        .HasColumnType("boolean");

                    b.Property<DateOnly?>("LastActive")
                        .HasColumnType("date");

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

                    b.ToTable("user", t =>
                        {
                            t.HasComment("User who signed up for the newsletter");
                        });
                });

            modelBuilder.Entity("Web.Entities.User.UserEquipment", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "EquipmentId");

                    b.HasIndex("EquipmentId");

                    b.ToTable("user_equipment");
                });

            modelBuilder.Entity("Web.Entities.User.UserExercise", b =>
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

                    b.ToTable("user_exercise", t =>
                        {
                            t.HasComment("User's progression level of an exercise");
                        });
                });

            modelBuilder.Entity("Web.Entities.User.UserExerciseVariation", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("ExerciseVariationId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("LastSeen")
                        .HasColumnType("date");

                    b.HasKey("UserId", "ExerciseVariationId");

                    b.HasIndex("ExerciseVariationId");

                    b.ToTable("user_exercise_variation", t =>
                        {
                            t.HasComment("User's progression level of an exercise variation");
                        });
                });

            modelBuilder.Entity("Web.Entities.User.UserToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<DateOnly>("Expires")
                        .HasColumnType("date");

                    b.HasKey("UserId", "Token");

                    b.ToTable("user_token", t =>
                        {
                            t.HasComment("Auth tokens for a user");
                        });
                });

            modelBuilder.Entity("Web.Entities.User.UserVariation", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("LastSeen")
                        .HasColumnType("date");

                    b.HasKey("UserId", "VariationId");

                    b.HasIndex("VariationId");

                    b.ToTable("user_variation", t =>
                        {
                            t.HasComment("User's intensity stats");
                        });
                });

            modelBuilder.Entity("EquipmentEquipmentGroup", b =>
                {
                    b.HasOne("Web.Entities.Equipment.EquipmentGroup", null)
                        .WithMany()
                        .HasForeignKey("EquipmentGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.Equipment.Equipment", null)
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Web.Entities.Equipment.EquipmentGroup", b =>
                {
                    b.HasOne("Web.Entities.Equipment.EquipmentGroup", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.HasOne("Web.Entities.Exercise.Variation", "Variation")
                        .WithMany("EquipmentGroups")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("Web.Entities.Exercise.ExercisePrerequisite", b =>
                {
                    b.HasOne("Web.Entities.Exercise.Exercise", "Exercise")
                        .WithMany("Prerequisites")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.Exercise.Exercise", "PrerequisiteExercise")
                        .WithMany("PrerequisiteExercises")
                        .HasForeignKey("PrerequisiteExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("PrerequisiteExercise");
                });

            modelBuilder.Entity("Web.Entities.Exercise.ExerciseVariation", b =>
                {
                    b.HasOne("Web.Entities.Exercise.Exercise", "Exercise")
                        .WithMany("ExerciseVariations")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.Exercise.Variation", "Variation")
                        .WithMany("ExerciseVariations")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Web.Entities.Exercise.Progression", "Progression", b1 =>
                        {
                            b1.Property<int>("ExerciseVariationId")
                                .HasColumnType("integer");

                            b1.Property<int?>("Max")
                                .HasColumnType("integer");

                            b1.Property<int?>("Min")
                                .HasColumnType("integer");

                            b1.HasKey("ExerciseVariationId");

                            b1.ToTable("exercise_variation");

                            b1.WithOwner()
                                .HasForeignKey("ExerciseVariationId");
                        });

                    b.Navigation("Exercise");

                    b.Navigation("Progression")
                        .IsRequired();

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("Web.Entities.Exercise.Intensity", b =>
                {
                    b.HasOne("Web.Entities.Exercise.Variation", "Variation")
                        .WithMany("Intensities")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Web.Entities.Exercise.Proficiency", "Proficiency", b1 =>
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

            modelBuilder.Entity("Web.Entities.Newsletter.Newsletter", b =>
                {
                    b.HasOne("Web.Entities.User.User", "User")
                        .WithMany("Newsletters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Web.Entities.Newsletter.NewsletterRotation", "NewsletterRotation", b1 =>
                        {
                            b1.Property<int>("NewsletterId")
                                .HasColumnType("integer");

                            b1.Property<int>("Id")
                                .HasColumnType("integer");

                            b1.Property<int>("IntensityLevel")
                                .HasColumnType("integer");

                            b1.Property<int>("MovementPatterns")
                                .HasColumnType("integer");

                            b1.Property<int>("MuscleGroups")
                                .HasColumnType("integer");

                            b1.Property<int>("NewsletterType")
                                .HasColumnType("integer");

                            b1.HasKey("NewsletterId");

                            b1.ToTable("newsletter");

                            b1.WithOwner()
                                .HasForeignKey("NewsletterId");
                        });

                    b.Navigation("NewsletterRotation")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Entities.Newsletter.NewsletterVariation", b =>
                {
                    b.HasOne("Web.Entities.Newsletter.Newsletter", "Newsletter")
                        .WithMany("NewsletterVariations")
                        .HasForeignKey("NewsletterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.Exercise.Variation", "Variation")
                        .WithMany("NewsletterVariations")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Newsletter");

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("Web.Entities.User.UserEquipment", b =>
                {
                    b.HasOne("Web.Entities.Equipment.Equipment", "Equipment")
                        .WithMany("UserEquipments")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.User.User", "User")
                        .WithMany("UserEquipments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Entities.User.UserExercise", b =>
                {
                    b.HasOne("Web.Entities.Exercise.Exercise", "Exercise")
                        .WithMany("UserExercises")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.User.User", "User")
                        .WithMany("UserExercises")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Entities.User.UserExerciseVariation", b =>
                {
                    b.HasOne("Web.Entities.Exercise.ExerciseVariation", "ExerciseVariation")
                        .WithMany("UserExerciseVariations")
                        .HasForeignKey("ExerciseVariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.User.User", "User")
                        .WithMany("UserExerciseVariations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExerciseVariation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Entities.User.UserToken", b =>
                {
                    b.HasOne("Web.Entities.User.User", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Entities.User.UserVariation", b =>
                {
                    b.HasOne("Web.Entities.User.User", "User")
                        .WithMany("UserVariations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Entities.Exercise.Variation", "Variation")
                        .WithMany("UserVariations")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("Web.Entities.Equipment.Equipment", b =>
                {
                    b.Navigation("UserEquipments");
                });

            modelBuilder.Entity("Web.Entities.Equipment.EquipmentGroup", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("Web.Entities.Exercise.Exercise", b =>
                {
                    b.Navigation("ExerciseVariations");

                    b.Navigation("PrerequisiteExercises");

                    b.Navigation("Prerequisites");

                    b.Navigation("UserExercises");
                });

            modelBuilder.Entity("Web.Entities.Exercise.ExerciseVariation", b =>
                {
                    b.Navigation("UserExerciseVariations");
                });

            modelBuilder.Entity("Web.Entities.Exercise.Variation", b =>
                {
                    b.Navigation("EquipmentGroups");

                    b.Navigation("ExerciseVariations");

                    b.Navigation("Intensities");

                    b.Navigation("NewsletterVariations");

                    b.Navigation("UserVariations");
                });

            modelBuilder.Entity("Web.Entities.Newsletter.Newsletter", b =>
                {
                    b.Navigation("NewsletterVariations");
                });

            modelBuilder.Entity("Web.Entities.User.User", b =>
                {
                    b.Navigation("Newsletters");

                    b.Navigation("UserEquipments");

                    b.Navigation("UserExerciseVariations");

                    b.Navigation("UserExercises");

                    b.Navigation("UserTokens");

                    b.Navigation("UserVariations");
                });
#pragma warning restore 612, 618
        }
    }
}
