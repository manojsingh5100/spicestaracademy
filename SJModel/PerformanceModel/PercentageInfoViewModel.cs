using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PerformanceModel
{
    public class PercentageInfoViewModel
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int tblPerformanceMasterId { get; set; }
        public int tblReviewMasterId { get; set; }
        public double FromPercentage { get; set; }
        public double TillPercentage { get; set; }
    }
}
