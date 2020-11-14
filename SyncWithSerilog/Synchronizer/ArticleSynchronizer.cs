using Serilog;
using Serilog.Context;
using SyncWithSerilog.Logging.Events;
using SyncWithSerilog.Models;
using System;
using System.Collections.Generic;

namespace SyncWithSerilog.Synchronizer
{
    public class ArticleSynchronizer
    {
        private readonly Random _rng = new Random();

        public void Run()
        {
            Log
                .ForContext("Event", Event.SyncStarted)
                .Information("{Event}");

            var articles = GetArticles();
            UploadArticles(articles);

            Log
                .ForContext("Event", Event.SyncEnded)
                .Information("{Event}");
        }

        private IEnumerable<Article> GetArticles()
            => new[]
            {
                new Article { Sku = "100" },
                new Article { Sku = "101" },
                new Article { Sku = "102" },
                new Article { Sku = "103" }
            };

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
                            .Error("{Event} for {Article}", Event.UploadFailed);
                }
        }

        private bool UploadArticle(Article article)
            => _rng.Next(2) == 0;
    }
}
