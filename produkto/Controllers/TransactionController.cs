using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using produkto.DataConnection;
using produkto.Models;

namespace produkto.Controllers
{
    public class TransactionController : Controller
    {
        private readonly MySqlDbContext _context;

        public TransactionController(MySqlDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transaction
                .Include(t => t.Product)
                .ToListAsync();
            return View(transactions);
        }

   
        public IActionResult Create()
        {
            // Fetch the list of products from the database
            var products = _context.Products.ToList();

            // Convert the list of products to a list of SelectListItem objects
            var productListItems = products.Select(p => new SelectListItem
            {
                Value = p.idproducts.ToString(),
                Text = p.productname
            }).ToList();

            // Pass the list of SelectListItem objects to the view
            ViewBag.Products = productListItems;

            return View();
        }
        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                // Add the transaction to the database
                _context.Transaction.Add(transaction);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return to the create view with validation errors
            return View(transaction);
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Product)
                .FirstOrDefaultAsync(m => m.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
