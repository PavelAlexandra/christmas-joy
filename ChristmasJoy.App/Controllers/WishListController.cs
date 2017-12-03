using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChristmasJoy.App.DbRepositories;
using ChristmasJoy.App.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChristmasJoy.App.Controllers
{
  [Route("api/[controller]")]
    public class WishListController : Controller
    {
    private readonly IWishListRepository _wishListRepo;

    public WishListController(IWishListRepository wishListRepo)
    {
      _wishListRepo = wishListRepo;
    }

    [HttpGet("getWishList/{userId}")]
    public IActionResult GetWishList(int userId)
    {
      try
      {
        var items = _wishListRepo.GetWishList(userId);
        return Ok(items);
      }
      catch (Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddItem([FromBody] WishListItem item)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(item);
        }
        item.Id = null;
        var itemId = await _wishListRepo.AddWishItemAsync(item);
        return Ok(new { id= itemId });
      }
      catch (Exception ex)
      {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { error = ex.Message });
        Response.ContentType = "application/json";
        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return new JsonResult(result);
      }
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateItem([FromBody] WishListItem item)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(item);
        }

        await _wishListRepo.UpdateWishItemAsync(item);
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

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteItem([FromBody] WishListItem item)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(item);
        }

        await _wishListRepo.DeleteWishItemAsync(item);
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
