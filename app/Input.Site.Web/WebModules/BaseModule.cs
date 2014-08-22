using InputSite.Model;
using Nancy;

namespace InputSite.WebModules
{
    public class BaseModule : NancyModule {

        public PageModel PageModel { get; private set; }

        public BaseModule(string route) : base(route)
        {
            PageModel = new PageModel();
        }

        public BaseModule()
        {
            PageModel = new PageModel();
        }

    }
}