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
    public string Id { get; set; }
    public int FromUserId { get; set; }
    public string Content { get; set; }
    public CommentType CommentType { get; set; }
    public int ToUserId { get; set; }
    public DateTime CommentDate { get; set; }
    public int[] Likes { get; set; }
  }
 }
