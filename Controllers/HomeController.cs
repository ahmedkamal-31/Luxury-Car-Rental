using LuxuryCarRental.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxuryCarRental.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var featuredCars = await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Category)
            .Include(c => c.CarImages)
            .Where(c => c.IsFeatured && c.IsAvailable)
            .Take(6)
            .ToListAsync();

        var brands = await _context.Brands
            .Where(b => b.IsActive)
            .Take(8)
            .ToListAsync();

        ViewBag.Brands = brands;
        return View(featuredCars);
    }
}
