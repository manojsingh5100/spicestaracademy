using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PerformanceModel
{
    public class Mainclass
    {
        public string[] labels { get; set; }
        public List<BarGraph> datasets { get; set; }
    }

    public class BarGraph
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public decimal[] data { get; set; }
    }
}
