namespace GymTracker.Server.Dtos.Routine
{
    public class RoutineResponseDto
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public string? description { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
} 