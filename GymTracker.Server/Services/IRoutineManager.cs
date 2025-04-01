using GymTracker.Server.Models;

namespace GymTracker.Server.Services
{
    /// <summary>
    /// Interface for managing workout routines
    /// </summary>
    public interface IRoutineManager
    {
        /// <summary>
        /// Retrieves all non-deleted routines
        /// </summary>
        /// <returns>A collection of routine DTOs</returns>
        Task<IEnumerable<Routine>> GetRoutinesAsync();

        /// <summary>
        /// Retrieves a specific routine by ID
        /// </summary>
        /// <param name="id">The ID of the routine to retrieve</param>
        /// <returns>The routine DTO if found, null otherwise</returns>
        Task<Routine?> GetRoutineAsync(Guid id);

        /// <summary>
        /// Creates a new routine
        /// </summary>
        /// <param name="routineDto">The routine data to create</param>
        /// <returns>The created routine DTO</returns>
        Task<Routine> CreateRoutineAsync(Routine routine);

        /// <summary>
        /// Updates an existing routine
        /// </summary>
        /// <param name="id">The ID of the routine to update</param>
        /// <param name="routineDto">The updated routine data</param>
        /// <returns>The updated routine DTO</returns>
        /// <exception cref="KeyNotFoundException">Thrown when routine is not found</exception>
        Task<Routine> UpdateRoutineAsync(Guid id, Routine routine);

        /// <summary>
        /// Soft deletes a routine by marking it as deleted
        /// </summary>
        /// <param name="id">The ID of the routine to delete</param>
        /// <returns>True if routine was found and deleted, false otherwise</returns>
        Task DeleteRoutineAsync(Guid id);
    }
}
