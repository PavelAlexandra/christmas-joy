using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ChristmasJoy.App
{
  public class Program
    {
        public static void Main(string[] args)
        {
          CreateWebHostBuilder(args).Build().Run();
        }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((builderContext, config) =>
            {
              IHostingEnvironment env = builderContext.HostingEnvironment;

              config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            })
            .UseIISIntegration()
            .UseStartup<Startup>();
    }
}
