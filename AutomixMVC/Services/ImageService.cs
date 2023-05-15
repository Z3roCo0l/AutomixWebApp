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
        private readonly IConfiguration _configuration;

        public ImageService(AutomixDbContext context, IConfiguration configuration, string directoryPath = "wwwroot/Images/Gallery/")
        {
            _context = context;
            _configuration = configuration;
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
 
            string hostName = _configuration["HostConfig:HostName"] ??string.Empty;
            string scheme = _configuration["HostConfig:Scheme"] ??string.Empty;
            int port = _configuration.GetValue<int>("HostConfig:Port");
            string path = Path.Combine("/Images/Gallery", imageName + ".jpg").Replace("\\", "/");
            FileInfo fi = new FileInfo(Path.Combine("wwwroot", path));

            UriBuilder uriBuilder = new UriBuilder
            {
                Scheme = scheme,
                Host = hostName,
                Port = port,
                Path = path,
            };

            if (withModTime)
            {
                uriBuilder.Query = $"mod={fi.LastWriteTime.Ticks}";
            }

            return uriBuilder.Uri;
        }
    }
}
