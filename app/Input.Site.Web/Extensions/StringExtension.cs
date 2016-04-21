namespace InputSite.Extensions
{
    public static class StringExtension
    {
        private const string Ellipsis = "...";

        public static string TruncateOnWordBoundary(this string content, int maxLength)
        {
            return content.TruncateOnWordBoundary(maxLength, Ellipsis);
        }

        public static string TruncateOnWordBoundary(this string content, int maxLength, string suffix)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;

            if (content.Length <= maxLength) return content;

            var ii = maxLength;
            while (ii > 0)
            {
                if (char.IsWhiteSpace(content[ii])) break;
                ii--;
            }

            if (ii <= 0) return (suffix ?? Ellipsis);
            return content.Substring(0, ii) + (suffix ?? Ellipsis);
        }
    }
}