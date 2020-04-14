using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PerformanceModel
{
    public class PerformanceEntryMasterViewModel
    {
        public int Id { get; set; }
        public int RegistrationNo { get; set; }
        public int ReviewId { get; set; }
        public Nullable<int> tblWeeklyReviewMaster { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public int tblPerformanceMasterId { get; set; }
        public Nullable<int> tblWeeklyReviewMasterId { get; set; }
        public Nullable<int> BatchId { get; set; }
        public Nullable<int> AdmBatchId { get; set; }
        public Nullable<int> AddmissionMasterId { get; set; }
        public string Percentage { get; set; }
        public string PerformanceReview { get; set; }
    }
}
