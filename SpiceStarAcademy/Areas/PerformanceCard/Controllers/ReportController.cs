using SJModel;
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
    public class ReportController : Controller
    {
        private PerformanceReportService _report = null;
        private AddmissionService _admissionService = null;
        private PerformanceService _perforamnce = null;
        public ReportController()
        {
            _report = new PerformanceReportService();
            _admissionService = new AddmissionService();
            _perforamnce = new PerformanceService();
        }

        public ActionResult Index(int? RegNo)
        {
            ReportFilterViewModel Model = new ReportFilterViewModel();
            Model.GetBatchList = _admissionService.GetBatchList().Where(w=>w.BatchName != "Batch 0").ToList();
            Model.GetReviewList = _perforamnce.GetReViewList().Where(p => p.Name != "Weekly Term").ToList();
            Model.MonthList = (from MonthList e in Enum.GetValues(typeof(MonthList))
                               select new RoleViewModel
                               {
                                   Id = (int)e,
                                   Name = e.ToString()
                               }).ToList();
            Model.YearList = _report.GetYearList();

            if (RegNo.HasValue)
            {
                var info = _report.GetInfoByRegNo(RegNo.Value);
                if (info != null)
                {
                    Model.BatchId = info.BatchId.ToString();
                    Model.BatchNo = info.BatchId;
                    Model.RegNo = info.Id;
                }
            }
            return View(Model);
        }

        public ActionResult WeeklyReport(int? RegNo)
        {
            ReportFilterViewModel Model = new ReportFilterViewModel();
            Model.GetBatchList = _admissionService.GetBatchList().Where(w => w.BatchName != "Batch 0").ToList();
            Model.GetReviewList = _perforamnce.GetReViewList();
            Model.MonthList = (from MonthList e in Enum.GetValues(typeof(MonthList))
                               select new RoleViewModel
                               {
                                   Id = (int)e,
                                   Name = e.ToString()
                               }).ToList();
            Model.YearList = _report.GetYearList();
            if (RegNo.HasValue)
            {
                var info = _report.GetInfoByRegNo(RegNo.Value);
                if (info != null)
                {
                    Model.BatchId = info.BatchId.ToString();
                    Model.RegNo = info.Id;
                }
            }
            Model.ReviewId = 1;   // For Weekly Term
            Model.WeeklyTermList = _perforamnce.GetWeeklyTermTypeListById(1);
            return View(Model);
        }

        [HttpPost]
        public JsonResult GetCandidateListByBatchId(int BatchId)
        {
            return Json(_report.GetCandidateListByBatchIdForReport(BatchId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAssesmentChartInfo(ReportFilterViewModel Model)
        {
            return Json(_report.GetBarChartData(Model), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubParameterData(string ParameterType)
        {
            return Json(_report.GetSubParameterData(ParameterType), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAssesmentCompareChartInfo(ReportFilterViewModel Model)
        {
            return Json(_report.GetAssesmentCompareChartInfo(Model), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPerformanceCard(string batch, long type)
        {
            return Json(_report.GetPerformancReport(batch, type), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPerformanceCardOnConsolidatedReport(string batch, long type)
        {
            return Json(_report.GetPerformancReport(_report.GetBatchIdByNameId(batch), type), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompareReport()
        {
            ReportFilterViewModel Model = new ReportFilterViewModel();
            Model.GetBatchList = _admissionService.GetBatchList().Where(w=>w.BatchName != "Batch 0").ToList();
            Model.GetReviewList = _perforamnce.GetReViewList().Where(p => p.Name != "Weekly Term").ToList();
            Model.MonthList = (from MonthList e in Enum.GetValues(typeof(MonthList))
                               select new RoleViewModel
                               {
                                   Id = (int)e,
                                   Name = e.ToString()
                               }).ToList().Where(l => l.Name != "All").ToList();
            Model.CompareList = (from CompareType e in Enum.GetValues(typeof(CompareType))
                                 select new RoleViewModel
                                 {
                                     Id = (int)e,
                                     Name = e.ToString().Replace('_', ' ')
                                 }).ToList();
            Model.MonthQuarterList = (from MonthQurterList e in Enum.GetValues(typeof(MonthQurterList))
                                      select new RoleViewModel
                                      {
                                          Id = (int)e,
                                          Name = e.ToString().Replace('_', ',')
                                      }).ToList();
            Model.YearList = _report.GetYearList();
            return View(Model);
        }

        public ActionResult DashBoard()
        {
            ReportFilterViewModel Model = new ReportFilterViewModel();
            int currDate = Convert.ToInt32(Session["CurrentYear"]);
            Model.GetBatchList = _admissionService.GetBatchList().Where(w=>w.BatchName != "Batch 0").ToList();
            Model.GetReviewList = _perforamnce.GetReViewList();
            Model.TotalBatch = _report.GetNumberOfBatch();
            Model.TotalCourse = _report.GetNumberOfCourse();
            Model.TotalStudent = _report.GetNumberOfStudentBySession(currDate);
            Model.BatchRangeArray = _report.GetBatchListArray();
            Model.ReviewId = 3;
            return View(Model);
        }

        [HttpPost]
        public JsonResult GetPieChartInfo(ReportFilterViewModel obj)
        {
            return Json(_report.GetPieChartData(obj), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSelectedTopCandidates(ReportFilterViewModel obj)
        {
            return Json(_report.GetSelectedTopCandidates(obj), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PerformanceReport()
        {
            ReportFilterViewModel Model = new ReportFilterViewModel();
            Model.GetBatchList = _admissionService.GetBatchList().Where(w=>w.BatchName != "Batch 0").ToList();
            Model.GetReviewList = _perforamnce.GetReViewList().Where(p => p.Name != "Weekly Term").ToList();
            Model.GetCourseList = _admissionService.GetCourseList().OrderBy(o => o.CourseName).ToList();
            Model.GetParameterList = _report.GetParameterList();
            return View(Model);
        }
        public JsonResult GetPerformanceReportByBatch(int batchid,int reviewid)
        {
            return Json(_report.GetPerformanceReportList(batchid,reviewid), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsolidatedReport()
        {
            ReportFilterViewModel Model = new ReportFilterViewModel();
            Model.GetReviewList = _perforamnce.GetReViewList().Where(p => p.Name != "Weekly Term").ToList();
            Model.BatchJson = Newtonsoft.Json.JsonConvert.SerializeObject(_admissionService.GetBatchList().Where(w=>w.BatchName != "Batch 0").Select(item=> new SemesterMasterViewModel {
                Id = item.Id,
                BatchName = item.BatchName,
                SemesterName = item.BatchName.Split(' ')[1]
            }).ToList());
            Model.BatchRangeArray = _report.GetBatchListArray();
            Model.TotalBatch = _report.GetNumberOfBatch();
            return View(Model);
        }

        [HttpPost]
        public JsonResult GetBatchBarChartInfo(ReportFilterViewModel obj)
        {
            return Json(_report.GetBatchChartData(obj), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetBatchCandidateDataByBatch(ReportFilterViewModel obj)
        {
            return Json(_report.GetBatchCandidateDataByBatch(obj), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Getdatabycandidatename(string BatchName,string CandidateName,int ReviewId)
        {
            return Json(_report.Getdatabycandidatename(BatchName, CandidateName, ReviewId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStudentDataById(int RegistraionNo, string BatchId,  int ReviewId)
        {
            return Json(_report.GetSelectedCandidateOnConsolidatedpage(RegistraionNo, _report.GetBatchIdByName(BatchId), ReviewId), JsonRequestBehavior.AllowGet);
        }
    }
}