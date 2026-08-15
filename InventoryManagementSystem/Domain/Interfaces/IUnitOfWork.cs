namespace InventoryManagementSystem.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public IPaymentRepository PaymentRepository { get; }
        public IPersonRepository PersonRepository { get; }
        public IInventoryRepository InventoryRepository { get; }
        public IItemRepository ItemRepository { get; }
        public IInventoryItemRepository InventoryItemRepository { get; }
        public IOperationRepository OperationRepository { get; }
        Task<int> SaveChangesAsync();

    }
}