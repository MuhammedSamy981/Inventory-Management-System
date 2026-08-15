using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Infrastructure.Data;

namespace InventoryManagementSystem.Infrastructure.Repositories
{
    public class InventoryItemRepository : GenericRepository<InventoryItem>, IInventoryItemRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public async Task<List<Object>> GetAllByListInventoryIdsAndItemIdAsync(List<int> inventoryIds, int itemId, string applicationUserId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryItem>().AsNoTracking().Include(ii => ii.Inventory)
            .Where(ii => inventoryIds.Contains(ii.InventoryId) && ii.Inventory!.ApplicationUserId == applicationUserId &&
            ii.ItemId == itemId).Select(ii => new
            {
                InventoryId = ii.Inventory!.Id,
                InventoryName = ii.Inventory.Name,
                InventoryAddress = ii.Inventory.Address,
                Quantity = ii.Quantity
            }).ToListAsync<Object>(cancellationToken);

        }
        public async Task<List<Object>> GetAllByListInventoryIdsAsync(List<int> inventoryIds, string applicationUserId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryItem>().Include(ii => ii.Inventory).Include(ii => ii.Item).AsNoTracking()
          .Where(ii => inventoryIds.Contains(ii.InventoryId) && ii.Inventory!.ApplicationUserId == applicationUserId)
            .Select(ii => new
            {
                ItemId = ii.ItemId,
                ItemName = ii.Item!.Name,
                ItemCode = ii.Item.Code,
                ItemSize = ii.Item.Size,
                InventoryName = ii.Inventory!.Name
            }).ToListAsync<Object>(cancellationToken);


        }

        public async Task<List<Object>> GetAllbyinventoryIdAndStorageDateAsync(int inventoryId, string ApplicationUserId, DateTime? storageDate, CancellationToken cancellationToken = default)
        {

            return await (from ii in _context.InventoryItems
                          join i in _context.Items
                          on ii.ItemId equals i.Id
                          join o in _context.Operations
                          on ii.InventoryId equals o.InventoryId
                          where ii.ItemId == o.InventoryId
                          where o.Date <= storageDate && o.Date != null
                                && o.Type != "Exchange Permit"
                                && o.InventoryId == inventoryId
                                && o.ApplicationUserId == ApplicationUserId
                          select new
                          {
                              ItemId = i.Id,
                              ItemName = i.Name,
                              ItemSize = i.Size,
                              InventoryId = ii.InventoryId,
                              Quantity = ii.Quantity,
                              StorageDate = o.Date
                          }).ToListAsync<Object>(cancellationToken);
        }

        public async Task<List<Object>> GetAllbyInventoryIdAndExpiryDateAsync(int inventoryId, string applicationUserId, DateTime? expiryDate, CancellationToken cancellationToken = default)
        {
            // _context.Set<InventoryItem>().Join(_context.set<item)

            return await (from ii in _context.InventoryItems
                          join i in _context.Items
                          on ii.ItemId equals i.Id
                          join o in _context.Operations
                          on ii.InventoryId equals o.InventoryId
                          where ii.ItemId == o.InventoryId
                          where o.ExpiryDate <= expiryDate &&
                           o.ExpiryDate != null
                           && o.ApplicationUserId == applicationUserId
                           && ii.InventoryId == inventoryId
                          select new
                          {
                              ItemId = i.Id,
                              ItemName = i.Name,
                              ItemSize = i.Size,
                              InventoryId = ii.InventoryId,
                              Quantity = ii.Quantity,
                              ExpiryDate = o.ExpiryDate
                          }).ToListAsync<Object>(cancellationToken);

        }


        public async Task<InventoryItem?> GetSpecificAsync(int inventoryId, int itemId)
        {
            return await _context.Set<InventoryItem>()
            .FirstOrDefaultAsync(i => i.InventoryId == inventoryId &&
            i.ItemId == itemId);
        }

        public async Task<bool> ExistsAsync(int? inventoryId, int? itemId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryItem>().AnyAsync(ii => ii.InventoryId == inventoryId && ii.ItemId == itemId, cancellationToken);
        }


    }

}