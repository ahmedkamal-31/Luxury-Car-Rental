using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace LuxuryCarRental.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Admin/Admin/Users or /Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // POST: /Admin/Admin/ToggleBlockUser
        [HttpPost]
        public async Task<IActionResult> ToggleBlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (user.UserName == User.Identity.Name)
                {
                    TempData["Error"] = "You cannot block yourself!";
                    return RedirectToAction(nameof(Users));
                }

                if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow)
                {
                    await _userManager.SetLockoutEndDateAsync(user, null);
                    TempData["Success"] = "User unblocked successfully.";
                }
                else
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(75));
                    TempData["Success"] = "User blocked successfully.";
                }
            }

            return RedirectToAction(nameof(Users));
        }
    }
}
