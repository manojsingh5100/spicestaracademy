using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJModel.PTAModel;
using SJService.PTA;

namespace SpiceStarAcademy.Areas.PTA.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: PTA/Registration
        private RegistrationService _registrationService = null;
        public RegistrationController()
        {
            _registrationService = new RegistrationService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPilotCandidateList()
        {
            try
            {
                return Json(_registrationService.GetPilotCandidateList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Screening()
        {
            return View();
        }

        public JsonResult GetPilotCandidateScreeningList()
        {
            try
            {
                return Json(_registrationService.GetPilotCandidateScreenningList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetPartialScreeningInfo(int RegNo)
        {
            return PartialView("_PilotScreening", _registrationService.IntialScreeningInfo(RegNo));
        }

        [HttpPost]
        public JsonResult SaveUpdatePilotScreeningInfo(PilotScreeningViewModel Model)
        {
            Model.EnteredBy = Convert.ToInt32(Session["UserId"]);
            return Json(_registrationService.SaveUpdatePilotScreeningInfo(Model), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPartialMedicalInfo(int Id)
        {
            return PartialView("_PilotMedical", _registrationService.InitialMedicalInfo(Id));
        }

        [HttpPost]
        public ActionResult SaveMedicalUploadData()
        {
            PilotDocumentMasterViewModel Pic1Data = new PilotDocumentMasterViewModel();
            PilotDocumentMasterViewModel Pic2Data = new PilotDocumentMasterViewModel();
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                int RegId = Convert.ToInt32(Request.Form["CandidateId"]);
                Pic1Data.Id = RegId;
                Pic2Data.Id = RegId;
                Pic1Data.DocumentMasterId = Convert.ToInt32(Request.Form["Document1"]);
                Pic1Data.IsPreviousExist = Request.Form["hdnDoc1IsPrevoiusExist"].ToString();
                HttpPostedFile Pic1 = System.Web.HttpContext.Current.Request.Files["ImageF"];
                string FolderPath = Server.MapPath("~/PilotCandidateDocs/" + RegId);
                if (!(Directory.Exists(FolderPath)))
                    Directory.CreateDirectory(FolderPath);
                if (Pic1 != null)
                {
                    Pic1Data.DocumentName = "Class1";
                    Pic1Data.Extention = Path.GetExtension(Pic1.FileName);
                    Pic1Data.DocumentPath = "/PilotCandidateDocs/" + RegId + "/" + Pic1Data.DocumentName;
                    Pic1Data.EnteredBy = UserId;
                    Pic1.SaveAs(FolderPath + "/" + Pic1Data.DocumentName + Pic1Data.Extention);
                }
                Pic2Data.DocumentMasterId = Convert.ToInt32(Request.Form["Document2"]);
                Pic2Data.IsPreviousExist = Request.Form["hdnDoc2IsPrevoiusExist"].ToString();
                HttpPostedFile Pic2 = System.Web.HttpContext.Current.Request.Files["ImageS"];
                if (Pic2 != null)
                {
                    Pic2Data.DocumentName = "Class2";
                    Pic2Data.Extention = Path.GetExtension(Pic2.FileName);
                    Pic2Data.DocumentPath = "/PilotCandidateDocs/" + RegId + "/" + Pic2Data.DocumentName;
                    Pic2Data.EnteredBy = UserId;
                    Pic2.SaveAs(FolderPath + "/" + Pic2Data.DocumentName + Pic2Data.Extention);
                }
            }
            return Json(_registrationService.SaveMedicalUploadFiles(
                new List<PilotDocumentMasterViewModel>
                { Pic1Data, Pic2Data }), JsonRequestBehavior.AllowGet
                );
        }

        [HttpGet]
        public ActionResult GetPartialOtherDocsInfo(int Id,int AdmissionId)
        {
            return PartialView("_PilotOtherDocs", _registrationService.InitialOtherUploadDocsInfo(Id, AdmissionId));
        }

        [HttpPost]
        public ActionResult SaveOtherFilesUploadData()
        {
            List<PilotDocumentMasterViewModel> Model = new List<PilotDocumentMasterViewModel>();
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                int RegId = Convert.ToInt32(Request.Form["CandidateId"]);
                int AdmissionId = Convert.ToInt32(Request.Form["AdmissionId"]);
                string FolderPath = Server.MapPath("~/PilotCandidateDocs/" + RegId);
                if (!(Directory.Exists(FolderPath)))
                    Directory.CreateDirectory(FolderPath);
                for (int i = 0; i < System.Web.HttpContext.Current.Request.Files.Count; i++)
                {
                    HttpPostedFile Pic = System.Web.HttpContext.Current.Request.Files[i];
                    if (Pic != null)
                    {
                        PilotDocumentMasterViewModel Pic1Data = new PilotDocumentMasterViewModel();
                        Pic1Data.Id = RegId;
                        Pic1Data.DocumentName = Path.GetFileNameWithoutExtension(Pic.FileName);
                        Pic1Data.Extention = Path.GetExtension(Pic.FileName);
                        Pic1Data.DocumentPath = "/PilotCandidateDocs/" + RegId + "/" + Pic.FileName;
                        Pic1Data.EnteredBy = UserId;
                        Pic1Data.PilotAdmissionId = AdmissionId;
                        Pic.SaveAs(FolderPath + "/" + Pic1Data.DocumentName + Pic1Data.Extention);
                        Model.Add(Pic1Data);
                    }
                }
            }
            return Json(_registrationService.SaveOtherFilesUploadData(Model), JsonRequestBehavior.AllowGet
             );
        }

        [HttpPost]
        public JsonResult SendExamFeeNotification(int[] RegNoArr)
        {
            SpiceStarAcademy.Models.Common cm = new SpiceStarAcademy.Models.Common();
            return Json(cm.SendExamFeeNotificationFromPilotRegister(RegNoArr), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendExamFeeNotificationAgain(int[] RegNoArr)
        {
            SpiceStarAcademy.Models.Common cm = new SpiceStarAcademy.Models.Common();
            return Json(cm.SendExamFeeNotificationFromPilotRegisterAgain(RegNoArr), JsonRequestBehavior.AllowGet);
        }
    }
}