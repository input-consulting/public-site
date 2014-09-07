using System.Collections.Generic;

namespace InputSite.Bootstrap
{

    public class BundleFile
    {
        public string Url { get; set; }
        public bool Minify { get; set; }
    }

    public static class Bundles
    {
        public static List<BundleFile> PublicJavaScript = new List<BundleFile>  
        {
            new BundleFile { Url = "~/assets/js/jquery.js", Minify = true },
            new BundleFile { Url = "~/assets/js/jquery.sidr.min.js", Minify = true },
            new BundleFile { Url = "~/assets/js/input-common-init.js", Minify = true },
            new BundleFile { Url = "~/assets/js/articles.js", Minify = true },
        };

        public static List<BundleFile> PublicCss = new List<BundleFile>
        {
            new BundleFile{Url = "~/assets/css/jquery.sidr.dark.css", Minify = true},
            new BundleFile { Url = "~/assets/css/fonts.css", Minify = true },
            new BundleFile { Url = "~/assets/css/normalize.css", Minify = true },
            new BundleFile { Url = "~/assets/css/common.css", Minify = true },
            new BundleFile { Url = "~/assets/css/menu.css", Minify = true },
            new BundleFile { Url = "~/assets/css/mobile-menu.css", Minify = true },
            new BundleFile { Url = "~/assets/css/footer.css", Minify = true },
            new BundleFile { Url = "~/assets/css/nyheter.css", Minify = true },
            new BundleFile { Url = "~/assets/css/career.css", Minify = true },
            new BundleFile { Url = "~/assets/css/search.css", Minify = true },
            new BundleFile { Url = "~/assets/css/contact.css", Minify = true },
            new BundleFile { Url = "~/assets/css/konsulter.css", Minify = true },
            new BundleFile { Url = "~/assets/css/article.css", Minify = true },
            new BundleFile { Url = "~/assets/css/blog.css", Minify = true },
        };

    }
}