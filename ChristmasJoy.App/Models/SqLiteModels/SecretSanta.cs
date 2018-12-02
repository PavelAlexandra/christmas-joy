namespace ChristmasJoy.App.Models.SqLiteModels
{
    public class SecretSanta
    {
      public int Id { get; set; }
    
      public int ReceiverUserId { get; set; }
    
      public int? SantaUserId { get; set; }
    }
}
