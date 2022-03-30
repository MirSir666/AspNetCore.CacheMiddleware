using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspNetCore.CacheMiddleware.Samples.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET: api/<TestController>
        [HttpGet()]
        [CacheOpen]
        public IEnumerable<string> GetList()
        {
            return new string[] { "value1", "value2" };
        }



        [HttpGet()]
        [CacheOpen]
        public string GetString()
        {
            return "value";
        }
    }
}
