﻿// <auto-generated />
using System;
using GymTracker.Server.DatabaseConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GymTracker.Server.Migrations
{
    [DbContext(typeof(GymTrackerContext))]
    partial class GymTrackerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ExerciseSet", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("fk_exercise")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("fk_workout")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("order")
                        .HasColumnType("int");

                    b.Property<int>("reps")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("weight")
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("id");

                    b.HasIndex("fk_exercise");

                    b.HasIndex("fk_workout");

                    b.ToTable("ExerciseSet");
                });

            modelBuilder.Entity("GymTracker.Server.Models.BodyPart", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("BodyPart");

                    b.HasData(
                        new
                        {
                            id = new Guid("00000000-0000-0000-0000-000000000001"),
                            createdAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            isDeleted = false,
                            name = "Chest"
                        },
                        new
                        {
                            id = new Guid("00000000-0000-0000-0000-000000000002"),
                            createdAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            isDeleted = false,
                            name = "Back"
                        },
                        new
                        {
                            id = new Guid("00000000-0000-0000-0000-000000000003"),
                            createdAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            isDeleted = false,
                            name = "Shoulders"
                        },
                        new
                        {
                            id = new Guid("00000000-0000-0000-0000-000000000004"),
                            createdAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            isDeleted = false,
                            name = "Biceps"
                        },
                        new
                        {
                            id = new Guid("00000000-0000-0000-0000-000000000005"),
                            createdAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            isDeleted = false,
                            name = "Triceps"
                        },
                        new
                        {
                            id = new Guid("00000000-0000-0000-0000-000000000006"),
                            createdAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            isDeleted = false,
                            name = "Forearms"
                        },
                        new
                        {
                            id = new Guid("00000000-0000-0000-0000-000000000007"),
                            createdAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            isDeleted = false,
                            name = "Legs"
                        },
                        new
                        {
                            id = new Guid("00000000-0000-0000-0000-000000000008"),
                            createdAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            isDeleted = false,
                            name = "Core"
                        },
                        new
                        {
                            id = new Guid("00000000-0000-0000-0000-000000000009"),
                            createdAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            isDeleted = false,
                            name = "Trapezius"
                        });
                });

            modelBuilder.Entity("GymTracker.Server.Models.Exercise", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("fk_bodyPart")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("fk_bodyPart");

                    b.ToTable("Exercise");
                });

            modelBuilder.Entity("GymTracker.Server.Models.Routine", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("Routine");
                });

            modelBuilder.Entity("GymTracker.Server.Models.Workout", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("fk_workoutDay")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("observations")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime?>("updatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("workoutDate")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("fk_workoutDay");

                    b.ToTable("Workout");
                });

            modelBuilder.Entity("GymTracker.Server.Models.WorkoutDay", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("fk_routine")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("fk_routine");

                    b.ToTable("WorkoutDay");
                });

            modelBuilder.Entity("GymTracker.Server.Models.WorkoutDayExercise", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("fk_exercise")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("fk_workoutDay")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("id");

                    b.HasIndex("fk_exercise");

                    b.HasIndex("fk_workoutDay", "fk_exercise")
                        .IsUnique();

                    b.ToTable("WorkoutDayExercise");
                });

            modelBuilder.Entity("ExerciseSet", b =>
                {
                    b.HasOne("GymTracker.Server.Models.Exercise", "exercise")
                        .WithMany()
                        .HasForeignKey("fk_exercise")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymTracker.Server.Models.Workout", "workout")
                        .WithMany("exerciseSets")
                        .HasForeignKey("fk_workout")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("exercise");

                    b.Navigation("workout");
                });

            modelBuilder.Entity("GymTracker.Server.Models.Exercise", b =>
                {
                    b.HasOne("GymTracker.Server.Models.BodyPart", "bodyPart")
                        .WithMany()
                        .HasForeignKey("fk_bodyPart")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("bodyPart");
                });

            modelBuilder.Entity("GymTracker.Server.Models.Workout", b =>
                {
                    b.HasOne("GymTracker.Server.Models.WorkoutDay", "workoutDay")
                        .WithMany()
                        .HasForeignKey("fk_workoutDay");

                    b.Navigation("workoutDay");
                });

            modelBuilder.Entity("GymTracker.Server.Models.WorkoutDay", b =>
                {
                    b.HasOne("GymTracker.Server.Models.Routine", "routine")
                        .WithMany()
                        .HasForeignKey("fk_routine")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("routine");
                });

            modelBuilder.Entity("GymTracker.Server.Models.WorkoutDayExercise", b =>
                {
                    b.HasOne("GymTracker.Server.Models.Exercise", "exercise")
                        .WithMany("workoutDayExercises")
                        .HasForeignKey("fk_exercise")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymTracker.Server.Models.WorkoutDay", "workoutDay")
                        .WithMany("workoutDayExercises")
                        .HasForeignKey("fk_workoutDay")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("exercise");

                    b.Navigation("workoutDay");
                });

            modelBuilder.Entity("GymTracker.Server.Models.Exercise", b =>
                {
                    b.Navigation("workoutDayExercises");
                });

            modelBuilder.Entity("GymTracker.Server.Models.Workout", b =>
                {
                    b.Navigation("exerciseSets");
                });

            modelBuilder.Entity("GymTracker.Server.Models.WorkoutDay", b =>
                {
                    b.Navigation("workoutDayExercises");
                });
#pragma warning restore 612, 618
        }
    }
}
