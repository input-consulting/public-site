using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InputSite.Bootstrap
{
    public class EnsureThatRouteIsNotBlackListed : IRouteEvaluator
    {
        private readonly IEnumerable<string> _blackListedRoutes;

        public EnsureThatRouteIsNotBlackListed(IEnumerable<string> blackListedRoutes)
        {
            _blackListedRoutes = blackListedRoutes;
        }

        public bool Ensure(FileInfo fileInfo)
        {
            if (fileInfo.Directory == null) return true;

            var dir = fileInfo.Directory.Name.ToLower();
            return !_blackListedRoutes.Contains(dir);
        }
    }
}