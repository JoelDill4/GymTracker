using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BodyPart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyPart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseExecution",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Reps = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseExecution", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workout",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubBodyPart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    fk_bodypart = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubBodyPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubBodyPart_BodyPart_fk_bodypart",
                        column: x => x.fk_bodypart,
                        principalTable: "BodyPart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseBodyPart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fk_exercise = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fk_bodypart = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseBodyPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseBodyPart_BodyPart_fk_bodypart",
                        column: x => x.fk_bodypart,
                        principalTable: "BodyPart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseBodyPart_Exercise_fk_exercise",
                        column: x => x.fk_exercise,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutDay",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    fk_routine = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutDay_Routine_fk_routine",
                        column: x => x.fk_routine,
                        principalTable: "Routine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutDayExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fk_workoutday = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fk_exercise = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutDayExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutDayExercise_Exercise_fk_exercise",
                        column: x => x.fk_exercise,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutDayExercise_WorkoutDay_fk_workoutday",
                        column: x => x.fk_workoutday,
                        principalTable: "WorkoutDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseBodyPart_fk_bodypart",
                table: "ExerciseBodyPart",
                column: "fk_bodypart");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseBodyPart_fk_exercise",
                table: "ExerciseBodyPart",
                column: "fk_exercise");

            migrationBuilder.CreateIndex(
                name: "IX_SubBodyPart_fk_bodypart",
                table: "SubBodyPart",
                column: "fk_bodypart");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDay_fk_routine",
                table: "WorkoutDay",
                column: "fk_routine");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDayExercise_fk_exercise",
                table: "WorkoutDayExercise",
                column: "fk_exercise");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDayExercise_fk_workoutday",
                table: "WorkoutDayExercise",
                column: "fk_workoutday");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseBodyPart");

            migrationBuilder.DropTable(
                name: "ExerciseExecution");

            migrationBuilder.DropTable(
                name: "SubBodyPart");

            migrationBuilder.DropTable(
                name: "Workout");

            migrationBuilder.DropTable(
                name: "WorkoutDayExercise");

            migrationBuilder.DropTable(
                name: "BodyPart");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "WorkoutDay");

            migrationBuilder.DropTable(
                name: "Routine");
        }
    }
}
