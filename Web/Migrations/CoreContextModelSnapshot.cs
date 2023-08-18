﻿// <auto-generated />
using System;
using Data;
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
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Data.Entities.Equipment.Instruction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DisabledReason")
                        .HasColumnType("text");

                    b.Property<int>("Equipment")
                        .HasColumnType("integer");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Notes")
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

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("VariationId", "ExerciseId", "ExerciseType")
                        .IsUnique();

                    b.ToTable("exercise_variation", t =>
                        {
                            t.HasComment("Variation progressions for an exercise track");
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

                    b.Property<bool?>("PauseReps")
                        .HasColumnType("boolean");

                    b.Property<int>("SecondaryMuscles")
                        .HasColumnType("integer");

                    b.Property<int>("SportsFocus")
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

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_footnote", t =>
                        {
                            t.HasComment("Sage advice");
                        });
                });

            modelBuilder.Entity("Data.Entities.Newsletter.UserEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("EmailStatus")
                        .HasColumnType("integer");

                    b.Property<string>("LastError")
                        .HasColumnType("text");

                    b.Property<DateTime>("SendAfter")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("SendAttempts")
                        .HasColumnType("integer");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_email", t =>
                        {
                            t.HasComment("A day's workout routine");
                        });
                });

            modelBuilder.Entity("Data.Entities.Newsletter.UserWorkout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("Frequency")
                        .HasColumnType("integer");

                    b.Property<int>("Intensity")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeloadWeek")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_workout", t =>
                        {
                            t.HasComment("A day's workout routine");
                        });
                });

            modelBuilder.Entity("Data.Entities.Newsletter.UserWorkoutExerciseVariation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseVariationId")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<int>("Section")
                        .HasColumnType("integer");

                    b.Property<int>("UserWorkoutId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseVariationId");

                    b.HasIndex("UserWorkoutId");

                    b.ToTable("user_workout_exercise_variation", t =>
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

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Equipment")
                        .HasColumnType("integer");

                    b.Property<int>("Features")
                        .HasColumnType("integer");

                    b.Property<int>("FootnoteType")
                        .HasColumnType("integer");

                    b.Property<int>("Frequency")
                        .HasColumnType("integer");

                    b.Property<bool>("IncludeMobilityWorkouts")
                        .HasColumnType("boolean");

                    b.Property<int>("Intensity")
                        .HasColumnType("integer");

                    b.Property<DateOnly?>("LastActive")
                        .HasColumnType("date");

                    b.Property<string>("NewsletterDisabledReason")
                        .HasColumnType("text");

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

                    b.Property<bool>("ShowStaticImages")
                        .HasColumnType("boolean");

                    b.Property<int>("SportsFocus")
                        .HasColumnType("integer");

                    b.Property<int>("Verbosity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("user", t =>
                        {
                            t.HasComment("User who signed up for the newsletter");
                        });
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

                    b.Property<bool>("Ignore")
                        .HasColumnType("boolean");

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

            modelBuilder.Entity("Data.Entities.User.UserMuscleFlexibility", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("MuscleGroup")
                        .HasColumnType("integer");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "MuscleGroup");

                    b.ToTable("user_muscle_flexibility");
                });

            modelBuilder.Entity("Data.Entities.User.UserMuscleMobility", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("MuscleGroup")
                        .HasColumnType("integer");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "MuscleGroup");

                    b.ToTable("user_muscle_mobility");
                });

            modelBuilder.Entity("Data.Entities.User.UserMuscleStrength", b =>
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

                    b.ToTable("user_muscle_strength");
                });

            modelBuilder.Entity("Data.Entities.User.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone");

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

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "VariationId");

                    b.HasIndex("VariationId");

                    b.ToTable("user_variation", t =>
                        {
                            t.HasComment("User's intensity stats");
                        });
                });

            modelBuilder.Entity("Data.Entities.User.UserVariationWeight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("VariationId")
                        .HasColumnType("integer");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VariationId");

                    b.ToTable("user_variation_weight", t =>
                        {
                            t.HasComment("User variation weight log");
                        });
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

            modelBuilder.Entity("Data.Entities.Exercise.Variation", b =>
                {
                    b.HasOne("Data.Entities.Equipment.Instruction", "DefaultInstruction")
                        .WithMany()
                        .HasForeignKey("DefaultInstructionId");

                    b.Navigation("DefaultInstruction");
                });

            modelBuilder.Entity("Data.Entities.Footnote.Footnote", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserFootnotes")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.Newsletter.UserEmail", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserEmails")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.Newsletter.UserWorkout", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserWorkouts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Data.Entities.Newsletter.WorkoutRotation", "WorkoutRotation", b1 =>
                        {
                            b1.Property<int>("UserWorkoutId")
                                .HasColumnType("integer");

                            b1.Property<int>("Id")
                                .HasColumnType("integer");

                            b1.Property<int>("MovementPatterns")
                                .HasColumnType("integer");

                            b1.Property<int>("MuscleGroups")
                                .HasColumnType("integer");

                            b1.HasKey("UserWorkoutId");

                            b1.ToTable("user_workout");

                            b1.WithOwner()
                                .HasForeignKey("UserWorkoutId");
                        });

                    b.Navigation("User");

                    b.Navigation("WorkoutRotation")
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.Newsletter.UserWorkoutExerciseVariation", b =>
                {
                    b.HasOne("Data.Entities.Exercise.ExerciseVariation", "ExerciseVariation")
                        .WithMany("UserWorkoutExerciseVariations")
                        .HasForeignKey("ExerciseVariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Newsletter.UserWorkout", "UserWorkout")
                        .WithMany("UserWorkoutExerciseVariations")
                        .HasForeignKey("UserWorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExerciseVariation");

                    b.Navigation("UserWorkout");
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

                    b.OwnsOne("Data.Entities.Newsletter.WorkoutRotation", "Rotation", b1 =>
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

            modelBuilder.Entity("Data.Entities.User.UserMuscleFlexibility", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserMuscleFlexibilities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.User.UserMuscleMobility", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserMuscleMobilities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Entities.User.UserMuscleStrength", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserMuscleStrengths")
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

            modelBuilder.Entity("Data.Entities.User.UserVariationWeight", b =>
                {
                    b.HasOne("Data.Entities.User.User", "User")
                        .WithMany("UserVariationWeights")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Exercise.Variation", "Variation")
                        .WithMany("UserVariationWeights")
                        .HasForeignKey("VariationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Variation");
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
                    b.Navigation("UserExerciseVariations");

                    b.Navigation("UserWorkoutExerciseVariations");
                });

            modelBuilder.Entity("Data.Entities.Exercise.Variation", b =>
                {
                    b.Navigation("ExerciseVariations");

                    b.Navigation("Instructions");

                    b.Navigation("UserVariationWeights");

                    b.Navigation("UserVariations");
                });

            modelBuilder.Entity("Data.Entities.Newsletter.UserWorkout", b =>
                {
                    b.Navigation("UserWorkoutExerciseVariations");
                });

            modelBuilder.Entity("Data.Entities.User.User", b =>
                {
                    b.Navigation("UserEmails");

                    b.Navigation("UserExerciseVariations");

                    b.Navigation("UserExercises");

                    b.Navigation("UserFootnotes");

                    b.Navigation("UserFrequencies");

                    b.Navigation("UserMuscleFlexibilities");

                    b.Navigation("UserMuscleMobilities");

                    b.Navigation("UserMuscleStrengths");

                    b.Navigation("UserTokens");

                    b.Navigation("UserVariationWeights");

                    b.Navigation("UserVariations");

                    b.Navigation("UserWorkouts");
                });
#pragma warning restore 612, 618
        }
    }
}
