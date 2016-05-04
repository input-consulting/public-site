using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Input.Site.WebJob
{
    public class Functions
    {
        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called queue
        [NoAutomaticTrigger]
        public static void ManualTrigger(TextWriter log, int value, [Queue("queue")] out string message)
        {
            log.WriteLine("Function is invoked with value={0}", value);
            message = value.ToString();
            log.WriteLine("Following message will be written on the Queue={0}", message);
            PollSlackCommitGit(log).Wait();
        }

        public async static Task PollSlackCommitGit(TextWriter log)
        {
            log.Write("Creating Slack Client... ");
            SlackClient slackClient = new SlackClient();
            log.WriteLine("Created");
            log.Write("Creating Git Client... ");
            GitClient gitClient = new GitClient();
            log.WriteLine("Created");
            //ChannelHistory siteHistory;
            FileList filelist;
            string latest = await gitClient.GetLastBlogPostTimeStamp();
            log.WriteLine($"Last Commited Timestamp is {latest} ");
            //do
            //{
            //    siteHistory = await slackClient.GetSiteHistory(latest);
            //    log.WriteLine($"Found {siteHistory.messages.Count} new messages...");
            //    if (siteHistory.messages.Count > 0)
            //    {
            //        latest = siteHistory.messages.First().ts;
            //        foreach (Message mess in siteHistory.messages)
            //        {
            //            if (mess.text != "" && mess.subtype == "file_share")
            //            {
            //                string response = await slackClient.GetFileBlogPost(mess);
            //                await gitClient.CommitFile(mess);
            //            }
            //        }
            //    }
            //} while (siteHistory.has_more == "true");

            // files haves regular timestamp (without .xxxxxx endeing)
            // TODO Paging and many files?
                filelist = await slackClient.GetChannelFiles(latest);
                log.WriteLine($"Found {filelist.files.Count} new files...");
                if (filelist.files.Count > 0)
                {
                    latest = filelist.files.First().timestamp + ".000000";
                    foreach (File file in filelist.files)
                    {                       
                        await gitClient.CommitFile(file);                        
                    }
                }
            
        }
    }
}
