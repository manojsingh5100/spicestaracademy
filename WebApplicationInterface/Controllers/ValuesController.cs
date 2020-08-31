using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplicationInterface.Models;

namespace WebApplicationInterface.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Route("api/IsExistEmailOrMobile")]
        public Respose IsExistEmailOrMobile(string Email, string Mobile)
        {
            Common _common = new Common();
            Respose r = new Respose();
            try
            {
                r.Message = _common.IsExistEmailOrMobile(Email, Mobile);
                r.IsSuccess = true;
            }
            catch (Exception ex)
            {
                r.IsSuccess = false;
                r.Message = ex.Message + " " + ex.InnerException;
            }
            return r;
        }

        [HttpPost]
        [Route("api/UpdatePaymentDetail")]
        public Respose UpdatePaymentDetail(int RegNo, string PaymentId)
        {
            Common _common = new Common();
            Respose r = new Respose();
            try
            {
                r.IsSuccess = _common.UpdatePaymentDetail(RegNo, PaymentId);
                r.Message = "";
            }
            catch (Exception ex)
            {
                r.IsSuccess = false;
                r.Message = ex.Message + " " + ex.InnerException;
            }
            return r;
        }

        //[HttpPost]
        //[Route("api/GetPerformanceCandidateResponse")]
        //public Respose GetCandidateAcceptTancyPerformance(int PerformanceEntryMId, bool Status)
        //{
        //    Common _common = new Common();
        //    Respose r = new Respose();
        //    try
        //    {
        //        r.IsSuccess = _common.CreateUpdatePerformanceCandidateResponce(PerformanceEntryMId,Status);
        //        r.Message = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        r.IsSuccess = false;
        //        r.Message = ex.Message + " " + ex.InnerException;
        //    }
        //    return r;
        //}

        [HttpPost]
        [Route("api/CreateNewRegistration")]
        public Respose CreateNewRegistration([FromBody]RegistrationViewModel value)
        {
            Common _common = new Common();
            Respose r = new Respose();
            try
            {
                if (string.IsNullOrEmpty(value.Email))
                {
                    r.Id = 0;
                    r.IsSuccess = false;
                    r.Message = "Empty or Invalid Email!";
                }
                else if (string.IsNullOrEmpty(value.Mobile))
                {
                    r.Id = 0;
                    r.IsSuccess = false;
                    r.Message = "Empty or Invalid Mobile!";
                }
                else
                {
                    var data = _common.WebInterfaceAddReg(value);
                    if (data.RegistartionNo > 0)
                    {
                        r.Id = data.RegistartionNo;
                        r.IsSuccess = true;
                        r.Message = "Registration successfully completed.";
                    }
                    else
                    {
                        r.Id = data.Id;
                        r.IsSuccess = false;
                        r.Message = "";
                    }
                }
            }
            catch (Exception ex)
            {
                r.IsSuccess = false;
                r.Message = ex.Message + " " + ex.InnerException;
            }
            return r;
        }

        [HttpPost]
        [Route("api/CreatePilotTrainingRegistration")]
        public PilotResponse CreatePilotTrainingRegistration([FromBody]PilotRegistrationViewModel value)
        {
            Common _common = new Common();
            PilotResponse r = new PilotResponse();
            if (string.IsNullOrEmpty(value.CourseApplied))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Course Applied!";
            }
            else if (string.IsNullOrEmpty(value.Title))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Title!";
            }
            else if (string.IsNullOrEmpty(value.FirstName))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid FirstName!";
            }
            else if (string.IsNullOrEmpty(value.Lastname))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid LastName!";
            }
            else if (string.IsNullOrEmpty(value.Email))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Email!";
            }
            else if (string.IsNullOrEmpty(value.MobileNo))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Mobile!";
            }
            else if (string.IsNullOrEmpty(value.DateOfBirth))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid DateOfBirth!";
            }
            else if (string.IsNullOrEmpty(value.Gender))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Gender!";
            }
            else if (string.IsNullOrEmpty(value.Nationality))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Nationality!";
            }
            else if (string.IsNullOrEmpty(value.LeadSource))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid LeadSource!";
            }
            else if (string.IsNullOrEmpty(value.CorrespondenceCountry))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Email!";
            }
            else if (string.IsNullOrEmpty(value.CorrespondenceAddress1))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Correpondance Address1!";
            }
            else if (string.IsNullOrEmpty(value.CorrespondencePinCode))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Correspondance PINCode!";
            }
            else if (string.IsNullOrEmpty(value.PermanentCountry))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Permanent Country!";
            }
            else if (string.IsNullOrEmpty(value.PermanentAddress1))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Permanent Address1!";
            }
            else if (string.IsNullOrEmpty(value.PermanentPinCode))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Permanent PINCode!";
            }
            else if (string.IsNullOrEmpty(value.Tenth_NameOfInstitute))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid 10th name of institute!";
            }
            else if (string.IsNullOrEmpty(value.Tenth_BoardAndUniversity))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid 10th Board&University!";
            }
            else if (string.IsNullOrEmpty(value.Tenth_YearOfPassing))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid 10th year of passing!";
            }
            else if (string.IsNullOrEmpty(value.Tenth_EvaluationType))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid 10th Evaluation Type!";
            }
            else if (string.IsNullOrEmpty(value.Twelve_NameOfInstitute))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid 12th Name Of Institute!";
            }
            else if (string.IsNullOrEmpty(value.Twelve_BoardAndUniversity))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid 12th Board&University!";
            }
            else if (string.IsNullOrEmpty(value.Twelve_Stream))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid 12th stream!";
            }
            else if (string.IsNullOrEmpty(value.Twelve_YearOfPassing))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid 12th year of passing!";
            }
            else if (string.IsNullOrEmpty(value.MedicalStatus))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Madical Status!";
            }
            else if (string.IsNullOrEmpty(value.ApplicantName))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid ApplicantName!";
            }
            else if (string.IsNullOrEmpty(value.ParentName))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid ParentName!";
            }
            else if (string.IsNullOrEmpty(value.Date))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid FormDate!";
            }
            else if (string.IsNullOrEmpty(value.ApplicationNo))
            {
                r.RegistrationNo = "";
                r.IsSuccess = false;
                r.Message = "Empty or Invalid Application No.!";
            }
            else
            {
                try
                {
                    var data = _common.CreatePilotTrainingRegistration(value);
                    if (!string.IsNullOrEmpty(data.RegistrationNo))
                    {
                        r.RegistrationNo = data.RegistrationNo;
                        r.IsSuccess = true;
                        r.Message = "Pilot Registration successfully completed.";
                    }
                    else
                    {
                        r.RegistrationNo = "";
                        r.IsSuccess = false;
                        r.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    r.IsSuccess = false;
                    r.Message = ex.Message + " " + ex.InnerException;
                }
            }
            return r;
        }

        [HttpPost]
        [Route("api/ScreenningAmountExamInfo")]
        public Respose ScreenningAmountExamInfo([FromBody]PtaScreeningExamFeeInfoViewModel value)
        {
            Common _common = new Common();
            Respose r = new Respose();
            if (value.ScreeningAmount <= 0)
            {
                r.Id = 0;
                r.IsSuccess = false;
                r.Message = "Invalid amount!";
            }
            else if (value.ScreeningAmountTerm <= 0)
            {
                r.Id = 0;
                r.IsSuccess = false;
                r.Message = "Invalid exam term!";
            }
            else if (string.IsNullOrEmpty(value.PaymentId))
            {
                r.Id = 0;
                r.IsSuccess = false;
                r.Message = "Empty or Invalid PaymentId!";
            }
            else if (string.IsNullOrEmpty(value.ApplicationNo))
            {
                r.Id = 0;
                r.IsSuccess = false;
                r.Message = "Empty or Invalid ApplicationNo!";
            }
            else if (string.IsNullOrEmpty(value.DateOfAmount))
            {
                r.Id = 0;
                r.IsSuccess = false;
                r.Message = "Empty or Invalid DateOfAmount!";
            }
            else
            {
                try
                {
                    var data = _common.AddScreeningExamInfo(value);
                    if (data.Id > 0)
                    {
                        r.Id = data.Id;
                        r.IsSuccess = true;
                        r.Message = "Screening exam fee paid successfully.";
                    }
                    else
                    {
                        r.Id = data.Id;
                        r.IsSuccess = false;
                        r.Message = "ApllicationNo does not exist!";
                    }
                }
                catch (Exception ex)
                {
                    r.IsSuccess = false;
                    r.Message = ex.Message + " " + ex.InnerException;
                }
            }
            return r;
        }
    }
}
