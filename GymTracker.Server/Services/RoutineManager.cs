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

        /// <summary>
        /// Initializes a new instance of RoutineManager
        /// </summary>
        /// <param name="context">The database context for routine operations</param>
        public RoutineManager(GymTrackerContext context)
        {
            _context = context;
        }

        /*
        /// <inheritdoc/>
        public async Task<IEnumerable<RoutineResponseDto>> GetRoutinesAsync()
        {
            return await _context.Routine
                .Where(r => !r.IsDeleted)
                .Select(r => new RoutineResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                })
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<RoutineResponseDto?> GetRoutineAsync(Guid id)
        {
            var routine = await _context.Routine
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (routine == null)
                return null;

            return new RoutineResponseDto
            {
                Id = routine.Id,
                Name = routine.Name,
                Description = routine.Description,
                CreatedAt = routine.CreatedAt,
                UpdatedAt = routine.UpdatedAt
            };
        }

        /// <inheritdoc/>
        public async Task<RoutineResponseDto> CreateRoutineAsync(RoutineDto routineDto)
        {
            var routine = new Routine(routineDto.Name)
            {
                Id = Guid.NewGuid(),
                Description = routineDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Routine.Add(routine);
            await _context.SaveChangesAsync();

            return new RoutineResponseDto
            {
                Id = routine.Id,
                Name = routine.Name,
                Description = routine.Description,
                CreatedAt = routine.CreatedAt
            };
        }

        /// <inheritdoc/>
        public async Task<RoutineResponseDto> UpdateRoutineAsync(Guid id, RoutineDto routineDto)
        {
            var routine = await _context.Routine
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (routine == null)
                throw new KeyNotFoundException($"Routine with ID {id} not found");

            routine.Name = routineDto.Name;
            routine.Description = routineDto.Description;
            routine.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new RoutineResponseDto
            {
                Id = routine.Id,
                Name = routine.Name,
                Description = routine.Description,
                CreatedAt = routine.CreatedAt,
                UpdatedAt = routine.UpdatedAt
            };
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteRoutineAsync(Guid id)
        {
            var routine = await _context.Routine
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (routine == null)
                return false;

            routine.IsDeleted = true;
            routine.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }*/
    }
}
