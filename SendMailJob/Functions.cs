using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailSender;
using MailSender.Models;
using Microsoft.Azure.WebJobs;

namespace SendMailJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public async static Task ProcessMailQueueMessage([QueueTrigger("pending-mail")] SendViewModel model, TextWriter log)
        {
            await MessageSender.SendAsync(model);
            await log.WriteLineAsync(string.Format("from: {0} , to: {1}", model.FromEmail, model.ToEmail));
        }
    }
}
