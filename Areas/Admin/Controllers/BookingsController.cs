using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryCarRental.Data;

namespace LuxuryCarRental.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Bookings
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

            var bookings = await _context.Bookings
                .Include(b => b.Car)
                    .ThenInclude(c => c!.Brand)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        // POST: /Admin/Bookings/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
