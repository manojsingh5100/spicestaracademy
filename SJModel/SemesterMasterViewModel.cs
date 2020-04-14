using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class SemesterMasterViewModel
    {
        public int Id { get; set; }
        public string SemesterName { get; set; }
        public string BatchName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<DateTime> RegistrationDate { get; set; } 
        public int BatchId { get; set; }
    }
}
