using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class FeeEmailNotifacationLogViewModel
    {
        public int Id { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string CourseName { get; set; }
        public string FeeInstallmentName { get; set; }
        public System.DateTime LogDate { get; set; }
        public System.DateTime StartDateOfSendingMail { get; set; }
        public System.DateTime EndDateofSendingMail { get; set; }
        public string CandidateName { get; set; }
        public bool IsSenEmail { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string CourseChangeFeeAmountForMBA { get; set; }
    }
}
