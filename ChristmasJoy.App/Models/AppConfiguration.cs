using Microsoft.Extensions.Configuration;

namespace ChristmasJoy.App.Models
{
  public interface IAppConfiguration
  {
    string DocumentDBEndpointUrl { get; set; }
    string DocumentDBKey { get; set; }
  }

  public class AppConfiguration : IAppConfiguration
  {
    public AppConfiguration(IConfiguration configuration)
    {
      DocumentDBEndpointUrl = configuration.GetSection("MSAzureStorage:DocumentDBEndpointUrl").Value;
      DocumentDBKey = configuration.GetSection("MSAzureStorage:DocumentDBKey").Value;
    }

    public string DocumentDBEndpointUrl { get; set; }

    public string DocumentDBKey { get; set; }
  }
}
