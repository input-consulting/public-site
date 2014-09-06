using System.Collections.Generic;
using Nancy;
using SquishIt.Framework;
using SquishIt.Framework.Base;
using SquishIt.Framework.CSS;
using SquishIt.Framework.JavaScript;

namespace InputSite.Bootstrap
{
    public class BundlesStartup
    {
        protected static string BasePathForTesting = "";


        public static void Setup(IRootPathProvider rootPathProvider = null)
        {
            BasePathForTesting = rootPathProvider != null ? rootPathProvider.GetRootPath() : string.Empty;

            // CSS
            BuildCssBundle(Bundles.PublicCss).ForceRelease().AsCached("public-css", "/assets/css/public-css");
            BuildCssBundle(Bundles.PublicCss).ForceDebug().AsNamed("public-css-debug", "");

            // JS
            BuildJavaScriptBundle(Bundles.PublicJavaScript).ForceRelease().AsCached("public-js", "/assets/js/public-js");
            BuildJavaScriptBundle(Bundles.PublicJavaScript).ForceDebug().AsNamed("public-js-debug", "");
        }


        protected static JavaScriptBundle BuildJavaScriptBundle(List<BundleFile> files)
        {
            var bundle = Bundle.JavaScript();

            CreateBundle(files, bundle);

            return bundle;
        }

        protected static CSSBundle BuildCssBundle(List<BundleFile> files)
        {
            var bundle = Bundle.Css();

            CreateBundle(files, bundle);

            return bundle;
        }

        private static void CreateBundle<T>(IEnumerable<BundleFile> files, BundleBase<T> bundle) where T : BundleBase<T>
        {
            foreach (var item in files)
            {
                var url = item.Url;

                if (!string.IsNullOrWhiteSpace(BasePathForTesting))
                {
                    url = BasePathForTesting + item.Url.Replace("~", "");
                }

                if (item.Minify)
                {
                    bundle.Add(url);
                }
                else
                {
                    bundle.AddMinified(url);
                }
            }
        }
    }
}