using Microsoft.AspNetCore.Http;

namespace ChristmasJoy.App.ViewModels
{
  public class UserImportViewModel
  {
    public string CommonPassword { get; set; }

    public IFormFile CsvFile { get; set; }
  }
}
