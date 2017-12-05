using ChristmasJoy.App.DbRepositories;
using ChristmasJoy.App.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasJoy.App.Controllers
{
  [Route("api/[controller]")]
  [EnableCors("MyPolicy")]
  public class CommentsController : Controller
  {
    private readonly ICommentsRepository _commRepo;
    private readonly IUserRepository _userRepo;

    public CommentsController(ICommentsRepository commentsRepo,
      IUserRepository userRepo)
    {
      _commRepo = commentsRepo;
      _userRepo = userRepo;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddItem([FromBody] Comment item)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(item);
        }
        item.Id = null;
        item.CommentDate = DateTime.UtcNow;
        var itemId = await _commRepo.AddCommentAsync(item);
        var user = _userRepo.GetUser(item.FromUserId);

        //TBD
        var userStatus = new UserStatus
        {
          ChristmasStatus = Status.Grinch.ToString().ToLower(),
          UserName = user.UserName,
          Points = 0
        };
        return Ok(new { id = itemId, userStatus= userStatus, date = item.CommentDate});
      }
      catch (Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpGet("all/{userId}")]
    public IActionResult GetComments(int userId)
    {
      try
      {
        var comments = _commRepo.GetReceivedComments(userId).OrderByDescending(x => x.CommentDate).ToList();

        return Ok(new { data = comments });
      }
      catch (Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpPost("like/{userId}")]
    public IActionResult LikeComment(int userId, [FromBody] Like like)
    {
      try
      {
        _commRepo.SetLike(userId, like.CommentId);
        return Ok();
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
