using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Server.Migrations
{
    public partial class minorChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workout_WorkoutDay_fk_workoutDay",
                table: "Workout");

            migrationBuilder.AlterColumn<Guid>(
                name: "fk_workoutDay",
                table: "Workout",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "weight",
                table: "ExerciseSet",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Workout_WorkoutDay_fk_workoutDay",
                table: "Workout",
                column: "fk_workoutDay",
                principalTable: "WorkoutDay",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workout_WorkoutDay_fk_workoutDay",
                table: "Workout");

            migrationBuilder.AlterColumn<Guid>(
                name: "fk_workoutDay",
                table: "Workout",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "weight",
                table: "ExerciseSet",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_Workout_WorkoutDay_fk_workoutDay",
                table: "Workout",
                column: "fk_workoutDay",
                principalTable: "WorkoutDay",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
