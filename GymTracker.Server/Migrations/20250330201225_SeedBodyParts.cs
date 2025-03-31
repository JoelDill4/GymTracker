using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Server.Migrations
{
    public partial class SeedBodyParts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert all body parts from the enum
            migrationBuilder.InsertData(
                table: "BodyPart",
                columns: new[] { "id", "name", "createdAt", "isDeleted" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "Chest", DateTime.UtcNow, false },
                    { Guid.NewGuid(), "Back", DateTime.UtcNow, false },
                    { Guid.NewGuid(), "Shoulders", DateTime.UtcNow, false },
                    { Guid.NewGuid(), "Biceps", DateTime.UtcNow, false },
                    { Guid.NewGuid(), "Triceps", DateTime.UtcNow, false },
                    { Guid.NewGuid(), "Legs", DateTime.UtcNow, false },
                    { Guid.NewGuid(), "Core", DateTime.UtcNow, false }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove all seeded body parts
            migrationBuilder.DeleteData(
                table: "BodyPart",
                keyColumn: "name",
                keyValues: new object[]
                {
                    "Chest",
                    "Back",
                    "Shoulders",
                    "Biceps",
                    "Triceps",
                    "Legs",
                    "Core"
                });
        }
    }
}
