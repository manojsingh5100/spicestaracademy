using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class FeeRefundViewModel
    {
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public int Id { get; set; }
        public int RegNo { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string StudentName
        {
            get
            {
                return string.Format("{0} {1}", UppercaseFirst(Fname), UppercaseFirst(Lname));
            }
        }
        public string Email { get; set; }
        public string Mobile { get; set; } 
        public string Course { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public int AdmissionId { get; set; }
        public string Remark { get; set; }
    }
}
