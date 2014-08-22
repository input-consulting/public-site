using System.IO;
using System.Text;
using Nancy;
using Nancy.Responses;
using SquishIt.Framework;

namespace InputSite.WebModules
{
    public class AssetsModule : NancyModule
    {
        public AssetsModule() : base("/assets") {
            Get["/js/{name}"] = request => CreateResponse(Bundle.JavaScript().RenderCached((string)request.name), Configuration.Instance.JavascriptMimeType);
            Get["/css/{name}"] = request => CreateResponse(Bundle.Css().RenderCached((string)request.name), Configuration.Instance.CssMimeType);
        }

        internal Response CreateResponse(string content, string contentType)
        {
            return new StreamResponse(() => new MemoryStream(Encoding.UTF8.GetBytes(content)), contentType)
                        .WithHeader("Cache-Control", "max-age=45");
        }
    }
}