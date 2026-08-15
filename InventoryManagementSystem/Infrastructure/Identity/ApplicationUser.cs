using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Infrastructure.Identity
{
  public class ApplicationUser : IdentityUser
  {
    [Required]
    [StringLength(15)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(15)]
    public string? LastName { get; set; }
    public bool HasActiveSubscription { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public ICollection<Person> People { get; set; } = new HashSet<Person>();
    public ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();
    public ICollection<Item> Items { get; set; } = new HashSet<Item>();
    public ICollection<Operation> Operations { get; set; } = new HashSet<Operation>();
  }
}