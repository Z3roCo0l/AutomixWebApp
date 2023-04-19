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
                    var foodItem = new Food
                    {
                        Name = worksheet.Cells[i, 1].Value.ToString(),
                        DateTime = DateTime.Parse(worksheet.Cells[i, 2].Value.ToString()),
                        DailyMenuType = (DailyMenuType)Enum.Parse(typeof(DailyMenuType), worksheet.Cells[i, 3].Value.ToString())
                    };

                    foodItems.Add(foodItem);
                }
            }

            return foodItems;
        }
    }
}