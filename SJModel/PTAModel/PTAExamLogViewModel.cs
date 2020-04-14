using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PTAModel
{
    public class PTAExamLogViewModel
    {
        public int PTAPilotRegistrationMasterId { get; set; }
        public int PTAScreeningInfoId { get; set; }
        public int PTAScreeningTestTypeId { get; set; }
        public int PTAScreeningExamFeeInfoId { get; set; }
        public int EnteredBy { get; set; }
    }
}
