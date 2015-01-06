using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
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
                    //TODO use Hangfire
                }
                else
                {
                    //TODO use WebJobs
                }
                return RedirectToAction("Pending");
            }

            return View(model);
        }

        private async Task SendMailAsync(SendViewModel model)
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

            var credentials = new NetworkCredential(
               ConfigurationManager.AppSettings["mailAccount"],
               ConfigurationManager.AppSettings["mailPassword"]
               );

            var smtpClient = new SmtpClient("smtp.sendgrid.net");
            smtpClient.Credentials = credentials;
            await smtpClient.SendMailAsync(myMessage);
        }

        public ActionResult Pending()
        {
            return Content("<h1>邮件发送任务已经加入队列</h1>");
        }
    }
}