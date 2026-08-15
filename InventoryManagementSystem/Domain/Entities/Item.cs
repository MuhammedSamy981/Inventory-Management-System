using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Domain.Entities
{
      public class Item
      {
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int Id { get; set; }
            [Required]
            public string? Name { get; set; }
            [Required]
            public string? Code { get; set; }
            [Required]
            public string? Size { get; set; }
            [Required]
            [ForeignKey("ApplicationUserId")]
            public string? ApplicationUserId { get; set; }

            public ICollection<InventoryItem> InventoryItems { get; set; } = new HashSet<InventoryItem>();

            public ICollection<Operation> Operations { get; set; } = new HashSet<Operation>();

      }

}
