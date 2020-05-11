using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJService.PTA;
using SJModel.PTAModel;


namespace SpiceStarAcademy.Areas.PTA.Controllers
{
    public class StudentProfileController : Controller
    {
        // GET: PTA/StudentProfile
        private StudentProfileService _studentService = null;
        public StudentProfileController()
        {
            _studentService = new StudentProfileService();
        }
        public ActionResult Index()
        {
            string ApplNo = Convert.ToString(Session["ApplicationNo"]);
            var data = new StudentInfoViewModel();
            data = _studentService.GetStudentProfile(ApplNo);
            data.CandidateName = Session["UserName"].ToString();
            data.GetBoardAndUniversityList = _studentService.GetBoardAndUniversityList();
            data.GetEvaluationTypeList = _studentService.GetEvaluationList();
            data.GetBoardAndUniversityListUG = _studentService.GetBoardAndUniversityListUG();
            data.GetStream = _studentService.GetStreamView();
            data.Resultstatusinfo = _studentService.GetResultStatusView();
            data.ptaDocumentDetails = _studentService.GetDocumentDetails(ApplNo);
            data.CourseList = _studentService.GetCourseList();
            data.FeeTypeList = _studentService.GetFeeTypeList();
            return View(data);
        }

        [HttpPost]
        public ActionResult candidateUploadFiles()
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
                        string fname = "Photo-" + Request.Form.Get("AppNo");
                        string Fullfname = Path.Combine(Server.MapPath("~/Areas/PTA/CandidateUploadFiles/"), fname + ".jpg");
                        string Fullfname1 = Path.Combine(Server.MapPath("~/Areas/PTA/CandidateUploadFiles/"), fname + ".jpeg");
                        string Fullfname2 = Path.Combine(Server.MapPath("~/Areas/PTA/CandidateUploadFiles/"), fname + ".png");
                        if (System.IO.File.Exists(Fullfname))
                            System.IO.File.Delete(Fullfname);

                        if (System.IO.File.Exists(Fullfname1))
                            System.IO.File.Delete(Fullfname1);

