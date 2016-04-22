using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Input.Site.WebJob
{
    public class SlackClient : IDisposable
    {
        private readonly string channelname = CloudConfigurationManager.GetSetting("slack-channelname");
        private readonly string baseuri = CloudConfigurationManager.GetSetting("slack-baseuri");
        private readonly string token = CloudConfigurationManager.GetSetting("slack-token");
        private readonly string InputSitenId;

        private HttpClient client;

        public SlackClient()
        {
            client = new HttpClient();
            InputSitenId = GetChannelId().Result;
        }
        
        private async Task<string> GetChannelId()
        {
            Uri uri = SlackUri("channels.list", null);
            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                string str = await response.Content.ReadAsStringAsync();
                ChannelInfo cinfo = JsonConvert.DeserializeObject<ChannelInfo>(str);
                if (Convert.ToBoolean(cinfo.ok))
                {
                    var id = from Channel chan in cinfo.channels
                             where chan.name == channelname
                             select chan.id;
                    return id.FirstOrDefault<string>();
                }
                else
                {
                    Console.WriteLine($"Could not get id for Slackchannel {channelname}, wrong auth token?");
                }
            }
            return "";
        }

        public async Task<ChannelHistory> GetSiteHistory(string lastFetchedTimeStamp)
        {
            Uri uri = SlackUri("channels.history", 
                new NameValueCollection { { "channel", InputSitenId },
                                          //{ "latest", "" },
                                            { "oldest" , lastFetchedTimeStamp != string.Empty ? lastFetchedTimeStamp : "" },
                                            { "inclusive" , "0" },
                                            { "count" , "10" },
                                            { "unreads" , "0" }
                                            });
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                string str = await response.Content.ReadAsStringAsync();

                ChannelHistory chistory = JsonConvert.DeserializeObject<ChannelHistory>(str);
                if (Convert.ToBoolean(chistory.ok))
                {
                     return chistory;
                }
            }
            return null;         
        }

        private Uri SlackUri(string method, NameValueCollection parameters)
        {            
            return new Uri(baseuri + method)
                .AttachParameters(new NameValueCollection { { "token", token } })       // always attach token    
                .AttachParameters(parameters);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
