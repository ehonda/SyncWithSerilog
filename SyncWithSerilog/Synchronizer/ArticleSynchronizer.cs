using MathNet.Numerics.Distributions;
using Serilog;
using Serilog.Context;
using SyncWithSerilog.Logging.Events;
using SyncWithSerilog.Models;
using SyncWithSerilog.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncWithSerilog.Synchronizer
{
    public class ArticleSynchronizer
    {
        private Bernoulli _bernoulli = new(.5);

        public void Run(ArticleSynchronizationRequest request)
        {
            using (LogContext.PushProperty("ArticleSynchronizationId", Guid.NewGuid().ToString()))
            {
                Log.Debug("Article synchronization id {ArticleSynchronizationId}");

                Log
                    .ForContext("Event", Event.SyncStarted)
                    .Information("{Event}");

                _bernoulli = new(request.SuccessRate);
                var articles = GetArticles(request.Count);
                UploadArticles(articles);

                Log
                    .ForContext("Event", Event.SyncEnded)
                    .Information("{Event}");
            }
        }

        private static IEnumerable<Article> GetArticles(int count)
            => Enumerable
                .Range(0, count)
                .Select(number => new Article { Sku = (100 + number).ToString() });

        private void UploadArticles(IEnumerable<Article> articles)
        {
            foreach (var article in articles)
                using (LogContext.PushProperty("Article", article, true))
                {
                    if (UploadArticle(article))
                        Log
                            .ForContext("Event", Event.UploadSucceeded)
                            .Information("{Event} for {Article}");
                    else
                        Log
                            .ForContext("Event", Event.UploadFailed)
                            .ForContext("Alert", 1)
                            .Error("{Event} for {Article}");
                }
        }

        private bool UploadArticle(Article article)
            => _bernoulli.Sample() == 1;
    }
}
