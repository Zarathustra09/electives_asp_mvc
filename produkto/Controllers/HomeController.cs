using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using produkto.DataConnection;
using produkto.Models;
using System.Diagnostics;

namespace produkto.Controllers
{
    public class HomeController : Controller
    {
        private readonly MySqlDbContext _context;

        public HomeController(MySqlDbContext context)
        {
            _context = context;
        }

        // Assuming you have a method in your controller to retrieve the data
        public IActionResult Index()
        {
            // Query the database to get the most bought items
            var mostBoughtItems = _context.Transaction
                .GroupBy(t => t.ProductId)  // Group transactions by ProductId
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().ProductName, // Assuming ProductName is stored in Transaction model
                    TotalQuantity = g.Sum(t => t.Quantity) // Sum the quantities for each product
                })
                .OrderByDescending(g => g.TotalQuantity) // Order by total quantity in descending order
                .Take(5) // Take the top 5 most bought items
                .ToList();

            // Prepare data for the chart
            var labels = mostBoughtItems.Select(item => item.ProductName).ToArray();
            var quantities = mostBoughtItems.Select(item => item.TotalQuantity).ToArray();


            // Pass the data to the view
            ViewBag.ChartLabels = labels;
            ViewBag.ChartQuantities = quantities;

            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}