using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class SessionMasterViewModel
    {
        public int Id { get; set; }
        public string SessionName { get; set; }
        public bool IsActive { get; set; }
        public int CourseId { get; set; }
    }
}
