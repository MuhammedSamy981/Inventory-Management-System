
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Domain.Filters;
using InventoryManagementSystem.Infrastructure.Data;

namespace InventoryManagementSystem.Infrastructure.Repositories
{

    public class OperationRepository : GenericRepository<Operation>, IOperationRepository
    {
        private readonly ApplicationDbContext _context;

        public OperationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        /*        public async Task<List<Object>> GetAllItemsMovementByInventoryIdAsync(int inventoryId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
                {

                    var operations=   _context.Set<Operation>()
                     .Where(o=>o.InventoryId == inventoryId)
                    .Include(i => i.Inventory).AsSplitQuery()
                    .Include(i => i.Item).AsSplitQuery().AsNoTracking();
        if(fromDate.HasValue && toDate.HasValue)
                    {
                        operations = operations.Where(o => o.Date >= fromDate.Value && o.Date <= toDate.Value);
                    }
                            return await operations.Select(o => new 
                                    {
                                        ItemId = o.Item!.Id,
                                        ItemName = o.Item.Name,
                                        MoveName = o.Type,
                                        MoveDate = o.Date,
                                        Quantity = o.Quantity
                                    }).ToListAsync<Object>(cancellationToken);

                }*/

        public async Task<List<Operation>> GetAllByApplicationUserIdAsync(string applicationUserId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Operation>().AsNoTracking().Where(i => i.ApplicationUserId == applicationUserId).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<List<Operation>> GetAllByTypeAsync(string type, string applicationUserId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Operation>()//.Include(p=>p.Category)
           .Where(o => o.Type == type && o.ApplicationUserId == applicationUserId)
           .OrderByDescending(o => o.Id).AsNoTracking().ToListAsync(cancellationToken);
        }


        /*        public async Task<int> GetAllQuantitiesAsync(OperationFilters filters, CancellationToken cancellationToken = default)
                {
                    Console.WriteLine($"/n/n/n ItemQuantities  fromDate={filters.FromDate}, toDate={filters.ToDate} /n/n/n");
                    int totalQuantities=0;

                 await _context.Set<Operation>().Where(o=>
                 ((o.InventoryId == filters.InventoryId && o.ItemId == filters.ItemId)||
                 (o.Type!.Contains(o.InventoryId.ToString()) && o.InventoryId == filters.InventoryId))
                 &&
                (o.Date >= filters.FromDate && o.Date <= filters.ToDate)).ForEachAsync(o =>
                {
                    Console.WriteLine($"\n\n\n\nType='{o.Type}', InventoryId={o.InventoryId}, Contains={o.Type!.Contains(o.InventoryId.ToString())}\n\n\n\n");
                                if (o.Type == "Exchange Permit"
                                || o.Type!.Contains(o.InventoryId.ToString()))
                                {
                                    totalQuantities -= o.Quantity;
                                }
                                else
                                {
                                    totalQuantities += o.Quantity;
                                }

                            },cancellationToken);

                            return totalQuantities;

                }*/

        public async Task<int> GetAllQuantitiesAsync(OperationFilters filters, string applicationUserId, CancellationToken cancellationToken = default)
        {
            int totalQuantities = 0;

            await _context.Set<Operation>().Where(o =>
            ((o.ItemId == filters.ItemId && o.InventoryId == filters.InventoryId) ||
            (o.Type!.Contains(filters.InventoryId.ToString()) && o.ItemId == filters.ItemId))
             &&
             o.Date >= filters.FromDate && o.Date <= filters.ToDate && o.ApplicationUserId == applicationUserId).ForEachAsync(o =>
            {
               if (o.Type == "Exchange Permit"
                   || o.Type!.Contains(filters.InventoryId.ToString()))
                 {
                   totalQuantities -= o.Quantity;
                 }
              else
                 {
                   totalQuantities += o.Quantity;
                 }

               }, cancellationToken);

               return totalQuantities;

        }
        /*
                public Task<List<Object>> GetAllFilteredByInventoryAsync(OperationFilters filters, CancellationToken cancellationToken = default)
                {

                return _context.Set<Operation>().Where(o=>(o.InventoryId == filters.InventoryId && o.ItemId == filters.ItemId)||
                 (o.Type!.Contains(filters.InventoryId.ToString()) && o.InventoryId == filters.InventoryId) &&
                (o.Date >= filters.FromDate && o.Date <= filters.ToDate))
                .Select(o => new 
                            {
                                Type = o.Type,
                                Quantity = o.Quantity
                            }).ToListAsync<Object>(cancellationToken);

                }

                public Task<List<Object>> GetAllFilteredByItemAsync(OperationFilters filters, CancellationToken cancellationToken = default)
                {
                           return _context.Set<Operation>().Where(o=>(o.ItemId == filters.ItemId && o.InventoryId == filters.InventoryId)||
                 (o.Type!.Contains(filters.InventoryId.ToString()) && o.ItemId == filters.ItemId) &&
                (o.Date >= filters.FromDate && o.Date <= filters.ToDate)) 
                .Select(o => new 
                            {
                                Type = o.Type,
                                Quantity = o.Quantity
                            }).ToListAsync<Object>(cancellationToken);

                }
         */
        public async Task<List<int>> GetAllSupplierIdsAsync(int inventoryId, int itemId, string applicationUserId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Operation>()
            .Where(o => o.InventoryId == inventoryId && o.ItemId == itemId && o.Type == "Supply Permit" && o.ApplicationUserId == applicationUserId)
            .Select(o => o.PersonId).ToListAsync();
        }

        public async Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Operation>().AnyAsync(o => o.Id == id, cancellationToken);
        }

    }



}