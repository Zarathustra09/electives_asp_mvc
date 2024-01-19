using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.EntityFrameworkCore;
using produkto.DataConnection;

namespace produkto.Controllers
{
    public class ProductController : Controller
    {
        private readonly MySqlDbContext _context;
        // GET: ProductController
        public ProductController(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }
    }
}
