using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJModel;
using SJModel.PerformanceModel;
using SJService;
using SpiceStarAcademy.Helper;
using SpiceStarAcademy.Filter;

namespace SpiceStarAcademy.Areas.PerformanceCard.Controllers
{
    [AuthorizeActionFilterAttribute.LoggingPerFormanceFilter]
    public class PerformanceController : Controller
    {
        // GET: PerformanceCard/Performance
        private PerformanceService _perforamnce = null;
        private AddmissionService _admissionService = null;
        public PerformanceController()
        {
            _perforamnce = new PerformanceService();
            _admissionService = new AddmissionService();
        }

        public ActionResult Index(int RegNo, string Review)
        {
            var Model = _perforamnce.GetCandidatePerformanceIntialPageInfo(RegNo, Review);
            Model.ReviewList = _perforamnce.GetReViewList();
            var Option = _perforamnce.DisablePerformanceOption(RegNo, Model.BatchId.Value);
            Model.ReviewArr = Option.ReviewArr;
            Model.WeeklyArr = Option.WeeklyArr;
            if (!string.IsNullOrEmpty(Review))
            {
                string[] arr = Review.Split('-');
                Model.ReviewId = Convert.ToInt32(arr[0]);
                if (arr.Length == 2)
                    Model.WeeklyTermId = Convert.ToInt32(arr[1]);
            }
            return View(Model);
        }

        public ActionResult CandidateList()
        {
            PerformanceFilterViewModel model = new PerformanceFilterViewModel();
            model.BatchList = _admissionService.GetBatchList().Where(w => w.BatchName != "Batch 0").ToList();
            model.CouresList = _admissionService.GetCourseList();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetWeeklyTermList(int Id)
        {
            return Json(_perforamnce.GetWeeklyTermTypeListById(Id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCandidateList(DataTableFilterModel filter)
        {
            try
            {
                DataTableFilterModel dataFilter = _perforamnce.GetCandidateList(filter);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            { }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePerformanceCardDetail(PerformanceCardViewModel Model)
        {
            Model.UserId = Convert.ToInt32(Session["UserId"]);
            var result = _perforamnce.SavePerformanceCardDetail(Model);
            if (result.IsSuccess)
                TempData["msg"] = result.Massage;
            var tblPerformanceEntryMasterId = Model.tblPerformanceEntryMasterId.ToString();
            string Template = Email.GetDynamicTemplateForPerformance(tblPerformanceEntryMasterId,Model.Percentage, Model.ReviewArr, Model.WeeklyArr);
            TestEmail email = new TestEmail(Template, Model.ReviewArr + " Result Notification", "nagma.khetarpal@spicejet.com");
            email.SendTestingMail();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsParmeterKeyExist(string KeyId)
        {
            return Json(_perforamnce.IsParmeterKeyExist(KeyId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePercentageCritiria(int BatchId, int ReviewId)
        {
            return Json(_perforamnce.UpdatePercentageCritiria(BatchId, ReviewId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenBatchRegrateModelPopup(int RegistrationNo)
        {
            return PartialView("_ReligatedInfoPopUp", _perforamnce.GetBatchReligateDetails(RegistrationNo));
        }

        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel Model)
        {
            Model.Email = Session["Email"].ToString();
            return View(_perforamnce.ChangePasswordService(Model));
        }
    }
}