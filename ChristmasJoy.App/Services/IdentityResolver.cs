using ChristmasJoy.App.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.DbRepositories;

namespace ChristmasJoy.App.Services
{
  public interface IIdentityResolver
  {
    List<Claim> GetIdentityCaims(User user);
  }

  public class IdentityResolver: IIdentityResolver
  {
    IUserRepository _userRepository;

    public IdentityResolver(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public List<Claim> GetIdentityCaims(User user)
    {
      var claims = new List<Claim>
       {
          new Claim(JwtRegisteredClaimNames.Sub, user.Email),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64),
          new Claim(ClaimTypes.Role, Constants.GenericRole)
        };

      if (user.IsAdmin)
      {
        claims.Add(new Claim(ClaimTypes.Role, Constants.AdminRole));
      }
      
      return claims;
    }

    private static long ToUnixEpochDate(DateTime date)
       => (long)Math.Round((date.ToUniversalTime() -
                            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                           .TotalSeconds);
  }
}
