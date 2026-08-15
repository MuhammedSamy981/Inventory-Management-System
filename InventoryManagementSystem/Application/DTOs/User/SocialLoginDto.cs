namespace InventoryManagementSystem.Application.Dtos
{
  public class SocialLoginDto
  {
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public int AreaId { get; set; }

  }
}
