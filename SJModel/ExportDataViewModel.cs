using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class ExportDataViewModel
    {
        public string PageName { get; set; }   
        public string IsSelected { get; set; }
        public string CourseId { get; set; }
        public int BatchId { get; set; }
        public string Search { get; set; }
    }
}
