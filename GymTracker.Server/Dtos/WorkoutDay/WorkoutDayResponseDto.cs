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

    public class WorkoutDayExerciseResponseDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public Guid bodyPartId { get; set; }
        public string bodyPartName { get; set; }
    }

    /*public class BodyPartDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }*/
} 