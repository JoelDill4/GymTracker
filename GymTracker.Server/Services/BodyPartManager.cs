using GymTracker.Server.DatabaseConnection;
using GymTracker.Server.Dtos.BodyPart;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    public class BodyPartManager : IBodyPartManager
    {
        private readonly GymTrackerContext _context;

        public BodyPartManager(GymTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BodyPartDto>> GetBodyPartsAsync()
        {
            return await _context.BodyPart
                .Where(bp => !bp.isDeleted)
                .Select(bp => new BodyPartDto
                {
                    id = bp.id,
                    name = bp.name
                })
                .ToListAsync();
        }
    }
} 