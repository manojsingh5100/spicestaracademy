using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJModel;
using SJService.PTA;

namespace SpiceStarAcademy.Areas.PTA.Controllers
{
    public class DashBoardController : Controller
    {
        DashBoardService dashBoardService = null;
        public DashBoardController()
        {
            dashBoardService = new DashBoardService();
        }
        // GET: PTA/DashBoard
        public ActionResult Index()
        {
            return View(dashBoardService.GetDashBoardData());
        }
    }
}