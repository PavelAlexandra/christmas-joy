using System.Collections.Generic;

namespace ChristmasJoy.App.Models
{
  public class UserData
  {
    public User UserInfo { get; set; }

    public List<WishListItem> WishList { get; set; }

    public UserStatus Status { get; set; }
  }
}
