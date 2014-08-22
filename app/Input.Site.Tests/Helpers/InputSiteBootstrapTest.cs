using System;
using System.IO;
using System.Linq;
using InputSite;
using InputSite.Bootstrap;
using InputSite.Interfaces;
using InputSite.Services;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace Input.Site.Tests.Helpers
{

    public class TestRootPathProvider : IRootPathProvider
    {
        private static string _rootPath;

        public string GetRootPath()
        {
            if (!string.IsNullOrEmpty(_rootPath)) return _rootPath;

            var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            var rootPathFound = false;
            while (!rootPathFound)
            {
                var directoriesContainingViewFolder = currentDirectory.GetDirectories("Views", SearchOption.AllDirectories);
                if (directoriesContainingViewFolder.Any())
                {
                    _rootPath = directoriesContainingViewFolder.First().Parent.FullName;
                    rootPathFound = true;
                }

                currentDirectory = currentDirectory.Parent;
            }

            return _rootPath;
        }
    }

    public class InputSiteBootstrapTest : DefaultNancyBootstrapper
    {

        protected override IRootPathProvider RootPathProvider
        {
            get { return new TestRootPathProvider(); }
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            StaticConfiguration.DisableErrorTraces = false;

            base.ApplicationStartup(container, pipelines);

            BundlesStartup.Setup(RootPathProvider);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<IArticleReader, ArticleReader>();
            container.Register<IArticleStorage, ArticleStorage>();
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("content", "bin/content"));

            nancyConventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("bin/views/", viewName));
            nancyConventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("bin/views/", context.ModuleName, "/", viewName));

            nancyConventions.ViewLocationConventions.Add((viewName, model, context) =>
            {
                var path = context.ModulePath.TrimStart(new[] { '/' });
                return string.Concat("bin/views/", path, "/", viewName);
            });

        }
    }
}