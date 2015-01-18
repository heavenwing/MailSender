using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Hangfire;
using MailSender.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace MailSender.Controllers
{
    public class SendController : Controller
    {
        // GET: SendByQBWI
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SendViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SendBy == 0)
                    HostingEnvironment.QueueBackgroundWorkItem(ct => MessageSender.SendAsync(model));
                else if (model.SendBy == 1)
                {
                    BackgroundJob.Enqueue(() => MessageSender.SendWithEvent(model));
                }
                else
                {
                    //TODO send queue message to use WebJobs
                    var storageAccount = CloudStorageAccount.Parse(
                        ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
                    var queueClient = storageAccount.CreateCloudQueueClient();
                    var queue = queueClient.GetQueueReference("pending-mail");

                    // Create the queue if it doesn't already exist
                    queue.CreateIfNotExists();

                    var message = new CloudQueueMessage(JsonConvert.SerializeObject(model));
                    queue.AddMessage(message);
                }
                return RedirectToAction("Pending");
            }

            return View(model);
        }

        public ActionResult Pending()
        {
            return View();
        }

        public async Task<HttpStatusCodeResult> Notify()
        {
            await SentNotification.Notify();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}