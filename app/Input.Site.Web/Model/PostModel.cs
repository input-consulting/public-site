using System;
using System.Collections.Generic;

namespace InputSite.Model
{
    public class PostModel
    {
        public string Author { get; private set; }

        public string Title { get; private set; }

        public string Category { get; private set; }

        public string Abstract { get; private set; }

        public DateTime PostDate { get; private set; }

        public string FriendlyDate { get; private set; }

        public IEnumerable<string> Tags { get; private set; }

        public string ResourceName { get; private set; }

    }
}