using Newtonsoft.Json;

namespace ChristmasJoy.App.Models.Dtos
{
    public class UserViewModel
    {
        [JsonProperty(PropertyName = "customId")]
        public int CustomId { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty(PropertyName = "hashedPassword")]
        public string HashedPassword { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "secretSantaForId")]
        public int? SecretSantaForId { get; set; }

        [JsonProperty(PropertyName = "secretSantaFor")]
        public string SecretSantaFor { get; set; }
      }
}
