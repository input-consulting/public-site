using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace InputSite.Bootstrap
{
    public class InputSiteBootstrap : DefaultNancyBootstrapper
    {        
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            StaticConfiguration.DisableErrorTraces = false;

            base.ApplicationStartup(container, pipelines);        
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
            });

            pipelines.OnError.AddItemToEndOfPipeline((ctx, exc) =>
            {
                if (exc is Nancy.ViewEngines.ViewNotFoundException)
                {
                    return HttpStatusCode.NotFound;
                }

                return HttpStatusCode.InternalServerError;
            });

            base.RequestStartup(container, pipelines, context);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("static", "static"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("static", "bin/static"));

            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("assets", "assets"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("assets", "bin/assets"));

            nancyConventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("bin/views/", viewName));
            nancyConventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("bin/views/", context.ModuleName, "/", viewName));

            nancyConventions.ViewLocationConventions.Add((viewName, model, context) => {
                var path = context.ModulePath.TrimStart(new[] { '/' });
                return string.Concat("bin/views/", path, "/", viewName);
            });

        }
    }
}