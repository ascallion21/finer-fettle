﻿// <auto-generated />
using System;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web.Migrations
{
    [DbContext(typeof(CoreContext))]
    partial class CoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Data.Entities.Equipment.Equipment", b =>
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

            modelBuilder.Entity("Data.Entities.Equipment.Instruction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<string>("Link")
                        .HasColumnType("text");

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

                    b.ToTable("instruction", t =>
                        {
                            t.HasComment("Equipment that can be switched out for one another");
                        });
                });

            modelBuilder.Entity("Data.Entities.Equipment.InstructionLocation", b =>
                {
                    b.Property<int>("Location")
                        .HasColumnType("integer");

                    b.Property<int>("InstructionId")
                        .HasColumnType("integer");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Location", "InstructionId");

                    b.HasIndex("InstructionId");

                    b.ToTable("instruction_location", t =>
                        {
                            t.HasComment("Instructions that can be switched out for one another");
                        });
                });

            modelBuilder.Entity("Data.Entities.Exercise.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<int>("Groups")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<int>("Proficiency")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("exercise", t =>
                        {
                            t.HasComment("Exercises listed on the website");
                        });
                });

            modelBuilder.Entity("Data.Entities.Exercise.ExercisePrerequisite", b =>
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

            modelBuilder.Entity("Data.Entities.Exercise.ExerciseVariation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<int>("ExerciseType")
                        .HasColumnType("integer");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<int>("SportsFocus")
                        .HasColumnType("integer");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VariationId");

                    b.HasIndex("ExerciseId", "VariationId");

                    b.ToTable("exercise_variation", t =>
                        {
                            t.HasComment("Variation progressions for an exercise track");
                        });
                });

            modelBuilder.Entity("Data.Entities.Exercise.Intensity", b =>
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

            modelBuilder.Entity("Data.Entities.Exercise.Variation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AnimatedImage")
                        .HasColumnType("text");

                    b.Property<bool>("AntiGravity")
                        .HasColumnType("boolean");

                    b.Property<int?>("DefaultInstructionId")
                        .HasColumnType("integer");

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<int>("ExerciseFocus")
                        .HasColumnType("integer");

                    b.Property<bool>("IsWeighted")
                        .HasColumnType("boolean");

                    b.Property<int>("MobilityJoints")
                        .HasColumnType("integer");

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

                    b.Property<int>("SecondaryMuscles")
                        .HasColumnType("integer");

                    b.Property<string>("StaticImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StrengthMuscles")
                        .HasColumnType("integer");

                    b.Property<int>("StretchMuscles")
                        .HasColumnType("integer");

                    b.Property<bool>("Unilateral")
                        .HasColumnType("boolean");

                    b.Property<bool>("UseCaution")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("DefaultInstructionId");

                    b.ToTable("variation", t =>
                        {
                            t.HasComment("Variations of exercises");
                        });
                });

            modelBuilder.Entity("Data.Entities.Footnote.Footnote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("footnote", t =>
                        {
                            t.HasComment("Sage advice");
                        });
                });

            modelBuilder.Entity("Data.Entities.Newsletter.Newsletter", b =>
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

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("newsletter", t =>
                        {
                            t.HasComment("A day's workout routine");
                        });
                });

            modelBuilder.Entity("Data.Entities.Newsletter.NewsletterExerciseVariation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseVariationId")
                        .HasColumnType("integer");

                    b.Property<int?>("IntensityLevel")
                        .HasColumnType("integer");

                    b.Property<int>("NewsletterId")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<int>("Section")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseVariationId");

                    b.HasIndex("NewsletterId");

                    b.ToTable("newsletter_exercise_variation", t =>
                        {
                            t.HasComment("A day's workout routine");
                        });
                });

            modelBuilder.Entity("Data.Entities.User.User", b =>
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

                    b.Property<int>("Features")
                        .HasColumnType("integer");

                    b.Property<int>("FootnoteType")
                        .HasColumnType("integer");

                    b.Property<int>("Frequency")
                        .HasColumnType("integer");

                    b.Property<int>("IntensityLevel")
                        .HasColumnType("integer");

                    b.Property<DateOnly?>("LastActive")
                        .HasColumnType("date");

                    b.Property<int>("MobilityMuscles")
                        .HasColumnType("integer");

                    b.Property<int>("PrehabFocus")
                        .HasColumnType("integer");

                    b.Property<int>("RefreshAccessoryEveryXWeeks")
                        .HasColumnType("integer");

                    b.Property<int>("RefreshFunctionalEveryXWeeks")
                        .HasColumnType("integer");

                    b.Property<int>("RehabFocus")
                        .HasColumnType("integer");

                    b.Property<DateOnly?>("SeasonedDate")
                        .HasColumnType("date");

                    b.Property<int>("SendDays")
                        .HasColumnType("integer");

                    b.Property<int>("SendHour")
                        .HasColumnType("integer");

                    b.Property<bool>("SendMobilityWorkouts")
                        .HasColumnType("boolean");

                    b.Property<bool>("ShowStaticImages")
                        .HasColumnType("boolean");

                    b.Property<int>("SportsFocus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("user", t =>
                        {
                            t.HasComment("User who signed up for the newsletter");
                        });
                });

            modelBuilder.Entity("Data.Entities.User.UserEquipment", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "EquipmentId");

                    b.HasIndex("EquipmentId");

                    b.ToTable("user_equipment");
                });

            modelBuilder.Entity("Data.Entities.User.UserExercise", b =>
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

                    b.Property<DateOnly?>("RefreshAfter")
                        .HasColumnType("date");

                    b.HasKey("UserId", "ExerciseId");

                    b.HasIndex("ExerciseId");

                    b.ToTable("user_exercise", t =>
                        {
                            t.HasComment("User's progression level of an exercise");
                        });
                });

            modelBuilder.Entity("Data.Entities.User.UserExerciseVariation", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("ExerciseVariationId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("LastSeen")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("RefreshAfter")
                        .HasColumnType("date");

                    b.HasKey("UserId", "ExerciseVariationId");

                    b.HasIndex("ExerciseVariationId");

                    b.ToTable("user_exercise_variation", t =>
                        {
                            t.HasComment("User's progression level of an exercise variation");
                        });
                });

            modelBuilder.Entity("Data.Entities.User.UserFrequency", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "Id");

                    b.ToTable("user_frequency");
                });

            modelBuilder.Entity("Data.Entities.User.UserMuscle", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("MuscleGroup")
                        .HasColumnType("integer");

                    b.Property<int>("End")
                        .HasColumnType("integer");

                    b.Property<int>("Start")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "MuscleGroup");

                    b.ToTable("user_muscle");
                });

            modelBuilder.Entity("Data.Entities.User.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Expires")
                        .HasColumnType("date");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "Token");

                    b.ToTable("user_token", t =>
                        {
                            t.HasComment("Auth tokens for a user");
                        });
                });

            modelBuilder.Entity("Data.Entities.User.UserVariation", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.Property<bool>("Ignore")
                        .HasColumnType("boolean");

                    b.Property<int>("Pounds")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "VariationId");

                    b.HasIndex("VariationId");

                    b.ToTable("user_variation", t =>
                        {
                            t.HasComment("User's intensity stats");
                        });
                });

            modelBuilder.Entity("EquipmentInstruction", b =>
                {
                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.Property<int>("InstructionsId")
                        .HasColumnType("integer");

                    b.HasKey("EquipmentId", "InstructionsId");

                    b.HasIndex("InstructionsId");

                    b.ToTable("instruction_equipment", (string)null);
                });

            modelBuilder.Entity("Data.Entities.Equipment.Instruction", b =>
                {
                    b.HasOne("Data.Entities.Equipment.Instruction", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.HasOne("Data.Entities.Exercise.Variation", "Variation")
                        .WithMany("Instructions")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("Data.Entities.Equipment.InstructionLocation", b =>
                {
                    b.HasOne("Data.Entities.Equipment.Instruction", "Instruction")
                        .WithMany("Locations")
                        .HasForeignKey("InstructionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instruction");
                });

            modelBuilder.Entity("Data.Entities.Exercise.ExercisePrerequisite", b =>
                {
                    b.HasOne("Data.Entities.Exercise.Exercise", "Exercise")
                        .WithMany("Prerequisites")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Exercise.Exercise", "PrerequisiteExercise")
                        .WithMany("PrerequisiteExercises")
                        .HasForeignKey("PrerequisiteExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("PrerequisiteExercise");
                });

            modelBuilder.Entity("Data.Entities.Exercise.ExerciseVariation", b =>
                {
                    b.HasOne("Data.Entities.Exercise.Exercise", "Exercise")
                        .WithMany("ExerciseVariations")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Exercise.Variation", "Variation")
                        .WithMany("ExerciseVariations")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Data.Entities.Exercise.Progression", "Progression", b1 =>
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

            modelBuilder.Entity("Data.Entities.Exercise.Intensity", b =>
                {
                    b.HasOne("Data.Entities.Exercise.Variation", "Variation")
                        .WithMany("Intensities")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Data.Entities.Exercise.Proficiency", "Proficiency", b1 =>
                        {
                            b1.Property<int>("IntensityId")
                                .HasColumnType("integer");

                            b1.Property<int?>("MaxReps")
                                .HasColumnType("integer");

                            b1.Property<int?>("MaxSecs")
                                .HasColumnType("integer");

                            b1.Property<int?>("MinReps")
                                .HasColumnType("integer");

                            b1.Property<int?>("MinSecs")
                                .HasColumnType("integer");

                            b1.Property<int?>("Sets")
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

            modelBuilder.Entity("Data.Entities.Exercise.Variation", b =>
                {
                    b.HasOne("Data.Entities.Equipment.Instruction", "DefaultInstruction")
                        .WithMany()
                        .HasForeignKey("DefaultInstructionId");

                    b.Navigation("DefaultInstruction");
                });

            modelBuilder.Entity("Data.Entities.Newsletter.Newsletter", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("Newsletters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Data.Entities.Newsletter.NewsletterRotation", "NewsletterRotation", b1 =>
                        {
                            b1.Property<int>("NewsletterId")
                                .HasColumnType("integer");

                            b1.Property<int>("Id")
                                .HasColumnType("integer");

                            b1.Property<int>("MovementPatterns")
                                .HasColumnType("integer");

                            b1.Property<int>("MuscleGroups")
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

            modelBuilder.Entity("Data.Entities.Newsletter.NewsletterExerciseVariation", b =>
                {
                    b.HasOne("Data.Entities.Exercise.ExerciseVariation", "ExerciseVariation")
                        .WithMany("NewsletterExerciseVariations")
                        .HasForeignKey("ExerciseVariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Newsletter.Newsletter", "Newsletter")
                        .WithMany("NewsletterExerciseVariations")
                        .HasForeignKey("NewsletterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExerciseVariation");

                    b.Navigation("Newsletter");
                });

            modelBuilder.Entity("Data.Entities.User.UserEquipment", b =>
                {
                    b.HasOne("Data.Entities.Equipment.Equipment", "Equipment")
                        .WithMany("UserEquipments")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserEquipments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.User.UserExercise", b =>
                {
                    b.HasOne("Data.Entities.Exercise.Exercise", "Exercise")
                        .WithMany("UserExercises")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserExercises")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.User.UserExerciseVariation", b =>
                {
                    b.HasOne("Data.Entities.Exercise.ExerciseVariation", "ExerciseVariation")
                        .WithMany("UserExerciseVariations")
                        .HasForeignKey("ExerciseVariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserExerciseVariations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExerciseVariation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.User.UserFrequency", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserFrequencies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Data.Entities.Newsletter.NewsletterRotation", "Rotation", b1 =>
                        {
                            b1.Property<int>("UserFrequencyUserId")
                                .HasColumnType("integer");

                            b1.Property<int>("UserFrequencyId")
                                .HasColumnType("integer");

                            b1.Property<int>("Id")
                                .HasColumnType("integer");

                            b1.Property<int>("MovementPatterns")
                                .HasColumnType("integer");

                            b1.Property<int>("MuscleGroups")
                                .HasColumnType("integer");

                            b1.HasKey("UserFrequencyUserId", "UserFrequencyId");

                            b1.ToTable("user_frequency");

                            b1.WithOwner()
                                .HasForeignKey("UserFrequencyUserId", "UserFrequencyId");
                        });

                    b.Navigation("Rotation")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.User.UserMuscle", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserMuscles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.User.UserToken", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.User.UserVariation", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserVariations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Exercise.Variation", "Variation")
                        .WithMany("UserVariations")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Variation");
                });

            modelBuilder.Entity("EquipmentInstruction", b =>
                {
                    b.HasOne("Data.Entities.Equipment.Equipment", null)
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Equipment.Instruction", null)
                        .WithMany()
                        .HasForeignKey("InstructionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.Equipment.Equipment", b =>
                {
                    b.Navigation("UserEquipments");
                });

            modelBuilder.Entity("Data.Entities.Equipment.Instruction", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Locations");
                });

            modelBuilder.Entity("Data.Entities.Exercise.Exercise", b =>
                {
                    b.Navigation("ExerciseVariations");

                    b.Navigation("PrerequisiteExercises");

                    b.Navigation("Prerequisites");

                    b.Navigation("UserExercises");
                });

            modelBuilder.Entity("Data.Entities.Exercise.ExerciseVariation", b =>
                {
                    b.Navigation("NewsletterExerciseVariations");

                    b.Navigation("UserExerciseVariations");
                });

            modelBuilder.Entity("Data.Entities.Exercise.Variation", b =>
                {
                    b.Navigation("ExerciseVariations");

                    b.Navigation("Instructions");

                    b.Navigation("Intensities");

                    b.Navigation("UserVariations");
                });

            modelBuilder.Entity("Data.Entities.Newsletter.Newsletter", b =>
                {
                    b.Navigation("NewsletterExerciseVariations");
                });

            modelBuilder.Entity("Data.Entities.User.User", b =>
                {
                    b.Navigation("Newsletters");

                    b.Navigation("UserEquipments");

                    b.Navigation("UserExerciseVariations");

                    b.Navigation("UserExercises");

                    b.Navigation("UserFrequencies");

                    b.Navigation("UserMuscles");

                    b.Navigation("UserTokens");

                    b.Navigation("UserVariations");
                });
#pragma warning restore 612, 618
        }
    }
}
