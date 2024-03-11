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

                // Insert record into TransactionLog
                var transactionLog = new TransactionLog
                {
                    ProductId = transaction.ProductId,
                    ProductName = transaction.ProductName,
                    Quantity = transaction.Quantity,
                    TransactionDate = transaction.TransactionDate
                };
                _context.TransactionLogs.Add(transactionLog);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, reload the products list and return the view with validation errors
            ViewBag.Products = new SelectList(_context.Products, "idproducts", "productname", transaction.ProductId);
            return View(transaction);
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,ProductId,Quantity")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                    // Retrieve the original transaction
                    var originalTransaction = await _context.Transaction.FindAsync(id);

                    // Calculate the quantity difference for refund
                    int quantityDifference = originalTransaction.Quantity - transaction.Quantity;

                    // Update the transaction quantity
                    originalTransaction.Quantity = transaction.Quantity;

                    // Update the product quantity for refund
                    var product = await _context.Products.FindAsync(originalTransaction.ProductId);
                    if (product != null)
                    {
                        product.quantity += quantityDifference;
                        await _context.SaveChangesAsync();
                    }

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                
               
                return RedirectToAction(nameof(Index));
            }

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
            if (transaction == null)
            {
                return NotFound();
            }

           
                // Retrieve the corresponding product
                var product = await _context.Products.FindAsync(transaction.ProductId);
                if (product != null)
                {
                    // Add the quantity back to the product
                    product.quantity += transaction.Quantity;
                }

                // Remove the transaction from the database
                _context.Transaction.Remove(transaction);
                await _context.SaveChangesAsync();
       

            return RedirectToAction(nameof(Index));
        }




    }
}
