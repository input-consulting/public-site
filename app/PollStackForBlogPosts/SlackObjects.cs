using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Input.Site.WebJob
{

    public enum MessageSubType
    {        
        bot_message, me_message, message_changed, message_deleted, channel_join, channel_leave, channel_topic, channel_purpose, channel_name,
        channel_archive, channel_unarchive, group_join, group_leave, group_topic, group_purpose, group_name, group_archive, group_unarchive,
        file_share, file_comment, file_mention, pinned_item, unpinned_item
    }
    public class ChannelInfo
    {
        public string ok { get; set; }
        public List<Channel> channels { get; set; }
    }
    public class ChannelHistory
    {
        public string ok { get; set; }
        public Message latest { get; set; }
        public List<Message> messages { get; set; }
        public string has_more { get; set; }
    }

    public class Channel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string is_channel { get; set; }
        public string created { get; set; }
        public string creator { get; set; }
        public string is_archived { get; set; }
        public string is_general { get; set; }
        public string is_member { get; set; }
        public List<string> members { get; set; }
        public Topic topic { get; set; }
        public Purpose purpose { get; set; }
        public int num_member { get; set; }
    }

    public class Purpose
    {
        public string value { get; set; }
        public string creator { get; set; }
        public string last_set { get; set; }
    }

    public class Topic
    {
        public string value { get; set; }
        public string creator { get; set; }
        public string last_set { get; set; }
    }

    public class Message
    {
        public string type { get; set; }
        public string channel { get; set; }
        public string user { get; set; }
        public string text { get; set; }
        public string ts { get; set; }
        public string subtype { get; set; }
    }
    
}

