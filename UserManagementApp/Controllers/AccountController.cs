using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Models;
using UserManagementApp.ViewModels;

namespace UserManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email or Password is incorrect.");
                    return View(model);
                }

                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "Your account is blocked.");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    // Update last login time in UTC
                    user.LastLoginTime = DateTime.UtcNow;
                    await userManager.UpdateAsync(user);

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                    IsActive = true // Ensure new users are active by default
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()  // Moved inside the class
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Admin");
        }
    }  // Correctly closing the class
}
