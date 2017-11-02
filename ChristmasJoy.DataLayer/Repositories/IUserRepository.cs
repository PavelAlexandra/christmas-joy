using ChristmasJoy.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristmasJoy.DataLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();

        Task<User> GetUser(string email);

        Task AddUser(User item);

        Task Update(User item);

        Task Delete(User user);
    }
}
