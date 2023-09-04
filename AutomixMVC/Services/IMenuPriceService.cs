namespace AutomixMVC.Services
{
    public interface IMenuPriceService
    {
        decimal GetCurrentMenuPrice();
        void UpdateMenuPrice(decimal newPrice);
    }
}
