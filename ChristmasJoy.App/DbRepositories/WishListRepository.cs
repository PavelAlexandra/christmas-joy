using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Models;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories
{
  public interface IWishListRepository
  {
    Task<string> AddWishItemAsync(WishListItem item);
    Task UpdateWishItemAsync(WishListItem item);
    Task DeleteWishItemAsync(WishListItem item);
    List<WishListItem> GetWishList(int userId);
  }
  public class WishListRepository : IWishListRepository
  {
    private readonly IAppConfiguration _configuration;
    private readonly DocumentClient client;

    public WishListRepository(IAppConfiguration configuration, IDocumentHelper documentClient)
    {
      _configuration = configuration;
      client = documentClient.GetDocumentClient(configuration);
    }

    public async Task<string> AddWishItemAsync(WishListItem item)
    {
      var docUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentWishListCollection);
      item.Id = null;
      var response = await this.client.CreateDocumentAsync(docUri, item);
      return response.Resource.Id;
    }

    public async Task UpdateWishItemAsync(WishListItem item)
    {
      var docUri = UriFactory.CreateDocumentUri(
                    Constants.DocumentDatabase,
                    Constants.DocumentWishListCollection,
                    item.Id);

      await this.client.ReplaceDocumentAsync(docUri, item);
    }

    public async Task DeleteWishItemAsync(WishListItem item)
    {
      var docUri = UriFactory.CreateDocumentUri(
           Constants.DocumentDatabase,
           Constants.DocumentWishListCollection,
           item.Id);

      await this.client.DeleteDocumentAsync(docUri);
    }

    public List<WishListItem> GetWishList(int userId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
      var collectionUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentWishListCollection);

      IQueryable<WishListItem> userQuery = this.client.CreateDocumentQuery<WishListItem>(
                  collectionUri, queryOptions)
               .Where(u => u.UserId == userId);

      return userQuery.ToList();
    }
  }
}
