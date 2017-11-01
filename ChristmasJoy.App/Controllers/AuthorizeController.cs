using ChristmasJoy.DataLayer;
using ChristmasJoy.DataLayer.Interfaces;
using ChristmasJoy.App.Helpers;
using ChristmasJoy.Models;
using ChristmasJoy.App.Services;
using ChristmasJoy.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace ChristmasJoy.App.Controllers
{
  [Route("api/[controller]")]
  public class AuthorizeController : Controller
  {
    SignInService _signInService;
    IUserRepository _userRepository;
    IdentityResolver _identityResolver;
    JwtIssuerOptions _jwtOptions;

    public AuthorizeController(JwtIssuerOptions jwtOptions)
    {
      _signInService = new SignInService();
      _userRepository = new UserRepository();
      _identityResolver = new IdentityResolver();
      _jwtOptions = jwtOptions;
    }
    
    public async Task<IActionResult> Post([FromBody]LoginViewModel model)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var user = _userRepository.FindByEmail(model.Email);
        if (user == null)
        {
          return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid email or password.", ModelState));
        }

        var passwordCheck = _signInService.CheckLoginInPassword(model.Password, user.HashedPassword);
        if (!passwordCheck)
        {
          return Unauthorized();
        }

        var claims = _identityResolver.GetIdentityCaims(user);

        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: _jwtOptions.Expiration,
            signingCredentials: _jwtOptions.SigningCredentials);

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
          access_token = encodedJwt,
          expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
          admin = user.IsAdmin,
          username = user.UserName
        };

        var json = JsonConvert.SerializeObject(response);
        return new OkObjectResult(json);
      }
      catch (Exception ex)
      {
        return BadRequest(Errors.AddErrorToModelState("application_error", ex.Message, ModelState));
      }
    }  
  }
}
