using AutomixMVC.Data;
using AutomixMVC.Models;

namespace AutomixMVC.Repositories
{
    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly AutomixDbContext _context;

        public FoodItemRepository(AutomixDbContext context)
        {
            _context = context;
        }

        public void StoreFoodItemsInDb(List<Food> foodItems)
        {
            _context.FoodItems.AddRange(foodItems);
            _context.SaveChanges();
        }
    }
}
