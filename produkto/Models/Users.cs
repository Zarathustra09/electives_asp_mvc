using System.ComponentModel.DataAnnotations;


namespace produkto.Models
{
  
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime DatetimeCreated { get; set; }
    }

}
