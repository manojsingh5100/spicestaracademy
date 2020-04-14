using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJService.PTA;

namespace SpiceStarAcademy.Areas.PTA.Controllers
{
    public class AdmissionController : Controller
    {
        AdmissionPilotService _admissionService = null;
        public AdmissionController()
        {
            _admissionService = new AdmissionPilotService();
        }
        // GET: PTA/Admission
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPilotAdmissionCandidateList()
        {
            try
            {
                return Json(_admissionService.GetPilotAdmissionCandidateList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}