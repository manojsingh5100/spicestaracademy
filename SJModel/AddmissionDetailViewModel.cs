using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class AddmissionDetailViewModel
    {
        public int Id { get; set; }
        public int AddmissionId { get; set; }
        public int SemesterId { get; set; }
        public int SessionId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
