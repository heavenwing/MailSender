using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MailSender.Models
{
    public class SendViewModel
    {
        [Display(Name = "发送人")]
        public string FromName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "发送地址")]
        public string FromEmail { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "接收地址")]
        public string ToEmail { get; set; }

        [Required]
        [Display(Name = "主题")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "内容")]
        public string Body { get; set; }

        /// <summary>
        /// 0:QBWI,1:Hangfire,2:WebJobs
        /// </summary>
        public int SendBy { get; set; }
    }
}