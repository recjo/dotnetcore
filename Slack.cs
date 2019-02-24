using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace slacker
{
    public class Slack
   {
       private readonly Uri _uri;
       private readonly Encoding _encoding = new UTF8Encoding();

       public Slack()
       {
           _uri = new Uri(Program.Configuration["slackUrl"]);
       }

       public void send(string msg, string channel = null, string username = null)
       {
           var payload = new Payload()
           {
               Channel = channel,
               Username = username,
               Text = msg
           };

           PostMessage(payload);
       }

       //Post a message using a Payload object
       public void PostMessage(Payload payload)
       {
           using (WebClient client = new WebClient())
           {
               var data = new NameValueCollection();
               data["payload"] = JsonConvert.SerializeObject(payload);
               var response = client.UploadValues(_uri, "POST", data);
               //The response text is usually "ok"
               var responseText = _encoding.GetString(response);
           }
       }
   }

   public class Payload
   {
       [JsonProperty("channel")]
       public string Channel { get; set; }

       [JsonProperty("username")]
       public string Username { get; set; }

       [JsonProperty("text")]
       public string Text { get; set; }
   }
}
