using SJModel;
using SJService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpiceStarAcademy.Helper;
using SpiceStarAcademy.Filter;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;
using System.Globalization;
using System.Data;
using ClosedXML.Excel;
using System.Web.Script.Serialization;

namespace SpiceStarAcademy.Controllers
{
    [AuthorizeActionFilterAttribute.LoggingFilterAttribute]
    public class RegistrationController : Controller
    {
        // GET: Registration
        private RegistrationService registerService = null;
        private AddmissionService addmissionService = null;
        private LogActivityService logActivityService = null;
        public RegistrationController()
        {
            registerService = new RegistrationService();
            addmissionService = new AddmissionService();
            logActivityService = new LogActivityService();
        }

        public ActionResult Index()
        {
            registerService.AddRegistrationIdInAdmissionMaster();
            return View(addmissionService.GetBatchList());
        }

        public ActionResult MedicalClearance()
        {
            return View();
        }

        public ActionResult UpdateMedicalRemark(int RegNo, string Remark, string MdlStatus, int Id, bool IsMedicalStandBy, string tag = null)
       {
            var Result = registerService.UpdateMedicalRemark(RegNo, Remark, MdlStatus, Id, IsMedicalStandBy,tag);
            Models.Common cm = new Models.Common();
            cm.GetResult(Result.Email, Convert.ToInt64(Result.ApplicationNo), "Selected", "Rejected");
            LogActivityViewModel log = new LogActivityViewModel();
            log.EnteredBy = Convert.ToInt32(Session["UserId"]);
            log.EnteredDate = DateTime.Now;
            log.ActioName = "UpdateMedicalRemark";
            log.ControllerName = "Registration";
            if (string.IsNullOrEmpty(Result.Message))
            {
                log.ModuleName = "Medical";
                log.Activity = "Medical Rejected";
                log.ActivityMessage = "Medical clearance of registraion no " + RegNo + " is not cleared.";
            }
            else
            {
                log.ModuleName = "Withdrawal";
                log.Activity = "Create Withdrawal";
                log.ActivityMessage = Result.Message;
            }
            logActivityService.CreateLogActivity(log);
            return Json(Result.MStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ScreenningClearance()
        {
            return View(addmissionService.GetBatchList());
        }

        [HttpPost]
        public ActionResult GetRagisterList(DataTableFilterModel filter, string Tag = null)
        {
            try
            {
                int currDate = Convert.ToInt32(Session["CurrentYear"]);
                Session["filter"] = Newtonsoft.Json.JsonConvert.SerializeObject(filter);
                DataTableFilterModel dataFilter = registerService.GetProjectList(filter, currDate, Tag);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(int id = 0, string type = null)
        {
            RegistrationViewModel model = new RegistrationViewModel();
            if (id > 0)
                model = registerService.GetRegisterInfoById(id);
            else
                model.RegistartionNo = registerService.MaxRegistartionNumber();
            model.GenderList = new List<SJModel.SelectList>()
            {
                new SJModel.SelectList{Value="F" , Text="Female" },
                new SJModel.SelectList{Value="M" , Text="Male" }
            };
            model.GetSourceList = registerService.GetSourceList();
            if (model.CourseId == null || model.CourseId == 0)
            {
                model.EducationList = new List<SJModel.SelectList>()
                {
                     new SJModel.SelectList { Value = "10+2", Text = "10+2" },
                     new SJModel.SelectList { Value = "Graduate", Text = "Graduate" },
                     new SJModel.SelectList { Value = "Post-Graduate", Text = "Post-Graduate" }
                };
            }
            else if (model.CourseId == 1)
            {
                model.EducationList = new List<SJModel.SelectList>()
                {
                     new SJModel.SelectList { Value = "10+2", Text = "10+2" },
                };
            }
            else
            {
                model.EducationList = new List<SJModel.SelectList>()
                {
                     new SJModel.SelectList { Value = "Graduate", Text = "Graduate" },
                     new SJModel.SelectList { Value = "Post-Graduate", Text = "Post-Graduate" }
                };
            }
            model.CourseList = addmissionService.GetCourseList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RegistrationViewModel model)
        {
            //if ((!string.IsNullOrEmpty(model.page) && model.page == "ReRegister") || registerService.IsManualExistByEmail(model.Email, model.Mobile))
            //{
            //    registerService.DisableReregisterOldRecord(model.Email, model.Mobile);
            //    model.Id = 0;
            //}
            LogActivityViewModel log = new LogActivityViewModel();
            log.EnteredBy = Convert.ToInt32(Session["UserId"]);
            log.EnteredDate = DateTime.Now;
            log.ActioName = "Create";
            log.ModuleName = "Registration";
            log.ControllerName = "Registration";
            model = registerService.AddUpdate(model);
            if (model.Id == 0)
            {
                TempData["msg"] = "Created Successfully!";
                log.Activity = "Create";
                log.ActivityMessage = "New candidate registraion no " + model.RegistartionNo + " is created.";
            }
            else
            {
                TempData["msg"] = "Updated Successfully!";
                log.Activity = "Update";
                log.ActivityMessage = "Candidate registration no " + model.RegistartionNo + " details are updated.";
            }
            logActivityService.CreateLogActivity(log);
            return RedirectToAction("");
            //if (!model.IsConsultantCandidate)
            //    return Redirect(Url.Action("Create", "Registration") + "?Id=" + model.Id);
            //else
            //    return RedirectToAction("");
        }

        [HttpGet]
        public JsonResult GetSessionNameByCourseIdAndCurrDate(int CourseId, int RegistrationNo, int Year, string CourseName)
        {
            int CurrentYr = Year > 0 ? Year : DateTime.Now.Year;
            return Json(registerService.GetSessionNameByCourseIdAndCurrDate(CourseId, CourseName, CurrentYr, RegistrationNo), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateMapping(RegistrationViewModel model)
        {
            string msg = "";
            if (model.MedicalRemark == null)
            {
                msg = "Created Successfully!";
                model = registerService.AddUpdate(model);
                if (model.Id > 0)
                    msg = "Updated Successfully!";
            }
            else
            {
                msg = model.MedicalRemark;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IsStudentScreenningMedicalClearance(int Id, bool status, string Tag)
        {
            var info = registerService.IsStudentScreenningMedicalClearance(Id, status, Tag);
            if (Tag == "Medical")
            {
                LogActivityViewModel log = new LogActivityViewModel();
                log.EnteredBy = Convert.ToInt32(Session["UserId"]);
                log.EnteredDate = DateTime.Now;
                log.ActioName = "IsStudentScreenningMedicalClearance";
                log.ModuleName = "Medical";
                log.ControllerName = "Registration";
                log.Activity = "Medical Selected";
                log.ActivityMessage = "Medical clearance of registraion no " + info.RegistartionNo + " is cleared.";
                logActivityService.CreateLogActivity(log);
                Models.Common cm = new Models.Common();
                cm.GetResult(info.Email, Convert.ToInt64(info.ApplicationNo), "Selected", "Selected");
            }
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReResistrationOpenModelPopup(string Email, string Mobile)
        {
            return PartialView("_ShowReRegistrationHistory", registerService.GetReRegisterHistory(Email, Mobile));
        }

        public ActionResult MedicalRemarkOpenModelPopup(int RegNo, int Id, string Tag = null)
        {
            CallCenterRemarkViewModel model = new CallCenterRemarkViewModel();
            model.Tag = Tag;
            model.remarkList = (from RemarksOfMedical e in Enum.GetValues(typeof(RemarksOfMedical))
                                select new RoleViewModel
                                {
                                    Id = (int)e,
                                    Name = e.ToString().Replace('_', ' ')
                                }).ToList();
            model.MedicalStatusList = (from MedicalStatus e in Enum.GetValues(typeof(MedicalStatus))
                                       select new RoleViewModel
                                       {
                                           ActiveStr = e.ToString(),
                                           Name = e.ToString()
                                       }).Where(r => r.Name != "FIT").ToList();
            model.RegistrationNo = RegNo;
            model.Id = Id;
            var RegisterData = registerService.GetRegisterInfoByRegNo(RegNo);
            if (RegisterData != null)
            {
                if (RegisterData.IsMedicalClear.HasValue && !RegisterData.IsMedicalClear.Value && RegisterData.MedicalRemark != null && RegisterData.MedicalRemark != "")
                {
                    var result = model.remarkList.Where(r => r.Name == RegisterData.MedicalRemark).FirstOrDefault();
                    if (result != null)
                        model.MedicalRemarkNum = result.Id;
                    else
                    {
                        model.MedicalRemarkNum = (int)RemarksOfMedical.Others_with_Free_Text;
                        model.Remarks = RegisterData.MedicalRemark;
                    }
                }
            }
            model.IsMedicalStandBy = RegisterData.IsMedicalStandBy;
            model.StudentName = RegisterData.StudentName;
            model.MedicalFitnessStatus = RegisterData.ModOfPayment;
            return PartialView("_MedicalRemark", model);
        }

        public ActionResult WithdrawalCandidates()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WithdrawalCandidateList(DataTableFilterModel filter)
        {
            try
            {
                int currDate = Convert.ToInt32(Session["CurrentYear"]);
                DataTableFilterModel dataFilter = registerService.GetWithdrawalCandidateList(filter, currDate);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ScreeningTestOpenModelPopup(int RegNo)
        {
            ScreeningInfoViewModel model = registerService.GetScreeningTestInfo(RegNo);
            model.GetDocumentList = registerService.GetScreeningDocument();
            model.GetTatooList = registerService.GetScreeningTatooList();
            model.GetBatchList = addmissionService.GetBatchList();
            model.GetCourseList = addmissionService.GetCourseList();
            return PartialView("_ScreenTestPopUp", model);
        }

        public ActionResult ScreeningTestOpenModelPopupShow(int RegNo)
        {
            ScreeningInfoViewModel model = registerService.GetScreeningTestInfoToShow(RegNo);
            model.GetDocumentList = registerService.GetScreeningDocument();
            model.GetTatooList = registerService.GetScreeningTatooList();
            model.GetBatchList = addmissionService.GetBatchList();
            model.GetCourseList = addmissionService.GetCourseList();
            return PartialView("_ScreenTestPopUp", model);
        }

        public JsonResult IsScreeningStandBy(bool IsStandBy, int RegNo)
        {
            return Json(registerService.IsScreeningStandBy(IsStandBy, RegNo), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsStudentExistByEmail(string Email, string Mobile)
        {
            return Json(registerService.IsStudentExistByEmail(Email, Mobile), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ScreeningTestSubmit(ScreeningInfoViewModel Model)
        {
            Models.Common cm = new Models.Common();
            Model.CreatedBy = Convert.ToInt32(Session["UserId"]);
            var Obj = registerService.ScreeningTestSubmit(Model);
            if (Obj.MStatus && Obj.IsSelected.HasValue && !Obj.IsSelected.Value && !Model.IsStandBy)
            {
                string currentTime = DateTime.Now.ToShortTimeString();
                string dt = Model.CreatedDate + " " + currentTime;
                SendingEmailForRejectedCandidateViewModel objSend = new SendingEmailForRejectedCandidateViewModel
                {
                    DateOfSending = DateTime.ParseExact(dt, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Email = Obj.Email,
                    IsActive = true,
                    IsSendEmail = false,
                    RegistrationNo = Obj.RegistrationId
                };
                registerService.SaveSendRejectedEmailInfo(objSend);
                cm.GetResult(Model.Email, Convert.ToInt64(Model.ApplicationNo), "Rejected", "Pending");
                //  Email.SendEmailWithSingleTemplate(Obj.Email, "Spice Star - Candidate Rejected", "~/Templates/RejectedEmailTemplate.html");
            }
            string msg = "";
            if (Obj.MStatus)
            {
                LogActivityViewModel log = new LogActivityViewModel();
                log.EnteredBy = Convert.ToInt32(Session["UserId"]);
                log.EnteredDate = DateTime.Now;
                log.ActioName = "ScreeningTestSubmit";
                log.ModuleName = "Screenning";
                log.ControllerName = "Registration";
                if (Model.IsSelected.HasValue && !Model.IsSelected.Value)
                {
                    if (Model.IsStandBy)
                    {
                        msg = "Screening Test (RegNo: " + Model.RegistrationId + ") is on Stand-By!";
                        log.Activity = "Screenning on stand by";
                        log.ActivityMessage = "Screening test of registration no " + Model.RegistrationId + " is on Stand-By!";
                        logActivityService.CreateLogActivity(log);
                    }
                    else
                    {
                        msg = "Screening Test (RegNo: " + Model.RegistrationId + ")  is rejected!";
                        log.Activity = "Screenning as rejected";
                        log.ActivityMessage = "Screening test of registration no " + Model.RegistrationId + " is rejected!";
                        logActivityService.CreateLogActivity(log);
                    }
                }
                else if (Model.IsSelected.HasValue && Model.IsSelected.Value)
                {
                    msg = "Screening Test cleared successfully and your Admission Number is: " + Model.RegistrationId + "";
                    log.Activity = "Screenning cleared";
                    log.ActivityMessage = "Screening test of registration no " + Model.RegistrationId + " is cleared successfully.";
                    logActivityService.CreateLogActivity(log);
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFeeTypeExist(int SessionId, int CourseId)
        {
            return Json(registerService.GetFeeTypeExist(SessionId, CourseId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteRegistrationByRegNo(int RegNo)
        {
            return Json(registerService.DeleteRegistrationByRegNo(RegNo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReRegisterCandidate(int id)
        {
            RegistrationViewModel model = new RegistrationViewModel();
            if (id > 0)
            {
                model = registerService.GetReRegisterDetailById(id);
                model.RegistartionNo = registerService.MaxRegistartionNumber();
            }
            model.GenderList = new List<SJModel.SelectList>()
            {
                new SJModel.SelectList{Value="F" , Text="Female" },
                new SJModel.SelectList{Value="M" , Text="Male" }
            };

            if (model.CourseId == 0 || model.CourseId == 1)
            {
                model.EducationList = new List<SJModel.SelectList>()
                {
                     new SJModel.SelectList { Value = "10+2", Text = "10+2" },
                     new SJModel.SelectList { Value = "Graduate", Text = "Graduate" },
                     new SJModel.SelectList { Value = "Post-Graduate", Text = "Post-Graduate" }
                };
            }
            else
            {
                model.EducationList = new List<SJModel.SelectList>()
                {
                     new SJModel.SelectList { Value = "Graduate", Text = "Graduate" },
                     new SJModel.SelectList { Value = "Post-Graduate", Text = "Post-Graduate" }
                };
            }
            model.CourseList = addmissionService.GetCourseList();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetSessionList()
        {
            return Json(registerService.GetSessionList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChangeSessionYr(int Yr)
        {
            Session["CurrentYear"] = Yr;
            return Json(Yr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBatchOptionString()
        {
            return Json(addmissionService.GetBatchList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Export(ExportDataViewModel Model)
        {
            DataTableFilterModel filter = new DataTableFilterModel();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[12] { new DataColumn("Sr.No"),
                                            new DataColumn("Reg.No"),
                                            new DataColumn("Student Name"),
                                            new DataColumn("Email"),
                                            new DataColumn("Mobile"),
                                            new DataColumn("DOB"),
                                            new DataColumn("Gender"),
                                            new DataColumn("Payment"),
                                            new DataColumn("Course"),
                                            new DataColumn("Batch"),
                                            new DataColumn("Reg.Date"),
                                            new DataColumn("Screening")
                });

            int currDate = Convert.ToInt32(Session["CurrentYear"]);
            var ScreenningData = registerService.GetScreenningExportData(Model, currDate);
            if (ScreenningData.Count() > 0)
            {
                int i = 1;
                if (Model.IsSelected == "Stand-By")
                {
                    foreach (var item in ScreenningData)
                    {
                        dt.Rows.Add(i, item.RegistartionNo, item.StudentName, item.Email, item.Mobile, item.DOBStr, item.Gender, item.PaymentStatusStr, item.CourseName, item.BatchName, item.RegisterDate, Model.IsSelected);
                        i++;
                    }
                }
                else
                {
                    foreach (var item in ScreenningData)
                    {
                        dt.Rows.Add(i, item.RegistartionNo, item.StudentName, item.Email, item.Mobile, item.DOBStr, item.Gender, item.PaymentStatusStr, item.CourseName, item.BatchName, item.RegisterDate, item.ScreenningStatus);
                        i++;
                    }
                }
            }
            string handle = Guid.NewGuid().ToString();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= EmployeeReport.xlsx");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    TempData["handle"] = stream.ToArray();
                }
            }
            string FileName = "ScreeningAllCandidateList";
            if (Model.IsSelected == "Pending")
                FileName = "ScreeningPendingCandidateList";
            else if (Model.IsSelected == "Selected")
                FileName = "ScreeningSelectedCandidateList";
            else if (Model.IsSelected == "Rejected")
                FileName = "ScreeningRejectedCandidateList";
            else if (Model.IsSelected == "Stand-By")
                FileName = "ScreeningStandByCandidateList";
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult Download(string fileName)
        {
            if (TempData["handle"] != null)
            {
                byte[] data = TempData["handle"] as byte[];
                return File(data, "application/vnd.ms-excel", (fileName + ".xlsx"));
            }
            else
                return new EmptyResult();
        }

        [HttpGet]
        public ActionResult SaveCourseChange(int CourseId, int SessionYr, int RegNo, string OldCourse)
        {
            FeeManagementService _feeService = new FeeManagementService();
            int UserId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
            string ChangeCourseWith = _feeService.GetCourseNameByCourseId(CourseId);
            LogActivityViewModel log = new LogActivityViewModel();
            log.EnteredBy = UserId;
            log.EnteredDate = DateTime.Now;
            log.ActioName = "SaveCourseChange";
            log.ModuleName = "Screenning";
            log.ControllerName = "Registration";
            log.Activity = "Change Course";
            log.ActivityMessage = "Course changed from " + OldCourse + " to " + ChangeCourseWith + " regarded registration No. " + RegNo;
            LogActivityService logActivityService = new LogActivityService();
            logActivityService.CreateLogActivity(log);
            return Json(_feeService.SaveCourseChange(CourseId, SessionYr, RegNo, UserId, 0, OldCourse, null), JsonRequestBehavior.AllowGet);
        }
    }
}