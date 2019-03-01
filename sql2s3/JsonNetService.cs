using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sql2s3
{
   public class JsonNetService
   {
      private XmlToJsonConverter xmlToJsonConverter;

      public JsonNetService()
      {
         xmlToJsonConverter = new XmlToJsonConverter();
      }

      public string convertXmlToJson(XmlNode order)
      {
         return xmlToJsonConverter.ConvertXmlNode(order, true);
      }

      public DateTime GetOrderDate(string order, DateTime startDate)
      {
         var jobj = JsonConvert.DeserializeObject<dynamic>(order);
         DateTime orderDate;
         if (DateTime.TryParse(jobj["OrderDate"].ToObject<string>(), out orderDate))
         {
            return orderDate;
         }
         return startDate;
      }
   }
}