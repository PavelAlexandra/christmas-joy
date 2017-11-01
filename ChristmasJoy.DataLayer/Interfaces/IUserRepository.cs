using ChristmasJoy.Models;

namespace ChristmasJoy.DataLayer.Interfaces
{
    public interface IUserRepository
    {
        User FindByEmail(string email);
    }
}
