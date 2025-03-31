using GymTracker.Server.Dtos.Routine;
using GymTracker.Server.Models;

namespace GymTracker.Server.Services
{
    /// <summary>
    /// Interface for managing workout routines
    /// </summary>
    public interface IRoutineManager
    {
        /*
        /// <summary>
        /// Retrieves all non-deleted routines
        /// </summary>
        /// <returns>A collection of routine DTOs</returns>
        Task<IEnumerable<RoutineResponseDto>> GetRoutinesAsync();

        /// <summary>
        /// Retrieves a specific routine by ID
        /// </summary>
        /// <param name="id">The ID of the routine to retrieve</param>
        /// <returns>The routine DTO if found, null otherwise</returns>
        Task<RoutineResponseDto?> GetRoutineAsync(Guid id);

        /// <summary>
        /// Creates a new routine
        /// </summary>
        /// <param name="routineDto">The routine data to create</param>
        /// <returns>The created routine DTO</returns>
        Task<RoutineResponseDto> CreateRoutineAsync(RoutineDto routineDto);

        /// <summary>
        /// Updates an existing routine
        /// </summary>
        /// <param name="id">The ID of the routine to update</param>
        /// <param name="routineDto">The updated routine data</param>
        /// <returns>The updated routine DTO</returns>
        /// <exception cref="KeyNotFoundException">Thrown when routine is not found</exception>
        Task<RoutineResponseDto> UpdateRoutineAsync(Guid id, RoutineDto routineDto);

        /// <summary>
        /// Soft deletes a routine by marking it as deleted
        /// </summary>
        /// <param name="id">The ID of the routine to delete</param>
        /// <returns>True if routine was found and deleted, false otherwise</returns>
        Task<bool> DeleteRoutineAsync(Guid id);*/
    }
}
