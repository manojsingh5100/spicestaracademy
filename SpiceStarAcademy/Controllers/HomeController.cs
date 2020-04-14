using SJModel;
using SJService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpiceStarAcademy.Controllers
{
    public class HomeController : Controller
    {
        private RegistrationService registerService = null;
        public HomeController()
        {
            registerService = new RegistrationService();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult shdgfj()
        {
            return View();
        }
    }
}