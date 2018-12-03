using AutoMapper;
using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using ChristmasJoy.App.Models.SqLiteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.SqLite
{
  public class SqLiteSecretSantasRepository : ISecretSantasRepository
  {
    private readonly IMapper _mapper;
    private readonly IAppConfiguration _appConfig;
    private readonly ChristmasDbContextFactory dbContextFactory;

    public SqLiteSecretSantasRepository(
       IMapper mapper,
       IAppConfiguration appConfig,
       ChristmasDbContextFactory contextFactory
       )
    {
      _mapper = mapper;
      _appConfig = appConfig;
      dbContextFactory = contextFactory;
    }

    public async Task AddUserAsync(int receiverId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var santa = new SecretSanta
        {
          ReceiverUserId = receiverId,
          SantaUserId = null
        };

        db.SecretSantas.Add(santa);
        await db.SaveChangesAsync();
      }
    }

    public List<SecretSantaViewModel> GetAvailableReceivers(int secretSantaId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var santas = db.SecretSantas
          .Where(u => u.SantaUserId == null && u.ReceiverUserId != secretSantaId);
        return santas
          .Select(s => _mapper.Map<SecretSanta, SecretSantaViewModel>(s))
          .ToList();
      }
    }

    public int? GetSantaReceiverId(int santaUserId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var santa = db.SecretSantas.Where(u => u.SantaUserId == santaUserId).FirstOrDefault();
        return santa?.ReceiverUserId;
      }
    }

    public async Task<string> SetSecretSantaAsync(int receiverId, int santaUserId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var receiver = db.SecretSantas
             .FirstOrDefault(u => u.ReceiverUserId == receiverId);

        var receiverUser = await db.Users.FindAsync(receiverId);
        var santaUser = await db.Users.FindAsync(santaUserId);

        if (receiver != null && receiverUser != null && receiverUser != null)
        {
          receiver.SantaUserId = santaUserId;

          santaUser.SecretSantaForId = receiverUser.Id;
          santaUser.SecretSantaFor = receiverUser.UserName;
          await db.SaveChangesAsync();

          return receiverUser.UserName;
        }
        else
        {
          throw new ArgumentException("Invalid ids");
        }
      }
    }
  }
}
