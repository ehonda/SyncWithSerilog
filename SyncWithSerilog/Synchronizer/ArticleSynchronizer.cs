using Serilog;
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
                if (UploadArticle(article))
                    Log.Logger.Information("Upload of article {@Article} succeeded", article);
                else
                    Log.Logger.Error("Upload of article {@Article} failed", article);
        }

        private bool UploadArticle(Article article)
            => _rng.Next(2) == 0;
    }
}
