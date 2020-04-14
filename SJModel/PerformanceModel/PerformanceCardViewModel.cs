using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PerformanceModel
{
    public class PerformanceCardViewModel
    {
        public int RegNo { get; set; }
        public int ReviewId { get; set; }
        public int WeeklyTermId { get; set; }
        public int PerformanceMasterId { get; set; } 
        public int UserId { get; set; }

        public string StudentName { get; set; }
        public int? BatchId { get; set; }
        public string BatchName { get; set; }
        public string PhotoPath { get; set; }
        public List<ParameterTypeViewModel> ParmeterTypeList { get; set; }
        public List<ReviewMasterViewModel> ReviewList { get; set; }
        public List<ReviewMasterViewModel> WeeklyTypeList { get; set; }

        public List<ParameterTypeInfoList> ParameterTypeInfoList { get; set; }
        public string WeeklyArr { get; set; }
        public string ReviewArr { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string GetRatingData { get; set; }
        public string UIN { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public int AdmissionId { get; set; }

        public string Percentage { get; set; }
        public string PerformanceReview { get; set; }
        public List<PercentageInfoViewModel> PercentageCritiriaInfo { get; set; }
        public string PercentageCritiriaJsonInfo { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
    }

    public class ParameterInfoList
    {
        public int Rating { get; set; }
        public int TblParameterId { get; set; }
        public string Remarks { get; set; }
        public string ReviewName { get; set; }
    }

    public class ParameterTypeInfoList
    {
        public int PerformanceMasterId { get; set; }
        public int ParameterTypeId { get; set; }
        public List<ParameterInfoList> ParameterInfoList { get; set; }
        public string ParameterType { get; set; }
        public int? BatchId { get; set; }

        public int ReviewId { get; set; }
        public DateTime EnteredDate { get; set; }
        public int RegNo { get; set; }
        public bool IsApplicable { get; set; }
        public int PtId { get; set; }
    }

    public class PerformanceCardListDetail
    {
        public int ReviewId { get; set; }
        public int? WeeklyTermId { get; set; }
        public string ReviewName { get; set; }
        public string WeeklyTermName { get; set; }
    }
}
