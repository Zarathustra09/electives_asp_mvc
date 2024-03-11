using System;
using System.ComponentModel.DataAnnotations;

namespace produkto.Models
{
    public class TransactionLog
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Allow manual insertion, so remove the [Required] attribute
        public string ProductName { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
