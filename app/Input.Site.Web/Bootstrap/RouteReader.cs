using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using Nancy.Routing;

namespace InputSite.Bootstrap
{
    public class RouteReader : IRouteReader
    {
        private readonly IList<IRouteEvaluator> _routeEvaluators;

        public RouteReader(IList<IRouteEvaluator> routeEvaluators)
        {
            _routeEvaluators = routeEvaluators;
        }

        public IEnumerable<string> Routes(string startFromHere, bool transformToRoutes = true)
        {
            var findings = RouteCrawler(startFromHere, route => _routeEvaluators.All(e => e.Ensure(route)));

            if (transformToRoutes)
            {
                findings = TransformToRoutes(findings, startFromHere);
            }
 
            return findings.Distinct();
        }

        public string TransformToResource(string rootPath, string path)
        {
            return MakeResource(rootPath, path);
        }

        public string TransformToRoute(string rootPath, string path)
        {
            return MakeRoute(rootPath, path);
        }

        private static IEnumerable<string> TransformToRoutes(IEnumerable<string> dirtyRoutes, string startFromHere)
        {
            return dirtyRoutes.Select(route => MakeRoute(startFromHere, route));
        }
        private static string MakeRoute(string rootPath, string path)
        {
            var cleanRoute = path.Replace(rootPath, "").Replace(@"\", @"/");
            return cleanRoute.Substring(0, cleanRoute.LastIndexOf(@"/", StringComparison.Ordinal));            
        }

        private static IEnumerable<string> TransformToResources(IEnumerable<string> dirtyRoutes, string startFromHere)
        {
            return dirtyRoutes.Select(route => MakeResource(startFromHere, route));
        }

        private static string MakeResource(string startFromHere, string route)
        {
            var resource = route.Replace(startFromHere, "").Replace(@"\", @"/");
            if (resource.StartsWith(@"/")) resource = resource.Remove(0, 1);
            return resource.Remove(resource.LastIndexOf(".", StringComparison.Ordinal));
        }


        private static IEnumerable<string> RouteCrawler(string path, Func<FileInfo, bool> predicate = null)
        {
            var queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                foreach (var dir in Directory.GetDirectories(path))
                {
                    queue.Enqueue(dir);
                }

                foreach (var file in  Directory.GetFiles(path))
                {
                    if (predicate == null || predicate(new FileInfo(file))) yield return file;
                }
            }            
        }
    }
}