using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryCarRental.Data;
using LuxuryCarRental.Models;
using System.Security.Claims;

namespace LuxuryCarRental.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Bookings/MyBookings
        public async Task<IActionResult> MyBookings()
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookings = await _context.Bookings
                .Include(b => b.Car)
                    .ThenInclude(c => c!.Brand)
                .Include(b => b.Car)
                    .ThenInclude(c => c!.CarImages)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.StartDate)
                .ToListAsync();

            return View(bookings);
        }

        // POST: /Bookings/Create
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Create(int carId, DateTime? startDate, DateTime? endDate, decimal? totalPrice)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                return RedirectToAction("Index", "Cars");
            }

            DateTime validStartDate = startDate ?? DateTime.Today;
            DateTime validEndDate = endDate ?? validStartDate.AddDays(1);

            if (validEndDate <= validStartDate)
            {
                validEndDate = validStartDate.AddDays(1);
            }

            int days = (validEndDate - validStartDate).Days;
            if (days <= 0) days = 1;

            decimal finalPrice = totalPrice ?? 0;
            if (finalPrice <= 0)
            {
                var priceProp = car.GetType().GetProperty("PricePerDay") ?? car.GetType().GetProperty("Price") ?? car.GetType().GetProperty("DailyPrice");
                decimal dailyRate = priceProp != null ? Convert.ToDecimal(priceProp.GetValue(car) ?? 300) : 300;
                finalPrice = days * dailyRate;
            }

            var booking = new Booking
            {
                UserId = userId,
                CarId = carId,
                StartDate = validStartDate,
                EndDate = validEndDate,
                TotalPrice = finalPrice,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyBookings", "Bookings");
        }
    }
}
