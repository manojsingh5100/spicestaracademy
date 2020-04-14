using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class FeePaymentViewModel
    {
        public FeePaymentViewModel()
        {
            FeeInfoDetail = new FeeDetailViewModel();
        }
        public string FeeName { get; set; }
        public decimal Fee { get; set; }
        public int Id { get; set; }
        public string PaymentMode { get; set; }

        public FeeDetailViewModel FeeInfoDetail { get; set; }
    }
}
