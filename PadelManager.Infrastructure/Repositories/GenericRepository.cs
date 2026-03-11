using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Repositories
{
    //Esta clase es la que implementa la interfaz IGenericRepository<T> para cualquier entidad T
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly PadelManagerDbContext _context;

        protected readonly DbSet<T> _dbSet;

        public GenericRepository(PadelManagerDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        // Actualizar: EF Core rastrea la entidad y marca lo que cambió
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            // El guardado real lo hace el UnitOfWork.SaveAsync()
            return await Task.FromResult(entity);
        }

        // Soft Delete Toggle: Si está borrado, lo restaura. Si no, lo borra.
        public async Task<T> SoftDeleteToggleAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null) return null;

            // Verificamos si la entidad hereda de tu BaseEntity
            if (entity is BaseEntity auditEntity)
            {
                if (auditEntity.DeletedAt.HasValue)
                {
                    // Restaurar
                    auditEntity.DeletedAt = null;
                    auditEntity.Status = "Active";
                }
                else
                {
                    // Borrado Lógico
                    auditEntity.DeletedAt = DateTime.UtcNow;
                    auditEntity.Status = "Inactive";
                }

                _dbSet.Update(entity);
            }

            return entity;
        }
    }
}
