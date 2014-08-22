using System.IO;

namespace InputSite.Bootstrap
{
    public interface IRouteEvaluator
    {
        bool Ensure(FileInfo fileInfo);

    }
}