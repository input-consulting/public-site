using System.Collections.Generic;

namespace InputSite.Interfaces
{
    public interface IArticleLocator
    {
        IEnumerable<string> GetAllArticles();
        string PathToResource(string path);
    }
}