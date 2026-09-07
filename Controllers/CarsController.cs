using LuxuryCarRental.Data;
using LuxuryCarRental.Models;
using LuxuryCarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxuryCarRental.Controllers;

public class CarsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CarsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? brand, string? category, string? search)
    {
        var query = _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Category)
            .Include(c => c.CarImages)
            .Where(c => c.IsAvailable)
            .AsQueryable();

        if (!string.IsNullOrEmpty(brand))
            query = query.Where(c => c.Brand.Slug == brand);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(c => c.Category.Slug == category);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(c => c.Name.Contains(search) || c.Description.Contains(search));

        ViewBag.Brands = await _context.Brands.Where(b => b.IsActive).ToListAsync();
        ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();

        return View(await query.ToListAsync());
    }

    public async Task<IActionResult> Details(string slug)
    {
        var car = await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Category)
            .Include(c => c.CarImages)
            .FirstOrDefaultAsync(c => c.Slug == slug);

        if (car == null) return NotFound();

        var primaryImg = car.CarImages.FirstOrDefault(i => i.IsPrimary)?.ImagePath ?? "https://images.unsplash.com/photo-1544829099-b9a0c07fad1a?auto=format&fit=crop&w=1200&q=80";

        var inquiryVm = new RentalInquiryViewModel
        {
            CarId = car.Id,
            CarName = car.Name,
            CarImage = primaryImg,
            DailyPrice = car.DailyPrice,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1)
        };

        ViewBag.Car = car;
        return View(inquiryVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitInquiry(RentalInquiryViewModel model)
    {
        if (model.EndDate <= model.StartDate)
        {
            ModelState.AddModelError("EndDate", "تاريخ الانتهاء يجب أن يكون بعد تاريخ البدء.");
        }

        if (ModelState.IsValid)
        {
            var inquiry = new RentalInquiry
            {
                CarId = model.CarId,
                CustomerName = model.CustomerName,
                Email = model.Email,
                Phone = model.Phone,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.RentalInquiries.Add(inquiry);
            await _context.SaveChangesAsync();

            TempData["BookingSuccess"] = "Your booking inquiry has been submitted successfully! Our team will contact you shortly to confirm your reservation.";
            
            var car = await _context.Cars.FindAsync(model.CarId);
            return RedirectToAction(nameof(Details), new { slug = car?.Slug });
        }

        var carDetail = await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Category)
            .Include(c => c.CarImages)
            .FirstOrDefaultAsync(c => c.Id == model.CarId);

        ViewBag.Car = carDetail;
        return View("Details", model);
    }
}
