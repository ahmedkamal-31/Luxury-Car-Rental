using LuxuryCarRental.Data;
using LuxuryCarRental.Models;
using LuxuryCarRental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LuxuryCarRental.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CarsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public CarsController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        var cars = await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Category)
            .Include(c => c.CarImages)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return View(cars);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Brands = new SelectList(await _context.Brands.Where(b => b.IsActive).ToListAsync(), "Id", "Name");
        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CarCreateViewModel model)
    {
        if (!model.BrandId.HasValue && string.IsNullOrWhiteSpace(model.NewBrandName))
        {
            ModelState.AddModelError("BrandId", "يرجى اختيار ماركة من القائمة أو كتابة اسم ماركة جديدة.");
        }

        if (ModelState.IsValid)
        {
            int selectedBrandId;
            if (!string.IsNullOrWhiteSpace(model.NewBrandName))
            {
                var trimmedBrandName = model.NewBrandName.Trim();
                var brandSlug = trimmedBrandName.ToLower().Replace(" ", "-").Replace("/", "-");

                var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name.ToLower() == trimmedBrandName.ToLower());
                if (existingBrand != null)
                {
                    selectedBrandId = existingBrand.Id;
                }
                else
                {
                    var newBrand = new Brand
                    {
                        Name = trimmedBrandName,
                        Slug = brandSlug,
                        IsActive = true
                    };
                    _context.Brands.Add(newBrand);
                    await _context.SaveChangesAsync();
                    selectedBrandId = newBrand.Id;
                }
            }
            else
            {
                selectedBrandId = model.BrandId!.Value;
            }

            string slug = model.Name.ToLower().Replace(" ", "-").Replace("/", "-");
            if (await _context.Cars.AnyAsync(c => c.Slug == slug))
            {
                slug += "-" + DateTime.Now.Ticks;
            }

            var car = new Car
            {
                Name = model.Name,
                Slug = slug,
                BrandId = selectedBrandId,
                CategoryId = model.CategoryId,
                Year = model.Year,
                Color = model.Color,
                BodyType = model.BodyType,
                Seats = model.Seats,
                Doors = model.Doors,
                Transmission = model.Transmission,
                FuelType = model.FuelType,
                Engine = model.Engine,
                EnginePower = model.EnginePower,
                Description = model.Description,
                DailyPrice = model.DailyPrice,
                WeeklyPrice = model.WeeklyPrice,
                IsFeatured = model.IsFeatured,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "cars");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                var carImage = new CarImage
                {
                    CarId = car.Id,
                    ImagePath = "/uploads/cars/" + uniqueFileName,
                    IsPrimary = true,
                    DisplayOrder = 1
                };

                _context.CarImages.Add(carImage);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Car added successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Brands = new SelectList(await _context.Brands.Where(b => b.IsActive).ToListAsync(), "Id", "Name", model.BrandId);
        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "Id", "Name", model.CategoryId);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var car = await _context.Cars
            .Include(c => c.CarImages)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null) return NotFound();

        var primaryImg = car.CarImages.FirstOrDefault(i => i.IsPrimary)?.ImagePath;

        var model = new CarEditViewModel
        {
            Id = car.Id,
            Name = car.Name,
            BrandId = car.BrandId,
            CategoryId = car.CategoryId,
            Year = car.Year,
            Color = car.Color,
            BodyType = car.BodyType,
            Seats = car.Seats,
            Doors = car.Doors,
            Transmission = car.Transmission,
            FuelType = car.FuelType,
            Engine = car.Engine,
            EnginePower = car.EnginePower,
            Description = car.Description,
            DailyPrice = car.DailyPrice,
            WeeklyPrice = car.WeeklyPrice,
            IsFeatured = car.IsFeatured,
            ExistingImagePath = primaryImg
        };

        ViewBag.Brands = new SelectList(await _context.Brands.Where(b => b.IsActive).ToListAsync(), "Id", "Name", car.BrandId);
        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "Id", "Name", car.CategoryId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CarEditViewModel model)
    {
        if (id != model.Id) return NotFound();

        if (!model.BrandId.HasValue && string.IsNullOrWhiteSpace(model.NewBrandName))
        {
            ModelState.AddModelError("BrandId", "يرجى اختيار ماركة من القائمة أو كتابة اسم ماركة جديدة.");
        }

        if (ModelState.IsValid)
        {
            var car = await _context.Cars.Include(c => c.CarImages).FirstOrDefaultAsync(c => c.Id == id);
            if (car == null) return NotFound();

            int selectedBrandId;
            if (!string.IsNullOrWhiteSpace(model.NewBrandName))
            {
                var trimmedBrandName = model.NewBrandName.Trim();
                var brandSlug = trimmedBrandName.ToLower().Replace(" ", "-").Replace("/", "-");

                var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name.ToLower() == trimmedBrandName.ToLower());
                if (existingBrand != null)
                {
                    selectedBrandId = existingBrand.Id;
                }
                else
                {
                    var newBrand = new Brand
                    {
                        Name = trimmedBrandName,
                        Slug = brandSlug,
                        IsActive = true
                    };
                    _context.Brands.Add(newBrand);
                    await _context.SaveChangesAsync();
                    selectedBrandId = newBrand.Id;
                }
            }
            else
            {
                selectedBrandId = model.BrandId!.Value;
            }

            car.Name = model.Name;
            car.BrandId = selectedBrandId;
            car.CategoryId = model.CategoryId;
            car.Year = model.Year;
            car.Color = model.Color;
            car.BodyType = model.BodyType;
            car.Seats = model.Seats;
            car.Doors = model.Doors;
            car.Transmission = model.Transmission;
            car.FuelType = model.FuelType;
            car.Engine = model.Engine;
            car.EnginePower = model.EnginePower;
            car.Description = model.Description;
            car.DailyPrice = model.DailyPrice;
            car.WeeklyPrice = model.WeeklyPrice;
            car.IsFeatured = model.IsFeatured;
            car.UpdatedAt = DateTime.UtcNow;

            // Handle New Image Upload if supplied
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "cars");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                // Remove or unset old primary image
                var oldPrimary = car.CarImages.FirstOrDefault(i => i.IsPrimary);
                if (oldPrimary != null)
                {
                    _context.CarImages.Remove(oldPrimary);
                }

                var newCarImage = new CarImage
                {
                    CarId = car.Id,
                    ImagePath = "/uploads/cars/" + uniqueFileName,
                    IsPrimary = true,
                    DisplayOrder = 1
                };

                _context.CarImages.Add(newCarImage);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Car updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Brands = new SelectList(await _context.Brands.Where(b => b.IsActive).ToListAsync(), "Id", "Name", model.BrandId);
        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "Id", "Name", model.CategoryId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car != null)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Car deleted successfully!";
        }
        return RedirectToAction(nameof(Index));
    }
}
