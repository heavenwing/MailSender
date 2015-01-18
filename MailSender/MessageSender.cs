using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using MailSender.Models;

namespace MailSender
{
    public static class MessageSender
    {
        public static Func<Task> NotifyCompleted;
        public static async Task SendAsync(SendViewModel model)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.sendgrid.net");
                smtpClient.Credentials = GetCredential(); ;
                await smtpClient.SendMailAsync(GetMailMessage(model));
                if (NotifyCompleted != null)
                    await NotifyCompleted();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        private static NetworkCredential GetCredential()
        {
            var credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["mailAccount"],
                ConfigurationManager.AppSettings["mailPassword"]
                );
            return credentials;
        }

        public static void SendWithEvent(SendViewModel model)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.sendgrid.net");
                smtpClient.Credentials = GetCredential();
                smtpClient.SendCompleted += smtpClient_SendCompleted;
                smtpClient.SendAsync(GetMailMessage(model), null);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
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

        static async void smtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (NotifyCompleted != null)
                    await NotifyCompleted();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }
    }
}