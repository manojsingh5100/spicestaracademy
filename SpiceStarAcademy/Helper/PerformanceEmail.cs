using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace SpiceStarAcademy.Helper
{
    public class PerformanceEmail
    {
        public static string GetDynamicTemplateForPtaScreening(string Content)
        {
            var strContent = new StringBuilder();
            string line;
            var file = new System.IO.StreamReader(
                   HttpContext.Current.Server.MapPath("~/Templates/PTAEmailNotificaionContent.html"));
            while ((line = file.ReadLine()) != null)
            {
                strContent.Append(line);
            }
            file.Close();
            strContent = strContent.Replace("@@Content", Content);
            return strContent.ToString();
        }
    }

    public class PerformanceTestEmail
    {
        private string TemplatePath;
        private string Subject;
        private string UserEmail;
        private string UserEmail2;

        public PerformanceTestEmail(string TemplatePath1, string SubjectInfo, string Usermail, string UserEmail2)
        {
            this.TemplatePath = TemplatePath1;
            this.Subject = SubjectInfo;
            this.UserEmail = Usermail;
            this.UserEmail2 = UserEmail2;
            EmailSender = "priyanka4680verma@gmail.com";
            EmailSenderPassword = "priyanka46";
            EmailSenderHost = "smtp.gmail.com";
            EmailSenderPort = 587;
            EmailIsSSl = true;
        }
        public string EmailSender { get; set; }
        public string EmailSenderPassword { get; set; }
        public string EmailSenderHost { get; set; }
        public int EmailSenderPort { get; set; }
        public bool EmailIsSSl { get; set; }

        public void SendTestingMail()
        {
            string MailText = TemplatePath;
            System.Net.Mail.MailMessage _mailmsg = new System.Net.Mail.MailMessage();
            _mailmsg.IsBodyHtml = true;
            _mailmsg.From = new System.Net.Mail.MailAddress(EmailSender);
            _mailmsg.To.Add(UserEmail);
            _mailmsg.To.Add(UserEmail2);
            _mailmsg.Subject = Subject;
            _mailmsg.Body = MailText;
            SmtpClient _smtp = new SmtpClient();
            _smtp.Host = EmailSenderHost;
            _smtp.Port = EmailSenderPort;
            _smtp.EnableSsl = EmailIsSSl;
            NetworkCredential _network = new NetworkCredential(EmailSender, EmailSenderPassword);
            _smtp.Credentials = _network;
            _smtp.Send(_mailmsg);
        }
    }
}