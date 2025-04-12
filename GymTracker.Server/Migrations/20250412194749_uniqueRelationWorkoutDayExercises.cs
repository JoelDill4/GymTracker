using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Server.Migrations
{
    public partial class uniqueRelationWorkoutDayExercises : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDayExercise_fk_workoutDay_fk_exercise",
                table: "WorkoutDayExercise",
                columns: new[] { "fk_workoutDay", "fk_exercise" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDayExercise_fk_workoutDay",
                table: "WorkoutDayExercise",
                column: "fk_workoutDay");
        }
    }
}
