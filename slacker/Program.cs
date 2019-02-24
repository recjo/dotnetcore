using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace slacker
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }

        static void Main(string[] args)
        {
            //load settings configuration file
            Configuration =  new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string msg = string.Empty;
            Console.WriteLine("Enter message to post to slack:");
            do {
                msg = Console.ReadLine();
            } while (String.IsNullOrEmpty(msg));
            
            var slack = new Slack();
            slack.send(msg);
            Console.WriteLine("Message sent.");
        }
    }
}
