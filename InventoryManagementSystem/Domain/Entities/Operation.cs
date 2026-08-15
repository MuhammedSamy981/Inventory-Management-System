
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Domain.Entities
{
      public class Operation
      {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int Id { get; set; }
            [Required]
            [Column(TypeName = "date")]
            public DateTime? Date { get; set; }
            [Required]
            public int Quantity { get; set; }
            [Required]
            public string? Type { get; set; }

            [Column(TypeName = "date")]
            public DateTime? ProDate { get; set; }

            [Column(TypeName = "date")]
            public DateTime? ExpiryDate { get; set; }

            [Required]
            [ForeignKey("ItemId")]
            public int ItemId { get; set; }
            public Item? Item { get; set; }
            [Required]
            [ForeignKey("PersonId")]
            public int PersonId { get; set; }
            public Person? Person { get; set; }

            [Required]
            [ForeignKey("InventoryId")]
            public int InventoryId { get; set; }
            public Inventory? Inventory { get; set; }

            [Required]
            [ForeignKey("ApplicationUserId")]
            public string? ApplicationUserId { get; set; }

      }
}