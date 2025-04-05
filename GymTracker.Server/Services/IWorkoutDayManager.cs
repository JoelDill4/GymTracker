using GymTracker.Server.Dtos.WorkoutDay;
using GymTracker.Server.Models;

namespace GymTracker.Server.Services
{
    /// <summary>
    /// Interface for managing workout days
    /// </summary>
    public interface IWorkoutDayManager
    {
        /// <summary>
        /// Retrieves all workout days from the database
        /// </summary>
        /// <returns>A collection of workout day response DTOs</returns>
        Task<IEnumerable<WorkoutDayResponseDto>> GetWorkoutDaysAsync();

        /// <summary>
        /// Retrieves a specific workout day by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the workout day</param>
        /// <returns>The workout day response DTO if found, null otherwise</returns>
        Task<WorkoutDayResponseDto?> GetWorkoutDayAsync(Guid id);

        /// <summary>
        /// Retrieves all workout days associated with a specific routine
        /// </summary>
        /// <param name="routineId">The ID of the routine to filter by</param>
        /// <returns>A collection of workout day response DTOs for the specified routine</returns>
        Task<IEnumerable<WorkoutDayResponseDto>> GetWorkoutDaysByRoutineAsync(Guid routineId);

        /// <summary>
        /// Creates a new workout day in the database
        /// </summary>
        /// <param name="workoutDayDto">The workout day data to create</param>
        /// <returns>The created workout day response DTO</returns>
        Task<WorkoutDayResponseDto> CreateWorkoutDayAsync(WorkoutDayDto workoutDayDto);

        /// <summary>
        /// Updates an existing workout day
        /// </summary>
        /// <param name="id">The ID of the workout day to update</param>
        /// <param name="workoutDayDto">The updated workout day data</param>
        /// <returns>The updated workout day response DTO</returns>
        Task<WorkoutDayResponseDto> UpdateWorkoutDayAsync(Guid id, WorkoutDayDto workoutDayDto);

        /// <summary>
        /// Deletes a workout day from the database
        /// </summary>
        /// <param name="id">The ID of the workout day to delete</param>
        /// <returns>True if the workout day was successfully deleted, false otherwise</returns>
        Task<bool> DeleteWorkoutDayAsync(Guid id);
    }
}
