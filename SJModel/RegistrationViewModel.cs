using SJModel.PerformanceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SJModel
{
    public class RegistrationViewModel
    {
        public RegistrationViewModel()
        {
            Education = "10+2";
            SourceOfCandidate = "";
        }
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string DOBStr
        {
            get
            {
                return string.Format("{0}", (DOB.HasValue ? DOB.Value.Day.ToString().PadLeft(2, '0') + "/" + DOB.Value.Month.ToString().PadLeft(2, '0') + "/" + DOB.Value.Year : ""));
            }
        }
        public string Gender { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Width { get; set; }
        public string CorrespondenceAddress { get; set; }
        public string CorrespondenceCity { get; set; }
        public string CorrespondenceState { get; set; }
        public string CorrespondenceZip { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentState { get; set; }
        public string PermanentZip { get; set; }
        public bool IsAppeared { get; set; }
        public string PassingYear { get; set; }
        public bool IsPassport { get; set; }
        public string PaymentId { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public bool PaymentStatus { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public int RegistartionNo { get; set; }
        public bool IsAddmission { get; set; }
        public string Education { get; set; }
        public bool IsConsultantCandidate { get; set; }
        public string StudentName
        {
            get
            {
                return string.Format("{0} {1}", UppercaseFirst(Fname), UppercaseFirst(Lname));
            }
        }
        public string PayStatus { get; set; }
        public string DateOfBirth
        {
            get
            {
                return string.Format("{0}", (DOB.HasValue ? DOB.Value.Day.ToString().PadLeft(2, '0') + "/" + DOB.Value.Month.ToString().PadLeft(2, '0') + "/" + DOB.Value.Year : ""));
            }
        }
        public string DateOfBirthNew { get; set; }
        public string RegisterDate
        {
            get
            {
                return string.Format("{0}", (RegistrationDate.HasValue ? RegistrationDate.Value.Day.ToString().PadLeft(2, '0') + "/" + RegistrationDate.Value.Month.ToString().PadLeft(2, '0') + "/" + RegistrationDate.Value.Year : ""));
            }
        }
        public string PaymentStatusStr { get; set; }
        public bool IsRejected { get; set; }
        public bool IsStandBy { get; set; }
        public int BatchId { get; set; }
        public string BatchName { get; set; }

        public List<SelectList> GenderList { get; set; }
        public List<SelectList> EducationList { get; set; }
        public List<SessionMasterViewModel> SessionList { get; set; }
        public List<CourseMasterViewModel> CourseList { get; set; }
        public List<SemesterMasterViewModel> SemesterList { get; set; }
        public int? SessionId { get; set; }
        public int? CourseId { get; set; }
        public int SemesterId { get; set; }
        public Nullable<int> AddMissionId { get; set; }
        public bool IsFeePayment { get; set; }
        public bool IsRefunded { get; set; }
        public string ModOfPayment { get; set; }
        public string CourseName { get; set; }
        public string AmityEnNo { get; set; }
        public int CreatedBy { get; set; }
        public string MedicalRemark { get; set; }
        public string MedicalStatus { get; set; }
        public int SrNo { get; set; }
        public string SessionName { get; set; }
        public bool IsMedicalStandBy { get; set; }
        public bool IsFeePayStandBy { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string AdmissionDateStr { get { return string.Format("{0}", AdmissionDate.Day.ToString().PadLeft(2, '0') + "/" + AdmissionDate.Month.ToString().PadLeft(2, '0') + "/" + AdmissionDate.Year); } }
        public bool IsHRCandidate { get; set; }

        public int ChSessionId { get; set; }
        public Nullable<bool> IsMedicalClear { get; set; }
        public Nullable<bool> IsScreenningClear { get; set; }
        public string ScreenningStatus { get; set; }
        public int CurrentYear { get; set; }
        public List<PerformanceCardListDetail> PerformanceCardList { get; set; }
        public bool IsReregister { get; set; }
        public string page { get; set; }
        public bool IsResistrationHistory { get; set; }
        public string ApplicationNo { get; set; }
        public bool IsActive { get; set; }
        public string SourceOfCandidate { get; set; }
        public List<CourseMasterViewModel> GetSourceList { get; set; }
        public string ReRegisterHistoryDate
        {
            get
            {
                return string.Format("{0}", (RegistrationDate.HasValue ? RegistrationDate.Value.Day.ToString().PadLeft(2, '0') + "/" + RegistrationDate.Value.Month.ToString().PadLeft(2, '0') + "/" + RegistrationDate.Value.Year : ""));
            }
        }
        public string RegistrationNoStr { get; set; }
        public int? SessionYr { get; set; }
        public bool IsAnyCourseFeePay { get; set; }
        public DateTime? BatchCreationDate { get; set; }
        public DateTime? DateOfWithdrawal { get; set; }
        public string DateOfWithdrawalStr
        {
            get
            {
                return string.Format("{0}", (DateOfWithdrawal.HasValue ? DateOfWithdrawal.Value.Day.ToString().PadLeft(2, '0') + "/" + DateOfWithdrawal.Value.Month.ToString().PadLeft(2, '0') + "/" + DateOfWithdrawal.Value.Year : ""));
            }
        }
        public string UserFname { get; set; }
        public string UserLname { get; set; }
        public string FullUserName
        {
            get
            {
                return string.Format("{0} {1}", UppercaseFirst(UserFname), UppercaseFirst(UserLname));
            }
        }

        public int ExceedPaymentRefundId { get; set; }
        public string RefundStatus { get; set; }
        public string IsPartWiseOrFullPay { get; set; }
        public string RefundRemark { get; set; }

        public decimal? FirstInstallment { get; set; }
        public decimal? SecondInstallment { get; set; }
        public decimal? ThirdInstallment { get; set; }
        public decimal? FeeTotalAmount { get; set; }
        public decimal? FeeDueAmount { get; set; }
        public DateTime? BatchCommencementDate { get; set; }
        public string FeeDueDate { get; set; }
        public bool ShowMedicalConsultPopUp { get; set; }
    }
}
