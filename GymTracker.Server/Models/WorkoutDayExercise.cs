using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WorkoutDayExercise
{
    public WorkoutDayExercise() { }

    public WorkoutDayExercise(WorkoutDay workoutDay, Exercise exercise)
    {
        fk_workoutday = workoutDay.Id;
        fk_exercise = exercise.Id;
        WorkoutDay = workoutDay;
        Exercise = exercise;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } 

    public Guid fk_workoutday { get; set; }
    [ForeignKey("fk_workoutday")]
    public WorkoutDay WorkoutDay { get; set; }

    public Guid fk_exercise { get; set; }
    [ForeignKey("fk_exercise")]
    public Exercise Exercise { get; set; }
}