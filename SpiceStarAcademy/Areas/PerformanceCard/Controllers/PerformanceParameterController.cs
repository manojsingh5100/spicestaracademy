using SJModel;
using SJModel.PerformanceModel;
using SJService;
using SpiceStarAcademy.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpiceStarAcademy.Areas.PerformanceCard.Controllers
{
    [AuthorizeActionFilterAttribute.LoggingPerFormanceFilter]
    public class PerformanceParameterController : Controller
    {
        public ParameterTypeService _parameterTypeService = null;

        public PerformanceParameterController()
        {
            _parameterTypeService = new ParameterTypeService();
        }
        // GET: PerformanceCard/PerformanceParameterType

        public ActionResult Index()
        {
            return View(new ParameterTypeViewModel());
        }

        [HttpPost]
        public ActionResult GetParameterTypeList(DataTableFilterModel filter)
        {
            try
            {
                DataTableFilterModel dataFilter = _parameterTypeService.GetParemeterTypeList(filter);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            { }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUpadteParameterType(ParameterTypeViewModel Model)
        {
            return Json(_parameterTypeService.AddUpadteParameterType(Model), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetParameterTyeInfoById(int Id)
        {
            return Json(_parameterTypeService.GetParameterTyeInfoById(Id));
        }

        [HttpPost]
        public ActionResult GetParameterList(DataTableFilterModel filter,int ParameterTypeId)
        {
            try
            {
                DataTableFilterModel dataFilter = _parameterTypeService.GetParemeterList(filter, ParameterTypeId);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            { }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        // GET: PerformanceCard/PerformanceParameter
        public ActionResult CreateParameter(int Id)
        {
            ParameterListViewModel Model = new ParameterListViewModel();
            Model.ParameterTypeName = _parameterTypeService.ParameterTypeNameById(Id); 
            Model.tblParameterTypeId = Id;
            return View(Model);
        }

        [HttpPost]
        public JsonResult GetParameterInfoById(int Id)
        {
            return Json(_parameterTypeService.GetParameterInfoById(Id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ParameterSaveUpdate(ParameterListViewModel Model)
        {
            try
            {
                var data = _parameterTypeService.ParameterSaveUpdate(Model);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Model.Message = ex.Message + " " + ex.InnerException;
                return Json(Model, JsonRequestBehavior.AllowGet);
            }
        }
    }
}