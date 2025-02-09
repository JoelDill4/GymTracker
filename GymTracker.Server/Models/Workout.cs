using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Workout
{
    public Workout () { }

    public Workout(DateTime date, string? observations, Guid fk_workoutday, WorkoutDay workoutDay)
    {
        this.Date = date;
        this.Observations = observations;
        this.fk_workoutday = fk_workoutday;
        this.WorkoutDay = workoutDay;
        this.Exercises = workoutDay.WorkoutDaysExercises.Select(x => x.Exercise).ToList();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public string? Observations { get; set; }

    [Required]
    public Guid fk_workoutday {  get; set; }
    [ForeignKey("fk_workoutday")]

    public WorkoutDay WorkoutDay { get; set; }

    public List<Exercise> Exercises { get; set; } = new List<Exercise>();
}