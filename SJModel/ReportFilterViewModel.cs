using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJModel.PerformanceModel;

namespace SJModel
{
    public class ReportFilterViewModel
    {
        public ReportFilterViewModel()
        {
            GetLeadSourceList = new List<RoleViewModel>();
        }
        public string CourseId { get; set; }
        public string BatchId { get; set; }
        public string CandidateName { get; set; }
        public int SessionId { get; set; }
        public string State { get; set; }
        public string City { get;set; }
        public string PaymentStatus { get; set; }
        public string PieClickPartName { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public int RegNo { get; set; }
        public int ReviewId { get; set; }
        public int BatchNo { get; set; }
        public int ddlWeeklyId { get; set; }

        public int Compare { get; set; }
        public int CompareYear { get; set; }
        public int CompareMonth { get; set; }
        public int QuarterMonth { get; set; }
        public int CompareQuarterMonth { get; set; }
        public int InstallmentNo { get; set; }


        public List<SemesterMasterViewModel> GetBatchList { get; set; }
        public List<CourseMasterViewModel> GetCourseList { get; set; }
        public List<RoleViewModel> MonthList { get; set; }
        public List<RoleViewModel> CompareList { get; set; }
        public List<RoleViewModel> MonthQuarterList { get; set; }
        public List<RoleViewModel> YearList { get; set; }
        public List<RoleViewModel> GetCandidateListById { get; set; }
        public List<ReviewMasterViewModel> GetReviewList { get; set; }
        public List<ReviewMasterViewModel> WeeklyTermList { get; set; }
        public List<RoleViewModel> GetLeadSourceList { get; set; }
        public List<SemesterMasterViewModel> GetParameterList { get; set; }

        public int TotalStudent { get; set; }
        public int TotalCourse { get; set; }
        public int TotalBatch { get; set; }
        public int[] BatchRangeArray { get; set; }     
        public decimal[] GetPieChartInfo { get; set;}
        public int PieIndex { get; set; }
        public dynamic GetPerformanceReport { get; set; }
        public string BatchJson { get; set; }
    }
}
