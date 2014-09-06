using System;
using System.Diagnostics;
using Nancy.Hosting.Self;

namespace Input.Site.Web.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var nancyHost = new NancyHost(new Uri("http://localhost:8888/")))
            {
                nancyHost.Start();

                System.Console.WriteLine("Selfhost is running on http://localhost:8888");

                Process.Start("http://localhost:8888");

                System.Console.WriteLine("Press enter to exit.");
                System.Console.ReadKey();

                nancyHost.Stop();
            }

        }
    }
}
