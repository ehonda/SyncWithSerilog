using Microsoft.AspNetCore.Mvc;
using Serilog;
using SyncWithSerilog.Filters;
using SyncWithSerilog.Requests;
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
        public void Post([FromQuery] ArticleSynchronizationRequestFilter filter)
        {
            var request = ArticleSynchronizationRequest.FromFilter(filter);
            Log.Logger.Information(
                "Article synchronization requested for {Count} articles with success rate {SuccessRate}",
                request.Count, request.SuccessRate);
            _articleSynchronizer.Run(request);
        }
    }
}
