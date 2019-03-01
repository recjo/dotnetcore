using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace sql2s3
{
    public class OrderService
    {
        private OrderRepository orderRepository;
        private JsonNetService jsonNetService;

        public OrderService()
        {
            orderRepository = new OrderRepository();
            jsonNetService = new JsonNetService();
        }

        public List<string> GetOrders(DateTime startDate, DateTime endDate)
        {
            var orderList = new List<string>();
            //get orders returned as XML
            var ordersXml = orderRepository.GetOrders(startDate, endDate);
            //extract all orders nodes
            var nodeList = ordersXml.GetElementsByTagName("order");
            Console.WriteLine($"Found {nodeList.Count} orders.");
            //loop thru list or order nodes
            foreach (XmlNode order in nodeList)
            {
                var data = order.OuterXml;
                if (Boolean.Parse(Program.Configuration["exportJson"]))
                {
                    // convert order XML to JSON string
                    data = jsonNetService.convertXmlToJson(order);
                }
                //add order to list
                orderList.Add(data);
            }
            return orderList;
        }

        public DateTime GetOrderDate(string order, DateTime startDate)
        {
            DateTime orderDate;
            try {
                if (Boolean.Parse(Program.Configuration["exportJson"]))
                {
                    return jsonNetService.GetOrderDate(order, startDate);
                }
                return GetDateFromXml(order, startDate);
            }
            catch {}
            return startDate;
        }

      private DateTime GetDateFromXml(string order, DateTime startDate)
      {
        var doc = new XmlDocument();
        doc.Load(new StringReader(order));
        var node = doc.SelectSingleNode("/order/OrderDate");
        if (node != null)
        {
            DateTime orderDate;
            if (DateTime.TryParse(node.InnerText, out orderDate))
            {
                return orderDate;
            }
        }
        return startDate;
      }
    }
}