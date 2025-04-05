using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Models;

namespace GymTracker.Server.Services
{
    /// <summary>
    /// Interface for managing exercise-related operations
    /// </summary>
    public interface IExerciseManager
    {
        /// <summary>
        /// Retrieves all exercises from the database
        /// </summary>
        /// <returns>A collection of exercise response DTOs</returns>
        Task<IEnumerable<ExerciseResponseDto>> GetExercisesAsync();

        /// <summary>
        /// Searches for exercises by name
        /// </summary>
        /// <param name="name">The name or partial name to search for</param>
        /// <returns>A collection of matching exercise response DTOs</returns>
        Task<IEnumerable<ExerciseResponseDto>> GetExercisesByNameAsync(string name);

        /// <summary>
        /// Retrieves a specific exercise by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the exercise</param>
        /// <returns>The exercise response DTO if found, null otherwise</returns>
        Task<ExerciseResponseDto?> GetExerciseAsync(Guid id);

        /// <summary>
        /// Creates a new exercise in the database
        /// </summary>
        /// <param name="exerciseDto">The exercise data to create</param>
        /// <returns>The created exercise response DTO</returns>
        Task<ExerciseResponseDto> CreateExerciseAsync(ExerciseDto exerciseDto);

        /// <summary>
        /// Updates an existing exercise
        /// </summary>
        /// <param name="id">The ID of the exercise to update</param>
        /// <param name="exerciseDto">The updated exercise data</param>
        /// <returns>The updated exercise response DTO</returns>
        Task<ExerciseResponseDto> UpdateExerciseAsync(Guid id, ExerciseDto exerciseDto);

        /// <summary>
        /// Deletes an exercise from the database
        /// </summary>
        /// <param name="id">The ID of the exercise to delete</param>
        Task DeleteExerciseAsync(Guid id);

        /// <summary>
        /// Retrieves all exercises associated with a specific body part
        /// </summary>
        /// <param name="bodyPartId">The ID of the body part to filter by</param>
        /// <returns>A collection of exercise response DTOs for the specified body part</returns>
        Task<IEnumerable<ExerciseResponseDto>> GetExercisesByBodyPartAsync(Guid bodyPartId);
    }
}
