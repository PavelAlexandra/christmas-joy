using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.ViewModels
{
  public class UserViewModel
  {
    public string CustomId { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public bool IsAdmin { get; set; }

    public string Password { get; set; }
  }
}
