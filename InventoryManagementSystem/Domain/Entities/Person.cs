using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Domain.Entities
{
        public class Person
        {
           [Key]
           [DatabaseGenerated(DatabaseGeneratedOption.None)]
           public int Id { get; set; }
           public string? Name { get; set; }

           [StringLength(15)]
           public string? Telephone { get; set; }
           public string? Fax { get; set; }

           [Required]
           [StringLength(15)]
           public string? Phone { get; set; }

           [Required]
           public string? Email { get; set; }
           public string? Website { get; set; }

           [Required]
           public string? Type { get; set; }

           [Required]
           [ForeignKey("ApplicationUserId")]
           public string? ApplicationUserId { get; set; }
           public ICollection<Operation> Operations { get; set; } = new HashSet<Operation>();
           public ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();

        }
}