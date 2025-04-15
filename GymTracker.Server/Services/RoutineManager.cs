using GymTracker.Server.DatabaseConnection;
using GymTracker.Server.Dtos.Routine;
using GymTracker.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    /// <summary>
    /// Implementation of IRoutineManager for managing workout routines
    /// </summary>
    public class RoutineManager : IRoutineManager
    {
        private readonly GymTrackerContext _context;

        public RoutineManager(GymTrackerContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Routine>> GetRoutinesAsync()
        {
            return await _context.Routine
                .Where(r => !r.isDeleted)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Routine?> GetRoutineAsync(Guid id)
        {
            return await _context.Routine
                .FirstOrDefaultAsync(r => r.id == id && !r.isDeleted);
        }

        /// <inheritdoc/>
        public async Task<Routine> CreateRoutineAsync(Routine routine)
        {
            routine.id = Guid.NewGuid();
            routine.createdAt = DateTime.UtcNow;
            routine.updatedAt = DateTime.UtcNow;
            routine.isDeleted = false;

            _context.Routine.Add(routine);
            await _context.SaveChangesAsync();
            return routine;
        }

        /// <inheritdoc/>
        public async Task<Routine> UpdateRoutineAsync(Guid id, Routine routine)
        {
            var existingRoutine = await _context.Routine
                .FirstOrDefaultAsync(r => r.id == id && !r.isDeleted);

            if (existingRoutine == null)
                throw new KeyNotFoundException($"Routine with ID {id} not found");

            existingRoutine.name = routine.name;
            existingRoutine.description = routine.description;
            existingRoutine.updatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingRoutine;
        }

        /// <inheritdoc/>
        public async Task DeleteRoutineAsync(Guid id)
        {
            var routine = await _context.Routine
                .FirstOrDefaultAsync(r => r.id == id && !r.isDeleted);

            if (routine == null)
                throw new KeyNotFoundException($"Routine with ID {id} not found");

            routine.isDeleted = true;
            routine.updatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
