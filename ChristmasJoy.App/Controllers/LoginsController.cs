using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Models.Dtos;
using ChristmasJoy.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
    private readonly ISecretSantasRepository _santasRepo;

    public LoginsController(IAppConfiguration appConfiguration,
      IUserRepository userRepository,
      ISignInService signInService,
      ISecretSantasRepository santasRepository
      )
    {
      _configuration = appConfiguration;
      _userRepository = userRepository;
      _signInService = signInService;
      _santasRepo = santasRepository;
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

    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser([FromBody]UserViewModel model)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(model);
        }

        var existingUser = _userRepository.GetUser(model.Email);
        if(existingUser != null)
        {
          return BadRequest(Errors.AddErrorToModelState("email", "Email address is already used.", ModelState));
        }
          
        model.CustomId = _userRepository.LastCustomId() + 1;
        model.Id = null;
        model.HashedPassword = _signInService.GetHashedPassword(model.HashedPassword);

        await _userRepository.AddUserAsync(model);

        var newUser = _userRepository.GetUser(model.Email);
        if(newUser != null)
        {
         await _santasRepo.AddUserAsync(newUser.CustomId);
        }

        return new JsonResult(new { id = newUser.Id, customId = newUser.CustomId});
      }
      catch (System.Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody]UserViewModel model)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(model);
        }

        await _userRepository.UpdateUserAsync(model);
        return Ok();
      }
      catch (System.Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(new { error = ex.Message });
      }
    }

    [HttpPost("DeleteUser")]
    public async Task<IActionResult> DeleteUser([FromBody]UserViewModel model)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(model);
        }

        await _userRepository.DeleteUserAsync(model);
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
