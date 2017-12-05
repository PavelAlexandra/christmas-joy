using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Models;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories
{
  public interface ICommentsRepository
  {
    Task<string> AddCommentAsync(Comment item);
    List<Comment> GetReceivedComments(int userId);
    List<Comment> GetSentComments(int fromUserId);
    void SetLike(int fromUserId, string commentId);
  }

  public class CommentsRepository: ICommentsRepository
  {
    private readonly IAppConfiguration _configuration;
    private readonly DocumentClient client;
    private static object locker = new object();

    public CommentsRepository(IAppConfiguration configuration, IDocumentHelper documentClient)
    {
      _configuration = configuration;
      client = documentClient.GetDocumentClient(configuration);
    }

    public async Task<string> AddCommentAsync(Comment item)
    {
      var docUri = UriFactory.CreateDocumentCollectionUri(
        Constants.DocumentDatabase,
        Constants.DocumentCommentsCollection);

      item.Id = null;
      var response = await this.client.CreateDocumentAsync(docUri, item);
      return response.Resource.Id;
    }

    public List<Comment> GetReceivedComments(int userId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
      var collectionUri = UriFactory.CreateDocumentCollectionUri(
        Constants.DocumentDatabase,
        Constants.DocumentCommentsCollection);

      IQueryable<Comment> commentsQuery = this.client.CreateDocumentQuery<Comment>(
                  collectionUri, queryOptions)
               .Where(u => u.ToUserId == userId);

      return commentsQuery.ToList();
    }

    public List<Comment> GetSentComments(int fromUserId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
      var collectionUri = UriFactory.CreateDocumentCollectionUri(
        Constants.DocumentDatabase,
        Constants.DocumentCommentsCollection);

      IQueryable<Comment> commentsQuery = this.client.CreateDocumentQuery<Comment>(
                  collectionUri, queryOptions)
               .Where(u => u.FromUserId == fromUserId);

      return commentsQuery.ToList();
    }

    public void SetLike(int fromUserId, string commentId)
    {
      lock (locker)
      {
        var docUri = UriFactory.CreateDocumentUri(
          Constants.DocumentDatabase,
          Constants.DocumentCommentsCollection,
          commentId);
        var response = this.client.ReadDocumentAsync(docUri);

        var comment = (Comment)(dynamic)response.Result.Resource;
        comment.Likes.Add(fromUserId);

        this.client.ReplaceDocumentAsync(docUri, comment);
      }
    }
  }
}
