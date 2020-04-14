using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class SendingEmailForRejectedCandidateViewModel
    {
        public int Id { get; set; }
        public int RegistrationNo { get; set; }
        public string Email { get; set; }
        public bool IsSendEmail { get; set; }
        public System.DateTime DateOfSending { get; set; }
        public bool IsActive { get; set; }
    }
}
