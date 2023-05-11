namespace AutomixMVC.Services
{
    public interface IImageService
    {
        Task AddImagesToDatabase();
        Uri GetUrl(string imageName, bool withModTime = true);
    }
}
