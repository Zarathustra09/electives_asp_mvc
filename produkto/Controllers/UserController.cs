using Microsoft.AspNetCore.Mvc;
using produkto.Models;
using System.Collections.Generic;
using produkto.Models; // Adjust the namespace
using produkto.DataConnection;
using Microsoft.EntityFrameworkCore;

public class UsersController : Controller
{

    private readonly MySqlDbContext _context;

    public UsersController(MySqlDbContext context)
    {
        _context = context;
    }

    // GET: UsersController
    public async Task<IActionResult> Index()
    {
        return View(await _context.Users.ToListAsync());
    }


}
