namespace ChristmasJoy.App.Models.SqLiteModels
{
    public class User
    {    
        public int Id { get; set; }
    
        public string Email { get; set; }
    
        public string UserName { get; set; }
    
        public bool IsAdmin { get; set; }
    
        public string HashedPassword { get; set; }
    
        public int Age { get; set; }
    
        public int? SecretSantaForId { get; set; }
    
        public string SecretSantaFor { get; set; }
    }
}
