using System;
using System.Linq;
using InputSite.Bootstrap;
using InputSite.Interfaces;
using InputSite.Model;
using Nancy;
using Nancy.Responses.Negotiation;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace InputSite.WebModules
{
    public class DateRoutesModule : BaseModule
    {
        private readonly IArticleReader _articleReader;
        private readonly IRouteLocatorProvider _routeLocatorProvider;

        public DateRoutesModule(IArticleReader articleReader, IRouteLocatorProvider routeLocatorProvider)
        {
            _articleReader = articleReader;
            _routeLocatorProvider = routeLocatorProvider;

            SetupDateRoutes();
        }

        private IEnumerable<string> GetArticleDateRoutes()
        {
            var dateRoutesFromStorage = new List<string>();
            var articleRoutes = _articleReader.AllArticleRoutes().ToList();
            var reg = new Regex(@"[/](\d+)[/](\d+)[/](\d+)[/]", RegexOptions.Compiled);

            foreach (var articleRoute in articleRoutes)
            {
                var match = reg.Match(articleRoute);
                if (match.Success)
                {
                    dateRoutesFromStorage.Add(articleRoute.Substring(0, match.Index));
                }
            }

            var routes = _routeLocatorProvider.DateRoutes().Concat(dateRoutesFromStorage).Distinct();
            return routes;
        }

        private void SetupDateRoutes()
        {
            var articleRoutes = GetArticleDateRoutes();
            var routes = articleRoutes.Select(r => r.Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries).First()).Distinct();

            foreach (var route in routes)
            {
                Get[$"/{route}"] = parameters =>
                {
                    var path = Request.Path.Replace("/", "");
                    PageModel.Meta.Articles = _articleReader.ArticlesByRoute(path);
                    return RenderView($"_layout/{Request.Path}", PageModel);
                };

                Get[$"/{route}/{{year:int}}"] = parameters => FetchArticles(route, parameters);
                Get[$"/{route}/{{year:int}}/{{month}}"] = parameters => FetchArticles(route, parameters);
                Get[$"/{route}/{{year:int}}/{{month}}/{{day}}"] = parameters => FetchArticles(route, parameters);
            }
        }

        private Negotiator FetchArticles(string configuredRoute, dynamic parameters)
        {
            PageModel.Meta.Articles = _articleReader.ArticlesByRoute(RecreateRoute(configuredRoute, parameters));
            return RenderView($"_layout/{configuredRoute}", PageModel);
        }

        private Negotiator RenderView(string view, PageModel model)
        {
            return Negotiate
                .WithView(view)
                .WithModel(model)
                .WithMediaRangeModel("application/json", model);
        }

        private static string RecreateRoute(string route, dynamic parameters)
        {
            var path = string.Concat(route, "/", string.Join("/", parameters.Values));
            return path.TrimStart(new[] {'/'});
        }
    }
}