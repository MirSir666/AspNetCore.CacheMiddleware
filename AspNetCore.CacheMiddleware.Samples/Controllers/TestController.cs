using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            Task.Delay(5000).GetAwaiter().GetResult();
            var list= new string[] { "value1", "value2" };
            return list;
        }



        [HttpGet()]
        [CacheOpen]
        public string GetString()
        {
            return "value";
        }
    }
}
