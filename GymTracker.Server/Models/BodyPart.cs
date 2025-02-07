using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BodyPart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } 

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public List<ExerciseBodyPart> ExercisesBodyParts { get; set; }
}