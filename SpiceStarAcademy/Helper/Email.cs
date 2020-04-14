using SJModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace SpiceStarAcademy.Helper
{
    public class Email
    {
        public static string EmailTemplate(string mailTemplatePath)
        {
            var strContent = new StringBuilder();
            string line;
            var file = new System.IO.StreamReader(
                   System.Web.Hosting.HostingEnvironment.MapPath(mailTemplatePath));
            while ((line = file.ReadLine()) != null)
            {
                strContent.Append(line);
            }
            file.Close();
            return strContent.ToString();
        }

        public static string EmailTemplateForFeeNotification(string mailTemplatePath, FeeEmailNotifacationLogViewModel Model)
        {
            
            var strContent = new StringBuilder();
            string line;
            var file = new System.IO.StreamReader(
                   System.Web.Hosting.HostingEnvironment.MapPath(mailTemplatePath));
            while ((line = file.ReadLine()) != null)
            {
                strContent.Append(line);
            }
            file.Close();
            DateTime currDate = DateTime.Now;
            strContent = strContent.Replace("@@CurrentDate", currDate.ToString("dd/MM/yyyy"));
            strContent = strContent.Replace("@@CandidateName", Model.CandidateName);
            strContent = strContent.Replace("@@ProgrameName", Model.CourseName);
            strContent = strContent.Replace("@@Money", Model.CourseChangeFeeAmountForMBA);
            strContent = strContent.Replace("@@CurrMonth", currDate.ToString("MMMM"));
            strContent = strContent.Replace("@@CurrYear", currDate.Year.ToString());

            return strContent.ToString();
        }

        public static string SendFeeNotificationEmail(FeeEmailNotifacationLogViewModel Model)
        {
            try
            {
                EmailNotificationModel obj = new EmailNotificationModel();
                obj.SenderEmailId = Model.Email;
                obj.SmtpUser = ConfigurationManager.AppSettings["smtpUser"];
                obj.CCEmail = new string[] { "contact@spicestaracademy.edu.in" };
                obj.EmailSubjectName = "Fee Reminder from Spice Star";
                obj.TemplateFilePath = EmailTemplateForFeeNotification("~/Templates/FeeReminderTemplate.html", Model);
                SendEmail(obj);
            }
            catch (Exception ex)
            {
                return ex.Message + "," + ex.InnerException.StackTrace;
            }
            return "Successfull";
        }

        public static string RegEmailTemplate(string StudentName, string mailTemplate)
        {
            var strContent = new StringBuilder();
            string line;
            string path = mailTemplate.ToLower() == "rejected" ? "~/Templates/RejectedEmailTemplate.html" : "~/Templates/ApprovedEmailTemplate.html";
            var file = new System.IO.StreamReader(
                   HttpContext.Current.Server.MapPath(path));
            while ((line = file.ReadLine()) != null)
            {
                strContent.Append(line);
            }
            file.Close();
            if (mailTemplate.ToLower() == "rejected")
            {
                strContent = strContent.Replace("@@Name", StudentName);
            }
            else if (mailTemplate.ToLower() == "approved")
            {
                strContent = strContent.Replace("@@Name", StudentName);
            }
            return strContent.ToString();
        }

        public static bool SendEmailThroughAPI(string MailApiUrl, string jRequest)
        {
            bool result = false;
            using (WebClient _proxy = new WebClient())
            {
                _proxy.Headers.Add("Content-Type", "application/json");
                _proxy.Encoding = System.Text.Encoding.UTF8;
                var _response = _proxy.UploadString(MailApiUrl, "POST", jRequest);
                if (_response.Contains("200"))
                {
                    result = true;
                }
            }
            return result;
        }

        public static string SendEmailWithSingleTemplate(string Email, string SubjectName, string TemplateFilePath)
        {
            try
            {
                EmailNotificationModel obj = new EmailNotificationModel();
                obj.SenderEmailId = Email;
                obj.SmtpUser = ConfigurationManager.AppSettings["smtpUser"];
                obj.CCEmail = new string[] { "contact@spicestaracademy.edu.in" };
                obj.EmailSubjectName = SubjectName;     // (EmailTitle == "rejected" ? "Spice Star - Registration Rejected" : "Spice Star - Registration Approved");
                obj.TemplateFilePath = EmailTemplate(TemplateFilePath);     //  RegEmailTemplate(Name, EmailTitle);
                SendEmail(obj);
            }
            catch (Exception ex)
            {
                return ex.Message + "," + ex.InnerException.StackTrace;
            }
            return "Successfull";
        }

        public static string SendApprovedAndRejectedEmail(string Name, string Email, string EmailTitle)
        {
            try
            {
                EmailNotificationModel obj = new EmailNotificationModel();
                obj.SenderEmailId = Email;
                obj.SmtpUser = ConfigurationManager.AppSettings["smtpUser"];

                obj.EmailSubjectName = (EmailTitle == "rejected" ? "Spice Star - Registration Rejected" : "Spice Star - Registration Approved");
                obj.TemplateFilePath = RegEmailTemplate(Name, EmailTitle);

                SendEmail(obj);
            }
            catch (Exception ex)
            {
                return ex.Message + "," + ex.InnerException.StackTrace;
            }
            return "Successfull";
        }

        public static Byte[] ConvertFileToByteArr(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filename);
                fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
        }

        public static void SendEmail(EmailNotificationModel objEMailDac)
        {
            string[] senderEmailIds = new string[] { objEMailDac.SenderEmailId };
            string MailApi = ConfigurationManager.AppSettings["MailApi"];
            var _jsonSerialiser = new JavaScriptSerializer();
            var _jsonRequest = _jsonSerialiser.Serialize
            (new Mail()
            {

                applicationKey = ConfigurationManager.AppSettings["MailApiKey"],
                cc = objEMailDac.CCEmail,
                from = objEMailDac.SmtpUser,
                plainTextContent = string.Empty,
                attachmentContent = objEMailDac.attachments,
                htmlContent = objEMailDac.TemplateFilePath.Trim(),
                subject = objEMailDac.EmailSubjectName,
                to = senderEmailIds
            }
            );
            try
            {
                var s = SendEmailThroughAPI(MailApi, _jsonRequest);
            }
            catch (Exception ex)
            {
                //  ErrorLog.AddEmailLogg(ex);
                return;
            }
        }
    }

    public class Mail
    {
        public string from { get; set; }
        public string subject { get; set; }
        public string plainTextContent { get; set; }
        public string htmlContent { get; set; }
        public string[] to { get; set; }
        public string[] bcc { get; set; }
        public string[] cc { get; set; }
        public string applicationKey { get; set; }
        public List<Attachments> attachmentContent { get; set; }
    }

    public class Attachments
    {
        public Byte[] attachment { get; set; }
        public string fileName { get; set; }
    }
}