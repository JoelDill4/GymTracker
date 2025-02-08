using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ExerciseBodyPart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid fk_exercise { get; set; }
    [ForeignKey("fk_exercise")]
    public Exercise Exercise { get; set; }

    public Guid fk_bodypart { get; set; }
    [ForeignKey("fk_bodypart")]
    public BodyPart BodyPart { get; set; }
}
