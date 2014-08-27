using System.Collections.Generic;
using InputSite.Interfaces;
using InputSite.Model;
using Nancy;
using Nancy.Helpers;

namespace InputSite.WebModules
{
    public class SearchModule : BaseModule
    {

        public SearchModule(IArticleReader articleReader)
        {
            Get["/search"] = o =>
            {
                PageModel.Meta.QueryResult = default(IEnumerable<ArticleModel>);
                PageModel.Meta.Query = string.Empty;

                if (Request.Query.q.HasValue && !string.IsNullOrEmpty(Request.Query.q))
                {
                    string query = HttpUtility.UrlDecode(Request.Query.q);
                    PageModel.Meta.QueryResult = articleReader.ArticlesByFreeText(query);
                    PageModel.Meta.Query = query;                        
                }

                PageModel.Meta.TagCloud = articleReader.TagCloud();
               
                return Negotiate
                       .WithView("_layout/search")
                       .WithModel(PageModel)
                       .WithMediaRangeModel("application/json", PageModel);

            };

        }
    }
}