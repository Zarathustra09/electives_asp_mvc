using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using produkto.Models;

namespace produkto.DataConnection
{
    public class MySqlDbContext:DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }
    }
}
