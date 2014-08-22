using System.Collections.Generic;
using InputSite.Bootstrap;
using InputSite.Interfaces;
using InputSite.Model;
using Nancy;

namespace InputSite.Services
{
    public class ArticleLocator : IArticleLocator
    {
        private readonly string _rootPath;

        public ArticleLocator(IRootPathProvider rootPathProvider)
        {
            _rootPath = rootPathProvider.GetRootPath() + "\\Views";
        }
        
        public IEnumerable<string> Articles()
        {
            var articleEvaluators = new List<IRouteEvaluator>
            {
                new EnsureThatRouteContainMarkupDocument()
            };

            var routeReader = new RouteReader(articleEvaluators);
            var routes = routeReader.Routes(_rootPath, transformToRoutes:false);

            return routes;
        }

        public string PathToResource(string path)
        {
            var routeReader = new RouteReader(null);
            return routeReader.TransformToResource(_rootPath, path);
        }
    }
}