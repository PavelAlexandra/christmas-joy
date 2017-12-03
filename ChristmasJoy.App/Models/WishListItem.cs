using Newtonsoft.Json;

namespace ChristmasJoy.App.Models
{
  public class WishListItem
  {
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "userId")]
    public int UserId { get; set; }

    [JsonProperty(PropertyName = "item")]
    public string Item { get; set; }
  }
}
