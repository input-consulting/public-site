using System.Collections.Generic;
using System.Linq;
using Nancy;

namespace InputSite.Bootstrap
{
    public class RouteLocatorProvider : IRouteLocatorProvider
    {
        private readonly IEnumerable<string> _blackListedRoutes = new List<string> { @"views" };
        private readonly IRootPathProvider _rootPathProvider;
        private static  IEnumerable<string> _staticRoutes = new List<string>();
        private static  IEnumerable<string> _dateRoutes = new List<string>(); 

        public RouteLocatorProvider(IRootPathProvider rootPathProvider)
        {
            _rootPathProvider = rootPathProvider;
        }

        public IEnumerable<string> StaticRoutes()
        {
            if (_staticRoutes.Any()) return _staticRoutes;

            var rootPath = _rootPathProvider.GetRootPath() + "\\Views";

            var staticEvaluators = new List<IRouteEvaluator>
            {
                new EnsureThatRouteIsNotBlackListed(_blackListedRoutes),
                new EnsureThatRouteDoesNotContainNumbers(),
                new EnsureThatRouteContainMarkupDocument(),
                new EnsureThatDocumentIsValidMarkdown()
            };

            var routeReader = new RouteReader(staticEvaluators);
            _staticRoutes = routeReader.Routes(rootPath);           

            return _staticRoutes;
        }

        public IEnumerable<string> DateRoutes()
        {
            if (_dateRoutes.Any()) return _dateRoutes;

            var rootPath = _rootPathProvider.GetRootPath() + "\\Views";

            var dateEvaluators = new List<IRouteEvaluator>
            {
                new EnsureThatRouteIsNotBlackListed(_blackListedRoutes),
                new EnsureThatRouteContainDateParts(),
                new EnsureThatRouteContainMarkupDocument(),
                new EnsureThatDocumentIsValidMarkdown()
            };

            var routeReader = new RouteReader(dateEvaluators);
            _dateRoutes = routeReader.Routes(rootPath);

            return _dateRoutes;
        }
    }
}