using System.Collections.Generic;
using System.Threading.Tasks;
using ChristmasJoy.App.Models;
using Microsoft.Azure.Documents.Client;
using ChristmasJoy.App.Helpers;
using Microsoft.Azure.Documents;
using System.Net;
using System.Linq;

namespace ChristmasJoy.App.DbRepositories
{
  public interface IUserRepository
  {
    List<Models.User> GetAllUsers();

    Models.User GetUser(string email);

    Models.User GetUser(int customId);

    Task AddUserAsync(Models.User item);

    Task UpdateUserAsync(Models.User item);

    Task DeleteUserAsync(Models.User user);

    int LastCustomId();
  }

  public class UserRepository : IUserRepository
  {
    private readonly IAppConfiguration _configuration;
    private readonly DocumentClient client;

    public UserRepository(IAppConfiguration configuration, IDocumentHelper documentClient)
    {
      _configuration = configuration;
      client = documentClient.GetDocumentClient(configuration);
     
    }

    public async Task AddUserAsync(Models.User user)
    {
        var docUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection);
        await this.client.CreateDocumentAsync(docUri, user);
    }

    public async Task DeleteUserAsync(Models.User user)
    {
        await this.client.DeleteDatabaseAsync(UriFactory.CreateDocumentUri(
          Constants.DocumentDatabase,
          Constants.DocumentUsersCollection,
          user.id)); 
    }

    public Models.User GetUser(string email)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      IQueryable<Models.User> userQuery  = this.client.CreateDocumentQuery<Models.User>(
                UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection), queryOptions)
                .Where(u => u.Email.Equals(email));

      return userQuery.ToList().FirstOrDefault();
    }

    public Models.User GetUser(int customId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      IQueryable<Models.User> userQuery = this.client.CreateDocumentQuery<Models.User>(
                UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection), queryOptions)
                .Where(u => u.CustomId == customId);

      return userQuery.ToList().FirstOrDefault();
    }

    public List<Models.User> GetAllUsers()
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      var users = this.client.CreateDocumentQuery<Models.User>(
                 UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection),
                 queryOptions)
                 .ToList();

      return users;
    }

    public async Task UpdateUserAsync(Models.User item)
    {
      await this.client.ReplaceDocumentAsync(
        UriFactory.CreateDocumentUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection, item.id), item);
    }

    public int LastCustomId()
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      var maxCustomId = this.client.CreateDocumentQuery<Models.User>(
                 UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection),
                 queryOptions)
                 .Select(x=> x.CustomId)
                 .Max();

      return maxCustomId;
    }
  }

}
