using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Input.Site.Tests.Helpers;
using InputSite.Interfaces;
using InputSite.Services;
using NUnit.Framework;

namespace Input.Site.Tests.ArticleTests
{
    [TestFixture]
    public class ValidateAllArticles
    {
        private readonly IArticleLocator _articleLocator = new ArticleLocator(new TestRootPathProvider());

        public IEnumerable<string> TestCases()
        {            
            return _articleLocator.Articles();
        }

        [Test, TestCaseSource(typeof(ValidateAllArticles), "TestCases")]
        public void should_parse(string article)
        {
            using (var sr = new StreamReader(article))
            {
                var markdown = sr.ReadToEnd();
                var resource = _articleLocator.PathToResource(article);
                var parsedArticle = new ArticleParser(markdown, resource);
            }
        }
    }
}