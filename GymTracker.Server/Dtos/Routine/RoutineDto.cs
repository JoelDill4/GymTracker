using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.Routine;

public class RoutineDto
{
    [Required]
    [StringLength(100)]
    public string name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string description { get; set; } = string.Empty;
} 