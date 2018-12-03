using ChristmasJoy.App.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.Interfaces
{
  public interface ICommentsRepository
  {
    Task<string> AddCommentAsync(CommentViewModel item);
    List<CommentViewModel> GetReceivedComments(int userId);
    List<CommentViewModel> GetSentComments(int fromUserId);
    Task SetLikeAsync(int fromUserId, int commentId);
  }

}