                        if (System.IO.File.Exists(Fullfname2))
                            System.IO.File.Delete(Fullfname2);
                        //StudentProfileService.UploadImagePath(Convert.ToInt32(Request.Form.Get("AppNo")), "/Areas/PTA/CandidateUploadFiles/" + fname);
                        file.SaveAs(Path.Combine(Server.MapPath("~/Areas/PTA/CandidateUploadFiles/"), fname + ext));
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
                return Json("No File selected");
            }
        }

        [HttpPost]
        public ActionResult RemovePhoto(string ApplicationNo)
        {
            string filePath = Server.MapPath("~/Areas/PTA/CandidateUploadFiles/Photo-" + ApplicationNo + ".jpg");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return Json("Delete", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Defaultimageload(string Application)
        {
            var ImagePath = "";
            ImagePath = Server.MapPath("~/Areas/PTA/CandidateUploadFiles/Photo-" + Application);
            if (System.IO.File.Exists(ImagePath + ".jpg"))
                ImagePath = "/Areas/PTA/CandidateUploadFiles/Photo-" + Application + ".jpg";
            else if (System.IO.File.Exists(ImagePath + ".jpeg"))
                ImagePath = "/Areas/PTA/CandidateUploadFiles/Photo-" + Application + ".jpeg";
            else if (System.IO.File.Exists(ImagePath + ".png"))
                ImagePath = "/Areas/PTA/CandidateUploadFiles/Photo-" + Application + ".png";
            else
                ImagePath = "";
            return Json(ImagePath, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveFile(int DocumentId)
        {
            return Json(_studentService.DeleteFile(DocumentId), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateAddressDetails(PTAAddressInfoViewModel Model)
        {
            return Json(_studentService.UpdateAddressDetails(Model), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateBasicsDetails(PilotRegistrationViewModel Model)
        {
            return Json(_studentService.UpdateBasicDetails(Model), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FillCityCountry(string Id, string Country = null, string State = null, string District = null, string City = null)
        {
            var data = _studentService.GetCityCountryInfoList(Country, Id, State, District, City);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // ****************Upload Medical(17/03/20)*************//

        [HttpGet]
        public ActionResult GetPartialMedicalInfo()
        {
            int Id = Convert.ToInt32(Session["ptaPilotRegistrationMasterId"]);
            return PartialView("_PilotMedical", _studentService.InitialMedicalInfo(Id));
        }

        [HttpPost]
        public ActionResult SaveMedicalUploadData()
        {
            PilotDocumentMasterViewModel Pic1Data = new PilotDocumentMasterViewModel();
            PilotDocumentMasterViewModel Pic2Data = new PilotDocumentMasterViewModel();
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                string ApplNo = Convert.ToString(Session["ApplicationNo"]);
                int AdmissionId = Convert.ToInt32(Session["AdmissionId"]);
                int Id = Convert.ToInt32(Session["ptaRegistrationInfoId"]);
                int PilotRegId = Convert.ToInt32(Session["ptaPilotRegistrationMasterId"]);
                Pic1Data.ApplicationNo = ApplNo;
                Pic2Data.ApplicationNo = ApplNo;
                Pic1Data.DocumentMasterId = Convert.ToInt32(Request.Form["Document1"]);
                Pic1Data.IsPreviousExist = Request.Form["hdnDoc1IsPrevoiusExist"].ToString();
                HttpPostedFile Pic1 = System.Web.HttpContext.Current.Request.Files["ImageF"];
                string FolderPath = Server.MapPath("/Areas/PTA/CandidateUploadFiles/" + ApplNo);
                if (!(Directory.Exists(FolderPath)))
                    Directory.CreateDirectory(FolderPath);
                if (Pic1 != null)
                {
                    Pic1Data.DocumentName = "Class1";
                    Pic1Data.Extention = Path.GetExtension(Pic1.FileName);
                    Pic1Data.DocumentPath = "/Areas/PTA/CandidateUploadFiles/" + ApplNo + "/" + Pic1Data.DocumentName;
                    Pic1Data.EnteredStudentBy = UserId;
                    Pic1Data.ptaPilotRegistrationMasterId = PilotRegId;
                    Pic1Data.PilotAdmissionId = AdmissionId;
                    Pic1.SaveAs(FolderPath + "/" + Pic1Data.DocumentName + Pic1Data.Extention);
                }
                Pic2Data.DocumentMasterId = Convert.ToInt32(Request.Form["Document2"]);
                Pic2Data.IsPreviousExist = Request.Form["hdnDoc2IsPrevoiusExist"].ToString();
                HttpPostedFile Pic2 = System.Web.HttpContext.Current.Request.Files["ImageS"];
                if (Pic2 != null)
                {
                    Pic2Data.DocumentName = "Class2";
                    Pic2Data.Extention = Path.GetExtension(Pic2.FileName);
                    Pic2Data.DocumentPath = "/Areas/PTA/CandidateUploadFiles/" + ApplNo + "/" + Pic2Data.DocumentName;
                    Pic2Data.EnteredStudentBy = UserId;
                    Pic2Data.ptaPilotRegistrationMasterId = PilotRegId;
                    Pic2Data.PilotAdmissionId = AdmissionId;
                    Pic2.SaveAs(FolderPath + "/" + Pic2Data.DocumentName + Pic2Data.Extention);
                }
            }
            return Json(_studentService.SaveMedicalUploadFiles(new List<PilotDocumentMasterViewModel> { Pic1Data, Pic2Data }), JsonRequestBehavior.AllowGet);
        }

        // ****************Upload Education(17/03/20)*************//

        [HttpPost]
        public ActionResult SaveOtherFilesUploadData()
        {
            List<PilotDocumentMasterViewModel> Model = new List<PilotDocumentMasterViewModel>();
            int RegId = Convert.ToInt32(Request.Form["CandidateId"]);
            int UserId = Convert.ToInt32(Session["UserId"]);
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                string ApplNo = Convert.ToString(Session["ApplicationNo"]);
                int AdmissionId = Convert.ToInt32(Session["AdmissionId"]);
                int Id = Convert.ToInt32(Session["ptaRegistrationInfoId"]);
                int PilotRegId = Convert.ToInt32(Session["ptaPilotRegistrationMasterId"]);
                string FolderPath = Server.MapPath("/Areas/PTA/CandidateUploadFiles/" + ApplNo);
                if (!(Directory.Exists(FolderPath)))
                    Directory.CreateDirectory(FolderPath);
                for (int i = 0; i < System.Web.HttpContext.Current.Request.Files.Count; i++)
                {
                    HttpPostedFile Pic = System.Web.HttpContext.Current.Request.Files[i];
                    if (Pic != null)
                    {
                        PilotDocumentMasterViewModel Pic1Data = new PilotDocumentMasterViewModel();
                        Pic1Data.Id = RegId;
                        Pic1Data.DocumentName = "Other";
                        Pic1Data.Extention = Path.GetExtension(Pic.FileName);
                        Pic1Data.DocumentPath = "/Areas/PTA/CandidateUploadFiles/" + ApplNo + "/" + Pic.FileName;
                        Pic1Data.EnteredStudentBy = UserId;
                        Pic1Data.PilotAdmissionId = AdmissionId;
                        Pic1Data.ptaPilotRegistrationMasterId = PilotRegId;
                        // Pic1Data.ptaRegistrationInfoId = Id;
                        Pic.SaveAs(FolderPath + "/" + Pic1Data.DocumentName + Pic1Data.Extention);
                        Model.Add(Pic1Data);
                    }
                }
            }
            return Json(_studentService.SaveOtherFilesUploadData(Model), JsonRequestBehavior.AllowGet
             );
        }

        [HttpGet]
        public ActionResult GetPartialOtherDocsInfo()
        {
            int Id = Convert.ToInt32(Session["ptaPilotRegistrationMasterId"]);
            int AdmissionId = Convert.ToInt32(Session["AdmissionId"]);
            return PartialView("_PilotOtherDocs", _studentService.InitialOtherUploadDocsInfo(Id, AdmissionId));
        }
        public ActionResult AddFeeType()
        {
            StudentInfoViewModel model = new StudentInfoViewModel();
            model.FeeTypeList = _studentService.GetFeeTypeList();
            model.SessionNameList = _studentService.GetSessionList();
            model.CourseList = _studentService.GetCourseList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddFeeType(StudentInfoViewModel model)
        {
            TempData["msg"] = _studentService.AddUpdateFeeTypeDetail(model);
            return Redirect("AddFeeType");
        }


        public JsonResult GetSessionListByCourseId(int CourseId)
        {
            return Json(_studentService.GetSessionListByCourseId(CourseId), JsonRequestBehavior.AllowGet);
        }
    }
}