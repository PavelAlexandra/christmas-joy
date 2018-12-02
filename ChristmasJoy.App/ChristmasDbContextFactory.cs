using ChristmasJoy.App.DbRepositories.SqLite;
using ChristmasJoy.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ChristmasJoy.App
{
  public class ChristmasDbContextFactory : IDesignTimeDbContextFactory<ChristmasDbContext>
  {
    public ChristmasDbContext CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

      var connectionString = configuration.GetSection("SqLite:ConnectionString").Value;
      var builder = new DbContextOptionsBuilder<ChristmasDbContext>();
      builder.UseSqlite(connectionString);
      return new ChristmasDbContext(builder.Options);
    }

    public ChristmasDbContext CreateDbContext(IAppConfiguration appConfig)
    {
      var connectionString = appConfig.SqLiteConnectionString;
      var builder = new DbContextOptionsBuilder<ChristmasDbContext>();
      builder.UseSqlite(connectionString);
      return new ChristmasDbContext(builder.Options);
    }
  }
}
