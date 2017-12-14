using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Services;
using ChristmasJoy.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.DbRepositories;
using Microsoft.AspNetCore.Cors;

namespace ChristmasJoy.App.Controllers
{
  [Route("api/[controller]")]
  [EnableCors("MyPolicy")]
  public class AuthorizeController : Controller
  {
    ISignInService _signInService;
    IUserRepository _userRepository;
    IIdentityResolver _identityResolver;
    JwtIssuerOptions _jwtOptions;

    public AuthorizeController(JwtIssuerOptions jwtOptions,
      ISignInService signInService,
      IIdentityResolver identityResolver,
      IUserRepository userRepository)
    {
      _signInService = signInService;
      _userRepository = userRepository;
      _identityResolver = identityResolver;
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
        var hashedPass = _signInService.GetHashedPassword(model.Password);

        var user =  _userRepository.GetUser(model.Email);
        
        if (user == null)
        {
          return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid email or password.", ModelState));
        }

        var passwordCheck = _signInService.CheckLoginInPassword(model.Password, user.HashedPassword);
        if (!passwordCheck)
        {
          return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid email or password.", ModelState));
        }

        var claims = _identityResolver.GetIdentityCaims(user);
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: now,
            expires: now.Add(_jwtOptions.ValidFor),
            signingCredentials: _jwtOptions.SigningCredentials);

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
          access_token = encodedJwt,
          expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
          admin = user.IsAdmin,
          username = user.UserName,
          id = user.CustomId
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
