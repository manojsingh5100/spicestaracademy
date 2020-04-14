using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class GetNotificationViewModel
    {
        public GetNotificationViewModel()
        {
            IsAddAdmFee = false;
        }
        public string Message { get; set; }
        public string DueAdmissionFee { get; set; }
        public bool IsAddAdmFee { get; set; }
        public int NoOfCollection { get; set; }
    }
}
