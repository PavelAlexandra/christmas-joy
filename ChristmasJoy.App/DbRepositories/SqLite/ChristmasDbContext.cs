using ChristmasJoy.App.Models.SqLiteModels;
using Microsoft.EntityFrameworkCore;

namespace ChristmasJoy.App.DbRepositories.SqLite
{
  public class ChristmasDbContext : DbContext
  {
    public ChristmasDbContext(DbContextOptions options)
      : base(options)
    {
    }

    public ChristmasDbContext(string connectionString)
    {

    }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<SecretSanta> SecretSantas { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Like> Likes { get; set; }

    public DbSet<WishListItem> WishListItems { get; set; }
  }
}
