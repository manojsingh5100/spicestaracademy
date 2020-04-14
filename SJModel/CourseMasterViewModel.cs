using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class CourseMasterViewModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}
