using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SyncWithSerilog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleSynchronizerController : ControllerBase
    {

        // POST api/<ArticleSynchronizerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
