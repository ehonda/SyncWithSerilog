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
                    Console.WriteLine($"Upload of article {article.Sku} succeeded");
                else
                    Console.WriteLine($"Upload of article {article.Sku} failed");
        }

        private bool UploadArticle(Article article)
            => _rng.Next(2) == 0;
    }
}
