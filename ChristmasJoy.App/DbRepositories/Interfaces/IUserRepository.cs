using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.Interfaces
{
  public interface IUserRepository
  {
    List<UserViewModel> GetAllNonAdminUsers();

    UserViewModel GetUser(string email);

    UserViewModel GetUser(int customId);

    Task AddUserAsync(UserViewModel item);

    Task UpdateUserAsync(UserViewModel item);

    Task DeleteUserAsync(UserViewModel user);

    int LastCustomId();
  }
}
