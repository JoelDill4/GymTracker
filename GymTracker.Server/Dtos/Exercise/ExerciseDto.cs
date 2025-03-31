using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.Exercise
{
    public class ExerciseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? description { get; set; }

        [Required]
        public Guid fk_bodypart { get; set; }
    }
} 