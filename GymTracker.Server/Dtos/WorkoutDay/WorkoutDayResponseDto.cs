namespace GymTracker.Server.Dtos.WorkoutDay
{
    public class WorkoutDayResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid RoutineId { get; set; }
        public string RoutineName { get; set; }
        public List<WorkoutDayExerciseResponseDto> Exercises { get; set; } = new();
    }

    public class WorkoutDayExerciseResponseDto
    {
        public Guid Id { get; set; }
        public Guid ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal? Weight { get; set; }
        public List<BodyPartDto> BodyParts { get; set; } = new();
    }

    public class BodyPartDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
} 