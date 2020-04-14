using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiceStarAcademy.Helper
{
    public class EmailNotificationModel
    {
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public string TemplateFilePath { get; set; }
        public string EmailSubjectName { get; set; }
        public string SenderEmailId { get; set; }
        public string[] CCEmail { get; set; }
        public List<Attachments> attachments { get; set; }
    }
}