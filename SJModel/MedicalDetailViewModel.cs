using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class MedicalDetailViewModel
    {
        public int Id { get; set; }
        public int AddmissionId { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Weight { get; set; }
        public string VisionLeft { get; set; }
        public string VisionRight { get; set; }
        public string MedicalCenter { get; set; }
        public string MedicalStatus { get; set; }

        public string MedicalDate { get; set; }
        public string FitnessDate { get; set; }
        public string JoiningDate { get; set; }

        public Nullable<DateTime> MDate { get; set; }
        public Nullable<DateTime> FDate { get; set; }
        public Nullable<DateTime> JDate { get; set; }
        public string RegNo { get; set; }
    }
}
