using System.Collections.Generic;
using System.Threading.Tasks;
using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using AutoMapper;
using ChristmasJoy.App.Models.SqLiteModels;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

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
        await db.SaveChangesAsync();
        return dbItem.Id.ToString();
      } 
    }

    public async Task<List<CommentViewModel>> GetReceivedCommentsAsync(int userId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var comments = await db.Comments
          .Include(cmd => cmd.Likes)
          .Where(cmd => cmd.ToUserId == userId)
          .ToListAsync();

        var data = new List<CommentViewModel>();
        foreach (var cmd in comments)
        {
          var command = _mapper.Map<Comment, CommentViewModel>(cmd);
          command.Likes = cmd.Likes.Select(l => l.FromUserId).ToList();
          data.Add(command);
        }

        return data;
      }
    }

    public async Task<List<CommentViewModel>> GetSentCommentsAsync(int fromUserId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var comments = await db.Comments
          .Include(cmd => cmd.Likes)
          .Where(cmd => cmd.FromUserId == fromUserId)
          .ToListAsync();

        var data = new List<CommentViewModel>();
        foreach(var cmd in comments)
        {
          var command = _mapper.Map<Comment, CommentViewModel>(cmd);
          command.Likes = cmd.Likes.Select(l => l.FromUserId).ToList();
          data.Add(command);
        }

        return data;
      }
    }

    public async Task SetLikeAsync(int fromUserId, int commentId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var comment = await db.Comments
          .Include(cmd => cmd.Likes)
          .Where(cmd => cmd.Id == commentId)
          .FirstOrDefaultAsync();

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
