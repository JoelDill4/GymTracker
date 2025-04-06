using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Server.Migrations
{
    public partial class fixWorkoutDayForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDayExercise_WorkoutDay_workoutDayId",
                table: "WorkoutDayExercise");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutDayExercise_workoutDayId",
                table: "WorkoutDayExercise");

            migrationBuilder.DropColumn(
                name: "workoutDayId",
                table: "WorkoutDayExercise");

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 17, 42, 989, DateTimeKind.Utc).AddTicks(7166));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 17, 42, 989, DateTimeKind.Utc).AddTicks(7209));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 17, 42, 989, DateTimeKind.Utc).AddTicks(7220));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 17, 42, 989, DateTimeKind.Utc).AddTicks(7327));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 17, 42, 989, DateTimeKind.Utc).AddTicks(7337));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 17, 42, 989, DateTimeKind.Utc).AddTicks(7348));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 17, 42, 989, DateTimeKind.Utc).AddTicks(7356));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 17, 42, 989, DateTimeKind.Utc).AddTicks(7364));

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDayExercise_fk_workoutDay",
                table: "WorkoutDayExercise",
                column: "fk_workoutDay");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDayExercise_WorkoutDay_fk_workoutDay",
                table: "WorkoutDayExercise",
                column: "fk_workoutDay",
                principalTable: "WorkoutDay",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDayExercise_WorkoutDay_fk_workoutDay",
                table: "WorkoutDayExercise");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutDayExercise_fk_workoutDay",
                table: "WorkoutDayExercise");

            migrationBuilder.AddColumn<Guid>(
                name: "workoutDayId",
                table: "WorkoutDayExercise",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8930));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8939));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8947));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8955));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8965));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8973));

            migrationBuilder.UpdateData(
                table: "BodyPart",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                column: "createdAt",
                value: new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8981));

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDayExercise_workoutDayId",
                table: "WorkoutDayExercise",
                column: "workoutDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDayExercise_WorkoutDay_workoutDayId",
                table: "WorkoutDayExercise",
                column: "workoutDayId",
                principalTable: "WorkoutDay",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
