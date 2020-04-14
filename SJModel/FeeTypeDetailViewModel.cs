using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class FeeTypeDetailViewModel
    {
        public int Id { get; set; }
        public int FeeTypeId { get; set; }
        public int CourseDurationId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public bool IsActive { get; set; }
        public int SessionMasterId { get; set; }

        public string FeeTypeName { get; set; }
        public string CourseSessionName { get; set; }
        public string SessionName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int EnteredBy { get; set; }
        public List<RoleViewModel> FeeTypeList { get; set; }
        public List<RoleViewModel> SessionNameList { get; set; }
        public List<RoleViewModel> CourseList { get; set; }
    }
}
