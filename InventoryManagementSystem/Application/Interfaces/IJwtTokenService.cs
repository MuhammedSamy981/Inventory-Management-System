using System.Security.Claims;

namespace InventoryManagementSystem.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string firstName, string lastName, string phoneNumber, string role);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken();
    }
}