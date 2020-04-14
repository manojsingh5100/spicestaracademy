using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class DocumentDetailViewModel
    {
        public int Id { get; set; }
        public int AddmissionId { get; set; }
        public int DocumentId { get; set; }
        public bool IsSubmmited { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
