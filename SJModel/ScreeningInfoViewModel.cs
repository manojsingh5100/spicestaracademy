using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class ScreeningInfoViewModel
    {
        public ScreeningTestViewModel ScreeningTest { get; set; }
        public List<RoleViewModel> GetDocumentList { get; set; }
        public List<RoleViewModel> GetTatooList { get; set; }
        public List<ScreeningParameterOptionViewModel> ParameterOption { get; set; }
        public List<ScreeningInterviewerResultViewModel> InterviewOption { get; set; }
        public List<SemesterMasterViewModel> GetBatchList { get; set; }
        public List<CourseMasterViewModel> GetCourseList { get; set; }
        public int RegistrationId { get; set; }
        public int RegId { get; set; }   // id of registration Master
        public string FName { get; set; }
        public string LName { get; set; }
        public string Age { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public DateTime? DOB { get; set; }

        public bool MStatus { get; set; }
        public bool? IsSelected { get; set; }
        public string Email { get; set; }
        public string ApplicationNo { get; set; }
        public bool IsStandBy { get; set; }
        public bool? IsScreeningClr { get; set; }
        public int CreatedBy { get; set; }
        public string SessionName {get;set;}
        public string CourseName { get; set; }
        public int? BatchId { get; set; }
        public string CreatedDate { get; set; }
        public int CourseId { get; set; }
        public int SessionYr { get; set; }
        public bool IsAnyCourseFeePay { get; set; }
    }
}
