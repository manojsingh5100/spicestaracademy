using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SJService;
using SpiceStarAcademy.Helper;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.IO;
using SJModel;
using Newtonsoft.Json;

namespace SpiceStarAcademy.Models
{
    public class Common
    {
        RegistrationService _registrationService = null;
        FeeManagementService _feemanagementService = null;
        public Common()
        {
            _registrationService = new RegistrationService();
            _feemanagementService = new FeeManagementService();
        }

        public void SendFeeNotificationToCadidates()
        {
            var info = _feemanagementService.SendFeeEmailOneByOne();
            if (info != null && info.Count > 0)
            {
                foreach (var item in info)
                {
                    if (item.FeeCollection.Count() > 0)
                    {
                        FeeEmailNotifacationLogViewModel Model = new FeeEmailNotifacationLogViewModel();
                        Model.CandidateName = item.Fname + " " + item.Lname;
                        Model.CourseName = item.CourseName;
                        Model.RegNo = item.RegNo;
                        Model.LogDate = DateTime.Now;
                        Model.Email = item.RegisterEmail;
                        if (item.FeeCollection.FirstOrDefault().InstallmentId == 1)
                        {
                            Model.StartDateOfSendingMail = item.BatchStartDate.Value.AddMonths(11);
                            Model.EndDateofSendingMail = item.BatchStartDate.Value.AddMonths(12);
                            Model.FeeInstallmentName = "Second Installment";
                        }
                        else if (item.FeeCollection.FirstOrDefault().InstallmentId == 2)
                        {
                            Model.StartDateOfSendingMail = item.BatchStartDate.Value.AddMonths(23);
                            Model.EndDateofSendingMail = item.BatchStartDate.Value.AddMonths(24);
                            Model.FeeInstallmentName = "Third Installment";
                        }
                        if (_feemanagementService.GetFeeEmailNotifyDetailBiId(Model))
                            _feemanagementService.AddFeeNotifaicationLog(Model);
                    }
                }
                var sendingData = _feemanagementService.GetFeeEmailNotifyList();
                if (sendingData.Count() > 0)
                {
                    foreach (var obj in sendingData)
                    {
                        var currDate = DateTime.Now;
                        if (obj.StartDateOfSendingMail <= currDate && obj.EndDateofSendingMail >= currDate)
                        {
                            obj.CourseChangeFeeAmountForMBA = _feemanagementService.GetCourseChangeFeeReminder(obj.RegNo.Value, obj.CourseName, obj.FeeInstallmentName);
                            if (string.IsNullOrEmpty(obj.CourseChangeFeeAmountForMBA) || (obj.CourseChangeFeeAmountForMBA == "0"))
                                _feemanagementService.UpdateFeeEmailNotification(obj.Id);
                            else if (Email.SendFeeNotificationEmail(obj) == "Successfull")
                                _feemanagementService.UpdateFeeEmailNotification(obj.Id);
                        }
                    }
                }
            }
        }

        public bool SendExamFeeNotificationFromPilotRegister(int[] RegNoArr)
        {
            SJService.PTA.RegistrationService obj = new SJService.PTA.RegistrationService();
            if (RegNoArr.Length > 0)
            {
                foreach (var item in RegNoArr)
                {
                    var data = obj.GetPilotCandidateInfoByRegNo(item);
                    if (data != null)
                    {
                        string Status = "";
                        Status = Email.SendEmailWithSingleTemplate(data.Email, "Pilot Traning Academy||Screening Exam Fee Notification||" + DateTime.Now.ToString("MMMM dd, yyyy"), "~/Templates/PilotScreeningFeePay.html");
                        if (Status == "Successfull")
                        {
                            data.ExamAmount = 20000;
                            data.ExamTerm = 1;
                            data.IsSendEmail = true;
                            data.IsActive = false;
                            obj.SaveExamFeeNitificationLogs(data);
                        }
                    }
                }
            }
            return true;
        }

        public bool SendExamFeeNotificationFromPilotRegisterAgain(int[] RegNoArr)
        {
            SJService.PTA.RegistrationService obj = new SJService.PTA.RegistrationService();
            if (RegNoArr.Length > 0)
            {
                foreach (var item in RegNoArr)
                {
                    var data = obj.GetPilotCandidateInfoByRegNo(item);
                    if (data != null)
                    {
                        string Status = "Successfull";
                        //Status = Email.SendEmailWithSingleTemplate(data.Email, "Pilot Traning Academy||Screening Exam Fee Notification||" + DateTime.Now.ToString("MMMM dd, yyyy"), "~/Templates/PilotScreeningFeePay.html");
                        if (Status == "Successfull")
                        {
                            data.ExamAmount = data.ExamTerm == 0 ? 20000 : 10000;
                            data.ExamTerm = data.ExamTerm == 0 ? 1 : data.ExamTerm + 1;
                            data.IsSendEmail = true;
                            data.IsActive = false;
                            obj.SaveExamFeeNitificationLogs(data);
                        }
                    }
                }
            }
            return true;
        }

        public void SendRejectedEmailWithIn24Hours()
        {
            var data = _registrationService.SendRejectedEmailWithIn24Hours();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    string Status = "";
                    var time1 = DateTime.Now;
                    var time2 = DateTime.Now.AddMinutes(-5);
                    var DateOfSending = item.DateOfSending.AddDays(1);
                    if (DateOfSending > time2 && DateOfSending < time1)
                        Status = Email.SendEmailWithSingleTemplate(item.Email, "Spice Star Academy||Screening and Interview||" + item.DateOfSending.ToString("MMMM dd, yyyy"), "~/Templates/RejectedEmailTemplate.html");
                    if (Status == "Successfull")
                        _registrationService.SaveSendRejectedEmailInfo(item);
                }
            }
        }

        public ReverseResponse GetResult(string Email, Int64 ApplicationNo, string ScreeningStatus, string MedicalStatus)
        {
            string url = "https://api.nopaperforms.com/post-application/344/1713";
            string data = "{\"secret_key\":\"8944bacfb52da67bac5a1ba3e5a20d94\", \"form_id\":\"1713\",\"email\":\"" + Email + "\",\"application_no\":\"" + ApplicationNo + "\",\"enrolled_campus\":\"" + MedicalStatus + "\",\"application_stage\":\"Enrolled\",\"enrolled_department\":\"" + ScreeningStatus + "\",\"mode\":\"update\"}";
            WebRequest myReq = WebRequest.Create(url);
            myReq.Method = "POST";
            myReq.ContentLength = data.Length;
            myReq.ContentType = "application/json; charset=UTF-8";
            UTF8Encoding enc = new UTF8Encoding();
            using (Stream ds = myReq.GetRequestStream())
            {
                ds.Write(enc.GetBytes(data), 0, data.Length);
            }
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            ReverseResponse obj = JsonConvert.DeserializeObject<ReverseResponse>(content);
            return obj;
        }

        public void TerminationResignationSheduler()
        {
            AddmissionService _addmission = new AddmissionService();
            var data = _addmission.GetSheduledTRinfo();
            if (data.Count() > 0)
            {
                foreach(var item in data)
                    _addmission.AddEndTRSheduledInfo(item);
            }
        }
    }
}