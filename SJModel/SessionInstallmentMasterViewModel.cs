using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class SessionInstallmentMasterViewModel
    {
        public int Id { get; set; }
        public int InstallmentMasterId { get; set; }
        public int FeeTypeDetailId { get; set; }
        public decimal Amount { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EneteredDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string InstallmentName { get; set; }
    }
}
