namespace AutomixMVC.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public FoodType FoodTypeId { get; set; }
        public decimal FoodPrice { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<FoodIngredientAssociation>? FoodIngredientAssociations { get; set; }
        public DateTime DateTime { get; set; }
        public DailyMenuType? DailyMenuType { get; set; }
    
        public Food(string name, FoodType foodTypeId, decimal foodPrice)
        {

            Name = name;
            FoodTypeId = foodTypeId;
            FoodPrice = foodPrice;
        }
    }
}
