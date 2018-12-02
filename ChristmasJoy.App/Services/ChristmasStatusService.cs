using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Models;
using System.Collections.Generic;
using System.Linq;

namespace ChristmasJoy.App.Services
{
  public interface IChristmasStatusService
  {
    UserStatus GetUserStatus(int customUserId, string userName);
  }

  public class ChristmasStatusService: IChristmasStatusService
  {
    public Dictionary<int, Status> statusLevels = new Dictionary<int, Status>
    {
      {3, Status.Cookie },
      {10, Status.Snowman },
      {25, Status.Elf },
      {50, Status.Santa },
      {67, Status.Magus }
    };

    public const double CommentReceivedPoints = 0.3;
    public const double CommentPositiveSentPoints = 1;
    public const double CommentNegativeSentPoints = 0.2;
    public const double CommentLikeReceived = 0.3;

    public const int MaxPos = 45;
    public const int MaxNeg = 10;

    private readonly ICommentsRepository _commRepo;

    public ChristmasStatusService(ICommentsRepository commRepo)
    {
      this._commRepo = commRepo;
    }

    public UserStatus GetUserStatus(int customUserId, string userName)
    {
      var receivedComments = _commRepo.GetReceivedComments(customUserId);
      var sentComments = _commRepo.GetSentComments(customUserId);

      double receivedCommentsPoints = 0;
      double commentLikesPoints = 0;
      double sentPosCommentsPoints = 0;
      double sentNegativeCommentsPoints = 0;

      if (receivedComments != null)
      {
        receivedCommentsPoints = receivedComments.Count * CommentReceivedPoints;
      }

      if (sentComments != null)
      {
        commentLikesPoints = sentComments.Sum(x => x.Likes.Count) * CommentLikeReceived;
      }

      if (sentComments != null)
      {
        var sentPosCommNo = sentComments
                          .Where(x => x.CommentType == CommentType.Positive)
                          .GroupBy(x => x.ToUserId)
                          .SelectMany(x => x.Take(2))
                          .Count();


        var sentNegCommNo = sentComments
                          .Where(x => x.CommentType == CommentType.Negative)
                          .GroupBy(x => x.ToUserId)
                          .SelectMany(x => x.Take(2))
                          .Count();

        sentPosCommentsPoints = (sentPosCommNo > MaxPos ? MaxPos : sentPosCommNo) * CommentPositiveSentPoints;
        sentNegativeCommentsPoints = (sentNegCommNo > MaxNeg? MaxNeg : sentNegCommNo ) * CommentNegativeSentPoints;
      }   

      var totalPoints = sentPosCommentsPoints +
                sentNegativeCommentsPoints +
                receivedCommentsPoints +
                commentLikesPoints;

      Status status = Status.Grinch;
      foreach(var key in statusLevels.Keys)
      {
        if(totalPoints >= key)
        {
          status = statusLevels[key];
        }
      }

      var userStatus = new UserStatus
      {
        Id = customUserId,
        UserName = userName,
        ChristmasStatus = status.ToString().ToLower(),
        Points = totalPoints,
        NoOfComments = receivedComments.Count()
      };

      return userStatus;
    }
  }
}
