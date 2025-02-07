using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WorkoutDayExercise
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } 

    public Guid fk_workoutday { get; set; }
    [ForeignKey("WorkoutDayId")]
    public WorkoutDay WorkoutDay { get; set; }

    public Guid fk_exercise { get; set; }
    [ForeignKey("ExerciseId")]
    public Exercise Exercise { get; set; }
}