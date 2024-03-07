using System.ComponentModel.DataAnnotations;


namespace produkto.Models
{
    public class Product
    {
        [Key]
        public int idproducts { get; set; }
        public string productname { get; set; }
        public string category { get; set; }
        public double price { get; set; }
        public DateTime datetimeadded { get; set; }
        public string description { get; set; }

        public int quantity { get; set; }
    }

}