using GymTracker.Server.Dtos.ExerciseSet;
using GymTracker.Server.Dtos.Workout;

namespace GymTracker.Server.Services
{
    /// <summary>
    /// Interface for managing workout-related operations
    /// </summary>
    public interface IWorkoutManager
    {
        /// <summary>
        /// Retrieves all non-deleted workouts from the database
        /// </summary>
        /// <returns>A collection of workout response DTOs</returns>
        Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsAsync();

        /// <summary>
        /// Retrieves a specific workout by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the workout</param>
        /// <returns>The workout response DTO if found, null otherwise</returns>
        Task<WorkoutResponseDto?> GetWorkoutAsync(Guid id);

        /// <summary>
        /// Retrieves workouts within a specified date range
        /// </summary>
        /// <param name="startDate">The start date of the range (optional)</param>
        /// <param name="endDate">The end date of the range (optional)</param>
        /// <returns>A collection of workout response DTOs within the date range</returns>
        Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsByDateRangeAsync(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Retrieves all workouts associated with a specific workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day to filter by</param>
        /// <returns>A collection of workout response DTOs for the specified workout day</returns>
        Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsByWorkoutDayAsync(Guid workoutDayId);

        /// <summary>
        /// Creates a new workout in the database
        /// </summary>
        /// <param name="workoutDto">The workout data to create</param>
        /// <returns>The created workout response DTO</returns>
        Task<WorkoutResponseDto> CreateWorkoutAsync(WorkoutDto workoutDto);

        /// <summary>
        /// Updates an existing workout
        /// </summary>
        /// <param name="id">The ID of the workout to update</param>
        /// <param name="workoutDto">The updated workout data</param>
        /// <returns>The updated workout response DTO</returns>
        Task<WorkoutResponseDto> UpdateWorkoutAsync(Guid id, WorkoutDto workoutDto);

        /// <summary>
        /// Soft deletes a workout by marking it as deleted
        /// </summary>
        /// <param name="id">The ID of the workout to delete</param>
        /// <returns>True if workout was found and deleted, false otherwise</returns>
        Task<bool> DeleteWorkoutAsync(Guid id);

        /// <summary>
        /// Retrieves all exercise sets associated with a specific workout
        /// </summary>
        /// <param name="workoutId">The ID of the workout</param>
        /// <returns>A collection of exercise set DTOs</returns>
        List<ExerciseSetDto> GetExerciseSetsFromWorkout(Guid workoutId);

        /// <summary>
        /// Adds an exercise set to a workout
        /// </summary>
        /// <param name="workoutId">The ID of the workout</param>
        /// <param name="exerciseSet">The exercise set data to add</param>
        void AddExerciseSetToWorkout(Guid workoutId, ExerciseSetDto exerciseSet);

        /// <summary>
        /// Removes an exercise set from a workout
        /// </summary>
        /// <param name="workoutId">The ID of the workout</param>
        /// <param name="exerciseSetId">The ID of the exercise set to remove</param>
        void RemoveExerciseSetFromWorkout(Guid workoutId, Guid exerciseSetId);
    }
}
