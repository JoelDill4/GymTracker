using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymTracker.Server.Models
{
    public class Workout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [Required]
        public DateTime workoutDate { get; set; }

        [StringLength(1000)]
        public string? observations { get; set; }

        public Guid fk_workoutDay { get; set; }

        [ForeignKey("fk_workoutDay")]
        public virtual WorkoutDay workoutDay { get; set; }

        public virtual ICollection<ExerciseSet> exerciseSets { get; set; } = new List<ExerciseSet>();

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public DateTime? updatedAt { get; set; }

        public bool isDeleted { get; set; } = false;
    }
}