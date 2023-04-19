using AutomixMVC.Models;
using AutomixMVC.Repositories;
using AutomixMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

public class ImportController : Controller
{
    private readonly IExcelReader _excelReader;
    private readonly IFoodItemRepository _foodItemRepository;

    public ImportController(IExcelReader excelReader, IFoodItemRepository foodItemRepository) 
    {
        _excelReader = excelReader;
        _foodItemRepository = foodItemRepository;
    }

    [HttpPost]
    public IActionResult ImportData(IFormFile excelFile)
    {
        if (excelFile == null || excelFile.Length == 0)
        {
            ModelState.AddModelError("File", "Please upload a valid Excel file.");
            return View(); // You can return a view with a file upload form
        }

        using var stream = new MemoryStream();
        excelFile.CopyTo(stream);
        stream.Position = 0;

        List<Food> foodItems = _excelReader.ReadItemsFromExcel(stream);
        _foodItemRepository.StoreFoodItemsInDb(foodItems);

        return View("ImportSuccess");
    }
}
kukucs