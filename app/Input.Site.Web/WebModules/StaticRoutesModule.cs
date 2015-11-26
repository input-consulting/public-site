using InputSite.Extensions;
using InputSite.Interfaces;
using InputSite.Model;
using Nancy;
using Nancy.Responses.Negotiation;
using System.Linq;
using Nancy.Helpers;

namespace InputSite.WebModules
{
    public class StaticRoutesModule : BaseModule
    {
        private readonly IArticleReader _articleReader;

        public StaticRoutesModule(IArticleReader articleReader)
        {
            _articleReader = articleReader;

            var articles = _articleReader.AllArticleRoutes();
            articles.Select(r => $"/{r}").Map(route => Get[route] = parameters => FetchArticle(Request.Path.TrimStart('/')));
        }


        private Negotiator FetchArticle(string routeToArticle)
        {
            var article = _articleReader.ArticlesByRoute(routeToArticle).FirstOrDefault();
            if (article == null) return Negotiate.WithStatusCode(404);

            var model = new PageModel(article);

            model.Meta.ResourceName = $"{Request.Url.SiteBase}/{model.Meta.ResourceName}";
            model.Meta.SafeTitle = HttpUtility.UrlEncode(model.Meta.Title);

            return Negotiate
                .WithView(article.AbsolutePath)
                .WithModel(model)
                .WithMediaRangeModel("application/json", model);
        }

    }
}