using ChristmasJoy.App.Models.Dtos;
using System.Collections.Generic;

namespace ChristmasJoy.App.Models
{
  public class UserData
  {
    public UserViewModel UserInfo { get; set; }

    public List<WishListItemViewModel> WishList { get; set; }

    public UserStatus Status { get; set; }
  }
}
