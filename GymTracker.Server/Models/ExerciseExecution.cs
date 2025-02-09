using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ExerciseExecution
{
    public ExerciseExecution() { }

    public ExerciseExecution(int order, int weight, int reps, Workout workout, Exercise exercise)
    {
        this.Order = order;
        this.Weight = weight;
        this.Reps = reps;
        this.fk_workout = workout.Id;
        this.Workout = workout;
        this.fk_exercise = exercise.Id;
        this.Exercise = exercise;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int Order { get; set; }

    public int Weight { get; set; }

    public int Reps { get; set; }

    [Required]
    public Guid fk_workout { get; set; }
    [ForeignKey("fk_workout")]

    public Workout Workout { get; set; }

    [Required]
    public Guid fk_exercise { get; set; }
    [ForeignKey("fk_exercise")]

    public Exercise Exercise { get; set; }
}