﻿using GymTracker.Server.Dtos;
using GymTracker.Server.Dtos.Workout;
using GymTracker.Server.Models;

namespace GymTracker.Server.Services
{
    public interface IWorkoutManager
    {
        /*public List<Workout> GetWorkouts();

        public Workout? GetWorkout(Guid id);

        public Workout CreateOrEditWorkout(WorkoutDto workout);

        public void DeleteWorkout(Guid id);

        public List<Exercise> GetExercisesFromWorkout(Guid workoutId);

        public void AddExerciseToWorkout(Guid workoutId, Guid exerciseId);

        public void RemoveExerciseFromWorkout(Guid workoutId, Guid exerciseId);

        public List<ExerciseExecutionDto> GetExercisesExecutionsFromWorkout(Guid workoutId);

        public void AddExerciseExecutionToWorkout(Guid workoutId, ExerciseExecutionDto exercise);

        public void RemoveExerciseExecutionFromWorkout(Guid workoutId, Guid exerciseId);

        Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsAsync();
        Task<WorkoutResponseDto?> GetWorkoutAsync(Guid id);
        Task<WorkoutResponseDto> CreateWorkoutAsync(WorkoutDto workoutDto);
        Task<WorkoutResponseDto> UpdateWorkoutAsync(Guid id, WorkoutDto workoutDto);
        Task<bool> DeleteWorkoutAsync(Guid id);
        Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsByWorkoutDayAsync(Guid workoutDayId);*/
    }
}
