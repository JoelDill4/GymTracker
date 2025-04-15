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
    }
} 