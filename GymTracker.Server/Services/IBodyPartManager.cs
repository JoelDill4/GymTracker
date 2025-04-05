using GymTracker.Server.Dtos.Exercise;

namespace GymTracker.Server.Services
{
    /// <summary>
    /// Interface for managing body part-related operations
    /// </summary>
    public interface IBodyPartManager
    {
        /// <summary>
        /// Retrieves all non-deleted body parts from the database
        /// </summary>
        /// <returns>A collection of body part DTOs</returns>
        Task<IEnumerable<BodyPartDto>> GetBodyPartsAsync();
    }
} 