using GymTracker.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ExerciseSet
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }

    public int order { get; set; }

    [Range(0, 1000)]
    [Column(TypeName = "decimal(5,2)")]
    public decimal weight { get; set; }

    [Range(0, 100)]
    public int reps { get; set; }

    [Required]
    public Guid fk_workout { get; set; }
    [ForeignKey("fk_workout")]

    public Workout workout { get; set; }

    [Required]
    public Guid fk_exercise { get; set; }
    [ForeignKey("fk_exercise")]

    public Exercise exercise { get; set; }

    public DateTime createdAt { get; set; } = DateTime.UtcNow;

    public DateTime? updatedAt { get; set; }

    public bool isDeleted { get; set; } = false;
}