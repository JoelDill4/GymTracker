using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ExerciseBodyPart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } 

    public Guid fk_exercise { get; set; }
    [ForeignKey("ExerciseId")]
    public Exercise Exercise { get; set; }

    public Guid fk_bodypart { get; set; }
    [ForeignKey("BodyPartId")]
    public BodyPart BodyPart { get; set; }

}