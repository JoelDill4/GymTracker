using System.Collections.Generic;

namespace GymTracker.Server.Models
{
    public class BodyPartData
    {
        public List<BodyPartItem> body_parts { get; set; } = new List<BodyPartItem>();
    }

    public class BodyPartItem
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
    }
} 