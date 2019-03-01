using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.S3;
using Amazon.S3.Model;

namespace sql2s3
{
    public class Process
    {
        private OrderService orderService;
        private S3Service s3Service;

        public Process()
        {
            this.orderService = new OrderService();
            this.s3Service = new S3Service(Program.Configuration["bucketName"], Program.Configuration["bucketPrefix"]);
        }

        public void Go(DateTime day)
        {
            Go(day, day);
        }

        public void Go(DateTime startDate, DateTime endDate)
        {
            var fileNum = 0;
            var orderList = new OrderList() {
                orders = orderService.GetOrders(startDate, endDate)
            };
            Console.WriteLine("order(s) found: " + orderList.orders.Count);
            foreach (string order in orderList.orders)
            {
                //upload batch or orders to S3
                var orderDate = orderService.GetOrderDate(order, startDate);
                UploadS3(order, ++fileNum, orderDate);
            }
            Console.WriteLine("Complete.");
            Console.WriteLine();
        }

        public void UploadS3(String content, int fileNum, DateTime defaultDate)
        {
            if (String.IsNullOrEmpty(content))
                return;

            //default to using date range for S3 subfolders
            var year = defaultDate.Year;
            var mon = defaultDate.Month;
            var day = defaultDate.Day;

            //upload batch to S3
            var keyName = $"{year}/file_{mon.ToString("00")}_{day.ToString("00")}_{fileNum.ToString("000000")}";
            s3Service.UploadContentToS3(keyName, content).Wait();
            Console.WriteLine($"Uploaded #{fileNum.ToString("000")}: {keyName}");
        }
    }

    public class OrderList
    {
        public List<string> orders { get; set; }
    }
}