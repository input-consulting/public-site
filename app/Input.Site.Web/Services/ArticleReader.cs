using System;
using System.Collections.Generic;
using System.Linq;
using InputSite.Extensions;
using InputSite.Interfaces;
using InputSite.Model;

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

        public IEnumerable<ArticleModel> ArticlesByCategory(string categoryName)
        {
            return _articleStorage.ArticlesByCategory(categoryName)
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
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> TagCloud()
        {
            return _articleStorage.TagCloud();
        }
    }
}