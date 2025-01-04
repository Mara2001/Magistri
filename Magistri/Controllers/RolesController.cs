using Magistri.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace Magistri.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(
                    new IdentityRole(name));
                if (result.Succeeded) { return RedirectToAction("Index"); }
                else { Errors(result); }
            }
            return View(name);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded) { return RedirectToAction("Index"); }
                else { Errors(result); }
            }
            else
                ModelState.AddModelError("", "No Role Found");
            return View("Index", roleManager.Roles);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser appUser in userManager.Users)
            {
                var list = await
                    userManager.IsInRoleAsync(appUser, role.Name) ? members : nonMembers;
                list.Add(appUser);
            }
            return View( new RoleEdit { Role = role, Members = members, NonMembers = nonMembers });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleModification roleModification)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in roleModification.AddIds ?? new string[] { })
                {
                    AppUser appUser = await userManager.FindByIdAsync(userId);
                    if (appUser != null)
                    {
                        result = await
                            userManager.AddToRoleAsync(appUser, roleModification.RoleName);
                        if (!result.Succeeded) { Errors(result); }
                    }
                }
                foreach (string userId in roleModification.DeleteIds ?? new string[] { })
                {
                    AppUser appUser = await userManager.FindByIdAsync(userId);
                    if (appUser != null)
                    {
                        result = await
                            userManager.RemoveFromRoleAsync(appUser, roleModification.RoleName);
                        if (!result.Succeeded) { Errors(result); }
                    }
                }
            }
            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return View(roleModification.RoleId);                            
        }
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
