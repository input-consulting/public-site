using System.Linq;
using InputSite.Extensions;
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
                PageModel.Meta.ServicesArticle = articleReader.ArticleById("services");
                PageModel.Meta.SalesArticle = articleReader.ArticleById("sales");
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