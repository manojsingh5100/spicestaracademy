using SJModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJService;
using SpiceStarAcademy.Filter;

namespace SpiceStarAcademy.Controllers
{
    [AuthorizeActionFilterAttribute.LoggingFilterAttribute]
    public class CallCenterInfoController : Controller
    {
        private CallCenterInfoService callCenterInfoService = null;
        public CallCenterInfoController()
        {
            callCenterInfoService = new CallCenterInfoService();
        }
        // GET: CallCenterInfo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCallCenterInfo(DataTableFilterModel filter)
        {
            try
            {
                int currDate = Convert.ToInt32(Session["CurrentYear"]);
                DataTableFilterModel dataFilter = callCenterInfoService.GetCallCenterInfoList(filter, currDate);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCallCenterRemarks(DataTableFilterModel filter,int RegNo)
        {
            try
            {
                DataTableFilterModel dataFilter = callCenterInfoService.GetCallCenterRemarkList(filter, RegNo);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemarkOpenModelPopup(int RegNo)
        {
            CallCenterRemarkViewModel model = new CallCenterRemarkViewModel();
            model.RegistrationNo = RegNo;
            return PartialView("_Remark", model);
        }

        [HttpPost]
        public ActionResult SaveCallCenterRemarks(int RegNo, string Remark)
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            bool status = callCenterInfoService.SaveCallCenterRemarks(RegNo,Remark, userId);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

    }
}