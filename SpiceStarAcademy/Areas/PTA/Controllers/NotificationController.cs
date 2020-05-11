using SJService.PTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpiceStarAcademy.Areas.PTA.Controllers
{
    public class NotificationController : Controller
    {
        private NotificationService _NotificationService = null;

        public NotificationController()
        {
            _NotificationService = new NotificationService();
        }
        // GET: PTA/Notification
        public ActionResult Index()
        {
            return View();
        }

        //*****************************GetScreeningList*******************************************//

        public JsonResult GetPilotCandidateScreeningList()
        {
            try
            {
                return Json(_NotificationService.GetPilotCandidateScreenningList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //*************************************GEtAdmissionList***************************************//
        public JsonResult GetPilotAdmissionDetails()
        {
            try
            {
                return Json(_NotificationService.GetPilotAdmissionList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}