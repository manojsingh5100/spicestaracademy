using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class AddFeeCollectionViewModel
    {
        public int Id { get; set; }
        public int FeePaymentDetailId { get; set; }
        public string RecieptNo { get; set; }
        public string Remark { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public string PanNumber { get; set; }
        public int EnteredBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int Installment { get; set; }
    }
}
