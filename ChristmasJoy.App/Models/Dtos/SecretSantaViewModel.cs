using Newtonsoft.Json;

namespace ChristmasJoy.App.Models.Dtos
{
    public class SecretSantaViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "receiverUserId")]
        public int ReceiverUserId { get; set; }

        [JsonProperty(PropertyName = "santaUserId")]
        public int? SantaUserId { get; set; }
    }
}
