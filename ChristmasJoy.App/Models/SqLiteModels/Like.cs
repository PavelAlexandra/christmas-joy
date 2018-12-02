namespace ChristmasJoy.App.Models.SqLiteModels
{
    public class Like
    {
        public int Id { get; set; }

        public int FromUserId { get; set; }

        public string CommentId { get; set; }
    }
}
