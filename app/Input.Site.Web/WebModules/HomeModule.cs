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
                PageModel.Meta.Articles = articleReader.ArticlesByCategory(@"nyheter").Take(3);
			    PageModel.Meta.Headline = articleReader.ArticlesByCategory(@"headline").Random().FirstOrDefault();
			    PageModel.Meta.Inputtare = articleReader.ArticlesByCategory(@"inputtare").Random().Take(4);

			    var articles = articleReader.ArticlesByCategory(@"artiklar").Random().Take(4).ToList();
			    PageModel.Meta.ArticlesOne = articles.Take(2);
                PageModel.Meta.ArticlesTwo = articles.Skip(2).Take(2);

                return Negotiate
                       .WithView("_layout/home")
                       .WithModel(PageModel)
                       .WithMediaRangeModel("application/json", PageModel);
			};
        }
    }
}