using System;
using PadelManager.Infrastructure.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PadelManager.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly PadelManagerDbContext _dbContext;

        public CategoryRepository(PadelManagerDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            return await _dbContext.Categories
                .Where(c => c.Name == name && c.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoryByDescriptionAsync(string description)
        {
            return await _dbContext.Categories
                .Where(c => c.Description != null &&
                            c.Description.ToLower().Contains(description.ToLower()) &&
                            c.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByTournamentIdAsync(Guid tournamentId)
        {
            return await _dbContext.Categories
                .Where(c => c.TournamentId == tournamentId && c.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByStageIdAsync(Guid stageId)
        {
            return await _dbContext.Categories
                .Include(c => c.Stages)
                .Where(c => c.Stages.Any(s => s.Id == stageId) && c.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryWithDetailsByIdAsync(Guid categoryId)
        {
            return await _dbContext.Categories
                .Include(c => c.Tournament)
                .Include(c => c.Stages)
                .Include(c => c.Registrations)
                .Where(c => c.Id == categoryId && c.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<Category?> GetCategoryWithRegistrationsAsync(Guid id)
        {
            return await _context.Categories
                .Include(c => c.Registrations)
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
        }

        public async Task<IEnumerable<Category>> GetCategoriesByTournamentWithRegistrationsAsync(Guid tournamentId)
        {
            return await _context.Categories
                .Include(c => c.Registrations)
                .Where(c => c.TournamentId == tournamentId && c.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByMaxTeamsWithRegistrationsAsync(int maxTeams)
        {
            return await _context.Categories
                .Include(c => c.Registrations)
                .Where(c => c.MaxTeams == maxTeams && c.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByNameWithRegistrationsAsync(string name)
        {
            return await _context.Categories
                .Include(c => c.Registrations) //  Cargamos los hijos
                                               // Usamos .Contains para que la búsqueda sea más flexible (ej: "6ta" traiga "6ta Caballeros")
                .Where(c => c.Name.Contains(name) && c.DeletedAt == null)
                .ToListAsync();
        }

    }
}
