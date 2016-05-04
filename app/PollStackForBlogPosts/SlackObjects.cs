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

    public class FileList
    {
        public string ok { get; set; }
        public List<File> files { get; set; }
        public Paging paging { get; set; }
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

    public class Paging
    {
        public int count { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
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

    public class FileResponse
    {
        public bool ok { get; set; }
        public File file { get; set; }
        public string content_html { get; set; }
        public object comments { get; set; }
        public object paging { get; set; }
    }

    public class File
    {
        public string id { get; set; }
        public int created { get; set; }
        public int timestamp { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string mimetype { get; set; }
        public string filetype { get; set; }
        public string pretty_type { get; set; }
        public string user { get; set; }
        public string mode { get; set; }
        public bool editable { get; set; }
        public bool is_external { get; set; }
        public string external_type { get; set; }
        public string username { get; set; }
        public int size { get; set; }
        public string url_private { get; set; }
        public string url_private_download { get; set; }
        public string thumb_64 { get; set; }
        public string thumb_80 { get; set; }
        public string thumb_360 { get; set; }
        public string thumb_360_gif { get; set; }
        public int thumb_360_w { get; set; }
        public int thumb_360_h { get; set; }
        public string thumb_480 { get; set; }
        public int thumb_480_w { get; set; }
        public int thumb_480_h { get; set; }
        public string thumb_160 { get; set; }
        public string permalink { get; set; }
        public string permalink_public { get; set; }
        public string edit_link { get; set; }
        public string preview { get; set; }
        public string preview_highlight { get; set; }
        public int lines { get; set; }
        public int lines_more { get; set; }
        public bool is_public { get; set; }
        public bool public_url_shared { get; set; }
        public bool display_as_bot { get; set; }
        public List<string> channels { get; set; }
        public List<string> groups { get; set; }
        public List<string> ims { get; set; }
        public string initial_comment { get; set; }
        public int num_stars { get; set; }
        public bool is_starred { get; set; }
        public List<string> pinned_to { get; set; }
        public List<Reaction> reactions { get; set; }
        public string comments_count { get; set; }

    }

    public class Reaction
    {
        public string name { get; set; }
        public int count { get; set; }
        public List<string> users { get; set; }
    }
}

