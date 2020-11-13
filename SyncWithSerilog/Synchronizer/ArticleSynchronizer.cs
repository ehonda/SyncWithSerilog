using Serilog;
using Serilog.Context;
using SyncWithSerilog.Models;
using SyncWithSerilog.Synchronizer.LogEvents;
using System;
using System.Collections.Generic;

namespace SyncWithSerilog.Synchronizer
{
    public class ArticleSynchronizer
    {
        private readonly Random _rng = new Random();

        public void Run()
        {
            var articles = GetArticles();
            UploadArticles(articles);
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
                using (LogContext.PushProperty("@Article", article))
                {
                    if (UploadArticle(article))
                        Log.Logger.Information("{@Article} {@Event}", article, Event.UploadSucceeded);
                    else
                        Log.Logger.Error("{@Article} {@Event}", article, Event.UploadFailed);
                }
                
        }

        private bool UploadArticle(Article article)
            => _rng.Next(2) == 0;
    }
}
