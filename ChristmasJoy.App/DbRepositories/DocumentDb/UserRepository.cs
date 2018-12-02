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
  public class UserRepository : IUserRepository
  {
    private readonly IAppConfiguration _configuration;
    private readonly DocumentClient client;
    private readonly IMapper _mapper;

    public UserRepository(
      IAppConfiguration configuration,
      IDocumentHelper documentClient,
      IMapper mapper)
    {
      _configuration = configuration;
      client = documentClient.GetDocumentClient(configuration);
      _mapper = mapper;
    }

    public async Task AddUserAsync(UserViewModel user)
    {
        var docUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection);
        var dbUser = _mapper.Map<DbUser>(user);
        await this.client.CreateDocumentAsync(docUri, dbUser);
    }

    public async Task DeleteUserAsync(UserViewModel user)
    {
      await this.client.DeleteDatabaseAsync(UriFactory.CreateDocumentUri(
          Constants.DocumentDatabase,
          Constants.DocumentUsersCollection,
          user.Id)); 
    }

    public UserViewModel GetUser(string email)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      IQueryable<DbUser> userQuery  = this.client.CreateDocumentQuery<DbUser>(
                UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection), queryOptions)
                .Where(u => u.Email.Equals(email));

      var dbUser = userQuery.FirstOrDefault();
      return _mapper.Map<UserViewModel>(dbUser);
    }

    public UserViewModel GetUser(int customId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      IQueryable<DbUser> userQuery = this.client.CreateDocumentQuery<DbUser>(
                UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection), queryOptions)
                .Where(u => u.CustomId == customId);

      var dbUser = userQuery.FirstOrDefault();
      return _mapper.Map<UserViewModel>(dbUser);
    }

    public List<UserViewModel> GetAllUsers()
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      var usersQuery = this.client.CreateDocumentQuery<DbUser>(
                 UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection),
                 queryOptions);

      return usersQuery.Select(user => _mapper.Map<UserViewModel>(user)).ToList();
    }

    public async Task UpdateUserAsync(UserViewModel item)
    {
      var dbUser = _mapper.Map<DbUser>(item);
      await this.client.ReplaceDocumentAsync(
        UriFactory.CreateDocumentUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection, dbUser.id), dbUser);
    }

    public int LastCustomId()
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      var maxCustomId = this.client.CreateDocumentQuery<DbUser>(
                 UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentUsersCollection),
                 queryOptions)
                 .Select(x=> x.CustomId)
                 .Max();

      return maxCustomId;
    }
  }

}
