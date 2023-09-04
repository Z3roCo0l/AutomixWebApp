
using System.Collections.Generic;

namespace AutomixMVC.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public int? UserId { get; set; }
        public virtual User User { get; set; }
       
    }
}
