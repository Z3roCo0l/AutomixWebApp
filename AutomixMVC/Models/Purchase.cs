
using System;
using System.Collections.Generic;

namespace AutomixMVC.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

 
    }
}
