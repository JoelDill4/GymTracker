﻿using GymTracker.Server.DatabaseConnection;
using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Dtos.Routine;
using GymTracker.Server.Dtos.WorkoutDay;
using GymTracker.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    public class WorkoutDayManager : IWorkoutDayManager
    {
        private readonly GymTrackerContext _context;

        public WorkoutDayManager(GymTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkoutDayResponseDto>> GetWorkoutDaysAsync()
        {
            return await _context.WorkoutDay
                .Where(wd => !wd.isDeleted)
                .Include(wd => wd.routine)
                .Select(wd => new WorkoutDayResponseDto
                {
                    id = wd.id,
                    name = wd.name,
                    description = wd.description,
                    routine = new RoutineResponseDto
                    {
                        id = wd.routine.id,
                        name = wd.routine.name,
                        description = wd.routine.description,
                        createdAt = wd.routine.createdAt,
                        updatedAt = wd.routine.updatedAt
                    },
                    createdAt = wd.createdAt,
                    updatedAt = wd.updatedAt
                })
                .ToListAsync();
        }

        public async Task<WorkoutDayResponseDto?> GetWorkoutDayAsync(Guid id)
        {
            var workoutDay = await _context.WorkoutDay
                .Include(wd => wd.routine)
                .FirstOrDefaultAsync(wd => wd.id == id && !wd.isDeleted);

            if (workoutDay == null)
                return null;

            return new WorkoutDayResponseDto
            {
                id = workoutDay.id,
                name = workoutDay.name,
                description = workoutDay.description,
                routine = new RoutineResponseDto
                {
                    id = workoutDay.routine.id,
                    name = workoutDay.routine.name,
                    description = workoutDay.routine.description,
                    createdAt = workoutDay.routine.createdAt,
                    updatedAt = workoutDay.routine.updatedAt
                },
                createdAt = workoutDay.createdAt,
                updatedAt = workoutDay.updatedAt
            };
        }

        public async Task<IEnumerable<WorkoutDayResponseDto>> GetWorkoutDaysByRoutineAsync(Guid routineId)
        {
            return await _context.WorkoutDay
                .Where(wd => !wd.isDeleted && wd.fk_routine == routineId)
                .Include(wd => wd.routine)
                .Select(wd => new WorkoutDayResponseDto
                {
                    id = wd.id,
                    name = wd.name,
                    description = wd.description,
                    routine = new RoutineResponseDto
                    {
                        id = wd.routine.id,
                        name = wd.routine.name,
                        description = wd.routine.description,
                        createdAt = wd.routine.createdAt,
                        updatedAt = wd.routine.updatedAt
                    },
                    createdAt = wd.createdAt,
                    updatedAt = wd.updatedAt
                })
                .ToListAsync();
        }

        public async Task<WorkoutDayResponseDto> CreateWorkoutDayAsync(WorkoutDayDto workoutDayDto)
        {
            var routine = await _context.Routine
                .FirstOrDefaultAsync(r => r.id == workoutDayDto.routineId && !r.isDeleted);

            if (routine == null)
                throw new KeyNotFoundException($"Routine with ID {workoutDayDto.routineId} not found");

            var workoutDay = new WorkoutDay
            {
                id = Guid.NewGuid(),
                name = workoutDayDto.name,
                description = workoutDayDto.description,
                fk_routine = workoutDayDto.routineId,
                createdAt = DateTime.UtcNow
            };

            _context.WorkoutDay.Add(workoutDay);
            await _context.SaveChangesAsync();

            return await GetWorkoutDayAsync(workoutDay.id) ?? throw new Exception("Failed to create workout day");
        }

        public async Task<WorkoutDayResponseDto> UpdateWorkoutDayAsync(Guid id, WorkoutDayDto workoutDayDto)
        {
            var workoutDay = await _context.WorkoutDay
                .Include(wd => wd.routine)
                .FirstOrDefaultAsync(wd => wd.id == id && !wd.isDeleted);

            if (workoutDay == null)
                throw new KeyNotFoundException($"Workout day with ID {id} not found");

            workoutDay.name = workoutDayDto.name;
            workoutDay.description = workoutDayDto.description;
            workoutDay.fk_routine = workoutDayDto.routineId;
            workoutDay.updatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetWorkoutDayAsync(workoutDay.id) ?? throw new Exception("Failed to update workout day");
        }

        public async Task<bool> DeleteWorkoutDayAsync(Guid id)
        {
            var workoutDay = await _context.WorkoutDay
                .FirstOrDefaultAsync(wd => wd.id == id && !wd.isDeleted);

            if (workoutDay == null)
                return false;

            workoutDay.isDeleted = true;
            workoutDay.updatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WorkoutDayExerciseResponseDto>> GetExercisesFromWorkoutDayAsync(Guid workoutDayId)
        {
            var workoutDay = await _context.WorkoutDay
                .Include(wd => wd.workoutDayExercises)
                    .ThenInclude(wde => wde.exercise)
                        .ThenInclude(e => e.bodyPart)
                .FirstOrDefaultAsync(wd => wd.id == workoutDayId && !wd.isDeleted);

            if (workoutDay == null)
                throw new KeyNotFoundException($"Workout day with ID {workoutDayId} not found");

            return workoutDay.workoutDayExercises
                .Select(wde => new WorkoutDayExerciseResponseDto
                {
                    id = wde.exercise.id,
                    name = wde.exercise.name,
                    description = wde.exercise.description,
                    bodyPartId = wde.exercise.fk_bodyPart,
                    bodyPartName = wde.exercise.bodyPart.name
                })
                .ToList();
        }

        public async Task AddExerciseToWorkoutDayAsync(Guid workoutDayId, Guid exerciseId)
        {
            var workoutDay = await _context.WorkoutDay
                .FirstOrDefaultAsync(wd => wd.id == workoutDayId && !wd.isDeleted);

            if (workoutDay == null)
                throw new KeyNotFoundException($"Workout day with ID {workoutDayId} not found");

            var exercise = await _context.Exercise
                .FirstOrDefaultAsync(e => e.id == exerciseId && !e.isDeleted);

            if (exercise == null)
                throw new KeyNotFoundException($"Exercise with ID {exerciseId} not found");

            var workoutDayExercise = new WorkoutDayExercise
            {
                id = Guid.NewGuid(),
                fk_workoutDay = workoutDayId,
                fk_exercise = exerciseId
            };

            _context.WorkoutDayExercise.Add(workoutDayExercise);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveExerciseFromWorkoutDayAsync(Guid workoutDayId, Guid exerciseId)
        {
            var workoutDayExercise = await _context.WorkoutDayExercise
                .FirstOrDefaultAsync(wde => wde.fk_workoutDay == workoutDayId && wde.fk_exercise == exerciseId);

            if (workoutDayExercise == null)
                throw new KeyNotFoundException($"Exercise with ID {exerciseId} not found in workout day with ID {workoutDayId}");

            _context.WorkoutDayExercise.Remove(workoutDayExercise);
            await _context.SaveChangesAsync();
        }
    }
}
