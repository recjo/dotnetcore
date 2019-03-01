using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace sql2s3
{
    class Program
    {
        public static IConfiguration Configuration { get; private set; }

        static void Main(string[] args)
        {
            var startDate = DateTime.Now;
            var endDate = DateTime.Now; //default to 1 day

            //check dates passed in thru command line
            if (args != null && args.Length > 0)
            {
                var startDateStr = args[0].ToString().Trim();
                if (!DateTime.TryParse(startDateStr, out startDate))
                {
                    Console.WriteLine("Start date not valid. Run app and try again.");
                    return;
                }

                if (args.Length > 1)
                {
                    var endDateStr = args[1].ToString().Trim();
                    if (!DateTime.TryParse(endDateStr, out endDate))
                    {
                        Console.WriteLine("End date not valid. Run app and try again.");
                        return;
                    }
                }
                else //use start date as end date (1 day)
                {
                    DateTime.TryParse(startDateStr, out endDate);
                }
            }
            else
            {
                Console.WriteLine("No date range entered. Using today's date.");
            }
            Console.WriteLine($"Date range: {startDate.ToString("yyyy-MM-dd 00:00:00")} to {endDate.ToString("yyyy-MM-dd 23:59:59")}");

            //load settings configuration file
            Configuration =  new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            //start main process, loop through each day in range
            var process = new Process();
            for (DateTime date = startDate; date.Date <= endDate.Date; date = date.AddDays(1))
            {
                //process one day at a time to prevent SQL timeouts
                Console.WriteLine($"Starting {date.ToShortDateString()}...");
                process.Go(date);
            }
            Console.WriteLine("Program Done.");
        }
    }
}
