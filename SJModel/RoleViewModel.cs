using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> IsPerformanceDept { get; set; }
        public string ActiveStr { get; set; }
        public int Year { get; set; }
        public int BatchId { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }

        public Nullable<DateTime> BatchStartDate { get; set; }
        public Nullable<DateTime> BatchEndDate { get; set; }
        public string BatchStartDateStr { get; set; }
        public string BatchEndDateStr { get; set; }



        //public string BatchStartDateStr
        //{
        //    get { return string.Format("{0}", (BatchStartDate.HasValue ? BatchStartDate.Value.Day.ToString().PadLeft(2, '0') + "/" + BatchStartDate.Value.Month.ToString().PadLeft(2, '0') + "/" + BatchStartDate.Value.Year : "")); }
        //    set { }
        //}
        //public string BatchEndDateStr
        //{
        //    get { return string.Format("{0}", (BatchEndDate.HasValue ? BatchEndDate.Value.Day.ToString().PadLeft(2, '0') + "/" + BatchEndDate.Value.Month.ToString().PadLeft(2, '0') + "/" + BatchEndDate.Value.Year : "")); }
        //    set { }
        //}
        public List<SemesterMasterViewModel> SessionListByCId { get; set; }
        public int EnteredBy { get; set; }
    }
}
