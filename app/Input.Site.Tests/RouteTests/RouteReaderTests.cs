using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Input.Site.Tests.Helpers;
using InputSite.Bootstrap;
using NUnit.Framework;

namespace Input.Site.Tests.RouteTests
{
    [TestFixture]
    public class RouteReaderTests
    {
        private RouteReader _sut;
        private string _rootPath;

        [SetUp]
        public void Setup()
        {
            var blackList = new List<string> { @"views"};
            _rootPath = new TestRootPathProvider().GetRootPath() + "\\Views";

            var staticEvaluators = new List<IRouteEvaluator>
            {
                new EnsureThatDocumentIsValidMarkdown(),
                new EnsureThatRouteContainMarkupDocument(),
                new EnsureThatRouteDoesNotContainNumbers(),
                new EnsureThatRouteIsNotBlackListed(blackList)
            };
            
            _sut = new RouteReader(staticEvaluators);
        }

        [Test]
        public void should_find_staticly_bindable_routes()
        {
            var routes = _sut.Routes(_rootPath);
            routes.Count().Should().BeGreaterThan(0);
        }
    }
}