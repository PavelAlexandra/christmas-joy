using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChristmasJoy.App.DbRepositories;
using System;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.Helpers;

namespace ChristmasJoy.App.Controllers
{
  [Route("api/[controller]")]
  [Authorize(Roles = "Generic")]
  public class UsersController : Controller
  {
    private readonly ISecretSantasRepository _santasRepo;
    private readonly IUserRepository _userRepo;
    private readonly IWishListRepository _wishListRepo;
    private static object locker = new object();

    public UsersController(ISecretSantasRepository santasRepository,
      IUserRepository userRepository,
      IWishListRepository wishListRepo)
    {
      _santasRepo = santasRepository;
      _userRepo = userRepository;
      _wishListRepo = wishListRepo;
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
          return BadRequest(Errors.AddErrorToModelState("error", "Invalid user id.", ModelState));
        }
        if(user.SecretSantaForId != null)
        {
          return BadRequest(Errors.AddErrorToModelState("error", "User already is a secret santa.", ModelState));
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
          return BadRequest($"No user found for id {userId}");
        }

        var wishlist = _wishListRepo.GetWishList(user.CustomId);

        var userData = new UserData
        {
          UserInfo = user,
          WishList = wishlist
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
  }
}
