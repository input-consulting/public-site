using Nancy;
using Nancy.ViewEngines;

namespace Input.Site.Tests.Helpers
{
    public class TestViewFactory : IViewFactory
    {
        private readonly DefaultViewFactory _defaultViewFactory;

        public TestViewFactory(DefaultViewFactory defaultViewFactory)
        {
            _defaultViewFactory = defaultViewFactory;
        }

        public Response RenderView(string viewName, dynamic model, ViewLocationContext viewLocationContext)
        {
            // Intercept model
            viewLocationContext.Context.Items["###ViewModel###"] = model;

            return _defaultViewFactory.RenderView(viewName, model, viewLocationContext);
        }
    }
}