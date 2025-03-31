namespace GymTracker.Server.Dtos.Workout
{
    public class WorkoutResponseDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string? Observations { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public WorkoutDayDto WorkoutDay { get; set; }
        public List<ExerciseExecutionDto> ExerciseExecutions { get; set; } = new();
    }

    public class WorkoutDayDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RoutineId { get; set; }
        public string RoutineName { get; set; }
    }

    public class ExerciseExecutionDto
    {
        public Guid Id { get; set; }
        public string ExerciseName { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal? Weight { get; set; }
        public string? Notes { get; set; }
    }
} 