namespace ChristmasJoy.App.ViewModels
{
  public class UserViewModel
  {
    public int CustomId { get; set; }

    public string Id { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public bool IsAdmin { get; set; }

    public string HashedPassword { get; set; }

    public int? SecretSantaForId { get; set; }

    public int Age { get; set; }
  }
}
