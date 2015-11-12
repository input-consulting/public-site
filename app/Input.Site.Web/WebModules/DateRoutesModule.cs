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

        private void SetupDateRoutes()
        {
            // quick fix
            var dateRoutesFromStorage = new List<string>();
            var articles = _articleReader.AllArticleRoutes().ToList();
            var reg = new Regex(@"[/](\d+)[/](\d+)[/](\d+)[/]", RegexOptions.Compiled);

            foreach ( var article in articles) {
                var match = reg.Match(article);
                if (match.Success)
                {
                    var r = reg.Match(article);
                    dateRoutesFromStorage.Add( article.Substring(0, r.Index ) );
                }
            }
            // quick fix

            var routes = _routeLocatorProvider.DateRoutes().Concat(dateRoutesFromStorage).Distinct();
            var roots = routes.Select(r => r.Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries).First()).Distinct();

            foreach (var root in roots)
            {
                var configuredRoute = root;

                var category = string.Format("/{0}", root);
                Get[category] = parameters =>
                {
                    var path = Request.Path.Replace("/", "");
                    PageModel.Meta.Articles = _articleReader.ArticlesByRoute(path);
                    return RenderView(string.Concat("_layout/", Request.Path), PageModel);
                };

                var categoryAndYear = string.Format(@"/{0}/{{year:int}}", root);
                Get[categoryAndYear] = parameters => FetchArticles(configuredRoute, parameters);

                var categoryAndYearAndMonth = string.Format(@"/{0}/{{year:int}}/{{month}}", root);
                Get[categoryAndYearAndMonth] = parameters => FetchArticles(configuredRoute, parameters);

                var categoryAndYearAndMonthAndDay = string.Format(@"/{0}/{{year:int}}/{{month}}/{{day}}", root);
                Get[categoryAndYearAndMonthAndDay] = parameters => FetchArticles(configuredRoute, parameters);
            }
        }


        private Negotiator FetchArticles(string configuredRoute, dynamic parameters)
        {
            var path = ConstructSearchPath(configuredRoute, parameters);
            PageModel.Meta.Articles = _articleReader.ArticlesByRoute(path);
            return RenderView(string.Concat("_layout/", configuredRoute), PageModel);
        }

        private Negotiator RenderView(string view, PageModel model)
        {
            return Negotiate
                .WithView(view)
                .WithModel(model)
                .WithMediaRangeModel("application/json", model);
        }

        private static string ConstructSearchPath(string route, dynamic parameters)
        {
            var path = string.Concat(route, "/", string.Join("/", parameters.Values));
            return path.TrimStart(new[] {'/'});
        }
    }
}