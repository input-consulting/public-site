using System;
using System.Collections.Generic;
using System.Linq;
using InputSite.Interfaces;
using InputSite.Model;
using System.Text.RegularExpressions;

namespace InputSite.Services
{
    public class ArticleReader : IArticleReader
    {
        private readonly IArticleStorage _articleStorage;

        public ArticleReader(IArticleStorage articleStorage)
        {
            _articleStorage = articleStorage;
        }

        public IEnumerable<ArticleModel> ArticlesByFreeText(string search)
        {
            return _articleStorage.ArticlesByFreeText(search)
                        .Where(a => a.BlogDate.Date <= DateTime.Today);
        }

        public IEnumerable<ArticleModel> ArticlesByRoute(string categoryName)
        {
            return _articleStorage.ArticlesByRoute(categoryName)
                        .Where( a=> a.BlogDate.Date <= DateTime.Today)
                        .OrderByDescending(d => d.BlogDate);
        }

        public IEnumerable<ArticleModel> ArticlesByTags(IEnumerable<string> tags)
        {
            return _articleStorage.ArticlesByTags(tags)
                        .Where(a => a.BlogDate.Date <= DateTime.Today)
                        .OrderByDescending(d => d.BlogDate);
        }

        public IEnumerable<ArticleModel> ArticlesByAuthor(string author)
        {
            return _articleStorage.ArticlesByAuthor(author)
                        .Where(a => a.BlogDate.Date <= DateTime.Today)
                        .OrderByDescending(d => d.BlogDate);
        }

        public ArticleModel ArticleById(string id)
        {
            return _articleStorage.ArticleById(id);
        }

        public IEnumerable<string> TagCloud()
        {
            return _articleStorage.TagCloud();
        }

        public IEnumerable<string> AllArticleRoutes()
        {
            return _articleStorage.AllArticleRoutes().Select(r => r.RouteName);
        }
    }
}