
using System.Linq;
using System.Threading.Tasks;
using AutomixMVC.Data;
using AutomixMVC.Models; 

namespace AutomixMVC.Services
{
    public class ShoppingBasketService
    {
        private readonly AutomixDbContext _context;
        private readonly Basket _basket;

        public ShoppingBasketService(AutomixDbContext context)
        {
            _context = context;
            _basket = new Basket(); // This can also be fetched from the DB based on the user session
        }

        public void AddToBasket(Food foodItem, int quantity = 1)
        {
            var basketItem = _basket.Items.FirstOrDefault(item => item.Food.Id == foodItem.Id);
            if (basketItem != null)
            {
                basketItem.Quantity += quantity;
            }
            else
            {
                _basket.Items.Add(new BasketItem { Food = foodItem, Quantity = quantity });
            }
        }

        public void RemoveFromBasket(Food foodItem)
        {
            var basketItem = _basket.Items.FirstOrDefault(item => item.Food.Id == foodItem.Id);
            if (basketItem != null)
            {
                _basket.Items.Remove(basketItem);
            }
        }

        public void UpdateBasketItemQuantity(Food foodItem, int quantity)
        {
            var basketItem = _basket.Items.FirstOrDefault(item => item.Food.Id == foodItem.Id);
            if (basketItem != null)
            {
                basketItem.Quantity = quantity;
            }
        }
        public void DecreaseItemQuantity(Food foodItem, int quantity = 1)
        {
            var basketItem = _basket.Items.FirstOrDefault(item => item.Food.Id == foodItem.Id);
            if (basketItem != null)
            {
                basketItem.Quantity -= quantity;
                if (basketItem.Quantity <= 0)
                {
                    _basket.Items.Remove(basketItem); // Remove the item if the quantity drops to zero or below
                }
            }
        }

        public decimal GetBasketTotalPrice()
        {
            return _basket.Items.Sum(item => item.Food.FoodPrice * item.Quantity);
        }

        public async Task<Purchase> FinalizePurchaseAsync()
        {
            var purchase = new Purchase
            {
                TotalPrice = GetBasketTotalPrice(),
                Items = _basket.Items
            };
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            _basket.Items.Clear(); // Clear the basket after finalizing the purchase

            return purchase;
        }
    }
}
