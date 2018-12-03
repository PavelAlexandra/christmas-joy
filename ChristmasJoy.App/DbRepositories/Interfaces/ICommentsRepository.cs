using ChristmasJoy.App.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.Interfaces
{
  public interface ICommentsRepository
  {
    Task<string> AddCommentAsync(CommentViewModel item);
    Task<List<CommentViewModel>> GetReceivedCommentsAsync(int userId);
    Task<List<CommentViewModel>> GetSentCommentsAsync(int fromUserId);
    Task SetLikeAsync(int fromUserId, int commentId);
  }

}
