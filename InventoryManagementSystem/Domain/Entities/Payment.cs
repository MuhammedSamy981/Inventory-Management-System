using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Domain.Entities
{
    public class Payment
    {
        [Key]
        public string? Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [ForeignKey("ApplicationUserId")]
        public string? ApplicationUserId { get; set; }
    }

    public enum PaymentStatus { Pending, Completed, Failed }

}