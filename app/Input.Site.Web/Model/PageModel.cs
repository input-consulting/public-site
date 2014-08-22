using System.Dynamic;
using SquishIt.Framework;

namespace InputSite.Model
{
    public class PageModel
    {
        public dynamic Meta { get; private set; }
        public string PublicCss { get; private set; }
        public string PublicJavaScript { get; private set; }

        public PageModel()
        {
            PublicCss = Bundle.Css().RenderCachedAssetTag("public-css");
            PublicJavaScript = Bundle.JavaScript().RenderCachedAssetTag("public-js");
            Meta = new ExpandoObject();
        }

        public PageModel(ArticleModel articleModel) : this()
        {
            Meta.Title = articleModel.Title;
            Meta.SafeTitle = articleModel.SafeTitle;
            Meta.FriendlyDate = articleModel.FriendlyDate;
            Meta.Tags = articleModel.TagsAsHtml;
        }
    }
}