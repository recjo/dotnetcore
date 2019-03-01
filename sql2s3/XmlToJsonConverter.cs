using System;
using System.Xml;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sql2s3
{
    public class XmlToJsonConverter
    {
        public XmlToJsonConverter()
        {
        }

        //takes an XML node and converts it to JSON
        public string ConvertXmlNode(XmlNode orderXml, bool omitRoot)
        {
            var json = JsonConvert.SerializeXmlNode(orderXml, Newtonsoft.Json.Formatting.None, omitRoot);
            return json;
        }

        //takes an XML string and converts it to JSON
        public string ConvertXmlString(string orderXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(orderXml);
            var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, false);
            return json;
        }
    }
}