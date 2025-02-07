using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Routine> Routine { get; set; }

    public DbSet<WorkoutDay> WorkoutDay { get; set; }

    public DbSet<Exercise> Exercise { get; set; }

    public DbSet<WorkoutDayExercise> WorkoutDayExercise { get; set; }

    public DbSet<BodyPart> BodyPart { get; set; }

    public DbSet<ExerciseBodyPart> ExerciseBodyPart  { get; set; }

    public DbSet<SubBodyPart> SubBodyPart { get; set; }

    public DbSet<Workout> Workout { get; set; }

    public DbSet<ExerciseExecution> ExerciseExecution { get; set; }
}