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
                    HostingEnvironment.QueueBackgroundWorkItem(ct => MessageSender.SendAsync(model));
                else if (model.SendBy == 1)
                {
                    BackgroundJob.Enqueue(() => MessageSender.SendWithEvent(model));
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

        public ActionResult Pending()
        {
            return Content("<h1>邮件发送任务已经加入队列</h1>");
        }

        public ActionResult Notify()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}