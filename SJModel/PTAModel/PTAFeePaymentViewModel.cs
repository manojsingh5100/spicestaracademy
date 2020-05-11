using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PTAModel
{
    public class PTAFeePaymentViewModel
    {
        public int Id { get; set; }
        public int PTAFeePaymentDetailId { get; set; }
        public string RecieptNo { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public int EnteredBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string PanNumber { get; set; }
        public Decimal? Amount { get; set; }
        public int PTAInstallmentMasterId { get; set; }

        public string ApplicationNo { get; set; }
        public string CandidateName { get; set; }
        public string SessionName { get; set; }
        public string CourseName { get; set; }
        public string Education { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string Mobile { get; set; }
        public string Batch { get; set; }
        public string Gender { get; set; }
        public DateTime CurrentTime { get; set; }
    }
}
