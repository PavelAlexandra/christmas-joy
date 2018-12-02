using AutoMapper;
using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.DocumentDb
{
  public class CommentsRepository: ICommentsRepository
  {
    private readonly IAppConfiguration _configuration;
    private readonly DocumentClient client;
    private readonly IMapper _mapper;
    private static object locker = new object();

    public CommentsRepository(
      IAppConfiguration configuration,
      IDocumentHelper documentClient,
      IMapper mapper)
    {
      _configuration = configuration;
      client = documentClient.GetDocumentClient(configuration);
    }

    public async Task<string> AddCommentAsync(CommentViewModel item)
    {
      var docUri = UriFactory.CreateDocumentCollectionUri(
        Constants.DocumentDatabase,
        Constants.DocumentCommentsCollection);

      item.Id = null;
      var dbItem = _mapper.Map<DbComment>(item);
      var response = await this.client.CreateDocumentAsync(docUri, dbItem);
      return response.Resource.Id;
    }

    public List<CommentViewModel> GetReceivedComments(int userId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
      var collectionUri = UriFactory.CreateDocumentCollectionUri(
        Constants.DocumentDatabase,
        Constants.DocumentCommentsCollection);

      IQueryable<DbComment> commentsQuery = this.client.CreateDocumentQuery<DbComment>(
                  collectionUri, queryOptions)
               .Where(u => u.ToUserId == userId);

      return commentsQuery.Select(comment => _mapper.Map<CommentViewModel>(comment)).ToList();
    }

    public List<CommentViewModel> GetSentComments(int fromUserId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
      var collectionUri = UriFactory.CreateDocumentCollectionUri(
        Constants.DocumentDatabase,
        Constants.DocumentCommentsCollection);

      IQueryable<DbComment> commentsQuery = this.client.CreateDocumentQuery<DbComment>(
                  collectionUri, queryOptions)
               .Where(u => u.FromUserId == fromUserId);

      return commentsQuery.Select(comment => _mapper.Map<CommentViewModel>(comment)).ToList();
    }

    public Task SetLikeAsync(int fromUserId, string commentId)
    {
      lock (locker)
      {
        var docUri = UriFactory.CreateDocumentUri(
          Constants.DocumentDatabase,
          Constants.DocumentCommentsCollection,
          commentId);
        var response = this.client.ReadDocumentAsync(docUri);

        var comment = (DbComment)(dynamic)response.Result.Resource;
        comment.Likes.Add(fromUserId);

        return this.client.ReplaceDocumentAsync(docUri, comment);
      }
    }
  }
}
