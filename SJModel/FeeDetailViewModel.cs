using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class FeeDetailViewModel
    {
        public int Id { get; set; }
        public int PaymentTypeId { get; set; }
        public int FeeTypeDetailId { get; set; }
        public int RegistrationMasterId { get; set; }
        public int RegistrationNo { get; set; }
        public bool IsActive { get; set; }
        public int EnteredBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public bool IsAdmissionFeePay { get; set; }
    }
}
