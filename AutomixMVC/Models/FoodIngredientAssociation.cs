namespace AutomixMVC.Models
{
    public class FoodIngredientAssociation
    {
        public int FoodId { get; set; }
        public Food Food { get; set; }
        public int FoodIngredientsID { get; set; }
        public FoodIngredients FoodIngredients { get; set; }
    }
}
