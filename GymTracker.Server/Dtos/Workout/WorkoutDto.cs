using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.Workout
{
    public class WorkoutDto
    {
        public Guid? Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(1000, ErrorMessage = "Observations cannot exceed 1000 characters")]
        public string? Observations { get; set; }

        [Required]
        public Guid WorkoutDayId { get; set; }
    }
} 