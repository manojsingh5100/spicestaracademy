using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class LogActivityViewModel
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string Activity { get; set; }
        public string ActivityMessage { get; set; }
        public string ActioName { get; set; }
        public string ControllerName { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string ActivityDate
        {
            get
            {
                return string.Format("{0}", EnteredDate.Day.ToString().PadLeft(2, '0') + "/" + EnteredDate.Month.ToString().PadLeft(2, '0') + "/" + EnteredDate.Year + " " + EnteredDate.Hour + ":" + EnteredDate.Minute);
            }
        }
        public string ByActivity
        {
            get
            {
                return string.Format("{0}", Fname + " " + Lname);
            }
        }
    }
}
