using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses;

namespace InputSite.Handlers
{
    public class NotFoundHandler : IStatusCodeHandler
    {
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
                context.Response = new TextResponse("<h1>Oh noo !!<h1/>")
                {
                    StatusCode = statusCode,
                    ContentType = "text/html"
                };

            }
        }
    }
}