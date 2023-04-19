namespace AutomixMVC.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FoodType FoodTypeId { get; set; }
        public decimal FoodPrice { get; set; }
        public string ImageUrl { get; set; }

        public DateTime DateTime { get; set; }
        public DailyMenuType? DailyMenuType { get; set; }

    }
}
