using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymTracker.Server.Models
{
    public class WorkoutDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string name { get; set; }

        [StringLength(500)]
        public string description { get; set; } = string.Empty;

        [Required]
        public Guid fk_routine { get; set; }

        [ForeignKey("fk_routine")]
        public virtual Routine routine { get; set; }

        public virtual ICollection<WorkoutDayExercise> workoutDayExercises { get; set; } = new List<WorkoutDayExercise>();

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime? updatedAt { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}