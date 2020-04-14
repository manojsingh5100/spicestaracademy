using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PerformanceModel
{
    public class ParameterTypeViewModel
    {
        public ParameterTypeViewModel()
        {
            IsActive = true;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<ParameterListViewModel> ParameterList { get; set; }
    }
}
