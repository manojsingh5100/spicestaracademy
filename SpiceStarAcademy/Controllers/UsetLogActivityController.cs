using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJService;

namespace SpiceStarAcademy.Controllers
{
    public class UsetLogActivityController : Controller
    {
        private LogActivityService logActivityService = null;
        public UsetLogActivityController()
        {
            logActivityService = new LogActivityService();
        }
        // GET: UsetLogActivity
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTopActivityToshowOnDehboard()
        {
            return Json(logActivityService.GetTopFiveActities(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTopThreeActivity(string ModuleName)
        {
            return Json(logActivityService.GetTopThreeActities(ModuleName), JsonRequestBehavior.AllowGet);
        }
    }
}