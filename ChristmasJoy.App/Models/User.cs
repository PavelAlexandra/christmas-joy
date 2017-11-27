namespace ChristmasJoy.App.Models
{
  public class User
  {
    public User()
    {

    }

    public string CustomId { get; set; }

    public string Id { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public bool IsAdmin { get; set; }

    public string HashedPassword { get; set; }

    public int Age { get; set; }

    public int? SecretSantaForId { get; set; }
  }
}
