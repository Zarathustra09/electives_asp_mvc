using System;
using System.ComponentModel.DataAnnotations;

namespace produkto.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Remove [Required] attribute to allow manual insertion
        public string? ProductName { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
