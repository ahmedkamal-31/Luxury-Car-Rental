using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using LuxuryCarRental.Models;
using LuxuryCarRental.Data;
using System;

namespace LuxuryCarRental.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(int carId, int rating, string comment)
        {
            if (!string.IsNullOrWhiteSpace(comment))
            {
                var review = new Review
                {
                    CarId = carId,
                    UserName = User.Identity?.Name ?? "Anonymous",
                    Rating = rating,
                    Comment = comment,
                    CreatedAt = DateTime.Now
                };
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
            }

            // Redirect back to the exact page where the user submitted the form
            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Details", "Cars", new { area = "", id = carId });
        }
    }
}
