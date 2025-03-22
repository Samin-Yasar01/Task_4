using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Models;

namespace UserManagementApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<Users> userManager;

        public AdminController(UserManager<Users> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users
                .OrderBy(u => u.LastLoginTime)
                .ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Block(string[] userIds)
        {
            foreach (var userId in userIds)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.IsActive = false;
                    await userManager.UpdateAsync(user);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(string[] userIds)
        {
            foreach (var userId in userIds)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.IsActive = true;
                    await userManager.UpdateAsync(user);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] userIds)
        {
            foreach (var userId in userIds)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.DeleteAsync(user);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
