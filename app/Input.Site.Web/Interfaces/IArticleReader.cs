using System.Collections.Generic;
using InputSite.Model;

namespace InputSite.Interfaces
{
    public interface IArticleReader
    {
        IEnumerable<ArticleModel> ArticlesByFreeText(string search);
        IEnumerable<ArticleModel> ArticlesByRoute(string categoryName);
        IEnumerable<ArticleModel> ArticlesByTags(IEnumerable<string> tags);
        IEnumerable<ArticleModel> ArticlesByAuthor(string author);
        ArticleModel ArticleById(string id);
        IEnumerable<string> TagCloud();
    }
}