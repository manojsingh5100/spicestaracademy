using SJData;
using SJModel.PTAModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SJService.PTA
{
    public class NotificationService
    {
        SJStarERPEntities _context = null;
        public NotificationService()
        {
            _context = new SJStarERPEntities();
        }

        //******************************PilotScreeningList****************************************//
        public IEnumerable<PilotRegistrationViewModel> GetPilotCandidateScreenningList()
        {
            return _context.ptaPilotRegistrationMasters.Where(p => p.IsActive).Select(item => new PilotRegistrationViewModel
            {
                Id = item.Id,
                Fname = item.Fname,
                Lname = item.Lname,
                PilotRegistartionNo = item.ptaRegistrationInfoes.FirstOrDefault().RegistartionNo,
                RegistartionNo = item.RegistrationNo,
                ApplicationNo = item.ApplicationNo,
                Email = item.Email,
                Mobile = item.Mobile,
                DOB = item.DOB,
                CourseName = item.CourseMaster.CourseName,
                RegistrationDate = item.RegistrationDate,
                ScreeningAmount = item.ptaScreeningExamFeeInfoes.FirstOrDefault() != null ? item.ptaScreeningExamFeeInfoes.OrderByDescending(o => o.Id).FirstOrDefault().ScreeningAmount : 20000,
                ScreeningAmountTerm = item.ptaScreeningExamFeeInfoes.FirstOrDefault() != null ? item.ptaScreeningExamFeeInfoes.OrderByDescending(o => o.Id).FirstOrDefault().ScreeningAmountTerm : 1,
                SourceOfCandidate = item.ptaRegistrationInfoes.FirstOrDefault().ptaLeadSourceMaster.Name,
                Gender = item.ptaRegistrationInfoes.FirstOrDefault().ptaGenderMaster.Name,
                CreatedBy = item.ptaScreeningInfoes.FirstOrDefault().ptaScreeningTestResults.Where(e => e.IsPassed.HasValue && e.IsPassed.Value).Count(),
                IsActive = item.ptaScreeningInfoes.FirstOrDefault().ptaScreeningTestResults.Where(e => e.IsPassed.HasValue && !e.IsPassed.Value).Any(),
                ScreeningExamFeeNo = item.ptaScreeningExamFeeInfoes.Count(),
                LastExamFailedStatus = item.ptaScreeningExamFeeInfoes.Count() == 0 ? true : (item.ptaScreeningExamFeeInfoes.Count() < 4 ? (item.ptaScreeningInfoes.FirstOrDefault().ptaScreeningTestResults.OrderByDescending(o => o.Id).Where(w => w.IsPassed.HasValue).FirstOrDefault().IsPassed.Value == false ? true : false) : false),
                IsAppeared = item.ptaScreeningEmailNotificationLogs.Count() == (item.ptaScreeningExamFeeInfoes.Count() - 1) ? false : true,
                SentNotificationCount = item.ptaScreeningEmailNotificationLogs.Select(x => x.PTAPilotRegistrationMasterId).Count()
            }).AsEnumerable();
        }

        public bool SendExamFeeNotificationContent(int[] regNoArr, string Content)
        {
            NotificationService obj = new NotificationService();

            if (regNoArr.Length > 0)
            {
                foreach (var item in regNoArr)
                {
                    var data = obj.GetPilotCandidateInfoByRegNo(item);

                    string Status = "Successfull";
                    //Status = NotificationService.Email(GetDynamicTemplateForScreeningContent(Content));
                    if (Status == "Successfull")
                    {
                        data.ExamAmount = data.ExamTerm == 0 ? 20000 : 10000;
                        data.ExamTerm = data.ExamTerm == 0 ? 1 : data.ExamTerm + 1;
                        data.IsSendEmail = true;
                        data.IsActive = false;
                        obj.SaveExamFeeNotificationLog(data);
                    }

                }
                obj.SaveEmailNotificationContent(regNoArr, Content);
            }
            return true;
        }

        public string SaveEmailNotificationContent(int[] regNoArr, string Content)
        {
            string status = "";
            for (int i = 0; i < regNoArr.Length; ++i)
            {
                ptaEmailNotificationSentContent log = new ptaEmailNotificationSentContent
                {

                    ptaScreeningEmailNotificationID = regNoArr[i],
                    EmailNotificationcontent = Content
                };
                _context.ptaEmailNotificationSentContents.Add(log);
                _context.SaveChanges();
            }

            status = "successfull";
            return status;
        }

        //public static string GetDynamicTemplateForScreeningContent(string Content)
        //{
        //    var strContent = new StringBuilder();
        //    string line;
        //    var file = new System.IO.StreamReader(
        //           HttpContext.Current.Server.MapPath("~/Templates/PTA/PTAEmailNotificaionContent.html"));
        //    while ((line = file.ReadLine()) != null)
        //    {
        //        strContent.Append(line);
        //    }
        //    file.Close();
        //    strContent = strContent.Replace("@@Content", Content);
        //    return strContent.ToString();
        //}

        ////******************************Send mail Notification***************************
        //public static string Email(string Content)
        //{
        //    string msg = "";
        //    try
        //    {

        //        MailMessage mail = new MailMessage();
        //        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

        //        mail.From = new MailAddress("priyankavrma94@gmail.com");
        //        mail.To.Add("tilakverma061980@gmail.com");
        //        mail.Subject = "Test Mail";
        //        mail.Body = GetDynamicTemplateForScreeningContent(Content);
        //        SmtpServer.Port = 587;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("priyankavrma94@gmail.com", "@bhavi#thav$4356");
        //        SmtpServer.EnableSsl = true;
        //        SmtpServer.Send(mail);
        //        msg = "Successfull";
        //        return msg;

        //    }

        //    catch (Exception ex)
        //    { return ex.InnerException.Message; }
        //}       

        public bool SaveExamFeeNotificationLog(PilotRegistrationViewModel Model)
        {
            bool status = false;
            ptaScreeningEmailNotificationLog log = new ptaScreeningEmailNotificationLog
            {
                Amount = Model.ExamAmount,
                CourseName = Model.CourseName,
                LogDate = DateTime.Now,
                CandidateName = Model.StudentName,
                Email = Model.Email,
                ExamTermNo = Model.ExamTerm,
                IsActive = Model.IsActive,
                IsSendEmail = Model.IsSendEmail,
                PTAPilotRegistrationMasterId = Model.Id
            };
            _context.ptaScreeningEmailNotificationLogs.Add(log);
            _context.SaveChanges();
            status = true;
            return status;
        }


        public PilotRegistrationViewModel GetPilotCandidateInfoByRegNo(int RegId)
        {
            return _context.ptaPilotRegistrationMasters.Where(p => p.IsActive && p.Id == RegId).Select(item => new PilotRegistrationViewModel
            {
                Id = item.Id,
                Fname = item.Fname,
                Lname = item.Lname,
                PilotRegistartionNo = item.ptaRegistrationInfoes.FirstOrDefault().RegistartionNo,
                RegistartionNo = item.RegistrationNo,
                Email = item.Email,
                Mobile = item.Mobile,
                DOB = item.DOB,
                CourseName = item.CourseMaster.CourseName,
                ExamTerm = item.ptaScreeningExamFeeInfoes.Count > 0 ? item.ptaScreeningExamFeeInfoes.OrderByDescending(o => o.ScreeningAmountTerm).FirstOrDefault().ScreeningAmountTerm : 0
            }).FirstOrDefault();
        }

        //*******************************GetAdmissionList************************************//
        public IEnumerable<PilotRegistrationViewModel> GetPilotAdmissionList()
        {
            return _context.ptaAdmissionMasters.Where(a => a.IsActive).Select(item => new PilotRegistrationViewModel
            {
                Id = item.Id,
                Fname = item.Fname,
                Lname = item.Lanme,
                PilotRegistartionNo = item.ptaRegistrationInfoes.FirstOrDefault().RegistartionNo,
                RegistartionNo = item.ptaRegistrationInfoes.FirstOrDefault().ptaPilotRegistrationMaster.RegistrationNo,
                ApplicationNo = item.ApplicationNo,
                Email = item.Email,
                Mobile = item.Mobile,
                DOB = item.DOB,
                CourseName = item.CourseMaster.CourseName,
                RegistrationDate = item.AdmissionDate,
                Gender = item.ptaRegistrationInfoes.FirstOrDefault().ptaGenderMaster.Name,
                PilotRegistrationId = item.ptaRegistrationInfoes.FirstOrDefault().ptaPilotRegistrationMaster.Id,

            }).AsEnumerable();
        }
        public IEnumerable<PilotRegistrationViewModel> GetScreeningPaymentTerm()
        {
            return _context.ptaScreeningExamFeeInfoes.Select(item => new PilotRegistrationViewModel
            {
                ScreeningAmountTerm = item.ScreeningAmountTerm,
                Id = item.Id
            }).AsEnumerable();
        }

    }
}
