using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class AdmissionInfoViewModel
    {
        public AddmissionMasterViewModel AdmissionDetail { get; set; }
        public MedicalDetailViewModel MedicalDetail { get; set;}
        public AddressDetailViewModel AddressDetail { get; set; }
        public List<SemesterMasterViewModel> BatchList { get; set; }
        public List<SemesterMasterViewModel> MedicalStatusList { get; set; }
        public FeeEmailNotificationViewModel GetFeeDetailOfCandidate { get; set; }
    }
}
