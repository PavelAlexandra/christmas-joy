using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ChristmasJoy.App.Controllers
{
  [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] //Constants.Generic
    public class UsersController : Controller
    {
        // GET: api/users
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "George Olteanu", "Alexandra Olteanu" };
        }
    }
}
