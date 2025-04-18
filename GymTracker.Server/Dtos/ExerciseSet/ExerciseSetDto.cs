using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.ExerciseSet
{
    public class ExerciseSetDto
    {
        [Required]
        public int order { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal weight { get; set; }

        [Required]
        [Range(0, 100)]
        public int reps { get; set; }

        [Required]
        public Guid exerciseId { get; set; }
    }
} 