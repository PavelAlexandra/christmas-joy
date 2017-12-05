using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.Models
{
  public enum CommentType
  {
    Negative = 0,
    Positive = 1
  }
  public class Comment
  {
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "fromUserId")]
    public int FromUserId { get; set; }

    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }

    [JsonProperty(PropertyName = "commentType")]
    public CommentType CommentType { get; set; }

    [JsonProperty(PropertyName = "toUserId")]
    public int ToUserId { get; set; }

    [JsonProperty(PropertyName = "commentDate")]
    public DateTime CommentDate { get; set; }

    [JsonProperty(PropertyName = "likes")]
    public List<int> Likes { get; set; }
  }

  public class Like
  {
    public string CommentId { get; set; }
  }
 }
