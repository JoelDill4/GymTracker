using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Server.Migrations
{
    public partial class Modifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Observations",
                table: "Workout",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Workout",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "fk_workoutday",
                table: "Workout",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "fk_exercise",
                table: "ExerciseExecution",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "fk_workout",
                table: "ExerciseExecution",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkoutId",
                table: "Exercise",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workout_fk_workoutday",
                table: "Workout",
                column: "fk_workoutday");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExecution_fk_exercise",
                table: "ExerciseExecution",
                column: "fk_exercise");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExecution_fk_workout",
                table: "ExerciseExecution",
                column: "fk_workout");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_WorkoutId",
                table: "Exercise",
                column: "WorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_Workout_WorkoutId",
                table: "Exercise",
                column: "WorkoutId",
                principalTable: "Workout",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseExecution_Exercise_fk_exercise",
                table: "ExerciseExecution",
                column: "fk_exercise",
                principalTable: "Exercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseExecution_Workout_fk_workout",
                table: "ExerciseExecution",
                column: "fk_workout",
                principalTable: "Workout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workout_WorkoutDay_fk_workoutday",
                table: "Workout",
                column: "fk_workoutday",
                principalTable: "WorkoutDay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Workout_WorkoutId",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseExecution_Exercise_fk_exercise",
                table: "ExerciseExecution");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseExecution_Workout_fk_workout",
                table: "ExerciseExecution");

            migrationBuilder.DropForeignKey(
                name: "FK_Workout_WorkoutDay_fk_workoutday",
                table: "Workout");

            migrationBuilder.DropIndex(
                name: "IX_Workout_fk_workoutday",
                table: "Workout");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseExecution_fk_exercise",
                table: "ExerciseExecution");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseExecution_fk_workout",
                table: "ExerciseExecution");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_WorkoutId",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Workout");

            migrationBuilder.DropColumn(
                name: "fk_workoutday",
                table: "Workout");

            migrationBuilder.DropColumn(
                name: "fk_exercise",
                table: "ExerciseExecution");

            migrationBuilder.DropColumn(
                name: "fk_workout",
                table: "ExerciseExecution");

            migrationBuilder.DropColumn(
                name: "WorkoutId",
                table: "Exercise");

            migrationBuilder.AlterColumn<string>(
                name: "Observations",
                table: "Workout",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
