using ChristmasJoy.DataLayer.Interfaces;
using ChristmasJoy.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristmasJoy.DataLayer
{
    public class UserRepository : IUserRepository
    {
        private readonly IAzureTableStorage<User> _storage;

        public UserRepository(IAzureTableStorage<User> storage)
        {
            _storage = storage;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _storage.GetList();
        }

        public async Task<User> GetUser(string email)
        {
            var userPartitionKey = email[0].ToString();
            return await _storage.GetItem(userPartitionKey, email);
        }

        public async Task AddUser(User item)
        {
            item.UserPartitionKey = item.Email[0].ToString();
            await _storage.Insert(item);
        }

        public async Task Update(User item)
        {
            await _storage.Update(item);
        }

        public async Task Delete(User user)
        {
            await _storage.Delete(user.UserPartitionKey, user.Email);
        }
    }
    
}
