using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using InputSite.Extensions;
using MarkdownSharp;

namespace InputSite.Services
{
    public class ArticleParser
    {
        private readonly string _markdown;
        private const string LINK_TAG_PATTERN = @"<a\b[^>]*>(.*?)</a>";
        private static readonly Regex SSVESubstitution = new Regex("^@[^$]*?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private readonly Markdown _parser = new Markdown();
        private readonly Dictionary<string, string> _metaData = new Dictionary<string, string>();

        public string Category { get; private set; }

        public string Author { get; private set; }

        public string Title { get; private set; }

        public string Abstract { get; private set; }

        public string Body { get; private set; }

        public DateTime BlogDate { get; private set; }

        public string FriendlyDate { get { return BlogDate.ToString("dd MMMM, yyyy"); } }

        public IEnumerable<string> Tags { get; private set; }

        public IEnumerable<string> Roles { get; private set; }

        public string ResourceName { get; private set; }

        public ArticleParser(string markdown, string resourceName)
        {
            _markdown = markdown;
            var metadata = _markdown.Contains("@Tags") ? markdown.Substring(markdown.IndexOf("@Tags", StringComparison.Ordinal) + 5,
				                                        markdown.IndexOf("@EndTags", StringComparison.Ordinal) - 7)
			                                            : string.Empty;

			var metadataSplit = metadata.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var item in metadataSplit)
			{
				var itemdata = item.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
				if (itemdata.Length < 2) continue;
				_metaData.Add(itemdata[0].Trim(), itemdata[1].Trim());
			}

            Author = GetAuthor();
			BlogDate = GetBlogDate();
			Title = GetTitle();
			Abstract = GetAbstract();
			Tags = GetTags();
            Roles = GetRoles();
            Body = GetBody();
            ResourceName = resourceName;
		}

        private string GetBody()
        {
            var content = _markdown;
            if (string.IsNullOrWhiteSpace(content)) return string.Empty;

            if (content.Contains("@Tags"))
            {
                content = content.Substring(content.IndexOf("@EndTags", StringComparison.Ordinal) + 8);
            }

            return SSVESubstitution.Replace(content, "").Trim();                
        }

        private IEnumerable<string> GetTags()
		{
			if (_metaData.All(x => x.Key != "Tags")) return Enumerable.Empty<string>();

			var csv = _metaData.FirstOrDefault(x => x.Key == "Tags").Value;
			return csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
		}

        private IEnumerable<string> GetRoles()
        {
            if (_metaData.All(x => x.Key != "Roles")) return Enumerable.Empty<string>();

            var csv = _metaData.FirstOrDefault(x => x.Key == "Roles").Value;
            return csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
        }

        private string GetAuthor()
        {
            if (_metaData.All(x => x.Key != "Author")) return string.Empty;

            return _metaData.FirstOrDefault(x => x.Key == "Author").Value;
        }


		private DateTime GetBlogDate()
		{
			if (_metaData.All(x => x.Key != "Date")) return DateTime.MinValue;

			var kvp = _metaData.FirstOrDefault(x => x.Key == "Date");
            var blogDateTime = DateTime.ParseExact(kvp.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);

			return blogDateTime;
		}

		private string GetTitle()
		{
			if (_metaData.All(x => x.Key != "Title")) return string.Empty;

			return _metaData.FirstOrDefault(x => x.Key == "Title").Value;
		}


        private string TransformContentToHtml(string content)
        {
            var html = _parser.Transform(content);
            return html;            
        }

		private string GetAbstract()
		{
		    var content = _markdown;
            if (string.IsNullOrWhiteSpace(content)) return string.Empty;

            if (content.Contains("@Tags"))
            {
                content = content.Substring(content.IndexOf("@EndTags", StringComparison.Ordinal) + 8);
            }

            content = SSVESubstitution.Replace(content, "").Trim();
		    content = content.TruncateOnWordBoundary(200, " ...");

		    return TransformContentToHtml(content);
		}

    }
}