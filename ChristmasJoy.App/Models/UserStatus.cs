namespace ChristmasJoy.App.Models
{
  public enum Status
  {
    Grinch = 0,
    Snowman = 1,
    Elf = 2,
    Cookie = 3,
    Santa = 4,
    Magus = 5
  }

  public class UserStatus
  {
    public int Id { get; set; }

    public string UserName { get; set; }

    public string ChristmasStatus { get; set; }

    public double Points { get; set; }

    public int NoOfComments { get; set; }
  }
}
