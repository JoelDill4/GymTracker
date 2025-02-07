using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WorkoutDay
{
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

    public List<WorkoutDayExercise> WorkoutDaysExercises { get; set; }
}