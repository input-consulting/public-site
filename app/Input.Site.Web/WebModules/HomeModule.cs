using System.Linq;
using InputSite.Interfaces;
using Nancy;

namespace InputSite.WebModules
{
    public class HomeModule : BaseModule
    {
        public HomeModule(IArticleReader articleReader)
        {
			Get["/"] = parameters =>
			{
                PageModel.Meta.HomeArticle = articleReader.ArticleById("home");
                PageModel.Meta.AboutArticle = articleReader.ArticleById("about");
                PageModel.Meta.CultureArticle = articleReader.ArticleById("culture");
			    PageModel.Meta.CultureArticles = articleReader.ArticlesByCategory("culture").Where( a => a.ResourceName.Count( r => r == '/' ) > 0);
                PageModel.Meta.ServicesArticle = articleReader.ArticleById("services");
                PageModel.Meta.ServicesArticles = articleReader.ArticlesByCategory("services").Where( a => a.ResourceName.Count( r => r == '/' ) > 0);
                PageModel.Meta.SalesArticle = articleReader.ArticleById("sales");
                PageModel.Meta.SalesArticles = articleReader.ArticlesByCategory("sales").Where(a => a.ResourceName.Count(r => r == '/') > 0);
                PageModel.Meta.ContactArticle = articleReader.ArticleById("contact");
                PageModel.Meta.CrewArticle = articleReader.ArticleById("crew");

                return Negotiate
                       .WithView("_layout/home")
                       .WithModel(PageModel)
                       .WithMediaRangeModel("application/json", PageModel);
			};
        }
    }
}