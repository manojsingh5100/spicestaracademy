using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class RefundInformationViewModel
    {
        public RefundInformationViewModel()
        {
            CaseClosed = false;
        }
        public int Id { get; set; }
        public int ExceedFeeAmountOnCourseChangeId { get; set; }
        public decimal RefundAmount { get; set; }
        public bool CaseClosed { get; set; }
        public int EnteredBy { get; set; }
        public DateTime EnteredDate { get; set; }
        public string StatusRemark { get; set; }
        public string Status { get; set; }
        public string Header { get; set; }
        public bool IsExist { get; set; }
        public decimal ExeedAmount { get; set; }
        public string CurrentDate { get; set; }
        public int? AdmissionId { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
