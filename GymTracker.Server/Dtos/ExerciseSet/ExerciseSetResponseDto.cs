using GymTracker.Server.Dtos.Exercise;

namespace GymTracker.Server.Dtos.ExerciseSet
{
    public class ExerciseSetResponseDto
    {
        public Guid id { get; set; }

        public int order { get; set; }

        public decimal weight { get; set; }

        public int reps { get; set; }

        public ExerciseResponseDto exercise { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
} 