using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class AddmissionMasterViewModel
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string Education { get; set; }
        public bool IsAppeared { get; set; }
        public string PassingYear { get; set; }
        public bool IsValidPassport { get; set; }
        public int CourseId { get; set; }
        public Nullable<System.DateTime> AddmissionDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string StudentName { get { return string.Format("{0} {1}", Fname, Lname); } set { } }
        public string CourseName { get; set; }
        public string SessionName { get; set; }
        public string BatchName { get; set; }
        public string DateOfBirth { get; set; }
        public int RegNo { get; set; }
        public string DateOfAddmission { get; set; }
        public string ResumeUrl { get; set; }

        public List<FeePaymentViewModel> FeePaymentList { get; set; }
        public string Remark { get; set; }
        public decimal Total { get; set; }
        public string ImageUrl { get; set; }

        public string Tag { get; set; }
        public int AdmissionId { get; set; }
        public string ModOfPayment { get; set; }
        public string TransectionId { get; set; }
        public string AmityEnNo { get; set; }
        public int? BatchId { get; set; }

        public string AccountNumber { get; set; }
        public string BeneficiaryName { get; set; }
        public string BankName { get; set; }
        public string AccountType { get; set; }
        public string BankAddress { get; set; }
        public string IFSCCode { get; set; }
        public DateTime BankIssueDate { get; set; }
        public string IssueDate { get; set; }
        public string MedicalStatus { get; set; }
        public string BatchDescription { get; set; }
        public int BatchfromId { get; set; }
        public bool IsBatchHistory { get; set; }
        public string RegisterEmail { get; set; }
        public decimal? FeeCapitalAmount { get; set; }
        public List<PaymentInfoViewModel> PaymentDetailList { get; set; }
        public string IsTermResgCandidate { get; set; }
    }
}
