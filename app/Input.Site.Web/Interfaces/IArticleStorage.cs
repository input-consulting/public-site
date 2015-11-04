using System.Collections.Generic;
using InputSite.Model;
using InputSite.Services;

namespace InputSite.Interfaces
{
    public interface IArticleStorage
    {
        IEnumerable<ArticleLocation> AllArticleRoutes();
        IEnumerable<ArticleModel> ArticlesByFreeText(string search);

        IEnumerable<ArticleModel> ArticlesByRoute(string category);

        IEnumerable<ArticleModel> ArticlesByTags(IEnumerable<string> tags);

        IEnumerable<ArticleModel> ArticlesByAuthor(string author);

        IEnumerable<string> TagCloud();

        ArticleModel ArticleById(string id);
    }
}