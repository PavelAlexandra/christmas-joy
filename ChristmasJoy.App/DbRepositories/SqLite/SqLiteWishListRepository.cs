using AutoMapper;
using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using ChristmasJoy.App.Models.SqLiteModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.SqLite
{
  public class SqLiteWishListRepository : IWishListRepository
  {
    private readonly IMapper _mapper;
    private readonly IAppConfiguration _appConfig;
    private readonly ChristmasDbContextFactory dbContextFactory;

    public SqLiteWishListRepository(
      IMapper mapper,
      IAppConfiguration appConfig,
      ChristmasDbContextFactory contextFactory
      )
    {
      _mapper = mapper;
      _appConfig = appConfig;
      dbContextFactory = contextFactory;
    }

    public async Task<string> AddWishItemAsync(WishListItemViewModel item)
    {
      var dbWishList = _mapper.Map<WishListItem>(item);
      using(var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        db.WishListItems.Add(dbWishList);
        var id = await db.SaveChangesAsync();
        return id.ToString();
      }
    }

    public async Task DeleteWishItemAsync(WishListItemViewModel item)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var dbItem = await db.WishListItems.FindAsync(item.Id);
        if (dbItem == null)
        {
          throw new KeyNotFoundException($"Wish list item with id {item.Id} was not found.");
        }

        db.WishListItems.Remove(dbItem);
        await db.SaveChangesAsync();
      }
    }

    public List<WishListItemViewModel> GetWishList(int userId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var items = db.WishListItems.Where(w => w.UserId == userId)
          .Select(i => _mapper.Map<WishListItemViewModel>(i))
          .ToList();

        return items;
      }
    }

    public async Task UpdateWishItemAsync(WishListItemViewModel item)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var dbItem = await db.WishListItems.FindAsync(item.Id);
        if (dbItem == null)
        {
          throw new KeyNotFoundException($"Wish list item with id {item.Id} was not found.");
        }

        dbItem.Item = item.Item;
        await db.SaveChangesAsync();
      }
    }
  }
}
