using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryCarRental.Data;
using LuxuryCarRental.Models;

namespace LuxuryCarRental.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(@"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Bookings')
                    BEGIN
                        CREATE TABLE [Bookings] (
                            [Id] int NOT NULL IDENTITY(1,1),
                            [UserId] nvarchar(450) NOT NULL,
                            [CarId] int NOT NULL,
                            [StartDate] datetime2 NOT NULL,
                            [EndDate] datetime2 NOT NULL,
                            [TotalPrice] decimal(18,2) NOT NULL,
                            [Status] nvarchar(max) NOT NULL DEFAULT 'Pending',
                            [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
                            CONSTRAINT [PK_Bookings] PRIMARY KEY ([Id]),
                            CONSTRAINT [FK_Bookings_Cars_CarId] FOREIGN KEY ([CarId]) REFERENCES [Cars] ([Id]) ON DELETE CASCADE
                        );
                    END");
            }
            catch { }

            var bookings = new List<Booking>();
            try
            {
                bookings = await _context.Bookings
                    .Include(b => b.Car)
                        .ThenInclude(c => c!.Brand)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                var cars = await _context.Cars
                    .Include(c => c.Brand)
                    .ToListAsync();

                ViewBag.CarsList = cars;
                ViewBag.TotalRevenue = bookings.Where(b => b.Status == "Confirmed").Sum(b => b.TotalPrice);
                ViewBag.TotalBookings = bookings.Count;
                ViewBag.PendingBookings = bookings.Count(b => b.Status == "Pending");
                ViewBag.ConfirmedBookings = bookings.Count(b => b.Status == "Confirmed");
                ViewBag.TotalCars = cars.Count;
            }
            catch
            {
                ViewBag.TotalRevenue = 0m;
                ViewBag.TotalBookings = 0;
                ViewBag.PendingBookings = 0;
                ViewBag.ConfirmedBookings = 0;
                ViewBag.TotalCars = 0;
                ViewBag.CarsList = new List<Car>();
            }

            return View(bookings);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = "CONFIRMED";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = "CANCELLED";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
