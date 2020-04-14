using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PTAModel
{
    public class PilotScreeningViewModel
    {
        public int Id { get; set; }
        public int PTAPilotRegistrationMasterId { get; set; }
        public int PTARegistrationNo { get; set; }
        public string Remark { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public List<PilotScreeningTestResultViewModel> PilotScreeningTestResult { get; set; }
        public PilotRegistrationViewModel PilotRegistrationInfo { get; set; }

        // For Intial 
        public List<PilotScreeningTestTypeViewModel> GetPilotTrainingTestTypeList { get; set; }
        public int PilotScreeningTestResultId { get; set; }
        public int PilotScreeningInfoId { get; set; }
    }

    public class PilotScreeningTestTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<double> InitialMark { get; set; }
        public double ObtainMark { get; set; }
        public bool IsActive { get; set; }
        public bool IsPassed { get; set; }
        public bool? IsSelected { get; set; }
        public string AgainExam { get; set; }
        public int ptaScreenningTestTypeId { get; set; }
    }

    public class PilotScreeningTestResultViewModel
    {
        public int Id { get; set; }
        public double ObtainMark { get; set; }
        public bool IsPassed { get; set; }
        public bool IsActive { get; set; }
        public PilotScreeningTestTypeViewModel ScreenigTestType { get; set; }
    }

    public class MessageForSucccessful
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string RegNo { get; set; }
    }
}
