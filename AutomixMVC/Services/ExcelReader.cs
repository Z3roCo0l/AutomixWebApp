using AutomixMVC.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace AutomixMVC.Services
{
    public class ExcelReader : IExcelReader
    {
        public List<Food> ReadItemsFromExcel(Stream excelFileStream)
        {
            var foodItems = new List<Food>();

            using (var package = new ExcelPackage(excelFileStream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet
                int totalRows = worksheet.Dimension.End.Row;

                for (int i = 2; i <= totalRows; i++) // Assuming the first row contains headers, so start reading from the second row
                {
                    var name = worksheet.Cells[i, 1].Value?.ToString();
                    var dateStr = worksheet.Cells[i, 2].Value?.ToString();
                    var dailyMenuTypeStr = worksheet.Cells[i, 3].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(dateStr) || string.IsNullOrWhiteSpace(dailyMenuTypeStr))
                    {
                        // Handle cases where any of the cells are empty or null
                        continue;
                    }

                    DateTime date;
                    if (!DateTime.TryParse(dateStr, out date))
                    {
                        // Handle cases where the date cannot be parsed
                        continue;
                    }

                    DailyMenuType dailyMenuType;
                    if (!Enum.TryParse<DailyMenuType>(dailyMenuTypeStr, out dailyMenuType))
                    {
                        // Handle cases where the enum value cannot be parsed
                        continue;
                    }

                    var foodItem = new Food
                    {
                        Name = name,
                        DateTime = date,
                        DailyMenuType = dailyMenuType
                    };

                    foodItems.Add(foodItem);
                }

            }

            return foodItems;
        }
    }
}