using System;
using FluentAssertions;
using Input.Site.Tests.Helpers;
using InputSite.Model;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Testing;
using Nancy.TinyIoc;
using NUnit.Framework;

namespace Input.Site.Tests.ModuleTests
{
    [TestFixture]
    public class HomeModuleTests
    {

        private Browser _browser;

        [SetUp]
        public void Setup()
        {
            var bootsrap = new InputSiteBootstrapTest();
            _browser = new Browser(bootsrap);
        }

        [Test]
        public void should_respond_to_a_get_on_root()
        {
            var result = _browser.Get("/", with => with.HttpRequest());
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void should_have_rendered_all_content()
        {
            var result = _browser.Get("/", with => with.HttpRequest());
            result.Body.Should().NotContain("[ERR!]");
        }

        [Test]
        [Ignore("Not really there yet")]
        public void should_have_a_correct_model()
        {
            var result = _browser.Get("/", with => with.HttpRequest());
            var model = result.GetModel<PageModel>();

            model.Meta.Articles.Count.Should().BeGeaterThan(0);
        }

    }
}