using System;
using System.ComponentModel.DataAnnotations;

namespace produkto.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
