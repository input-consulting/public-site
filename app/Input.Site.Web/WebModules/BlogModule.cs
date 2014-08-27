using System.Linq;
using InputSite.Interfaces;
using Nancy;

namespace InputSite.WebModules
{
    public class BlogModule : BaseModule
    {
        public BlogModule(IArticleReader articleReader) : base("/blog")
        {
            Get["/{firstname}-{surname}"] = parameters =>
            {
                string author = string.Concat(parameters.firstname, " ", parameters.surname);
                PageModel.Meta.Articles = articleReader.ArticlesByAuthor(author).Where( b => b.ResourceName.Contains(@"blog") );
                PageModel.Meta.Author = author;


                return Negotiate
                       .WithView("_layout/bloggare")
                       .WithModel(PageModel)
                       .WithMediaRangeModel("application/json", PageModel);
            };
        }
    }
}