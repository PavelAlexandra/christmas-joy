using ChristmasJoy.DataLayer;
using ChristmasJoy.DataLayer.Interfaces;
using ChristmasJoy.App.Helpers;
using ChristmasJoy.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChristmasJoy.App.Services
{
  public class IdentityResolver
  {
    IUserRepository _userRepository;

    public IdentityResolver()
    {
      _userRepository = new UserRepository();
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
