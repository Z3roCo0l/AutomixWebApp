namespace AutomixMVC.Models
{
    public class FoodIngredients
    {
        public int FoodIngredientsID { get; set; }
        public string? FoodIngredientName { get; set; }
        public ICollection<FoodIngredientAssociation>? FoodIngredientAssociations { get; set; }

    }
}
