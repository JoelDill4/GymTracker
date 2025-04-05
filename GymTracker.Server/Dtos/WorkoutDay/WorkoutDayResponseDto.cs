using GymTracker.Server.Dtos.Routine;

namespace GymTracker.Server.Dtos.WorkoutDay
{
    public class WorkoutDayResponseDto
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public string? description { get; set; }

        public RoutineResponseDto? routine { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

        // public List<WorkoutDayExerciseResponseDto> Exercises { get; set; } = new();
    }

    /*public class WorkoutDayExerciseResponseDto
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
    }*/
} 