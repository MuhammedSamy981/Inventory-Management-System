using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Infrastructure.Data;

namespace InventoryManagementSystem.Infrastructure.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<Item>> GetAllByApplicationUserIdAsync(string applicationUserId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
             return await _context.Set<Item>().AsNoTracking().Where(i => i.ApplicationUserId == applicationUserId).Skip((pageNumber - 1) * pageSize)
        .Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<Object?> GetAllDetailsByIdAndListInventoryIdsAsync(int id, List<int> inventoryIds,string applicationUserId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
        {
           //Console.WriteLine($"/n /n /n Item fromDate={fromDate}, toDate={toDate} /n /n /n");
            return await _context.Set<Item>().AsNoTracking()
            .Where(i => i.ApplicationUserId == applicationUserId && i.Id == id)
                .Include(i => i.InventoryItems)
                .ThenInclude(ii => ii.Inventory).ThenInclude(i => i!.Operations)
                .Select(i => new
                {
                    ItemId = i.Id,
                    ItemName = i.Name,
                    ItemCode = i.Code,
                    ItemSize = i.Size,
                    Inventories = (
                              fromDate != null && toDate != null ?
                              i.InventoryItems.Where(ii => inventoryIds.Contains(ii.InventoryId) && ii.Item!.Operations.Any(o => o.Date >= fromDate.Value && o.Date <= toDate.Value))
                              : i.InventoryItems.Where(ii => inventoryIds.Contains(ii.InventoryId))
                            ).Select(ii => new
                            {
                                InventoryId = ii.Inventory!.Id,
                                InventoryName = ii.Inventory.Name,
                                InventoryAddress = ii.Inventory.Address,
                                Quantity = ii.Quantity
                            }).ToList<Object>()
                }).FirstOrDefaultAsync(cancellationToken:cancellationToken);
        }

        public async Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Item>().AnyAsync(i => i.Id == id, cancellationToken);
        }

        /*        public async Task<List<ItemInInventoryDto>> GetAllByInventoryIdAsync(int? id)
                {
                    return await _context.Set<Item>().Include(i=>i.InventoryItems)
                    .ThenInclude(ii=>ii.Inventory).AsNoTracking()
                    .Where(i => i.InventoryItems.Any(ii => ii.InventoryId == id))
                    .Select(i=> new ItemInInventoryDto
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Size = i.Size,
                            Quantity = i.InventoryItems.FirstOrDefault(ii => ii.InventoryId == id).Quantity!
                        }).ToListAsync();

                }*/


        /*       public async Task<List<ItemInInventoriesDto>> GetAllByListInventoryIdsAsync(List<int> inventoryIds)
               {
                                 return await _context.Set<Item>().Include(i=>i.InventoryItems)
                   .ThenInclude(ii=>ii.Inventory).AsNoTracking()
                   .Where(i => i.InventoryItems.Any(ii => inventoryIds.Contains(ii.InventoryId)))
                   .Select(i=> new ItemInInventoriesDto
                                   {
                                       Id = i.Id,
                                       Name = i.Name,
                                       Code = i.Code,
                                       Size = i.Size,
                                       InventoryName = i.InventoryItems.FirstOrDefault(ii => inventoryIds.Contains(ii.InventoryId)).Inventory.Name
                                   }).ToListAsync();


               }
       */


    }

}