using GymTracker.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymTracker.Server.Models
{
    public class WorkoutDayExercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [Required]
        public Guid fk_exercise { get; set; }

        [ForeignKey("fk_exercise")]
        public virtual Exercise exercise { get; set; }

        [Required]
        public Guid fk_workoutDay { get; set; }

        [ForeignKey("fk_workoutDay")]
        public virtual WorkoutDay workoutDay { get; set; }
    }
}