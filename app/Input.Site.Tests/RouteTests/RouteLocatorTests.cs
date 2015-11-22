using System.Linq;
using FluentAssertions;
using Input.Site.Tests.Helpers;
using InputSite.Bootstrap;
using Nancy;
using NUnit.Framework;

namespace Input.Site.Tests.RouteTests
{
    [TestFixture]
    public class RouteLocatorTests
    {
        private IRouteLocatorProvider _sut;
        private IRootPathProvider _rootPathProvider;

        [SetUp]
        public void Setup()
        {
            _rootPathProvider = new TestRootPathProvider();
            _sut = new RouteLocatorProvider(_rootPathProvider);
        }

        [Test]
        public void should_find_static_routes()
        {
            var staticRoutes = _sut.StaticRoutes();
            staticRoutes.Count().Should().BeGreaterThan(0);
        }

    }
}