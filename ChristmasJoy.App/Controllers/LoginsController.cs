using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.DbRepositories;
using System.Threading.Tasks;
using ChristmasJoy.App.ViewModels;
using ChristmasJoy.App.Services;
using Microsoft.AspNetCore.Cors;

namespace ChristmasJoy.App.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] //Constants.Generic
    [EnableCors("MyPolicy")]
    public class LoginsController : Controller
    {
    private readonly IAppConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly ISignInService _signInService;

    public LoginsController(IAppConfiguration appConfiguration,
      IUserRepository userRepository,
      ISignInService signInService
      )
    {
      _configuration = appConfiguration;
      _userRepository = userRepository;
      _signInService = signInService;
    }

    // GET: api/users
    [HttpGet]
    public IActionResult GetAll()
    {
      try
      {
        var users = _userRepository.GetAllUsers();
        return Ok(users);

      }catch(System.Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody]UserViewModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          var existingUser = _userRepository.GetUser(model.Email);
          if(existingUser != null)
          {
            return BadRequest("Email address is already used.");
          }

          var user = new User() { CustomId = model.CustomId, Email = model.Email, IsAdmin = model.IsAdmin, UserName = model.UserName };
          user.HashedPassword = _signInService.GetHashedPassword(model.Password);
          await _userRepository.AddUser(user);
          return Ok();
        }
        else
        {
          return BadRequest(model);
        }
      }
      catch (System.Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromBody]User model)
    {
      try
      {
        await _userRepository.Update(model);
        return Ok();
      }
      catch (System.Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser([FromBody]User model)
    {
      try
      {
        await _userRepository.Delete(model);
        return Ok();
      }
      catch (System.Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }
  }
}
