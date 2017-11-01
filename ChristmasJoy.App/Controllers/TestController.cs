using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ChristmasJoy.App.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Generic")]
    public class TestController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
