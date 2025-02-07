using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class SubBodyPart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } 

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public Guid fk_bodypart { get; set; }
    [ForeignKey("BodyPartId")]
    public BodyPart BodyPart { get; set; }
}