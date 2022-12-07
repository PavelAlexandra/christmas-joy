using ChristmasJoy.App.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.Interfaces
{
  public interface IWishListRepository
  {
    Task<int> AddWishItemAsync(WishListItemViewModel item);
    Task UpdateWishItemAsync(WishListItemViewModel item);
    Task DeleteWishItemAsync(WishListItemViewModel item);
    List<WishListItemViewModel> GetWishList(int userId);
  }
}
