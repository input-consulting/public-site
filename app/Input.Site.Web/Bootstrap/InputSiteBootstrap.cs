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

            BundlesStartup.Setup();
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("static", "static"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("static", "bin/static"));

            nancyConventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("bin/views/", viewName));
            nancyConventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("bin/views/", context.ModuleName, "/", viewName));

            nancyConventions.ViewLocationConventions.Add((viewName, model, context) => {
                var path = context.ModulePath.TrimStart(new[] { '/' });
                return string.Concat("bin/views/", path, "/", viewName);
            });

        }
    }
}