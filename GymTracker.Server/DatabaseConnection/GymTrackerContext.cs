using Microsoft.EntityFrameworkCore;
using GymTracker.Server.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using GymTracker.Server.Dtos.BodyPart;

namespace GymTracker.Server.DatabaseConnection
{
    public class GymTrackerContext : DbContext
    {
        public GymTrackerContext(DbContextOptions<GymTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Exercise> Exercise { get; set; }

        public DbSet<BodyPart> BodyPart { get; set; }

        public DbSet<Routine> Routine { get; set; }

        public DbSet<WorkoutDay> WorkoutDay { get; set; }

        public DbSet<WorkoutDayExercise> WorkoutDayExercise { get; set; }

        // public DbSet<SubBodyPart> SubBodyPart { get; set; }

        // public DbSet<Workout> Workout { get; set; }

        // public DbSet<ExerciseExecution> ExerciseExecution { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var yamlPath = Path.Combine(AppContext.BaseDirectory, "Data", "body_parts.yaml");
            if (!File.Exists(yamlPath))
            {
                throw new FileNotFoundException($"YAML file not found at path: {yamlPath}");
            }

            var yamlContent = File.ReadAllText(yamlPath);
            
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var bodyPartData = deserializer.Deserialize<BodyPartData>(yamlContent);

            if (bodyPartData?.body_parts == null)
            {
                throw new InvalidOperationException("Failed to deserialize body parts from YAML file");
            }

            foreach (var bodyPart in bodyPartData.body_parts)
            {
                modelBuilder.Entity<BodyPart>().HasData(
                    new BodyPart 
                    { 
                        id = bodyPart.id, 
                        name = bodyPart.name, 
                        createdAt = DateTime.UtcNow, 
                        isDeleted = false 
                    }
                );
            }
        }
    }
}