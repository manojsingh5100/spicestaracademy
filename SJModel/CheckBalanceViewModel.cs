using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class CheckBalanceViewModel
    {
        public int RegNo { get; set; }
        public int PaymentTypeId { get; set; }
        public int FeeTypeDeatilId { get; set; }
        public int? InstallmentId { get; set; }

        public bool BalanceStatus { get; set; }
        public decimal RemainderPartWiseAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public List<BankAndAmountDetailViewModel> PartWisePaymentList { get; set; } 
    }
}
