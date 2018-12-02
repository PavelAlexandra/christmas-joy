using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.Interfaces
{
  public interface ISecretSantasRepository
  {
    Task AddUserAsync(int receiverId);
    Task<string> SetSecretSantaAsync(int receiverId, int santaUserId);
    int? GetSantaReceiverId(int santaUserId);
    List<SecretSantaViewModel> GetAvailableReceivers(int secretSantaId);
  }
}
