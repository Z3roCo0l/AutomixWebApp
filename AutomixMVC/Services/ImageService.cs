using AutomixMVC.Data;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Dynamic;
using System.Threading.Tasks;
using System;
using AutomixMVC.Models;

namespace AutomixMVC.Services
{
    public class ImageService : IImageService
    {
        private readonly AutomixDbContext _context;
        private readonly string _directoryPath;

        public ImageService(AutomixDbContext context, string directoryPath = "wwwroot/Images/Gallery/")
        {
            _context = context;
            _directoryPath = directoryPath;
        }

        public async Task AddImagesToDatabase()
        {
            var imageNames = Directory.GetFiles(_directoryPath)
                .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png"))
                .Select(Path.GetFileNameWithoutExtension);

            foreach (var imageName in imageNames)
            {
                var image = new Image
                {
                    Name = imageName,
                };

                await _context.Images.AddAsync(image);
            }

            await _context.SaveChangesAsync();
        }

        public Uri GetUrl(string imageName, bool withModTime = true)
        {
            string path = Path.Combine("/Images/Gallery", imageName + ".jpg");
            FileInfo fi = new FileInfo(Path.Combine("", path));

            UriBuilder uriBuilder = new UriBuilder
            {
                Path = path
            };

            if (withModTime)
            {
                uriBuilder.Query = $"mod={fi.LastWriteTime.Ticks}";
            }

            return uriBuilder.Uri;
        }
    }
}
