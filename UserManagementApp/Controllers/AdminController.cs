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
        private readonly SignInManager<Users> signInManager;

        public AdminController(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var users = await userManager.Users
                .OrderBy(u => u.LastLoginTime)
                .ToListAsync();

            if (currentUser != null && !currentUser.IsActive)
            {
                return View("RestrictedIndex", users);
            }

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Block(string[] userIds)
        {
            var currentUser = await userManager.GetUserAsync(User);

            foreach (var userId in userIds)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.IsActive = false;
                    await userManager.UpdateAsync(user);

                    if (user.Id == currentUser.Id)
                    {
                        await signInManager.SignOutAsync();
                        // return RedirectToAction("RestrictedIndex");
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Unblock(string[] userIds)
        {
            var currentUser = await userManager.GetUserAsync(User);
            //if (currentUser != null && !currentUser.IsActive)
            //{
               // return RedirectToAction("RestrictedIndex");
            //}

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
            var currentUser = await userManager.GetUserAsync(User);

            foreach (var userId in userIds)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.DeleteAsync(user);

                    if (user.Id == currentUser.Id)
                    {
                        await signInManager.SignOutAsync();
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RestrictedIndex()
        {
            var users = await userManager.Users
                .OrderBy(u => u.LastLoginTime)
                .ToListAsync();
            return View(users);
        }
    }
}