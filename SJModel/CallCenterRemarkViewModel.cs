using System;
using System.Collections.Generic;

namespace SJModel
{
    public class CallCenterRemarkViewModel
    {
        public int Id { get; set; }
        public int RegistrationNo { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
        public DateTime EnteredDate { get; set; }
        public string EnteredByName { get; set; }
        public string CreateDate { get; set; }
        public bool IsMedicalStandBy { get; set; }
        public int MedicalRemarkNum { get; set; }
        public string MedicalFitnessStatus { get; set; }
        public List<RoleViewModel> remarkList { get; set; }
        public List<RoleViewModel> MedicalStatusList { get; set; }
        public string StudentName { get; set; }
        public string Tag { get; set; }

        public string UserName { get; set; }
        public string EnterDateStr { get; set; }
    }
}
