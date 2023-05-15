using Xunit;
using Moq;
using AutomixMVC.Services;
using AutomixMVC.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using System.Diagnostics;
using System.Security.Policy;

namespace UnitTest
{
    public class ImageServiceTest
    {
        private readonly ImageService _imageService;
        private readonly AutomixDbContext _context;
        private readonly MockFileSystem _fileSystem;
        private readonly IConfiguration _configuration;

        public ImageServiceTest()
        {
            _fileSystem = new MockFileSystem();
            
            var inMemorySettings = new Dictionary<string, string> {
                {"Host", "LocalHost"},
                {"Database", "InMemoryTest"},
            };
            
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var options = new DbContextOptionsBuilder<AutomixDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AutomixDbContext(options, _configuration);
            _context.Database.EnsureDeleted();

            _imageService = new ImageService(_context, _configuration);
        }


        [Fact]
        public async void AddImagesToDatabase_AddsImages()
        {
            // Arrange
            // Create a temp directory and some temp files for testing
            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            for (int i = 0; i < 5; i++)
            {
                File.Create(Path.Combine(tempDir, $"test{i}.jpg")).Close();
            }

            // Change the ImagesService to accept a path to the directory as a parameter
            var imageService = new ImageService(_context, _configuration, tempDir);

            // Act
            await imageService.AddImagesToDatabase();

            // Assert
            Assert.Equal(5, _context.Images.Count());

            // Cleanup
            Directory.Delete(tempDir, recursive: true);
        }


        //[Fact]
        //public void GetUrl_ReturnsValidUri()
        //{
        //    // Arrange
        //    var configuration = new Mock<IConfiguration>();
        //    configuration.Setup(c => c["HostConfig:HostName"]).Returns("localhost");
        //    configuration.Setup(c => c["HostConfig:Scheme"]).Returns("https");

        //    var customConfiguration = new Mock<IConfiguration>();
        //    customConfiguration.Setup(c => c[It.IsAny<string>()]).Returns("44371");

        //    configuration.Setup(c => c.GetSection("HostConfig")).Returns(customConfiguration.Object.GetSection("HostConfig"));

        //    var imageService = new ImageService(null, configuration.Object);

        //    string imageName = "testImage";
        //    var expectedUri = new Uri($"https://localhost/Images/Gallery/{imageName}.jpg");

        //    // Act
        //    var result = imageService.GetUrl(imageName, false);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(expectedUri, result);
        //}


        [Fact]
        public async void AddImagesToDatabase_ThrowsExceptionWhenDirectoryNotFound()
        {
            // Arrange
            var imageService = new ImageService(_context, _configuration, "nonexistent/directory");

            // Act & Assert
            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => imageService.AddImagesToDatabase());
        }

        [Fact]
        public async void AddImagesToDatabase_IgnoresNonImageFiles()
        {
            // Arrange
            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            // Create image files
            for (int i = 0; i < 5; i++)
            {
                File.Create(Path.Combine(tempDir, $"test{i}.jpg")).Close();
            }

            // Create non-image files
            for (int i = 0; i < 5; i++)
            {
                File.Create(Path.Combine(tempDir, $"test{i}.txt")).Close();
            }

            var imageService = new ImageService(_context, _configuration, tempDir);

            // Act
            await imageService.AddImagesToDatabase();

            // Assert
            Assert.Equal(5, _context.Images.Count());

            // Cleanup
            Directory.Delete(tempDir, recursive: true);
        }
    }
}