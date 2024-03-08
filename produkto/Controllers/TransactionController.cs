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

        // GET: Transaction/Index
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transaction.ToListAsync();
            return View(transactions);
        }

        public IActionResult Create()
        {
            // Fetch list of products from the database
            var products = _context.Products.ToList();

            // Create a SelectList using product id and product name
            var productList = new SelectList(products, "idproducts", "productname");

            // Set ViewBag.Products with the SelectList
            ViewBag.Products = productList;

            // Return the view
            return View();
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,ProductId,Quantity,TransactionDate")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the selected product from the database
                var selectedProduct = await _context.Products.FindAsync(transaction.ProductId);

                // Set the ProductName property of the transaction
                transaction.ProductName = selectedProduct?.productname;

                // Deduct the quantity from the product
                if (selectedProduct != null && selectedProduct.quantity >= transaction.Quantity)
                {
                    selectedProduct.quantity -= transaction.Quantity;
                    _context.Products.Update(selectedProduct);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Handle insufficient quantity error
                    ModelState.AddModelError(string.Empty, "Insufficient quantity.");
                    ViewBag.Products = new SelectList(_context.Products, "idproducts", "productname", transaction.ProductId);
                    return View(transaction);
                }

                // Add the transaction to the context and save changes
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, reload the products list and return the view with validation errors
            ViewBag.Products = new SelectList(_context.Products, "idproducts", "productname", transaction.ProductId);
            return View(transaction);
        }

    }
}
