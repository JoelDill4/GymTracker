using Microsoft.EntityFrameworkCore;
using GymTracker.Server.Models;

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

        // public DbSet<WorkoutDay> WorkoutDay { get; set; }

        // public DbSet<WorkoutDayExercise> WorkoutDayExercise { get; set; }

        // public DbSet<SubBodyPart> SubBodyPart { get; set; }

        // public DbSet<Workout> Workout { get; set; }

        // public DbSet<ExerciseExecution> ExerciseExecution { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.bodyPart)
                .WithMany()
                .HasForeignKey("fk_bodypart");

            modelBuilder.Entity<Routine>();
        }
    }
}