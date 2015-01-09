using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SendMailJob
{
    public class SentNotification
    {
        public async static Task Notify()
        {
            //post notify action on contoller
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebSiteUrl"]);
            await client.GetAsync("Send/Notify");
        }
    }
}
