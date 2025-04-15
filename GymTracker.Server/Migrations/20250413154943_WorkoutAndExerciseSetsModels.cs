using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Server.Migrations
{
    public partial class WorkoutAndExerciseSetsModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workout",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    workoutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    observations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    fk_workoutDay = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workout", x => x.id);
                    table.ForeignKey(
                        name: "FK_Workout_WorkoutDay_fk_workoutDay",
                        column: x => x.fk_workoutDay,
                        principalTable: "WorkoutDay",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseExecution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<int>(type: "int", nullable: false),
                    reps = table.Column<int>(type: "int", nullable: false),
                    fk_workout = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fk_exercise = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseExecution", x => x.id);
                    table.ForeignKey(
                        name: "FK_ExerciseExecution_Exercise_fk_exercise",
                        column: x => x.fk_exercise,
                        principalTable: "Exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseExecution_Workout_fk_workout",
                        column: x => x.fk_workout,
                        principalTable: "Workout",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExecution_fk_exercise",
                table: "ExerciseExecution",
                column: "fk_exercise");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExecution_fk_workout",
                table: "ExerciseExecution",
                column: "fk_workout");

            migrationBuilder.CreateIndex(
                name: "IX_Workout_fk_workoutDay",
                table: "Workout",
                column: "fk_workoutDay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseExecution");

            migrationBuilder.DropTable(
                name: "Workout");
        }
    }
}
