using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SJModel;
using SJService;
using SpiceStarAcademy.Filter;

namespace SpiceStarAcademy.ControllersAddFeeType
{
    [AuthorizeActionFilterAttribute.LoggingFilterAttribute]
    public class FeeManagementController : Controller
    {
        public FeeManagementService feeManegementService = null;
        public FeeManagementController()
        {
            feeManegementService = new FeeManagementService();
        }
        // GET: FeeManagement
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetFeeDetailList(int RegNo)
        {
            return PartialView("_FeePayment", feeManegementService.GetFeeDetailList(RegNo));
        }

        public ActionResult FeePayment(int Id, int AdmissionId, string Name, string Tag)
        {
            AddmissionMasterViewModel model = new AddmissionMasterViewModel();
            model.RegNo = Id;
            model.StudentName = Name;
            model.AdmissionId = AdmissionId;
            var data = feeManegementService.GetFeeDataByRegId(Id);
            if (data != null && data.Id > 0)
            {
                model.FeePaymentList = data.FeePaymentList;
                model.Id = data.Id;
                model.Remark = data.Remark;
                model.ModOfPayment = data.ModOfPayment;
            }
            model.Tag = Tag == "" ? "Paid" : "Refund";
            return PartialView("_FeePayment", model);
        }

        public ActionResult FeeRefunded(int Id, int AdmissionId, string Name)
        {
            AddmissionMasterViewModel model = new AddmissionMasterViewModel();
            model.RegNo = Id;
            model.StudentName = Name;
            model.AdmissionId = AdmissionId;
            var data = feeManegementService.GetRefundedFeeDataByRegId(Id);
            if (data != null && data.Id > 0)
            {
                model.FeePaymentList = data.FeePaymentList;
                model.Id = data.Id;
                model.Remark = data.Remark;

            }
            model.Tag = "Refund";
            return PartialView("_FeePayment", model);
        }

