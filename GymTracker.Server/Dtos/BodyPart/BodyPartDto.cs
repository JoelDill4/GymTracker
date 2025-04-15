using GymTracker.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.BodyPart
{
    public class BodyPartDto
    {
        public Guid id { get; set; }

        public string name { get; set; }
    }

    public class BodyPartData
    {
        public List<BodyPartDto> body_parts { get; set; } = new List<BodyPartDto>();
    }
}