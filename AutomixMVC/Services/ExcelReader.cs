using AutomixMVC.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using static Azure.Core.HttpHeader;
using System.Text.RegularExpressions;

namespace AutomixMVC.Services
{
    
    public class ExcelReader : IExcelReader
    {
        public readonly IMenuPriceService _menuPriceService;
        public ExcelReader(IMenuPriceService menuPriceService)
        {
            _menuPriceService = menuPriceService;
        }
        public List<Food> ReadItemsFromExcel(Stream excelStream)
        {
            List<Food> foods = new List<Food>();
            using (var package = new ExcelPackage(excelStream))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    for (int row = 4; row <= worksheet.Dimension.End.Row; row += 6)
                    {
                        var dateTimeString = Convert.ToString(worksheet.Cells[row, 2].Value);
                        if (DateTime.TryParse(dateTimeString, out DateTime parsedDate))
                        {
                            for (int offset = 1; offset <= 5; offset++)
                            {
                                ReadAndAddFood(foods, worksheet, row + offset, 1, parsedDate);
                                ReadAndAddFood(foods, worksheet, row + offset, 5, parsedDate);
                                ReadAndAddFood(foods, worksheet, row + offset, 9, parsedDate);
                            }
                        }
                    }
                }
            }

            return foods;
        }

        private void ReadAndAddFood(List<Food> foods, ExcelWorksheet worksheet, int row, int col, DateTime date)
        {
            var dailyMenuTypeString = Convert.ToString(worksheet.Cells[row, col].Value);
            var foodName = Convert.ToString(worksheet.Cells[row, col + 1].Value);

            if (!string.IsNullOrEmpty(dailyMenuTypeString) &&
                Enum.TryParse(dailyMenuTypeString, out DailyMenuType dailyMenuType) &&
                !string.IsNullOrEmpty(foodName))  // Check if foodName is not null or empty
            {
                decimal foodPrice = _menuPriceService.GetCurrentMenuPrice();
                foods.Add(new Food(foodName, (FoodType)2, foodPrice)
                {
                    DateTime = date,
                    DailyMenuType = dailyMenuType,
                });
            }
        }
    }
}




