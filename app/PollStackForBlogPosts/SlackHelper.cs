using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Input.Site.WebJob
{
    public static class SlackHelper
    {
        public static Uri AttachParameters(this Uri uri, NameValueCollection parameters)
        {
            if (parameters == null)
                return uri;
            var stringBuilder = new StringBuilder();
            string str = uri.ToString().Contains("?") ? "&" : "?";
            for (int index = 0; index < parameters.Count; ++index)
            {
                stringBuilder.Append(str + parameters.AllKeys[index] + "=" + parameters[index]);
                str = "&";
            }
            return new Uri(uri + stringBuilder.ToString());
        }

        public static DateTime? SlackTimeStampToDateTime(string ts)
        {
            string[] splitstamp = ts.Split('.');
            if (splitstamp.Length > 2)
                return null;
            double timestamp = Convert.ToDouble(splitstamp[0]);
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime? UnixTimeStampToDateTime(int ts)
        {

            double timestamp = Convert.ToDouble(ts);
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
