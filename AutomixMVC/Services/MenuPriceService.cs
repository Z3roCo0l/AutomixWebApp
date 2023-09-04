using AutomixMVC.Data;
using AutomixMVC.Models;

namespace AutomixMVC.Services
{
    public class MenuPriceService : IMenuPriceService
    {
        private readonly AutomixDbContext _context;

        public MenuPriceService(AutomixDbContext context)
        {
            _context = context;
        }

        public decimal GetCurrentMenuPrice()
        {
            return Convert.ToDecimal(_context.Settings.FirstOrDefault(s => s.Name == "DefaultMenuPrice")?.Value ?? "1490.0");
        }

        public void UpdateMenuPrice(decimal newPrice)
        {
            var menuPriceSetting = _context.Settings.FirstOrDefault(s => s.Name == "DefaultMenuPrice");

            if (menuPriceSetting != null)
            {
                menuPriceSetting.Value = newPrice.ToString("F2");
            }
            else
            {
                // If the setting doesn't exist, create a new one
                _context.Settings.Add(new Setting
                {
                    Name = "DefaultMenuPrice",
                    Value = newPrice.ToString("F2")
                });
            }

            _context.SaveChanges();
        }
    }
}
