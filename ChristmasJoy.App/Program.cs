using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ChristmasJoy.App
{
  public class Program
    {
        public static void Main(string[] args)
        {
          CreateWebHostBuilder(args).Build().Run();
        }

    public static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
              webBuilder.UseIISIntegration();
              webBuilder.UseStartup<Startup>();
            });
  }
}
