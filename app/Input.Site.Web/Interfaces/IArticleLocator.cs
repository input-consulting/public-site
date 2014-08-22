using System.Collections.Generic;

namespace InputSite.Interfaces
{
    public interface IArticleLocator
    {
        IEnumerable<string> Articles();
        string PathToResource(string path);
    }
}