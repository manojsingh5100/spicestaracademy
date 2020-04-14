using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class FeeEmailNotificationViewModel
    {
        public int RegNo { get; set; }
        public decimal? FullAmount { get; set; }
        public string CourseName { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public IEnumerable<FeeCollectionViewModel> FeeCollection { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string RegisterEmail { get; set; }
        public int? BatchId { get; set; }
        public string BatchName { get; set; }
        public DateTime? BatchStartDate { get; set; }
        public string PaymentType { get; set; }
    }

    public class FeeCollectionViewModel
    {
        public dynamic PartWise { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public int? InstallmentId { get; set; }
        public string InstallmentName { get; set; }
        public DateTime EnterDate { get; set; }
    }
}
