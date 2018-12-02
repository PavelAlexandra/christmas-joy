using System;
using System.Collections.Generic;

namespace ChristmasJoy.App.Models.SqLiteModels
{
    public class Comment
    {
        public int Id { get; set; }
    
        public int FromUserId { get; set; }
      
        public string Content { get; set; }
    
        public CommentType CommentType { get; set; }
    
        public int ToUserId { get; set; }
    
        public DateTime CommentDate { get; set; }
    
        public List<Like> Likes { get; set; }

        public bool IsAnonymous { get; set; }
  }
}
