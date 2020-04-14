using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class BankInfoViewModel
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string BeneficiaryName { get; set; }
        public string BankName { get; set; }
        public string AccountType { get; set; }
        public string BranchAddress { get; set; }
        public string IFSC_Code { get; set; }
        public string StrIssueDate { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<int> FIN_FeeReceiptMasterID { get; set; }
    }
}
