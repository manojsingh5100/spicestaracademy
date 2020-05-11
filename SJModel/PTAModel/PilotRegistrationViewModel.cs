using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PTAModel
{
    public class PilotRegistrationViewModel
    {
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
        //public string DOBStr { get; set; }
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
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public string PilotRegistartionNo { get; set; }

        public int RegistartionNo { get; set; }
        public string StudentName
        {
            get
            {
                return string.Format("{0} {1}", UppercaseFirst(Fname), UppercaseFirst(Lname));
            }
        }
        public string DateOfBirth { get; set; }
        public string RegisterDate
        {
            get
            {
                return string.Format("{0}", (RegistrationDate.HasValue ? RegistrationDate.Value.Day.ToString().PadLeft(2, '0') + "/" + RegistrationDate.Value.Month.ToString().PadLeft(2, '0') + "/" + RegistrationDate.Value.Year : ""));
            }
        }

        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string SourceOfCandidate { get; set; }
        public dynamic PilotRegInfo { get; set; }
        public string SessionName { get; set; }
        public int SessionId { get; set; }
        public double? TenthCGPA { get; set; }
        public double? TwelveCGPA { get; set; }
        public string ResultStatus { get; set; }
        public int ScreeningInfoId { get; set; }
        public bool? IsClearScreening { get; set; }
        public string ApplicationNo { get; set; }
        public int ScreeningExamFeeNo { get; set; }
        public int PilotRegistrationId { get; set; }
        public int IsScrreningExamFeeEmailNotificationNo { get; set; }
        public int ExamAmount { get; set; }
        public int ExamTerm { get; set; }
        public bool IsSendEmail { get; set; }
        public bool LastExamFailedStatus { get; set; }
        public int ScreeningTestResultCount { get; set; }
        public int MedicalImagesCount { get; set; }
        public int ScreeningAmountTerm { get; set; }
        public List<PTAScreeningExamFeeInfo> ptaScreeningExamFeeInfo { get; set; }
    }

    public class PTAScreeningExamFeeInfo
    {
        public int Id { get; set; }
        public int ptaScreeningInfoID { get; set; }
        public int ScreeningAmountTerm { get; set; }
        public int ScreeningAmount { get; set; }
        public DateTime DateOfAmount { get; set; }
        public string PaymentId { get; set; }
        public string ExamResult { get; set; }
        public int PtaScreeningTestTypeId { get; set; }
        public DateTime EnterendDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int PTAPilotRegistrationMaterId { get; set; }
    }
}
