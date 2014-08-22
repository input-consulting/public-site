using FluentAssertions;
using Input.Site.Tests.Helpers;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace Input.Site.Tests.RouteTests
{
    [TestFixture]
    public class RouteTests
    {
        private Browser _browser;

        [SetUp]
        public void Setup()
        {
            var bootsrap = new InputSiteBootstrapTest();
            _browser = new Browser(bootsrap);
        }

        [Test]
        [TestCase("nyheter")]
        [TestCase("blog")]
        public void should_handle_request_for_route(string category)
        {
            var sut = string.Format(@"/{0}", category);

            var result = _browser.Get(sut, with => with.HttpRequest());
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        [TestCase("nyheter")]
        [TestCase("blog")]
        public void should_handle_request_with_year_for_route(string category)
        {
            var sut = string.Format(@"/{0}/2014", category);

            var result = _browser.Get(sut, with => with.HttpRequest());
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        [TestCase("nyheter")]
        [TestCase("blog")]
        public void should_handle_request_with_year_and_month_for_route(string category)
        {
            var sut = string.Format(@"/{0}/2014/01", category);

            var result = _browser.Get(sut, with => with.HttpRequest());
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}