using AutomixMVC.Models;

namespace AutomixMVC.Repositories
{
    public interface IFoodItemRepository
    {
        void StoreFoodItemsInDb(List<Food> foodItems);
    }
}
