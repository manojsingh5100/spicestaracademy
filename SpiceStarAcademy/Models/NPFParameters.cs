using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiceStarAcademy.Models
{
    public class NPFParameters
    {
        public NPFParameters()
        {
            secret_key = "8944bacfb52da67bac5a1ba3e5a20d94";
            form_id = "1713";
            mode = "update";
        }
        public string secret_key { get; set; }
        public string form_id { get; set; }
        public string email { get; set; }
        public string application_no { get; set; }
        public string enrolled_campus { get; set; }
        public string application_stage { get; set; }
        public string enrolled_department { get; set; }
        public string mode { get; set; }
    }
}