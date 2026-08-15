namespace InventoryManagementSystem.Infrastructure.Services.Email
{
    public class EmailSettings
{
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; }
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
}