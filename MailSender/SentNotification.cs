using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MailSender
{
    public class SentNotification
    {
        public async static Task Notify()
        {
            await Task.Run(() =>
            {
                //TODO send SignalR message to web browser
            });
        }
    }
}