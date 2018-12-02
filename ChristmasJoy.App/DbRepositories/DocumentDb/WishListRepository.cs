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
  public class WishListRepository : IWishListRepository
  {
    private readonly IAppConfiguration _configuration;
    private readonly DocumentClient client;
    private readonly IMapper _mapper;

    public WishListRepository(
      IAppConfiguration configuration,
      IDocumentHelper documentClient,
      IMapper mapper)
    {
      _configuration = configuration;
      client = documentClient.GetDocumentClient(configuration);
      _mapper = mapper;
    }

    public async Task<string> AddWishItemAsync(WishListItemViewModel item)
    {
      var dbItem = _mapper.Map<DbWishListItem>(item);
      var docUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentWishListCollection);
      dbItem.Id = null;
      var response = await this.client.CreateDocumentAsync(docUri, dbItem);
      return response.Resource.Id;
    }

    public async Task UpdateWishItemAsync(WishListItemViewModel item)
    {
      var docUri = UriFactory.CreateDocumentUri(
                    Constants.DocumentDatabase,
                    Constants.DocumentWishListCollection,
                    item.Id);
      var dbItem = _mapper.Map<DbWishListItem>(item);
      await this.client.ReplaceDocumentAsync(docUri, dbItem);
    }

    public async Task DeleteWishItemAsync(WishListItemViewModel item)
    {
      var docUri = UriFactory.CreateDocumentUri(
           Constants.DocumentDatabase,
           Constants.DocumentWishListCollection,
           item.Id);

      await this.client.DeleteDocumentAsync(docUri);
    }

    public List<WishListItemViewModel> GetWishList(int userId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
      var collectionUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentWishListCollection);

      IQueryable<DbWishListItem> userQuery = this.client.CreateDocumentQuery<DbWishListItem>(
                  collectionUri, queryOptions)
               .Where(u => u.UserId == userId);

      return userQuery.Select(item => _mapper.Map<WishListItemViewModel>(item)).ToList();
    }
  }
}
