using SpiceStarAcademy.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SJService;
using SJModel;

namespace SpiceStarAcademy.Controllers
{
    [AuthorizeActionFilterAttribute.LoggingFilterAttribute]
    public class AdminController : Controller
    {
        AdminService _adminService = null;
        public AdminController()
        {
            _adminService = new AdminService();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetBatchListinfo(DataTableFilterModel filter)
        {
            try
            {
                DataTableFilterModel dataFilter = _adminService.GetAdmissionBatchListinfo(filter);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult OpenBatchModelPopup(int Id)
        {
            return PartialView("_BatchModal", _adminService.GetBatchDataById(Id));
        }

        public JsonResult AddUpdateBatchMaster(RoleViewModel Model)
        {
            Model.EnteredBy = Convert.ToInt32(Session["UserId"]);
            return Json(_adminService.AddUpdateBatchMaster(Model), JsonRequestBehavior.AllowGet);
        }
    }
}