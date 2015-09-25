using System;
using System.Linq;
using InputSite.Bootstrap;
using InputSite.Extensions;
using InputSite.Interfaces;
using InputSite.Model;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.ViewEngines;

namespace InputSite.WebModules
{
    public class DefaultModule : BaseModule
    {
        private readonly IArticleReader _articleReader;
        private readonly IRouteLocatorProvider _routeLocatorProvider;
        private readonly IViewLocationProvider _viewLocationProvider;

        public DefaultModule(IArticleReader articleReader, IRouteLocatorProvider routeLocatorProvider, IViewLocationProvider viewLocationProvider)
        {
            _articleReader = articleReader;
            _routeLocatorProvider = routeLocatorProvider;
            _viewLocationProvider = viewLocationProvider;

            SetupDateRoutes();
            SetupStaticRoutes();
        }

        private void SetupStaticRoutes()
        {
            var routes = _routeLocatorProvider.StaticRoutes();
            routes.Map(route => Get[route + "/{article}"] = parameters => FetchArticle(Request.Path.TrimStart('/')));
            
            Get["/{article}"] = parameters =>
            {
                /* Need to know if it is a article or not */
                var isArticle = _viewLocationProvider.GetLocatedViews(new[] {"md", "markdown"}).Count(w => w.Name.ToLower().Contains(parameters.article)) == 1;
                if (isArticle) {
                    var article = _articleReader.ArticlesByCategory((string)parameters.article).FirstOrDefault();
                    if (article == null) return 404;
                    return RenderView(article.ResourceName, new PageModel(article));
                }

                /* TODO : This route setup interfears with the root route in date routes, need to sort it, its anoying */
                var path = Request.Path.Replace("/", "");
                PageModel.Meta.Articles = _articleReader.ArticlesByCategory(path);
                return RenderView(string.Concat("_layout/", Request.Path), PageModel);
            };
        }

        private void SetupDateRoutes()
        {
            var routes = _routeLocatorProvider.DateRoutes();
            var roots = routes.Select(r => r.Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries).First()).Distinct();

            foreach (var root in roots)
            {
                var configuredRoute = root;

                var category = string.Format("/{0}", root);
                Get[category] = parameters =>
                {
                    var path = Request.Path.Replace("/", "");
                    PageModel.Meta.Articles = _articleReader.ArticlesByCategory(path);
                    return RenderView(string.Concat("_layout/", Request.Path), PageModel);
                };

                var categoryAndYear = string.Format(@"/{0}/{{year:int}}", root);
                Get[categoryAndYear] = parameters => FetchArticles(configuredRoute, parameters);

                var categoryAndYearAndMonth = string.Format(@"/{0}/{{year:int}}/{{month}}", root);
                Get[categoryAndYearAndMonth] = parameters => FetchArticles(configuredRoute, parameters);

                var categoryAndYearAndMonthAndArticle = string.Format(@"/{0}/{{year:int}}/{{month}}/{{article}}", root);
                Get[categoryAndYearAndMonthAndArticle] = parameters =>
                {
                    var path = ConstructSearchPath(configuredRoute, parameters);
                    return FetchArticle(path);
                };
            }
        }

        private Negotiator FetchArticle(string routeToArticle)
        {
            var article = _articleReader.ArticlesByCategory(routeToArticle).FirstOrDefault();
            if (article == null) return Negotiate.WithStatusCode(404);
            return RenderView(article.ResourceName, new PageModel(article));
        }

        private Negotiator FetchArticles(string configuredRoute, dynamic parameters)
        {
            var path = ConstructSearchPath(configuredRoute, parameters);
            PageModel.Meta.Articles = _articleReader.ArticlesByCategory(path);
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