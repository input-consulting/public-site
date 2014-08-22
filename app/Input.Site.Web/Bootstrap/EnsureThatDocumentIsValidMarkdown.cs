using System.IO;

namespace InputSite.Bootstrap
{
    public class EnsureThatDocumentIsValidMarkdown : IRouteEvaluator
    {
        public bool Ensure(FileInfo fileInfo)
        {
            using (var f = new StreamReader(fileInfo.FullName))
            {
                var content = f.ReadToEnd();
                return content.Contains(@"@Master");
            }
        }
    }
}