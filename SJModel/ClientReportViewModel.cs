using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class ClientReportViewModel
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public DateTime InterViewDate { get; set; }
        public string DateOfInterView { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string FullName {
            get {
                return Fname + Lname;
            }
        }
        public DateTime? DOB { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string DateOfJoinning { get; set; }
        public string Age { get; set; }
        public string Addresee1 { get; set; }
        public string Addresee2 { get; set; }
        public string Addresee3 { get; set; }
        public string PinCode { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string DepartMent { get; set; }
        public string Location { get; set; }
        public string MedicalCenter { get; set; }
        public string NOC_PP { get; set; }
        public bool IsValidPassport { get; set; }
        public Nullable<DateTime> MDate { get; set; }
        public Nullable<DateTime> FDate { get; set; }
        public Nullable<DateTime> JDate { get; set; }
        public string MedicalDate { get; set; }
        public string FitnessDate { get; set; }
        public string JoiningDate { get; set; }
        public string MedicalStatus { get; set; }
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public int? SessionYr { get; set; }
    }
}
