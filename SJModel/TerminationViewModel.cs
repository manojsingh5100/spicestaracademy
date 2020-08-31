using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class TerminationViewModel
    {
        public int Id { get; set; }
        public int AddmissionMasterId { get; set; }
        public int CandidateActionMasterId { get; set; }
        public string CandidateActionName { get; set; }
        public int CandidateTerminationResignationInfoId { get; set; }
        public string StatusInfo { get; set; }
        public string SchedulingDate { get; set; }
        public bool IsActive { get; set; }
        public int RegNo { get; set; }
        public string StudentName { get; set; }
        public int EnteredBy { get; set; }
        public string TerminationOrResignationDate { get; set; }
        public string OutstandingFeeAmount { get; set; }
        public DateTime? SchedulingDatestr { get; set; }
        public string NoticePeriod { get; set; }
        public bool CandidateTerminationResignationIsActive { get; set; }
        public bool IsClosed { get; set; }
        public string ClosedComment { get; set; }
        public string DoneBy { get; set; }
        public string RefundStatus { get; set; }
        public bool IsFeePayment { get; set; }
        public string FundStatus { get; set; }

        public decimal? FeeDueAmount { get; set; }
    }
}
