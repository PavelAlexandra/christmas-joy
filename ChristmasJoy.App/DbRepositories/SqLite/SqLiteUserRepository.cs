using AutoMapper;
using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using ChristmasJoy.App.Models.SqLiteModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.DbRepositories.SqLite
{
  public class SqLiteUserRepository : IUserRepository
  {
    private readonly IMapper _mapper;
    private readonly IAppConfiguration _appConfig;
    private readonly ChristmasDbContextFactory dbContextFactory;

    public SqLiteUserRepository(
      IMapper mapper,
      IAppConfiguration appConfig,
      ChristmasDbContextFactory contextFactory
      )
    {
      _mapper = mapper;
      _appConfig = appConfig;
      dbContextFactory = contextFactory;
    }

    public async Task AddUserAsync(UserViewModel item)
    {
      var dbUser = _mapper.Map<UserViewModel, User>(item);
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        db.Users.Add(dbUser);
        await db.SaveChangesAsync();
      }
    }

    public async Task DeleteUserAsync(UserViewModel user)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var dbUser = await db.Users.FindAsync(user.Id);
        if(dbUser == null)
        {
          throw new KeyNotFoundException();
        }

        db.Users.Remove(dbUser);

        await db.SaveChangesAsync();
      }
    }

    public List<UserViewModel> GetAllUsers()
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        return db.Users.Select(user => _mapper.Map<User, UserViewModel>(user)).ToList();
      }
    }

    public UserViewModel GetUser(string email)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var user = db.Users.Where(u => u.Email == email).FirstOrDefault();
        if(user == null)
        {
          return null;
        }

        return _mapper.Map<User, UserViewModel>(user);
      }
    }

    public UserViewModel GetUser(int customId)
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var user = db.Users.Where(u => u.CustomId == customId).FirstOrDefault();
        if (user == null)
        {
          throw new KeyNotFoundException($"User with id '{customId}' was not found.");
        }

        return _mapper.Map<User, UserViewModel>(user);
      }
    }

    public int LastCustomId()
    {
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var maxId = db.Users.Max(u => u.CustomId);
        return maxId;
      }
    }

    public async Task UpdateUserAsync(UserViewModel item)
    {
      var dbUser = _mapper.Map<UserViewModel, User>(item);
      using (var db = dbContextFactory.CreateDbContext(_appConfig))
      {
        var user = await db.Users.FindAsync(item.Id);
        if (user == null)
        {
          throw new KeyNotFoundException($"User with id '{item.Id}' was not found.");
        }
        user = dbUser;
        await db.SaveChangesAsync();
      }
    }
  }
}
