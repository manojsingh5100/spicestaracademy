using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class BankAndAmountDetailViewModel
    {
        public int Id { get; set; }
        public string DDRTGSNo { get; set; }
        public decimal Amount { get; set; }
        public int FeeCollectionId { get; set; }
        public string RecieptNo { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }

        public bool IsOnePay { get; set; }
    }
}
