using System.Diagnostics;
using Azure.Identity;
using Magistri.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Magistri.Controllers
{
    [Authorize(Roles = "Student, Teacher, Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<AppUser> userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            this.userManager = userManager;
        }        
        public async Task<IActionResult> Index()
        {
            AppUser appUser = await userManager.GetUserAsync(HttpContext.User);
            _logger.LogInformation("Index called");
            string userName;
            if (appUser != null)
                userName = appUser.UserName;
            else
                userName = "";
            return View("Index", userName);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
