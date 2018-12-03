using System.Collections.Generic;
using System.Threading.Tasks;
using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using AutoMapper;
using ChristmasJoy.App.Models.SqLiteModels;
using System.Linq;
using System;

namespace ChristmasJoy.App.DbRepositories.SqLite
{
  public class SqlLiteCommentsRepository : ICommentsRepository
  {
    private readonly IMapper _mapper;
    private readonly IAppConfiguration _appConfig;
    private readonly ChristmasDbContextFactory dbContextFactory;

    public SqlLiteCommentsRepository(
     IMapper mapper,
     IAppConfiguration appConfig,
     ChristmasDbContextFactory contextFactory
     )
    {
      _mapper = mapper;
      _appConfig = appConfig;
      dbContextFactory = contextFactory;
    }

    public async Task<string> AddCommentAsync(CommentViewModel item)
    {
      using(var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var dbItem = _mapper.Map<Comment>(item);
        db.Comments.Add(dbItem);
        var id = await db.SaveChangesAsync();
        return id.ToString();
      } 
    }

    public List<CommentViewModel> GetReceivedComments(int userId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var comments = db.Comments.Where(c => c.ToUserId == userId)
          .Select(c => _mapper.Map<CommentViewModel>(c))
          .ToList();

        return comments;
      }
    }

    public List<CommentViewModel> GetSentComments(int fromUserId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var comments = db.Comments.Where(c => c.FromUserId == fromUserId)
          .Select(c => _mapper.Map<CommentViewModel>(c))
          .ToList();

        return comments;
      }
    }

    public async Task SetLikeAsync(int fromUserId, int commentId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var comment = await db.Comments.FindAsync(commentId);
        if(comment == null)
        {
          throw new KeyNotFoundException($"Comment with id {commentId} was not found.");
        }
        if(comment.Likes == null)
        {
          comment.Likes = new List<Like>();
        }

        comment.Likes.Add(new Like
        {
          CommentId = commentId,
          FromUserId = fromUserId
        });
        await db.SaveChangesAsync();
      }
    }
  }
}
