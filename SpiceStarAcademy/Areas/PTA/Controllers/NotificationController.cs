using SJModel.PTAModel;
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

        [HttpPost]
        public JsonResult SendScreeningEmailNotificationContent(int[] RegNoArr, string Content)
        {
            var templatestring = SpiceStarAcademy.Helper.PerformanceEmail.GetDynamicTemplateForPtaScreening(Content);
            SpiceStarAcademy.Helper.PerformanceTestEmail email = new SpiceStarAcademy.Helper.PerformanceTestEmail(templatestring, "Test Email", "soft.singhmanoj@gmail.com", "sksandeepshine@gmail.com");
            email.SendTestingMail();
            return Json(_NotificationService.SendExamFeeNotificationContent(RegNoArr, Content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendScreeningEmailNotification()
        {
            return PartialView("_FeePaymentEmailNotification");
        }

        [HttpGet]
        public JsonResult GetPaymentTermDropDown(long? id)
        {
            IEnumerable<PilotRegistrationViewModel> pp = null;

            if (id == null)
            {
                pp = _NotificationService.GetScreeningPaymentTerm().ToList().GroupBy(x => x.ScreeningAmountTerm).Select(f => f.First());
            }
            else if (id != null)
            {
                pp = _NotificationService.GetPilotCandidateScreenningList().Where(x => x.ScreeningAmountTerm == id);
            }
            return Json(pp, JsonRequestBehavior.AllowGet);
        }
    }
}
