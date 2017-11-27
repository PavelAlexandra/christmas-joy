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

    Task AddUser(Models.User item);

    Task Update(Models.User item);

    Task Delete(Models.User user);
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

    public async Task AddUser(Models.User user)
    {
      try
      {
        var docUri = UriFactory.CreateDocumentUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection, user.Id);
        await this.client.ReadDocumentAsync(docUri);
      }
      catch (DocumentClientException de)
      {
        if (de.StatusCode == HttpStatusCode.NotFound)
        {
          var docUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection);
          await this.client.CreateDocumentAsync(docUri, user);
        }
        else
        {
          throw;
        }
      }
    }

    public async Task Delete(Models.User user)
    {
        await this.client.DeleteDatabaseAsync(UriFactory.CreateDocumentUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection, user.Id)); 
    }

    public Models.User GetUser(string email)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      IQueryable<Models.User> userQuery  = this.client.CreateDocumentQuery<Models.User>(
                UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection), queryOptions)
                .Where(u => u.Email.Equals(email));

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

    public async Task Update(Models.User item)
    {
      await this.client.ReplaceDocumentAsync(
        UriFactory.CreateDocumentUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection, item.Id), item);
    }
  }

}
