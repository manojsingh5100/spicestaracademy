using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PerformanceModel
{
    public class ReligateInfoViewModel
    {
        public int Id { get; set; }
        public string FromBatchName { get; set; }
        public string ToBatchName { get; set; }
        public int RegistrationNo { get; set; }
        public int AdmissionId { get; set; }
        public int FromBatchId { get; set; }
        public int ToBatchId { get; set; }
        public List<PerformanceCardListDetail> PerformanceCardList { get; set; }
    }
}
