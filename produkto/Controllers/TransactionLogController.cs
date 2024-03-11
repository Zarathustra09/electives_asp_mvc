using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using produkto.DataConnection;
using System.Threading.Tasks;

namespace produkto.Controllers
{
    public class TransactionLogController : Controller
    {
        private readonly MySqlDbContext _context;

        public TransactionLogController(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var transactionLogs = await _context.TransactionLogs.ToListAsync();
            return View(transactionLogs);
        }

    }
}
