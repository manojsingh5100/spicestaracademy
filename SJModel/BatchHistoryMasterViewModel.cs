using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class BatchHistoryMasterViewModel
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }
        public int BatchFromId { get; set; }
        public int BatchToId { get; set; }
        public string Description { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
    }
}
