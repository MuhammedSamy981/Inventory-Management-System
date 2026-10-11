using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Infrastructure.Data;

namespace InventoryManagementSystem.Infrastructure.Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        private readonly ApplicationDbContext _context;
        public InventoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }




        public async Task<Object?> GetAllDetailsByIdAsync(int id, string applicationUserId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Inventory>().AsNoTracking()
            .Where(i => i.ApplicationUserId == applicationUserId && i.Id == id)
                .Include(i => i.InventoryItems)
                .ThenInclude(ii => ii.Item).ThenInclude(i => i!.Operations)
                .Select(i => new
                {
                    InventoryId = i.Id,
                    InventoryName = i.Name,
                    InventoryAddress = i.Address,
                    Items = (
                              fromDate != null && toDate != null ?
                              i.InventoryItems.Where(ii => ii.Item!.Operations.Any(o => o.Date >= fromDate.Value && o.Date <= toDate.Value))
                              : i.InventoryItems
                            ).Select(ii => new
                            {
                                ItemId = ii.ItemId,
                                ItemName = ii.Item!.Name,
                                ItemSize = ii.Item!.Size,
                                ItemCode = ii.Item!.Code,
                                Quantity = ii.Quantity
                            }).ToList<Object>()
                }).FirstOrDefaultAsync(cancellationToken:cancellationToken);
        }
        public async Task<List<Object>> GetAllItemsMovementByListInventoryIdsAsync(List<int> inventoryIds, string applicationUserId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
        {

            var inventories = _context.Set<Inventory>().AsNoTracking()
             .Where(i => inventoryIds.Contains(i.Id) && i.ApplicationUserId == applicationUserId)
            .Include(i => i.Operations)
            .ThenInclude(o => o.Item);

            return await inventories.Select(i => new
            {
                InventoryId = i.Id,
                InventoryName = i.Name,
                InventoryAddress = i.Address,
                Items = (
                                fromDate != null && toDate != null ?
                                i.Operations.Where(o => o.Date >= fromDate.Value
                                && o.Date <= toDate.Value)
                                : i.Operations
                                ).Select(o => new
                                {
                                    ItemId = o.Item!.Id,
                                    ItemName = o.Item.Name,
                                    MoveName = o.Type,
                                    MoveDate = o.Date,
                                    Quantity = o.Quantity
                                }).ToList<Object>()
            }).ToListAsync<Object>(cancellationToken);

        }

        public async Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Inventory>().AnyAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<List<Inventory>> GetAllByApplicationUserIdAsync(string applicationUserId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Inventory>().AsNoTracking().Where(i => i.ApplicationUserId == applicationUserId).Skip((pageNumber - 1) * pageSize)
       .Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<List<int>> GetAllIdsAsync(string applicationUserId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Inventory>().Where(i => i.ApplicationUserId == applicationUserId)
                .Select(i => i.Id).ToListAsync(cancellationToken);
        }

        public async Task<int> GetManagerIdByInventoryIdAsync(int id, string applicationUserId, CancellationToken cancellationToken = default)
        {
            var inventory = await _context.Set<Inventory>()
            .FirstOrDefaultAsync(i => i.Id == id && i.ApplicationUserId == applicationUserId, cancellationToken);

            return inventory?.ManagerId ?? 0;
        }
    }
}