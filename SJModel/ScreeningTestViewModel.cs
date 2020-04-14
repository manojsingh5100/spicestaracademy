using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class ScreeningTestViewModel
    {
        public ScreeningTestViewModel()
        {
            var date = DateTime.Now;
            Date = date.Day.ToString().PadLeft(2, '0') + "/" + date.Month.ToString().PadLeft(2, '0') + "/" + date.Year;
        }
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public Nullable<int> TatooOrScarId { get; set; }
        public string FlyingExp { get; set; }
        public Nullable<System.DateTime> PassportIssueDate { get; set; }
        public Nullable<System.DateTime> PassportExpiryDate { get; set; }
        public string EyeSightLeft { get; set; }
        public string EyeSightRight { get; set; }
        public string Interviewer { get; set; }
        public String Date { get; set; }
        public Nullable<System.DateTime> ReviewDate { get; set; }
        public Nullable<int> DocumentMasterId { get; set; }
        public string Remark { get; set; }

        public string PassportIssueDateStr { get; set; }
        public string PassportExpiryDateStr { get; set; }
        public string ReviewDateStr { get; set; }

    }

}
