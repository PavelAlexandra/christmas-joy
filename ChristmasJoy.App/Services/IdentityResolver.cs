using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Models.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChristmasJoy.App.Services
{
  public interface IIdentityResolver
  {
    List<Claim> GetIdentityCaims(UserViewModel user);
  }

  public class IdentityResolver: IIdentityResolver
  {
    IUserRepository _userRepository;

    public IdentityResolver(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public List<Claim> GetIdentityCaims(UserViewModel user)
    {
      var claims = new List<Claim>
       {
          new Claim(JwtRegisteredClaimNames.Sub, user.Email),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64),
          new Claim("id", user.Id.ToString())
        };

      if (user.IsAdmin)
      {
        claims.Add(new Claim("role", Constants.AdminRole));
      }
      else
      {
        claims.Add(new Claim("role", Constants.GenericRole));
      }
      
      return claims;
    }

    private static long ToUnixEpochDate(DateTime date)
       => (long)Math.Round((date.ToUniversalTime() -
                            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                           .TotalSeconds);
  }
}
