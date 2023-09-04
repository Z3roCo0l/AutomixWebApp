using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AutomixMVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Kitchen, Waiter")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult MenuUpload()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GalleryEdit()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult FoodItems()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Kitchen, Waiter")]
        public IActionResult Orders()
        {
            return View();
        }
    }
}
