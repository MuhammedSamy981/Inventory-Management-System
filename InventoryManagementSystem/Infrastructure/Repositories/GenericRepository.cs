
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Infrastructure.Data;

namespace InventoryManagementSystem.Infrastructure.Repositories
{

    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct)
        {
            return await _context.Set<TEntity>().AsNoTracking().Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(ct);
        }
        public async Task<TEntity?> GetByIdAsync(int? id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public async Task<TEntity?> GetByIdAsync(string? id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateRangeAsync(List<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }


    }
}