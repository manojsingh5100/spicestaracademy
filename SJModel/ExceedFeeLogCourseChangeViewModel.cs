using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class ExceedFeeLogCourseChangeViewModel
    {
        public int Id { get; set; }
        public decimal ExceedAmount { get; set; }
        public int RegistrationNo { get; set; }
        public int AdmissionId { get; set; }
        public int OldFeeDetailId { get; set; }
        public int FeeDetailId { get; set; }
        public DateTime LogDate { get; set; }
        public int EnteredBy { get; set; }
    }
}
