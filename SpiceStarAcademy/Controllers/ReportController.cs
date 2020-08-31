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
    public class ReportController : Controller
    {
        AddmissionService addmissionService = null;
        ReportService reportService = null;
        RegistrationService registrationService = null;
        public ReportController()
        {
            addmissionService = new AddmissionService();
            reportService = new ReportService();

        }
        // GET: Report
        public ActionResult Index()
        {
            ReportFilterViewModel model = new ReportFilterViewModel
            {
                GetBatchList = addmissionService.GetBatchList(),
                GetCourseList = addmissionService.GetCourseList(),
                MonthList = (from MonthList e in Enum.GetValues(typeof(MonthList))
                             select new RoleViewModel
                             {
                                 Id = (int)e,
                                 Name = e.ToString()
                             }).ToList(),
                YearList = reportService.GetYearList(),
            };
            model.GetLeadSourceList = addmissionService.GetLeadSourceList();
            model.PieClickPartName = "General";
            return View(model);
        }

        public ActionResult SSAReport()
        {
            ReportFilterViewModel model = new ReportFilterViewModel
            {
                GetBatchList = addmissionService.GetBatchList(),
                GetCourseList = addmissionService.GetCourseList(),
                MonthList = (from MonthList e in Enum.GetValues(typeof(MonthList))
                             select new RoleViewModel
                             {
                                 Id = (int)e,
                                 Name = e.ToString()
                             }).ToList(),
                YearList = reportService.GetYearList()
            };
            return View(model);
        }

        public ActionResult RevenueReport()
        {
            ReportFilterViewModel model = new ReportFilterViewModel
            {
                GetBatchList = addmissionService.GetBatchList(),
                GetCourseList = addmissionService.GetCourseList(),
                MonthList = (from MonthList e in Enum.GetValues(typeof(MonthList))
                             select new RoleViewModel
                             {
                                 Id = (int)e,
                                 Name = e.ToString()
                             }).ToList(),
                YearList = reportService.GetYearList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GetCandidateReport(DataTableFilterModel filter)
        {
            try
            {
                int currDate = Convert.ToInt32(Session["CurrentYear"]);
                DataTableFilterModel dataFilter = addmissionService.GetCandidateReport(filter, currDate);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSSAReportList(DataTableFilterModel filter)
        {
            try
            {
                int currDate = Convert.ToInt32(Session["CurrentYear"]);
                DataTableFilterModel dataFilter = reportService.GetSSAReportList(filter, currDate);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBatchList()
        {
            return Json(addmissionService.GetBatchList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSessionList()
        {
            return Json(addmissionService.GetSessionList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourseList()
        {
            return Json(addmissionService.GetCourseList(), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetSessionListByCourseId(int CourseId)
        //{
        //    return Json(new FeeManagementService().GetSessionListByCourseId(CourseId), JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetChartData(ReportFilterViewModel FilterModel)
        {
            return Json(reportService.GetFilterizeChartData(FilterModel), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GeneralReport()
        {
            return View(addmissionService.GetBatchList());
        }

        [HttpPost]
        public ActionResult GetGeneralReportList(DataTableFilterModel filter, string Tag = null)
        {
            try
            {
                int currDate = Convert.ToInt32(Session["CurrentYear"]);
                Session["filter"] = Newtonsoft.Json.JsonConvert.SerializeObject(filter);
                DataTableFilterModel dataFilter = reportService.GetScreenningReportList(filter, currDate);
                return Json(new { draw = filter.draw, recordsFiltered = dataFilter.recordsFiltered, recordsTotal = dataFilter.recordsTotal, data = dataFilter.data },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}