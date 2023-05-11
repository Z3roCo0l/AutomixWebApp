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

namespace UnitTest
{
    public class ImageServiceTest
    {
        private readonly ImageService _imageService;
        private readonly AutomixDbContext _context;
        private readonly MockFileSystem _fileSystem;

        public ImageServiceTest()
        {
            _fileSystem = new MockFileSystem();

            var inMemorySettings = new Dictionary<string, string> {
        {"Host", "LocalHost"},
        {"Database", "InMemoryTest"},
    };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var options = new DbContextOptionsBuilder<AutomixDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AutomixDbContext(options, configuration);
            _context.Database.EnsureDeleted();

            _imageService = new ImageService(_context);
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
            var imageService = new ImageService(_context, tempDir);

            // Act
            await imageService.AddImagesToDatabase();

            // Assert
            Assert.Equal(5, _context.Images.Count());

            // Cleanup
            Directory.Delete(tempDir, recursive: true);
        }


        [Fact]
        public void GetUrl_ReturnsUrlWithModTime()
        {
            // Arrange
            string imageName = "testImage";

            // Act
            var result = _imageService.GetUrl(imageName, true);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("mod=", result.Query);
        }

        [Fact]
        public async void AddImagesToDatabase_ThrowsExceptionWhenDirectoryNotFound()
        {
            // Arrange
            var imageService = new ImageService(_context, "nonexistent/directory");

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

            var imageService = new ImageService(_context, tempDir);

            // Act
            await imageService.AddImagesToDatabase();

            // Assert
            Assert.Equal(5, _context.Images.Count());

            // Cleanup
            Directory.Delete(tempDir, recursive: true);
        }
    }
}