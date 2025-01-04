using Magistri.Models;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace Magistri.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        public UsersController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHasher)
        {
            this.userManager = usrMgr;
            this.passwordHasher = passwordHasher;
        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AppUserViewModel appUserViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    UserName = appUserViewModel.UserName,
                    Email = appUserViewModel.Email
                };
                IdentityResult result = await userManager.CreateAsync(
                    appUser, appUserViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(appUserViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            AppUser appUser = await userManager.FindByIdAsync(id);
            if (appUser == null)
                return View("NotFound");
            else
                return View(appUser);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            AppUser appUser = await userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                if (!string.IsNullOrEmpty(email))
                    appUser.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");
                if (!string.IsNullOrEmpty(password))
                    appUser.PasswordHash = passwordHasher.HashPassword(appUser, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }

            }
            else
                ModelState.AddModelError("", "User not found");
            return View(appUser);
        }
        public async Task<IActionResult> Delete(string id)
        {
            AppUser appUser = await userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                IdentityResult result = await userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
