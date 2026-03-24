using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using PadelManager.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Repositories
{
    public class StageRepository : GenericRepository<Stage>, IStageRepository
    {
        private readonly PadelManagerDbContext _context;

        public StageRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stage>> GetStagesByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Stages
                .Include(s => s.Category)
                .Include(s => s.Zones)
                .Where(s => s.CategoryId == categoryId)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task<Stage?> GetStageByCategoryIdAndOrderAsync(Guid categoryId, int order)
        {
            return await _context.Stages
                .Include(s => s.Category)
                .Include(s => s.Zones)
                .FirstOrDefaultAsync(s => s.CategoryId == categoryId && s.Order == order);
        }

        public async Task<Stage?> GetGroupStageByCategoryAsync(Guid categoryId)
        {
            return await _context.Stages
                .FirstOrDefaultAsync(s => s.CategoryId == categoryId
                                     && s.Type == StageType.GroupPhase 
                                     && s.DeletedAt == null);
        }

    }
}
