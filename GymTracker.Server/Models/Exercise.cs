using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Exercise
{
    public Exercise() { }

    public Exercise(string name)
    {
        Name = name;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public List<WorkoutDayExercise> WorkoutDaysExercises { get; set; } = new List<WorkoutDayExercise>();

    public List<ExerciseBodyPart> ExercisesBodyParts { get; set; } = new List<ExerciseBodyPart>();
}
