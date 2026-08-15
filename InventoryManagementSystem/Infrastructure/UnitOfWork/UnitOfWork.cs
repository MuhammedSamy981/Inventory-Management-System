using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Infrastructure.Data;

namespace InventoryManagementSystem.Infrastructure.UnitOfWork
{
  public class UnitOfWork : IUnitOfWork
  {
    public IPaymentRepository PaymentRepository { get; }
    public IPersonRepository PersonRepository { get; }
    public IInventoryRepository InventoryRepository { get; }
    public IItemRepository ItemRepository { get; }
    public IInventoryItemRepository InventoryItemRepository { get; }
    public IOperationRepository OperationRepository { get; }

    private readonly ApplicationDbContext _context;
    public UnitOfWork
    (
     ApplicationDbContext context,
     IPaymentRepository paymentRepository,
     IPersonRepository personRepository,
     IInventoryRepository inventoryRepository,
     IItemRepository itemRepository,
     IInventoryItemRepository inventoryItemRepository,
     IOperationRepository operationRepository)
    {
      _context = context;
      PaymentRepository = paymentRepository;
      PersonRepository = personRepository;
      InventoryRepository = inventoryRepository;
      ItemRepository = itemRepository;
      InventoryItemRepository = inventoryItemRepository;
      OperationRepository = operationRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
      return await _context.SaveChangesAsync();
    }

  }
}