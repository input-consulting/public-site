using System.Dynamic;

namespace InputSite.Model
{
    public class PageModel
    {
        public dynamic Meta { get; private set; }

        public PageModel()
        {
            Meta = new ExpandoObject();
        }

        public PageModel(ArticleModel articleModel) : this()
        {
            Meta.Title = articleModel.Title;
            Meta.SafeTitle = articleModel.SafeTitle;
            Meta.FriendlyDate = articleModel.FriendlyDate;
            Meta.Author = articleModel.Author;
            Meta.Tags = articleModel.TagsAsHtml;
        }
    }
}