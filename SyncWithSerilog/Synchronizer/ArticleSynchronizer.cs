using Serilog;
using Serilog.Context;
using SyncWithSerilog.Filters;
using SyncWithSerilog.Logging.Events;
using SyncWithSerilog.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncWithSerilog.Synchronizer
{
    public class ArticleSynchronizer
    {
        private readonly Random _rng = new Random();

        public void Run(ArticleSynchronizationRequestFilter filter)
        {
            Log
                .ForContext("Event", Event.SyncStarted)
                .Information("{Event:L}");

            var articles = GetArticles(filter?.Count ?? 4);
            UploadArticles(articles);

            Log
                .ForContext("Event", Event.SyncEnded)
                .Information("{Event:L}");
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
                            .Information("{Event:L} for {Article}");
                    else
                        Log
                            .ForContext("Event", Event.UploadFailed)
                            .ForContext("Alert", 1)
                            .Error("{Event:L} for {Article}");
                }
        }

        private bool UploadArticle(Article article)
            => _rng.Next(2) == 0;
    }
}
