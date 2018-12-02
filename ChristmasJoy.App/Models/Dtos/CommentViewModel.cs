using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChristmasJoy.App.Models.Dtos
{
    public class CommentViewModel
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

        public bool IsAnonymous { get; set; }
    }
}
