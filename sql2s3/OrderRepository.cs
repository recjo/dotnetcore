using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace sql2s3
{
    public class OrderRepository
    {
        public XmlDocument GetOrders(DateTime startDate, DateTime endDate)
        {
            var connString = Program.Configuration["connString"];
            var cmdString = GetXmlOrderQuery(startDate, endDate);
            using(var connection = new SqlConnection(connString))
            {
                connection.Open();
                var cmd = new SqlCommand(cmdString, connection);
                cmd.CommandTimeout = 60;
                //use an XmlReader (instead of DataReader) to handle XML response
                using (XmlReader reader = cmd.ExecuteXmlReader())
                {
                    //convert to an XML document and return
                    var doc = new XmlDocument();
                    doc.Load(reader);
                    return doc;
                }
            }
        }

        private string GetXmlOrderQuery(DateTime startDate, DateTime endDate)
        {
            //query to return orders in desired XML schema
            var str = $@"
                SELECT * FROM Orders As [order]
                LEFT JOIN OrderDetailsFlat AS orderItems ON [order].OrderId = orderItems.OrderId
                WHERE [order].OrderDate >= '{startDate}' AND [order].OrderDate <= '{endDate}'
                FOR XML AUTO, ROOT('orders'), ELEMENTS;
            ";
            return str;
        }
    }
}