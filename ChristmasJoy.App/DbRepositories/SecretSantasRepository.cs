using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Models;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories
{
  public interface ISecretSantasRepository
  {
    Task AddUserAsync(int receiverId);
    Task SetSecretSanta(int receiverId, int santaUserId);
    int? GetSantaReceiverId(int santaUserId);
    List<SecretSanta> GetAvailableReceivers(int secretSantaId);
  }

  public class SecretSantasRepository: ISecretSantasRepository
  {
    private readonly IAppConfiguration _configuration;
    private readonly DocumentClient client;

    public SecretSantasRepository(IAppConfiguration configuration, IDocumentHelper documentClient)
    {
      _configuration = configuration;
      client = documentClient.GetDocumentClient(configuration);
    }

    public async Task AddUserAsync(int receiverId)
    {
      var docUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentSantasCollection);
      var santa = new SecretSanta
      {
        ReceiverUserId = receiverId,
        SantaUserId = null,
        Id = null
      };

      await this.client.CreateDocumentAsync(docUri, santa);
    }

    public async Task SetSecretSanta(int receiverId, int santaUserId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      IQueryable<SecretSanta> query = this.client.CreateDocumentQuery<SecretSanta>(
                UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentSantasCollection), queryOptions)
                .Where(u => u.ReceiverUserId == receiverId);

      var santaEntity = query.ToList().FirstOrDefault();
      if (santaEntity != null)
      {
        santaEntity.SantaUserId = santaUserId;
        await this.client.ReplaceDocumentAsync(
                  UriFactory.CreateDocumentUri(
                    Constants.DocumentDatabase,
                    Constants.DocumentSantasCollection,
                    santaEntity.Id),
                  santaEntity);
      }
     }

      public int? GetSantaReceiverId(int santaUserId)
      {
        FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

        IQueryable<SecretSanta> query = this.client.CreateDocumentQuery<SecretSanta>(
                  UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentSantasCollection), queryOptions)
                  .Where(u => u.SantaUserId == santaUserId);

        var santaEntity = query.ToList().FirstOrDefault();
        if (santaEntity != null)
        {
          return santaEntity.ReceiverUserId;
        }
        else
        {
          return null;
        }
      }

    public List<SecretSanta> GetAvailableReceivers(int secretSantaId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      IQueryable<SecretSanta> query = this.client.CreateDocumentQuery<SecretSanta>(
                UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentSantasCollection), queryOptions)
                .Where(u => u.SantaUserId == null && u.ReceiverUserId != secretSantaId);

      var santaReceivers = query.ToList();
      return santaReceivers;
    }
  }
}
