using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class ScreeningParameterOptionViewModel
    {
        public int Id { get; set; }
        public int ScreeningParameterId { get; set; }
        public string ParameterName { get; set; }
        public bool Good { get; set; }
        public bool Fair { get; set; }
        public bool Poor { get; set; }
        public string Remark { get; set; }
        public int ScreeningTestId { get; set; }
    }
}
