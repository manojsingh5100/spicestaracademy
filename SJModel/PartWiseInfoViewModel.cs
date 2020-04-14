using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class PartWiseInfoViewModel
    {
        public decimal? Amount { get; set; }
        public string PaymentMode { get; set; }
        public string RecieptNo { get; set; }
        public DateTime Date { get; set; }
        public string DDRTGSNo { get; set; }
        public string DateStr {get;set;}
    }
}
