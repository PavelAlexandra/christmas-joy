using Newtonsoft.Json;

namespace ChristmasJoy.App.Models.Dtos
{
    public class WishListItemViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "item")]
        public string Item { get; set; }
    }
}
