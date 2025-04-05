using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.WorkoutDay
{
    public class WorkoutDayDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? description { get; set; }

        [Required]
        public Guid routineId { get; set; }

        // public List<WorkoutDayExerciseDto> Exercises { get; set; } = new();
    }

    /*public class WorkoutDayExerciseDto
    {
        [Required]
        public Guid ExerciseId { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Sets must be between 1 and 10")]
        public int Sets { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Reps must be between 1 and 100")]
        public int Reps { get; set; }

        [Range(0, 1000, ErrorMessage = "Weight must be between 0 and 1000 kg")]
        public decimal? Weight { get; set; }
    }*/
} 