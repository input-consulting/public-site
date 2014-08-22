using System.IO;

namespace InputSite.Bootstrap
{
    public class EnsureThatRouteContainMarkupDocument : IRouteEvaluator
    {
        public bool Ensure(FileInfo fileInfo)
        {
            return fileInfo.Extension.Contains("md") || fileInfo.Extension.Contains("markdown");
        }
    }
}