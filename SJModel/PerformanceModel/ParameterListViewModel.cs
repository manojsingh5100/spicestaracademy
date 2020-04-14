using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PerformanceModel
{
    public class ParameterListViewModel
    {
        public ParameterListViewModel()
        {
            IsActive = true;
        }
        public int ParameterId { get; set; }
        public string KeyId { get; set; }
        public int tblParameterTypeId { get; set; }
        public string Name { get; set; }
        public int? ReviewId { get; set; }
        public string Gender { get; set; }

        public int GenderId { get; set; }
        public bool IsActive { get; set; }
        public string ParameterTypeName { get; set; }
        public string Message { get; set; }
        public string Remark { get; set; }
        public int Rating { get; set; }
        public bool IsApplicable { get; set; }
    }
}
