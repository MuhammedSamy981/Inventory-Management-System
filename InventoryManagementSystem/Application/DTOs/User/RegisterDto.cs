namespace InventoryManagementSystem.Application.Dtos
{
  public class RegisterDto
  {
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; } 
    public string Currency { get; set; } = string.Empty;
  }
}

//public record RegisterUserResponse(string UserId, string PayPalApprovalOrderId, string Message);