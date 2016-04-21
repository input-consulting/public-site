using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Input.Site.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            // For testing locally, could not get the azure queue thing to work :(
            //Functions.PollSlackCommitGit().Wait();
            //return;

            var host = new JobHost();
            // The following code will invoke a function called ManualTrigger and 
            // pass in data (value in this case) to the function
            Task callTask = host.CallAsync(typeof(Functions).GetMethod("ManualTrigger"), new { value = 20 });

            Console.WriteLine("Waiting for async operation...");
            callTask.Wait();
            Console.WriteLine("Task completed: " + callTask.Status);
        }


       
    }
}
