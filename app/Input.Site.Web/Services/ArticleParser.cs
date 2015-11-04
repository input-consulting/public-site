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
        public string AbsoluteResourceName { get; private set; }
        public string Id { get; private set; }

        public string Image { get; private set; }
        public string BgImage { get; private set; }

        private const string Meta = "@Meta";
        private const string EndMeta = "@EndMeta";

        public ArticleParser(string markdown, string resourceName)
        {
            _markdown = markdown;

            var metadata = string.Empty;
            if (_markdown.Contains(Meta))
            {
                metadata = Regex.Match(_markdown, @"@Meta(.*?)@EndMeta", RegexOptions.Singleline).Groups[1].Value;
            } 

			var metadataSplit = metadata.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var item in metadataSplit)
			{
				var itemdata = item.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
				if (itemdata.Length < 2) continue;

			    var key = itemdata[0].Trim();
			    var value = itemdata[1].Trim();
			    if (itemdata.Length > 2)
                    value = string.Join(":", itemdata.Skip(1));

				_metaData.Add(key, value);
			}

            Id = GetMetaValue("Id");
            Author = GetMetaValue("Author");
			BlogDate = GetBlogDate();
            Title = GetMetaValue("Title");
			Abstract = GetAbstract();
			Tags = GetTags();
            Roles = GetRoles();
            Body = GetBody();
            Image = GetMetaValue("Image");
            BgImage = GetMetaValue("BgImage");

            AbsoluteResourceName = resourceName;
            ResourceName = resourceName;

            var reg = new Regex(@"(\d+)[-](\d+)[-](\d+)[-]", RegexOptions.Compiled);
            if (reg.IsMatch(resourceName))
            {                
                var result = reg.Replace(resourceName, m => m.Groups[0].Value.Replace("-", "/"));
                ResourceName = result;
                AbsoluteResourceName = resourceName;
            }
            
		}

        private string GetMetaValue(string key)
        {
            if (_metaData.All(x => x.Key != key)) return string.Empty;
            return _metaData.FirstOrDefault(x => x.Key == key).Value.Trim();
        }

        private string GetBody()
        {
            var content = _markdown;
            if (string.IsNullOrWhiteSpace(content)) return string.Empty;

            if (content.Contains(Meta))
            {
                content = content.Substring(content.IndexOf(EndMeta, StringComparison.Ordinal) + 8);
            }

            content = SSVESubstitution.Replace(content, "").Trim();
            return TransformContentToHtml(content);
        }

        private IEnumerable<string> GetTags()
        {
            var csv = GetMetaValue("Meta");
			return csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
		}

        private IEnumerable<string> GetRoles()
        {
            var csv = GetMetaValue("Roles");
            return csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
        }

		private DateTime GetBlogDate()
		{
		    var date = GetMetaValue("Date");
			if (string.IsNullOrEmpty(date)) return DateTime.MinValue;

		    DateTime blogDateTime;
		    DateTime.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out blogDateTime);

			return blogDateTime;
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

            if (content.Contains(Meta))
            {
                content = content.Substring(content.IndexOf(EndMeta, StringComparison.Ordinal) + 8);
            }

            content = SSVESubstitution.Replace(content, "").Trim();
		    content = content.TruncateOnWordBoundary(200, " ...");

		    return TransformContentToHtml(content);
		}

    }
}