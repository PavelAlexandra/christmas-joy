using System.Collections.Generic;

namespace ChristmasJoy.App.Models
{
  public class UserData
  {
    public User UserInfo { get; set; }

    public List<WishListItem> WishList { get; set; }

    public List<Comment> ReceivedComments { get; set; }
  }
}
