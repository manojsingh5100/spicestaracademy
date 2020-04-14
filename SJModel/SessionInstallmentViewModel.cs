using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class SessionInstallmentViewModel
    {
        public List<SessionInstallmentMasterViewModel> InstallmentList { get; set; }
        public int FeeTypeDeatilId { get; set; }
        public decimal CapitalAmount { get; set; }
        public List<SemesterMasterViewModel> GetInstallmentList { get; set; }
    }
}
