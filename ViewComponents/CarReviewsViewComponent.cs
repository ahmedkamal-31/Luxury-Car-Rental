using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LuxuryCarRental.Data;

namespace LuxuryCarRental.ViewComponents
{
    public class CarReviewsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CarReviewsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int carId)
        {
            // Auto-create Reviews table if it does not exist in SQL Server
            try
            {
                await _context.Database.ExecuteSqlRawAsync(@"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
                    BEGIN
                        CREATE TABLE [Reviews] (
                            [Id] int NOT NULL IDENTITY,
                            [CarId] int NOT NULL,
                            [UserName] nvarchar(max) NULL,
                            [Rating] int NOT NULL,
                            [Comment] nvarchar(max) NOT NULL,
                            [CreatedAt] datetime2 NOT NULL,
                            CONSTRAINT [PK_Reviews] PRIMARY KEY ([Id])
                        );
                    END
                ");
            }
            catch
            {
                // Table auto-creation attempt completed
            }

            var reviews = await _context.Reviews
                .Where(r => r.CarId == carId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            ViewBag.CarId = carId;
            return View(reviews);
        }
    }
}
