namespace InputSite.Services
{
    public class ArticleLocation
    {
        public string RouteName { get; private set; }
        public string FileName { get; private set; }

        public ArticleLocation(string routeName, string fileName)
        {
            RouteName = routeName;
            FileName = fileName;
        }
    }
}