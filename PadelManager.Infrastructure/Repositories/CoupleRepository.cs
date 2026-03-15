using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Repositories
{
    public class CoupleRepository : GenericRepository<Couple>, ICoupleRepository
    {
        private readonly PadelManagerDbContext _context;

        public CoupleRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Couple?> GetCoupleByNicknameAsync(string nickname)
        {
            return await _context.Couples
                .Include(c => c.Player1)
                .Include(c => c.Player2)
                .Include(c => c.Zone)
                .FirstOrDefaultAsync(c => c.Nickname == nickname);
        }

        public async Task<IEnumerable<Couple>> GetCouplesByPlayerIdAsync(Guid playerId)
        {
            return await _context.Couples
                .Include(c => c.Player1)
                .Include(c => c.Player2)
                .Include(c => c.Zone)
                .Where(c => c.Player1Id == playerId || c.Player2Id == playerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Couple>> GetCouplesByZoneIdAsync(Guid zoneId)
        {
            return await _context.Couples
                .Include(c => c.Player1)
                .Include(c => c.Player2)
                .Include(c => c.Zone)
                .Where(c => c.ZoneId == zoneId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Couple>> GetCouplesWithoutZoneAsync()
        {
            return await _context.Couples
                .Include(c => c.Player1)
                .Include(c => c.Player2)
                .Where(c => c.ZoneId == null)
                .ToListAsync();
        }
    }
}
