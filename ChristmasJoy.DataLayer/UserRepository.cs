using ChristmasJoy.DataLayer.Interfaces;
using ChristmasJoy.Models;

namespace ChristmasJoy.DataLayer
{
    public class UserRepository : IUserRepository
    {
        public User FindByEmail(string email)
        {
            if (email == "alex@gmail.com")
                return new User
                {
                    Email = "alex@gmail.com",
                    UserName = "Alex",
                    IsAdmin = true,
                    HashedPassword = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08"
                };
            else
            {
                return new User
                {
                    Email = email,
                    UserName = "Guest",
                    IsAdmin = false,
                    HashedPassword = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08"
                };
            }
        }
    }
    
}
