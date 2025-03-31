using GymTracker.Server.Dtos.Exercise;

namespace GymTracker.Server.Services
{
    public interface IBodyPartManager
    {
        Task<IEnumerable<BodyPartDto>> GetBodyPartsAsync();
    }
} 