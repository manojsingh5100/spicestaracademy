using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class PaymentInfoViewModel
    {
        public string FeeName { get; set; }
        public string RecieptNo { get; set; }
        public decimal Amount { get; set; }
        public string InstallmentName { get; set;}
        public DateTime Date { get; set;}
        public string DateStr { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public string PartwiseNo { get; set; }
        public bool Isprint { get; set; }
        public bool IsShowPartWisePayment { get; set; }
        public dynamic PartWiseInfoList { get; set; }
        public string JsonInfo { get; set; }
        public string DDRTGSNo { get; set; }
    }
}
