using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AutomixMVC.Models;
using AutomixMVC.Data;


namespace AutomixMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly AutomixDbContext _context;

        public MenuItemsController(AutomixDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IDictionary<string, IDictionary<string, string>>> GetMenuItems()
        {
            var menuItems = _context.FoodItems
                .Where(item => item.FoodTypeId == FoodType.DailyMenu)
                .ToList();

            var menuItemsJson = new Dictionary<string, IDictionary<string, string>>();

            foreach (var item in menuItems)
            {
                var dateStr = item.DateTime.ToString("yyyy-MM-dd");
                if (!menuItemsJson.ContainsKey(dateStr))
                {
                    menuItemsJson[dateStr] = new Dictionary<string, string>();
                }

                // Check that neither item.Name nor item.Description are null before adding them to the dictionary
                if (!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.Description))
                {
                    menuItemsJson[dateStr][item.Name] = item.Description;
                }
            }

            return menuItemsJson;
        }
    }
}