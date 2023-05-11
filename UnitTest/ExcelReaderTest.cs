using AutomixMVC.Models;
using AutomixMVC.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class ExcelReaderTest
    {
        private readonly ExcelReader _excelReader;

        public ExcelReaderTest()
        {
            _excelReader = new ExcelReader();
        }

        [Fact]
        public void ReadItemsFromExcel_ReturnsCorrectItems()
        {
            // Arrange
            var foodItems = new List<Food>
            {
                new Food { Name = "Gulyas", DateTime = DateTime.Parse("2023-01-01"), DailyMenuType = DailyMenuType.A },
                new Food { Name = "Husleves", DateTime = DateTime.Parse("2023-01-02"), DailyMenuType = DailyMenuType.B },
                new Food { Name = "Rantott csirke", DateTime = DateTime.Parse("2023-01-03"), DailyMenuType = DailyMenuType.C },
                new Food { Name = "Bolognai", DateTime = DateTime.Parse("2023-01-03"), DailyMenuType = DailyMenuType.D },
                new Food { Name = "Suti", DateTime = DateTime.Parse("2023-01-03"), DailyMenuType = DailyMenuType.F },
            };

            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Test");
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "DateTime";
            worksheet.Cells[1, 3].Value = "DailyMenuType";

            for (int i = 0; i < foodItems.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = foodItems[i].Name;
                worksheet.Cells[i + 2, 2].Value = foodItems[i].DateTime.ToString();
                worksheet.Cells[i + 2, 3].Value = foodItems[i].DailyMenuType.ToString();
            }

            var excelFileStream = new MemoryStream();
            package.SaveAs(excelFileStream);
            excelFileStream.Position = 0;  // Reset the stream position to the start

            // Act
            var result = _excelReader.ReadItemsFromExcel(excelFileStream);

            // Assert
            Assert.Equal(foodItems.Count, result.Count);

            for (int i = 0; i < foodItems.Count; i++)
            {
                Assert.Equal(foodItems[i].Name, result[i].Name);
                Assert.Equal(foodItems[i].DateTime, result[i].DateTime);
                Assert.Equal(foodItems[i].DailyMenuType, result[i].DailyMenuType);
            }
        }
    }

}
