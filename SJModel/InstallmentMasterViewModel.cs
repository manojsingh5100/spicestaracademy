using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class InstallmentMasterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeeCollection { get; set; }
        public decimal Amount { get; set; }
        public dynamic CandidateDetail { get; set; }
        public bool Temp { get; set; }
        public bool TakeFeeInstallmentStatus { get; set; }

        public Nullable<int> InstallMentId { get; set; }
        public DateTime EnterDate { get; set; }
        public string FeeName { get; set; }
        public string SessionName { get; set; }
    }
}
