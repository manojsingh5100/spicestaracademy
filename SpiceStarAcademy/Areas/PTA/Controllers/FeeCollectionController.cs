using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJModel;
using SJService.PTA;

namespace SpiceStarAcademy.Areas.PTA.Controllers
{
    public class FeeCollectionController : Controller
    {
        public FeeCollectionService feeCollectionService = null;
        public FeeCollectionController()
        {
            feeCollectionService = new FeeCollectionService();
        }
        // GET: PTA/FeeCollection
        public ActionResult Index(int Id)
        {
            var data = feeCollectionService.FeeCollectionPaymentDetail(Id);
            data.RecieptNo = "RC#" + data.RegistrationInfo.RegistrationNoStr;
            return View(data);
        }



        public ActionResult CreateFee()
        {
            FeeTypeDetailViewModel model = new FeeTypeDetailViewModel();
            model.FeeTypeList = feeCollectionService.GetFeeTypeList();
            model.SessionNameList = feeCollectionService.GetSessionList();
            model.CourseList = feeCollectionService.GetCourseList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddFeeType(FeeTypeDetailViewModel model)
        {
            TempData["msg"] = feeCollectionService.AddUpdateFeeTypeDetail(model);
            return Redirect("CreateFee");
        }

        [HttpPost]
        public ActionResult GetFeeTypeDetailsList(DataTableFilterModel filter)
        {
            try
            {
                DataTableFilterModel dataFilter = feeCollectionService.GetFeeTypeDetailList(filter);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSessionListByCourseId(int CourseId)
        {
            return Json(feeCollectionService.GetSessionListByCourseId(CourseId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFeeTypeDetailById(int FeeTypeDetailId)
        {
            return Json(feeCollectionService.GetFeeTypeDetailById(FeeTypeDetailId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAmountByFeeType(int FeeTypeDeatilId)
        {
            return Json(feeCollectionService.GetAmountByFeeTypeDetail(FeeTypeDeatilId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsAdmissionFeePay(int AdmissionId, int FeeTypeDetailId, int SessionMasterId, int CourseId)
        {
            var result = feeCollectionService.IsAdmissionFeePay(AdmissionId, FeeTypeDetailId, SessionMasterId, CourseId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepositFeeByCourseId(int AdmissionId, int FeeTypeDetailId, int SessionMasterId, int CourseId)
        {
            return Json(feeCollectionService.GetDepositFeeByCourseYr(AdmissionId, FeeTypeDetailId, SessionMasterId, CourseId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFeeCollectionRecordByRecieptNo(string RecieptNo)
        {
            return Json(feeCollectionService.GetFeeCollectionRecordByRecieptNo(RecieptNo), JsonRequestBehavior.AllowGet);
        }
    }
}