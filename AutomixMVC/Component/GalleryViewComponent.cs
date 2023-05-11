using AutomixMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutomixMVC.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutomixMVC.Services;

namespace AutomixMVC.Component
{
    public class GalleryViewComponent : ViewComponent
    {
        private readonly AutomixDbContext _context;
        private readonly ILogger<GalleryViewComponent> _logger;
        private readonly IImageService _imageService;

        public GalleryViewComponent(AutomixDbContext context, ILogger<GalleryViewComponent> logger, IImageService imageService)
        {
            _context = context;
            _logger = logger;
            _imageService = imageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var images = new List<Image>();

            try
            {
                images = await _context.Images.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception.
                _logger.LogError(ex, "An error occurred while getting images.");
            }

            // Create a ViewModel for your view that includes the Image and its Uri
            var model = images.Select(image => new ImageViewModel
            {
                Image = image,
                Uri = _imageService.GetUrl(image.Name)
            }).ToList();

            return View(model);
        }
    }
}
