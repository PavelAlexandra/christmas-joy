using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChristmasJoy.App.DbRepositories;
using System;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using ChristmasJoy.App.Services;
using System.Linq;

namespace ChristmasJoy.App.Controllers
{
  [Route("api/[controller]")]
  [Authorize(Roles = "Generic")]
  [EnableCors("MyPolicy")]
  public class UsersController : Controller
  {
    private readonly ISecretSantasRepository _santasRepo;
    private readonly IChristmasStatusService _statusService;
    private readonly IUserRepository _userRepo;
    private readonly IWishListRepository _wishListRepo;
    private readonly ICommentsRepository _commentsRepo;

    private static object locker = new object();

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
    public IActionResult GetMySecretReceiver(int secretSantaId)
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

        lock (locker)
        {
          var availableReceivers = _santasRepo.GetAvailableReceivers(secretSantaId);

          Random rnd = new Random();
          var randomReceiverIndex = rnd.Next(0, availableReceivers.Count - 1);
          var receiver = availableReceivers[randomReceiverIndex];
          var receiverUser = _userRepo.GetUser(receiver.ReceiverUserId);
          user.SecretSantaForId = receiverUser.CustomId;
          user.SecretSantaFor = receiverUser.UserName;

          _santasRepo.SetSecretSanta(receiver.ReceiverUserId, secretSantaId);
          _userRepo.UpdateUserAsync(user);
          
          return Ok(new {
            receiverId = receiver.ReceiverUserId,
            receiverName = receiverUser.UserName
          });
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

        var wishlist = _wishListRepo.GetWishList(user.CustomId);
        var userStatus = _statusService.GetUserStatus(user.CustomId, user.UserName);

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
  
        var users = _userRepo.GetAllUsers();
        if (users != null)
        {
          foreach (var user in users)
          {
            var userStatus = _statusService.GetUserStatus(user.CustomId, user.UserName);
            usersMap.Add(user.CustomId, userStatus);
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
        var users = _userRepo.GetAllUsers();
        var userStatuses = new List<UserStatus>();
        foreach(var user in users)
        {
          var userStatus = _statusService.GetUserStatus(user.CustomId, user.UserName);
          userStatuses.Add(userStatus);
        }
        userStatuses.OrderBy(x => x.Points);

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
