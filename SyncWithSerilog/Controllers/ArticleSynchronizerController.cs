using Microsoft.AspNetCore.Mvc;
using SyncWithSerilog.Synchronizer;

namespace SyncWithSerilog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleSynchronizerController : ControllerBase
    {
        private readonly ArticleSynchronizer _articleSynchronizer;

        public ArticleSynchronizerController(ArticleSynchronizer articleSynchronizer)
            => _articleSynchronizer = articleSynchronizer;

        // POST api/<ArticleSynchronizerController>
        [HttpPost]
        public void Post()
        {
            _articleSynchronizer.Run();
        }
    }
}
