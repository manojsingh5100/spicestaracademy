using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationInterface.Models
{
    public class Respose
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class PilotResponse
    {
        public string RegistrationNo { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}