using AutomixMVC.Data;
using AutomixMVC.Models;
using AutomixMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;





namespace AutomixMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AutomixDbContext _context;
        private readonly IImageService _imageService;

        public HomeController(ILogger<HomeController> logger, AutomixDbContext context, IImageService imageService)
        {
            _logger = logger;
            _context = context;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            List<Image> model;

            try
            {
                model = await _context.Images.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load images for gallery.");
                model = new List<Image>(); // Use an empty list if loading failed
                
                // Add a message to ViewData to indicate that no images were loaded
                ViewData["GalleryMessage"] = "Failed to load images for gallery.";
            }

            return View(model);
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
        
        public IActionResult ExcelUploader()
        {
            return View();
        }
       
        public async Task<IActionResult> TestAddImages()
        {
            await _imageService.AddImagesToDatabase();

            return RedirectToAction("Index");
        }


        [HttpPost]

        [Authorize]
        public IActionResult ImportData(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0 || !Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("File", "Please upload a valid Excel file.");
                return View("ExcelUploader");
            }

            // Your file processing logic here

            return View("ImportSuccess");
        }
    }
}