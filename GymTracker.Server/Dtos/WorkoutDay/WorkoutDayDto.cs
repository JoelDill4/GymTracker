using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.WorkoutDay
{
    public class WorkoutDayDto
    {
        public Guid? Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [Required]
        public Guid RoutineId { get; set; }

        public List<WorkoutDayExerciseDto> Exercises { get; set; } = new();
    }

    public class WorkoutDayExerciseDto
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
    }
} 