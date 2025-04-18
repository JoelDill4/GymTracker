using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.Workout
{
    public class WorkoutDto
    {
        [Required]
        public DateTime workoutDate { get; set; }

        [StringLength(1000)]
        public string? observations { get; set; }

        public Guid? workoutDayId { get; set; }
    }
} 