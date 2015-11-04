using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.ModelBinding;

namespace InputSite.Model
{
    public class ArticleModel
    {
        public string Image { get; private set; }
        public string BgImage { get; private set; }

        public string Body { get; private set; }

        public string Category { get; private set; }

        public string Author { get; private set; }

		public string Title { get; private set; }

        public string SafeTitle
        {
            get
            {
                var title = Title ?? string.Empty;
                if (string.IsNullOrEmpty(title)) return string.Empty;

                title = title.Replace(@" ", @"-");
                title = title.Replace(@"å", @"a");
                title = title.Replace(@"ä", @"a");
                title = title.Replace(@"ö", @"o");
                title = title.Replace(@"é", @"e");

                return title.ToLower();
            } 
        }

        public string Abstract { get; private set; }

		public DateTime BlogDate { get; private set; }

		public string FriendlyDate { get { return BlogDate.ToString("dd MMMM, yyyy"); } }

        public IEnumerable<string> Tags { get; private set; }

        public string TagsAsHtml
        {
            get
            {
                /* The only reason for this one, is that Nancy does not support nested @Each in SSVE yet */
                var res = string.Join("", Tags.Select(s => string.Format("<a href=\"/search?q={0}\">{1}</a>", s, s)));
                return res;
            }
        } 

        public IEnumerable<string> Roles { get; private set; }

        /* This one could probably be named someting more clever */
        public string RolesAsHtml
        {
            get
            {
                /* The only reason for this one, is that Nancy does not support nested @Each in SSVE yet */
                var res = string.Join("", Roles.Select(s => string.Format("<a href=\"/search?q={0}\">{1}</a>", s, s)));
                return res;
            }
        } 

        public string ResourceName { get; private set; }
        public string AbsolutePath { get; private set; }

        public ArticleModel(string category, string author, string title, string ingress, IEnumerable<string> tags, IEnumerable<string> roles, string resourceName, DateTime date, string body, string image, string bgImage, string absolutePath)
        {
            Image = image;
            BgImage = bgImage;
            Body = body;
            Category = category;
            Author = author;
            Title = title;
            Abstract = ingress;
            Tags = tags;
            Roles = roles;
            ResourceName = resourceName;
            BlogDate = date;
            AbsolutePath = absolutePath;
        }

    }
}