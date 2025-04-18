using GymTracker.Server.Dtos.ExerciseSet;
using GymTracker.Server.Dtos.WorkoutDay;

namespace GymTracker.Server.Dtos.Workout
{
    public class WorkoutResponseDto
    {
        public Guid id { get; set; }

        public DateTime workoutDate { get; set; }

        public string? observations { get; set; }

        public WorkoutDayResponseDto? workoutDay { get; set; }

        public List<ExerciseSetResponseDto> exerciseSets { get; set; } = new List<ExerciseSetResponseDto>();

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
} 