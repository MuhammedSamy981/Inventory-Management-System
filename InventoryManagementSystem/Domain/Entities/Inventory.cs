
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Domain.Entities
{
      public class Inventory
      {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int Id { get; set; }
            [Required]
            public string? Name { get; set; }
            [Required]
            public string? Address { get; set; }
            public ICollection<Operation> Operations { get; set; } = new HashSet<Operation>();
            public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new HashSet<InventoryItem>();

            [Required]
            [ForeignKey("ManagerId")]
            public int ManagerId { get; set; }
            public Person? Manager { get; set; }

            [Required]
            [ForeignKey("ApplicationUserId")]
            public string? ApplicationUserId { get; set; }
      }
}