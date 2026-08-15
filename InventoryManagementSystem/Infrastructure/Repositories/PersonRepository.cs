
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Infrastructure.Data;

namespace InventoryManagementSystem.Infrastructure.Repositories
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Person>> GetAllByApplicationUserIdAsync(string applicationUserId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Person>().AsNoTracking().Where(i => i.ApplicationUserId == applicationUserId).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<List<Person>> GetAllByTypeAsync(string type, string applicationUserId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Person>()
            .Where(p => p.Type == type && p.ApplicationUserId == applicationUserId).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Person>().AnyAsync(p => p.Id == id, cancellationToken);
        }
    }
}