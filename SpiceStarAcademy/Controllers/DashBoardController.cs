using SJService;
using SpiceStarAcademy.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpiceStarAcademy.Controllers
{
    [AuthorizeActionFilterAttribute.LoggingFilterAttribute]
    public class DashBoardController : Controller
    {
        // GET: DashBoard
        private RegistrationService registerService = null;
        public DashBoardController()
        {
            registerService = new RegistrationService();
        }
        public ActionResult Index()
        {
            int currDate = Convert.ToInt32(Session["CurrentYear"]);
            return View(registerService.GetDesktopData(currDate));
        }
    }
}