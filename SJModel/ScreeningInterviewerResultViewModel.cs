using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class ScreeningInterviewerResultViewModel
    {
        public int Id { get; set; }
        public int ScreeningInterviewerId { get; set; }
        public string ScreeningInterviewerName { get; set; }
        public bool IsResult { get; set; }
        public string Remark { get; set; }
        public int ScreeningTestId { get; set; }
    }
}
