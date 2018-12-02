using AutoMapper;
using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.DocumentDb
{
  public class SecretSantasRepository: ISecretSantasRepository
  {
    private readonly IAppConfiguration _configuration;
    private readonly DocumentClient client;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public SecretSantasRepository(
      IAppConfiguration configuration,
      IDocumentHelper documentClient,
      IMapper mapper,
      IUserRepository userRepository)
    {
      _configuration = configuration;
      client = documentClient.GetDocumentClient(configuration);
      _mapper = mapper;
      _userRepository = userRepository;
    }

    public async Task AddUserAsync(int receiverId)
    {
      var docUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentSantasCollection);
      var santa = new DbSecretSanta
      {
        ReceiverUserId = receiverId,
        SantaUserId = null,
        Id = null
      };

      await this.client.CreateDocumentAsync(docUri, santa);
    }

    public async Task<string> SetSecretSantaAsync(int receiverId, int santaUserId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      IQueryable<DbSecretSanta> query = this.client.CreateDocumentQuery<DbSecretSanta>(
                UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentSantasCollection), queryOptions)
                .Where(u => u.ReceiverUserId == receiverId);

      var santaEntity = query.ToList().FirstOrDefault();
      var santaUser = _userRepository.GetUser(santaUserId);
      var receiverUser = _userRepository.GetUser(receiverId);

      if (santaEntity != null && santaUser != null && receiverUser != null)
      {
        santaEntity.SantaUserId = santaUserId;
        await this.client.ReplaceDocumentAsync(
                  UriFactory.CreateDocumentUri(
                    Constants.DocumentDatabase,
                    Constants.DocumentSantasCollection,
                    santaEntity.Id),
                  santaEntity);

        santaUser.SecretSantaFor = receiverUser.UserName;
        santaUser.SecretSantaForId = receiverUser.CustomId;

        await _userRepository.UpdateUserAsync(santaUser);
      }
      else
      {
        throw new ArgumentException("Invalid ids");
      }

      return receiverUser.UserName;
    }

      public int? GetSantaReceiverId(int santaUserId)
      {
        FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

        IQueryable<DbSecretSanta> query = this.client.CreateDocumentQuery<DbSecretSanta>(
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

    public List<SecretSantaViewModel> GetAvailableReceivers(int secretSantaId)
    {
      FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

      IQueryable<DbSecretSanta> query = this.client.CreateDocumentQuery<DbSecretSanta>(
                UriFactory.CreateDocumentCollectionUri(Constants.DocumentDatabase, Constants.DocumentSantasCollection), queryOptions)
                .Where(u => u.SantaUserId == null && u.ReceiverUserId != secretSantaId);

      var santaReceivers = query.Select(santa => _mapper.Map<SecretSantaViewModel>(santa)).ToList();
      return santaReceivers;
    }
  }
}