        public ActionResult SaveRegistrationFee(int Id, string PaymentId, decimal Fee, string MOP)
        {
            var status = false;
            if (Fee >= 1000)
            {
                status = feeManegementService.UpdateRegistrationFee(Id, PaymentId, Fee, MOP);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SaveFeePayment(AddmissionMasterViewModel model)
        //{
        //    model.CreatedBy = Convert.ToInt32(Session["UserId"]);
        //    model.UpdatedBy = Convert.ToInt32(Session["UserId"]);
        //    var data = feeManegementService.SaveFee(model);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult FeeRefund()
        {
            return View();
        }


        //=========================
        public ActionResult AddFeeType()
        {
            FeeTypeDetailViewModel model = new FeeTypeDetailViewModel();
            model.FeeTypeList = feeManegementService.GetFeeTypeList();
            model.SessionNameList = feeManegementService.GetSessionList();
            model.CourseList = feeManegementService.GetCourseList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddFeeType(FeeTypeDetailViewModel model)
        {
            model.EnteredBy = Convert.ToInt32(Session["UserId"]);
            TempData["msg"] = feeManegementService.AddUpdateFeeTypeDetail(model);
            return Redirect("AddFeeType");
        }

        [HttpGet]
        public ActionResult GetInstallmetsByFeetypedetailId(int FeeTypeDetailId)
        {
            var data = feeManegementService.GetInstallmetsByFeetypedetailId(FeeTypeDetailId);
            return PartialView("_AddUpdateInstallment", data);
        }

        [HttpPost]
        public ActionResult SaveUpdateInstallment(SessionInstallmentMasterViewModel Model)
        {
            int UserId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
            Model.EnteredBy = UserId;
            return Json(feeManegementService.SaveUpdateInstallment(Model), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetFeeTypeDetailsList(DataTableFilterModel filter)
        {
            try
            {
                DataTableFilterModel dataFilter = feeManegementService.GetFeeTypeDetailList(filter);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFeeTypeDetailById(int FeeTypeDetailId)
        {
            return Json(feeManegementService.GetFeeTypeDetailById(FeeTypeDetailId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FeeCandidatesList()
        {
            return View();
        }

        public JsonResult GetPaymentById(string Id)
        {
            return Json(JsonConvert.SerializeObject(new RazorServices().GetPaymentById(Id)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAmountByFeeType(int FeeTypeDeatilId)
        {
            return Json(feeManegementService.GetAmountByFeeTypeDetail(FeeTypeDeatilId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddFeeCollection(int Id, Nullable<int> PT)
        {
            var data = feeManegementService.FeeCollectionPaymentDetail(Id);
            AddmissionService _admissionService = new AddmissionService();
            data.GetCourseList = _admissionService.GetCourseList();
            data.RecieptNo = "RC#" + data.RegistrationInfo.RegistartionNo;
            data.PT = PT;
            return View(data);
        }

        public ActionResult IsFeePaymentStandBy(bool IsFeePayStandBy, int RegNo)
        {
            return Json(feeManegementService.IsFeePaymentStandBy(IsFeePayStandBy, RegNo), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CapitalAndPiecePaymentBalance(CheckBalanceViewModel Model)
        {
            return Json(feeManegementService.CapitalAndPiecePaymentBalance(Model.RegNo, Model.PaymentTypeId, Model.FeeTypeDeatilId, Model.InstallmentId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveFeeCollectionAndPayment(FeePaymentDetailViewModel Model)
        {
            int UserId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
            Model.EnteredBy = UserId;
            Model.EnteredDate = (!string.IsNullOrEmpty(Model.SettleDate) ? DateTime.ParseExact(Model.SettleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now);
            TempData["errorMsg"] = "Fee successfully submitted.";
            LogActivityViewModel log = new LogActivityViewModel();
            log.EnteredBy = Convert.ToInt32(Session["UserId"]);
            log.EnteredDate = DateTime.Now;
            log.ActioName = "SaveFeeCollectionAndPayment";
            log.ModuleName = "FeeCollection";
            log.ControllerName = "FeeManagement";
            log.Activity = "Taking Fee";
            log.ActivityMessage = "Fee paid successfully for registration no " + Model.FeeDetail.RegistrationNo + "";
            log.RegistrationNo = Model.FeeDetail.RegistrationNo;
            LogActivityService logActivityService = new LogActivityService();
            logActivityService.CreateLogActivity(log);
            if (Model.hdnFeeDetailId > 0)
                return Json(feeManegementService.SaveFeeCollectionAndPartWisePayment(Model), JsonRequestBehavior.AllowGet);
            else
                return Json(feeManegementService.SaveFeeCollectionAndPayment(Model), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepositFeeByCourseId(int RegistrationNo, int FeeTypeDetailId, int SessionMasterId, int CourseId)
        {
            return Json(feeManegementService.GetDepositFeeByCourseYr(RegistrationNo, FeeTypeDetailId, SessionMasterId, CourseId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSessionListByCourseId(int CourseId)
        {
            return Json(feeManegementService.GetSessionListByCourseId(CourseId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsAdmissionFeePay(int RegistrationNo, int FeeTypeDetailId, int SessionMasterId, int CourseId)
        {
            var result = feeManegementService.IsAdmissionFeePay(RegistrationNo, FeeTypeDetailId, SessionMasterId, CourseId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFeeCollectionRecordByRecieptNo(string RecieptNo, int SessionYr, int CourseId,int FeeDetailId, DateTime? BatchStartTime)
        {
            return Json(feeManegementService.GetFeeCollectionRecordByRecieptNo(RecieptNo, SessionYr, CourseId, FeeDetailId, BatchStartTime), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUpdateNotication(int RegistrationNo)
        {
            return Json(feeManegementService.GetNotificatioInfo(RegistrationNo), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendFeeEmailOneByOne(int RegNo)
        {
            // var ss = feeManegementService.SendFeeEmailOneByOne(RegNo, 4, 1);
            return Json(RegNo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CandidateCourseChange(int CourseId, int SessionYr)
        {
            return Json(feeManegementService.CandidateCourseChange(CourseId, SessionYr), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SaveCourseChange(int CourseId, int SessionYr, int RegNo, int FeeDetailId, string OldCourse, string Remark)
        {
            int UserId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
            string ChangeCourseWith = feeManegementService.GetCourseNameByCourseId(CourseId);
            LogActivityViewModel log = new LogActivityViewModel();
            log.EnteredBy = UserId;
            log.EnteredDate = DateTime.Now;
            log.ActioName = "SaveCourseChange";
            log.ModuleName = "FeeCollection";
            log.ControllerName = "FeeManagement";
            log.Activity = "Change Course";
            log.ActivityMessage = "Course changed from " + OldCourse + " to " + ChangeCourseWith + " regarded registration No. " + RegNo;
            log.RegistrationNo = RegNo;
            LogActivityService logActivityService = new LogActivityService();
            logActivityService.CreateLogActivity(log);
            string msg = feeManegementService.SaveCourseChange(CourseId, SessionYr, RegNo, UserId, FeeDetailId, OldCourse, Remark);
            if (msg != "")
                TempData["errorMsg"] = msg;
            else
                TempData["errorMsg"] = "Inner exception error!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRecieptDetail(string RecieptNo)
        {
            return Json(feeManegementService.GetRecieptDetail(RecieptNo), JsonRequestBehavior.AllowGet);
        }

        // Refund Code
        public ActionResult Refund()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetRefundDetailsList(DataTableFilterModel filter)
        {
            try
            {
                DataTableFilterModel dataFilter = feeManegementService.GetRefundDetailsList(filter);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OpenRefundPopup(int Id,int? AdmissionId)
        {
            var data = new RefundInformationViewModel();
            return PartialView("_RefundAR", feeManegementService.OpenRefundPopup(Id, AdmissionId));
        }

        [HttpPost]
        public ActionResult SaveApprovedAndReject(RefundInformationViewModel Model)
        {
            int UserId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
            Model.EnteredBy = UserId;
            return Json(feeManegementService.SaveApprovedAndReject(Model), JsonRequestBehavior.AllowGet);
        }
    }
}