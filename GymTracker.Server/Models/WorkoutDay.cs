using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class WorkoutDay
{
    public WorkoutDay() { }

    public WorkoutDay(string name, Routine routine)
    {
        Name = name;
        fk_routine = routine.Id;
        Routine = routine;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [Required]
    public Guid fk_routine { get; set; }
    [ForeignKey("fk_routine")]
    public Routine Routine { get; set; }

    public List<WorkoutDayExercise> WorkoutDaysExercises { get; set; } = new List<WorkoutDayExercise>();
}
