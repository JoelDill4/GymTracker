using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.Exercise
{
    public class ExerciseDto
    {
        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? description { get; set; }

        [Required]
        public Guid fk_bodyPart { get; set; }
    }
} 