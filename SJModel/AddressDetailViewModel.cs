using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class AddressDetailViewModel
    {
        public int Id { get; set; }
        public int AddmissionId { get; set; }
        public string CopAddress { get; set; }
        public string CopState { get; set; }
        public string CopCity { get; set; }
        public string CopZip { get; set; }
        public string PerAddress { get; set; }
        public string PerState { get; set; }
        public string PerCity { get; set; }
        public string PerZip { get; set; }

        public string Fname { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public int CourseId { get; set; }
        public int SessionId { get; set; }
        public int? BatchId { get; set; }

        public string RegNo { get; set; }
    }
}
