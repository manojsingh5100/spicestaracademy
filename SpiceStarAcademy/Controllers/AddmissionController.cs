using SJModel;
using SJService;
using SpiceStarAcademy.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpiceStarAcademy.Controllers
{
    [AuthorizeActionFilterAttribute.LoggingFilterAttribute]
    [AuthorizeActionFilterAttribute.CustomExceptionFilter]
    public class AddmissionController : Controller
    {
        // GET: Addmission
        public AddmissionService addmissionService = null;
        public RegistrationService registerService = null;
        public LogActivityService logActivityService = null;
        public AddmissionController()
        {
            addmissionService = new AddmissionService();
            registerService = new RegistrationService();
            logActivityService = new LogActivityService();
        }
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult GetAddmissionList(DataTableFilterModel filter)
        {
            try
            {
                DataTableFilterModel dataFilter = addmissionService.GetApprovedStudentList(filter);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Addmission(int id)
        {
            RegistrationViewModel model = new RegistrationViewModel();
            if (id > 0)
                model = registerService.GetRegisterInfoById(id);
            model.SessionList = addmissionService.GetSessionList();
            model.CourseList = addmissionService.GetCourseList();
            model.IsAddmission = addmissionService.IsAdmissionOfCandidate(id);
            if (TempData["Message"] == null && model.IsAddmission)
                return RedirectToAction("Index", "Addmission");
            return View(model);
        }

        [HttpPost]
        public ActionResult Addmission(RegistrationViewModel model)
        {
            try
            {
                model.SessionList = addmissionService.GetSessionList();
                model.CourseList = addmissionService.GetCourseList();
                model.CreatedBy = Convert.ToInt32(Session["UserId"]);
                int AddMissionId = addmissionService.CreateAddmission(model);
                TempData["Message"] = AddMissionId;
                TempData["msg"] = "Admission created successfully!";
                return RedirectToAction("Addmission", "Addmission", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return View(model);
            }
        }


        public ActionResult AddmissionView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAddmissionViewList(DataTableFilterModel filter)
        {
            try
            {
                int currDate = Convert.ToInt32(Session["CurrentYear"]);
                DataTableFilterModel dataFilter = addmissionService.GetAddmissionViewList(filter, currDate);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentProfile(int Id)
        {
            AdmissionInfoViewModel model = new AdmissionInfoViewModel();
            model = addmissionService.GetAddmissionDetail(Id);
            model.MedicalStatusList = (from MedicalStatus e in Enum.GetValues(typeof(MedicalStatus))
                                       select new SemesterMasterViewModel
                                       {
                                           BatchName = e.ToString(),
                                           SemesterName = e.ToString()
                                       }).ToList();
            return View(model);
        }

        public FileResult DownloadResume(int AdmissionId)
        {
            string contentType = string.Empty;
            var info = addmissionService.GetAddmissionMasterInfo(AdmissionId);
            var fileExt = info.ResumeUrl.Split('.')[1].ToString();

            if (fileExt.ToLower() == "pdf")
            {
                contentType = "application/pdf";
            }
            else if (fileExt.ToLower() == "docx")
            {
                contentType = "application/docx";
            }
            else if (fileExt.ToLower() == "doc")
            {
                contentType = "application/docx";
            }
            return File(info.ResumeUrl, contentType, "Resume-" + info.RegNo + "." + fileExt.ToLower());
        }

        public ActionResult UpdateAdmissionBasicDatail(AddmissionMasterViewModel model, HttpPostedFileBase imageFile)
        {
            if (imageFile != null)
            {
                var ext = Path.GetExtension(imageFile.FileName);
                string fname = "Resume-" + model.Id + ext;
                string Newfname = Path.Combine(Server.MapPath("~/Resumes/"), fname);
                addmissionService.UpdateResumePath(model.Id, "/Resumes/" + fname);
                imageFile.SaveAs(Newfname);
            }
            var userId = Convert.ToInt32(Session["UserId"]);
            model.UpdatedBy = userId;
            var status = addmissionService.UpdateAdmissionBasicDetail(model);
            LogActivityViewModel log = new LogActivityViewModel();
            log.EnteredBy = Convert.ToInt32(Session["UserId"]);
            log.EnteredDate = DateTime.Now;
            log.ActioName = "UpdateAdmissionBasicDatail";
            log.ModuleName = "Admission";
            log.ControllerName = "Addmission";
            log.Activity = "Update Basic Detail";
            log.ActivityMessage = "Admission basic detail of registraion no " + model.RegNo + " updated.";
            log.RegistrationNo = model.RegNo;
            logActivityService.CreateLogActivity(log);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemovePhoto(int AdmissionId)
        {
            string fileName = addmissionService.RemovePhoto(AdmissionId);
            var filePath = Server.MapPath("~" + fileName);
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
                file.Delete();
            return Json("Delete", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveResume(int AdmissionId)
        {
            string fileName = addmissionService.RemoveResume(AdmissionId);
            var filePath = Server.MapPath("~" + fileName);
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
                file.Delete();
            return Json("Delete", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAdressDetail(AddressDetailViewModel model)
        {
            bool status = addmissionService.UpdateAddressDetail(model);
            LogActivityViewModel log = new LogActivityViewModel();
            log.EnteredBy = Convert.ToInt32(Session["UserId"]);
            log.EnteredDate = DateTime.Now;
            log.ActioName = "UpdateAdressDetail";
            log.ModuleName = "Admission";
            log.ControllerName = "Addmission";
            log.Activity = "Update Address Detail";
            log.ActivityMessage = "Admission address detail of registraion no " + model.RegNo + " updated.";
            log.RegistrationNo = Convert.ToInt32(model.RegNo);
            logActivityService.CreateLogActivity(log);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateMedicalDetail(MedicalDetailViewModel model)
        {
            bool status = addmissionService.UpdateMedicalDetail(model);
            LogActivityViewModel log = new LogActivityViewModel();
            log.EnteredBy = Convert.ToInt32(Session["UserId"]);
            log.EnteredDate = DateTime.Now;
            log.ActioName = "UpdateMedicalDetail";
            log.ModuleName = "Admission";
            log.ControllerName = "Addmission";
            log.Activity = "Update Medical Detail";
            log.ActivityMessage = "Admission medical detail of registraion no " + model.RegNo + " updated.";
            log.RegistrationNo = Convert.ToInt32(model.RegNo);
            logActivityService.CreateLogActivity(log);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        var ext = Path.GetExtension(file.FileName);
                        string fname = "Photo-" + Request.Form.Get("regNo") + ext;
                        string Newfname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        addmissionService.UpdateImagePath(Convert.ToInt32(Request.Form.Get("regNo")), "/Uploads/" + fname);
                        file.SaveAs(Newfname);
                    }
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public ActionResult BatchHistoryOpenModelPopup(int AdmissionNo)
        {
            return PartialView("_BatchHistoryModel", addmissionService.BatchHistoryOpenModelPopup(AdmissionNo));
        }

        //=======================================================================================================

        public ActionResult CandidateTermination()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetCandidateTerminationList(DataTableFilterModel filter)
        {
            try
            {
                DataTableFilterModel dataFilter = addmissionService.GetApprovedStudentList(filter);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TerminationResignationPopup(int AdmissionId, string ActionName)
        {
            TerminationViewModel model = new TerminationViewModel();
            return PartialView("_TerminationResignationPopup", addmissionService.TerminationResignationPopupInfo(AdmissionId, ActionName));
        }

        [HttpPost]
        public ActionResult AddTerminationResignationInfo(TerminationViewModel Model)
        {
            Model.EnteredBy = Convert.ToInt32(Session["UserId"]);
            Model.IsActive = true;
            Model.CandidateActionMasterId = addmissionService.GetCandidateADMStatusByName(Model.CandidateActionName);
            var data = addmissionService.AddTerminationResignationInfo(Model);
            SpiceStarAcademy.Models.Common c = new SpiceStarAcademy.Models.Common();
            c.TerminationResignationSheduler();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTerminationResignationInfo()
        {
            var data = addmissionService.GetTerminationResignationInfo();
            return View(data);
        }
    }
}