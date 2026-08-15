namespace InventoryManagementSystem.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public string IPAddress { get; set; } = string.Empty;
    }
}