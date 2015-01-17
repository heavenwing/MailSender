using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace MailSender
{
    public class SentNotification
    {
        public async static Task Notify()
        {
            await Task.Run(() =>
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                hub.Clients.All.notify();
            });
        }
    }
}