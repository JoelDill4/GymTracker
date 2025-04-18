using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Dtos.WorkoutDay;

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

        /// <summary>
        /// Gets all exercises associated with a workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day</param>
        /// <returns>A collection of exercises</returns>
        Task<IEnumerable<ExerciseResponseDto>> GetExercisesFromWorkoutDayAsync(Guid workoutDayId);

        /// <summary>
        /// Assign exercises to a workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day</param>
        /// <param name="exercisesIds">The list of exercise IDs to add</param>
        /// <returns>Task</returns>
        Task AssignExercisesToWorkoutDayAsync(Guid workoutDayId, List<Guid> exercisesIds);

        /// <summary>
        /// Adds an exercise to a workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day</param>
        /// <param name="exerciseId">The ID of the exercise to add</param>
        /// <returns>Task</returns>
        Task AddExerciseToWorkoutDayAsync(Guid workoutDayId, Guid exerciseId);

        /// <summary>
        /// Removes an exercise from a workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day</param>
        /// <param name="exerciseId">The ID of the exercise to remove</param>
        /// <returns>Task</returns>
        Task RemoveExerciseFromWorkoutDayAsync(Guid workoutDayId, Guid exerciseId);
    }
}
