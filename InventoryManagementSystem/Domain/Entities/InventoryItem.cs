
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Domain.Entities
{

      public class InventoryItem
      {

            [Column(Order = 0)]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int InventoryId { get; set; }

            [Column(Order = 1)]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int ItemId { get; set; }
            [Required]
            public int Quantity { get; set; }

            public virtual Item? Item { get; set; }

            public virtual Inventory? Inventory { get; set; }
      }
}
