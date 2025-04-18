using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Server.Migrations
{
    public partial class renameTableExerciseSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseExecution_Exercise_fk_exercise",
                table: "ExerciseExecution");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseExecution_Workout_fk_workout",
                table: "ExerciseExecution");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseExecution",
                table: "ExerciseExecution");

            migrationBuilder.RenameTable(
                name: "ExerciseExecution",
                newName: "ExerciseSet");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseExecution_fk_workout",
                table: "ExerciseSet",
                newName: "IX_ExerciseSet_fk_workout");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseExecution_fk_exercise",
                table: "ExerciseSet",
                newName: "IX_ExerciseSet_fk_exercise");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseSet",
                table: "ExerciseSet",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseSet_Exercise_fk_exercise",
                table: "ExerciseSet",
                column: "fk_exercise",
                principalTable: "Exercise",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseSet_Workout_fk_workout",
                table: "ExerciseSet",
                column: "fk_workout",
                principalTable: "Workout",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseSet_Exercise_fk_exercise",
                table: "ExerciseSet");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseSet_Workout_fk_workout",
                table: "ExerciseSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseSet",
                table: "ExerciseSet");

            migrationBuilder.RenameTable(
                name: "ExerciseSet",
                newName: "ExerciseExecution");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseSet_fk_workout",
                table: "ExerciseExecution",
                newName: "IX_ExerciseExecution_fk_workout");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseSet_fk_exercise",
                table: "ExerciseExecution",
                newName: "IX_ExerciseExecution_fk_exercise");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseExecution",
                table: "ExerciseExecution",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseExecution_Exercise_fk_exercise",
                table: "ExerciseExecution",
                column: "fk_exercise",
                principalTable: "Exercise",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseExecution_Workout_fk_workout",
                table: "ExerciseExecution",
                column: "fk_workout",
                principalTable: "Workout",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
