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
                    HostingEnvironment.QueueBackgroundWorkItem(ct => SendMailAsync(model));
                else if (model.SendBy == 1)
                {
                    BackgroundJob.Enqueue(() => SendMail(model));
                }
                else
                {
                    //TODO use WebJobs
                    throw new NotImplementedException();
                }
                return RedirectToAction("Pending");
            }

            return View(model);
        }

        public static async Task SendMailAsync(SendViewModel model)
        {
            var smtpClient = new SmtpClient("smtp.sendgrid.net");
            smtpClient.Credentials = GetCredential(); ;
            await smtpClient.SendMailAsync(GetMailMessage(model));
            //TODO notify
        }

        private static NetworkCredential GetCredential()
        {
            var credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["mailAccount"],
                ConfigurationManager.AppSettings["mailPassword"]
                );
            return credentials;
        }

        public static void SendMail(SendViewModel model)
        {
            var smtpClient = new SmtpClient("smtp.sendgrid.net");
            smtpClient.SendCompleted += smtpClient_SendCompleted;
            smtpClient.Credentials = GetCredential();
            smtpClient.SendAsync(GetMailMessage(model), null);
        }

        private static MailMessage GetMailMessage(SendViewModel model)
        {
            var myMessage = new MailMessage();

            myMessage.From = string.IsNullOrEmpty(model.FromName)
                ? new MailAddress(model.FromEmail)
                : new MailAddress(model.FromEmail, model.FromName);
            myMessage.To.Add(model.ToEmail);
            myMessage.Subject = model.Subject;

            //Add the HTML and Text bodies
            myMessage.IsBodyHtml = false;
            myMessage.Body = model.Body;
            return myMessage;
        }

        static void smtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //TODO notify
        }

        public ActionResult Pending()
        {
            return Content("<h1>邮件发送任务已经加入队列</h1>");
        }
    }
}