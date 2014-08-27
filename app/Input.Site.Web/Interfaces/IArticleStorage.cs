using System.Collections.Generic;
using InputSite.Model;

namespace InputSite.Interfaces
{
    public interface IArticleStorage
    {
        IEnumerable<ArticleModel> ArticlesByFreeText(string search);

        IEnumerable<ArticleModel> ArticlesByCategory(string category);

        IEnumerable<ArticleModel> ArticlesByTags(IEnumerable<string> tags);

        IEnumerable<ArticleModel> ArticlesByAuthor(string author);

        IEnumerable<string> TagCloud();

    }
}