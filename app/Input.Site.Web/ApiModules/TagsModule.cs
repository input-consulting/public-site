using System.Linq;
using InputSite.Interfaces;
using Nancy;
using Nancy.Helpers;

namespace InputSite.ApiModules
{
    public class TagsModule : NancyModule
    {
        private readonly IArticleReader _articleReader;

        public TagsModule(IArticleReader articleReader) : base("/api")
        {
            _articleReader = articleReader;

            Get["/tags"] = o =>
            {
                if (Request.Query.q.HasValue) 
                {
                    string query = HttpUtility.UrlDecode(Request.Query.q);
                    var baked = query.Split(',');
                    var result = _articleReader.ArticlesByTags(baked);

                    if (!result.Any()) return HttpStatusCode.NotFound;

                    return Negotiate
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithModel(result);
                }

                return _articleReader.TagCloud();
            };
        }
    }
}