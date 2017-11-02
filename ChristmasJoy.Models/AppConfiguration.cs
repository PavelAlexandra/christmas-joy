using Microsoft.Extensions.Configuration;

namespace ChristmasJoy.Models
{
    public interface IAppConfiguration
    {
        string StorageConnectionString { get; set; }
    }

    public class AppConfiguration: IAppConfiguration
    {
        public AppConfiguration(IConfiguration configuration)
        {
            StorageConnectionString = configuration.GetSection("MSAzureStorage:StorageConnectionString").Value;
        }

        public string StorageConnectionString { get; set; }
    }
}
