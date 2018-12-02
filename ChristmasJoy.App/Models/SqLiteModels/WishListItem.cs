namespace ChristmasJoy.App.Models.SqLiteModels
{
    public class WishListItem
    {
        public int Id { get; set; }
    
        public int UserId { get; set; }
    
        public string Item { get; set; }
    }
}
