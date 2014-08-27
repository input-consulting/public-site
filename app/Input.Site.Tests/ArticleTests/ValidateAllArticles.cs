using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
        public void should_parse_markdown(string article)
        {
            Action parseAction = () =>
            {
                using (var sr = new StreamReader(article))
                {
                    var markdown = sr.ReadToEnd();
                    var resource = _articleLocator.PathToResource(article);
                    new ArticleParser(markdown, resource);
                }
            };

            parseAction.ShouldNotThrow();
        }

        [Test, TestCaseSource(typeof(ValidateAllArticles), "TestCases")]
        public void should_have_matching_sections(string article)
        {
            using (var sr = new StreamReader(article))
            {
                var articleSource = sr.ReadToEnd();
                var beginSection = new Regex(Regex.Escape("@Section")).Matches(articleSource).Count;
                var endSection = new Regex(Regex.Escape("@EndSection")).Matches(articleSource).Count;

                beginSection.ShouldBeEquivalentTo(endSection);
            }
        }

        [Test, TestCaseSource(typeof(ValidateAllArticles), "TestCases")]
        public void should_have_title(string article)
        {
            using (var sr = new StreamReader(article))
            {
                var markdown = sr.ReadToEnd();
                if (markdown.Contains("@Tags"))
                {
                    var resource = _articleLocator.PathToResource(article);
                    var parsedArticle = new ArticleParser(markdown, resource);
                    parsedArticle.Title.Should().NotBeNullOrEmpty();
                }
            }
        }

        [Test, TestCaseSource(typeof(ValidateAllArticles), "TestCases")]
        public void should_have_date(string article)
        {
            using (var sr = new StreamReader(article))
            {
                var markdown = sr.ReadToEnd();
                if (markdown.Contains("@Tags"))
                {
                    var resource = _articleLocator.PathToResource(article);
                    var parsedArticle = new ArticleParser(markdown, resource);
                    parsedArticle.BlogDate.Should().NotBe(DateTime.MinValue);                   
                }
            }
        }

        [Test, TestCaseSource(typeof(ValidateAllArticles), "TestCases")]
        public void should_have_matching_tags_if_present(string article)
        {
            using (var sr = new StreamReader(article))
            {
                var articleSource = sr.ReadToEnd();
                var beginTags = new Regex(Regex.Escape("@Tags")).Matches(articleSource).Count;
                var endTags = new Regex(Regex.Escape("@EndTags")).Matches(articleSource).Count;

                beginTags.ShouldBeEquivalentTo(endTags);
            }
        }

    }
}