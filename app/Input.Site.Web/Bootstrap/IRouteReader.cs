using System.Collections.Generic;

namespace InputSite.Bootstrap
{
    public interface IRouteReader
    {
        IEnumerable<string> Routes(string startFromHere, bool transformToRoutes = true);
        string TransformToResource(string rootPath, string path);
        string TransformToRoute(string rootPath, string path);
    }
}