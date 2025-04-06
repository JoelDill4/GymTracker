using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Server.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BodyPart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyPart", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Routine",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routine", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    fk_bodyPart = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.id);
                    table.ForeignKey(
                        name: "FK_Exercise_BodyPart_fk_bodyPart",
                        column: x => x.fk_bodyPart,
                        principalTable: "BodyPart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutDay",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    fk_routine = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutDay", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkoutDay_Routine_fk_routine",
                        column: x => x.fk_routine,
                        principalTable: "Routine",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutDayExercise",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fk_exercise = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fk_workoutDay = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    workoutDayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutDayExercise", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkoutDayExercise_Exercise_fk_exercise",
                        column: x => x.fk_exercise,
                        principalTable: "Exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutDayExercise_WorkoutDay_workoutDayId",
                        column: x => x.workoutDayId,
                        principalTable: "WorkoutDay",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BodyPart",
                columns: new[] { "id", "createdAt", "isDeleted", "name", "updatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8903), false, "Chest", null },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8930), false, "Back", null },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8939), false, "Shoulders", null },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8947), false, "Biceps", null },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8955), false, "Triceps", null },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8965), false, "Forearms", null },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8973), false, "Legs", null },
                    { new Guid("00000000-0000-0000-0000-000000000008"), new DateTime(2025, 4, 6, 20, 4, 49, 133, DateTimeKind.Utc).AddTicks(8981), false, "Core", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_fk_bodyPart",
                table: "Exercise",
                column: "fk_bodyPart");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDay_fk_routine",
                table: "WorkoutDay",
                column: "fk_routine");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDayExercise_fk_exercise",
                table: "WorkoutDayExercise",
                column: "fk_exercise");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDayExercise_workoutDayId",
                table: "WorkoutDayExercise",
                column: "workoutDayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutDayExercise");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "WorkoutDay");

            migrationBuilder.DropTable(
                name: "BodyPart");

            migrationBuilder.DropTable(
                name: "Routine");
        }
    }
}
