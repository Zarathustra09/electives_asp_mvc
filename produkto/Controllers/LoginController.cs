using Microsoft.AspNetCore.Mvc;
using produkto.Models;
using MySqlConnector;

namespace produkto.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {

            string DefaultConnection = "Server=localhost;Database=electives_mvc_asp;Uid=root;Pwd=12345;";
            // Connect to MySQL database (replace with your connection string)
            using (MySqlConnection connection = new MySqlConnection(DefaultConnection))
            {
                connection.Open();

                // Use a parameterized query for security
                string query = "SELECT UserID, Email, Name FROM Users WHERE Email = @Email AND Password = @Password";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password); // Store passwords securely in production!

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    // User found, store necessary details in session or create a temporary authentication token for security
                    return RedirectToAction("Index", "Home"); // Redirect to the homepage (or any other desired page)
                }
                else
                {
                    // Credentials incorrect
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(); // Render the login page again with error message
                }
            }
        }

        public IActionResult Logout()
        {           
            // Redirect to the login page
            return RedirectToAction("Login", "Login");
        }

    }
}
