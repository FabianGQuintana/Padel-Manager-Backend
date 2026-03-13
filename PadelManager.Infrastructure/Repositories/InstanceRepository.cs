using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class InstanceRepository : GenericRepository<Instance>, IInstanceRepository
    {
        private readonly PadelManagerDbContext _context;

        public InstanceRepository(PadelManagerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Instance>> GetInstancesByStageIdAsync(Guid stageId)
        {
            return await _context.Instances
                .Where(i => i.StageId == stageId && i.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Instance>> GetInstancesByNameAsync(string name)
        {
            return await _context.Instances
                .Where(i => i.Name == name && i.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Instance>> GetInstancesByDescriptionAsync(string description)
        {
            return await _context.Instances
                .Where(i => i.Description == description && i.DeletedAt == null)
                .ToListAsync();
        }

        //Un partido puede estar en una sola instancia, por lo que se devuelve un solo resultado

        public async Task<Instance?> GetInstanceByMatchIdAsync(Guid matchId)
        {
            return await _context.Instances
                .Include(i => i.Matches)
                .Where(i => i.Matches.Any(m => m.Id == matchId && m.DeletedAt == null) && i.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
    }
}