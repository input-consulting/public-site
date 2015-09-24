using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses;
using Nancy.ViewEngines;

namespace InputSite.Handlers
{
    public class PageNotFoundHandler : DefaultViewRenderer, IStatusCodeHandler
    {
        public PageNotFoundHandler(IViewFactory factory) : base(factory)
        {
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            if (context.Response is JsonResponse)
            {
                context.Response = new TextResponse("Resource not found")
                {
                    StatusCode = statusCode,
                    ContentType = "application/json"
                };
            }
            else
            {
                var response = RenderView(context, "_layout/PageNotFound");
                response.StatusCode = HttpStatusCode.NotFound;
                context.Response = response;
            }
        }

    }
}