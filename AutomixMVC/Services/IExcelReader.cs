using AutomixMVC.Models;

namespace AutomixMVC.Services
{
    public interface IExcelReader
    {
        List<Food> ReadItemsFromExcel(Stream excelFileStream);
    }
}
