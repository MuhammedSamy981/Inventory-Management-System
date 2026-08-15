namespace InventoryManagementSystem.Application.Dtos{

public class TokenDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
}

