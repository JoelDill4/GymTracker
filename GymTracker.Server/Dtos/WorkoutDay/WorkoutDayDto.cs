using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.WorkoutDay
{
    public class WorkoutDayDto
    {
        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? description { get; set; }

        [Required]
        public Guid routineId { get; set; }
    }
} 