using ChristmasJoy.App.DbRepositories.Interfaces;
using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChristmasJoy.App.Controllers
{
  [Route("api/[controller]")]
  [Authorize(Roles = "Generic, Admin")]
  [EnableCors("MyPolicy")]
  public class UsersController : Controller
  {
    private readonly ISecretSantasRepository _santasRepo;
    private readonly IChristmasStatusService _statusService;
    private readonly IUserRepository _userRepo;
    private readonly IWishListRepository _wishListRepo;
    private readonly ICommentsRepository _commentsRepo;
    
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public UsersController(ISecretSantasRepository santasRepository,
      IUserRepository userRepository,
      IWishListRepository wishListRepo,
      ICommentsRepository commentsRepo,
      IChristmasStatusService statusService)
    {
      _santasRepo = santasRepository;
      _userRepo = userRepository;
      _wishListRepo = wishListRepo;
      _commentsRepo = commentsRepo;
      _statusService = statusService;
    }

    // GET: api/getSantaReceiver
    [HttpGet("getReceiver/{secretSantaId}")]
    public async Task<IActionResult> GetMySecretReceiver(int secretSantaId)
    {
      try
      {
        var user = _userRepo.GetUser(secretSantaId);
        if (user == null)
        {
          return BadRequest(Errors.AddErrorToModelState("user_id", "Invalid user id.", ModelState));
        }
        if(user.SecretSantaForId != null)
        {
          return BadRequest(Errors.AddErrorToModelState("user_id", "User already is a secret santa.", ModelState));
        }

        await _semaphore.WaitAsync();
        try
        {
          var availableReceivers = _santasRepo.GetAvailableReceivers(secretSantaId);

          Random rnd = new Random();
          var randomReceiverIndex = rnd.Next(1, availableReceivers.Count);
          var receiver = availableReceivers[randomReceiverIndex-1];

          var secretName = await _santasRepo.SetSecretSantaAsync(receiver.ReceiverUserId, secretSantaId);
          
          return Ok(new {
            receiverId = receiver.ReceiverUserId,
            receiverName = secretName
          });
        }
          finally
        {
          _semaphore.Release();

        }
      }
      catch (Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpGet("getUserData/{userId}")]
    public IActionResult GetUserData(int userId)
    {
      try
      {
        var user = _userRepo.GetUser(userId);
        if (user == null)
        {
          return BadRequest(Errors.AddErrorToModelState("user_id", $"No user found for id {userId}", ModelState));
        }

        var wishlist = _wishListRepo.GetWishList(user.Id);
        var userStatus = _statusService.GetUserStatus(user.Id, user.UserName);

        var userData = new UserData
        {
          UserInfo = user,
          WishList = wishlist,
          Status = userStatus
        };

        return Ok(userData);
      }
      catch (Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpGet("getMap")]
    public IActionResult GetUsersMap()
    {
      try
      {
        var usersMap = new Dictionary<int, UserStatus>();
  
        var users = _userRepo.GetAllNonAdminUsers();
        if (users != null)
        {
          foreach (var user in users)
          {
            var userStatus = _statusService.GetUserStatus(user.Id, user.UserName);
            usersMap.Add(user.Id, userStatus);
          }
        }
        return Ok(new { data = usersMap });
      }
      catch (Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpGet("getStatuses")]
    public IActionResult GetUserStatuses()
    {
      try
      {
        var users = _userRepo.GetAllNonAdminUsers();
        var userStatuses = new List<UserStatus>();
        foreach(var user in users)
        {
          var userStatus = _statusService.GetUserStatus(user.Id, user.UserName);
          userStatuses.Add(userStatus);
        }
        userStatuses = userStatuses.OrderByDescending(x => x.Points).ThenBy(x => x.UserName).ToList();

        return Ok(new { data = userStatuses });
      }
      catch (Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }
  }
}
