using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Models.Dtos;
using CsvHelper;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ChristmasJoy.App.Services
{
  public interface IUserService
  {
    Task<int> UploadUsersFromCsvAsync(TextReader reader, string password);
  }

  public class UserService: IUserService
  {
    private readonly ISignInService _signInService;
    private readonly IUserRepository _userRepository;
    private readonly ISecretSantasRepository _secretSantasRepository;

    internal class CsvUser
    {
      public string UserName { get; set; }

      public string Email { get; set; }
    }

    public UserService(
      ISignInService signInService,
      IUserRepository userRepository,
      ISecretSantasRepository secretSantasRepository)
    {
      _signInService = signInService;
      _userRepository = userRepository;
      _secretSantasRepository = secretSantasRepository;
    }

    public async Task<int> UploadUsersFromCsvAsync(TextReader reader, string password)
    {
      var csvReader = new CsvReader(reader);
      var records = csvReader.GetRecords<CsvUser>();
      var hashedPassword = _signInService.GetHashedPassword(password);
      var importedUsersCount = 0;
      var random = new Random();

      foreach (var record in records)
      {
        var existingRecord = _userRepository.GetUser(record.Email);
        if (existingRecord != null) continue;

        var user = new UserViewModel
        {
          Email = record.Email,
          UserName = record.UserName,
          Age = random.Next(100, 200),
          IsAdmin = false,
          SecretSantaForId = null,
          HashedPassword = hashedPassword
        };
        await _userRepository.AddUserAsync(user);

        var newUser = _userRepository.GetUser(user.Email);
        if (newUser != null)
        {
          await _secretSantasRepository.AddUserAsync(newUser.Id);
        }

        importedUsersCount++;
      }

      return importedUsersCount;
    }
  }
}
