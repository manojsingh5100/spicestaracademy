using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PTAModel
{
    public class PilotMedicalInfoViewModel
    {
        public int PilotRegistrationNo { get; set; }
        public List<PilotDocumentMasterViewModel> Documents { get; set; }
        public int OptionOfDM { get; set; }
        public int AdmissionId { get; set; }
    }

    public class PilotDocumentMasterViewModel
    {
        public int Id { get; set; }
        public int DocumentMasterId { get; set; }
        public string DocumentPath { get; set; }
        public string Extention { get; set; }
        public bool IsActive { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public string DocumentName { get; set; }
        public string IsPreviousExist { get; set; }
        public int PilotAdmissionId { get; set; }
    }
}
