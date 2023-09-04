
using System;

namespace AutomixMVC.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public Food Food { get; set; } 
        public int Quantity { get; set; }
    }
}
