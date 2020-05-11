using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            RememberMe = false;
            Password = "";
        }
        public int Id { get; set; }
        public string Fname { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public int RoleId { get; set; }

        public int EnteredBy { get; set; }

        public string Designation { get; set; }
        public string Department { get; set; }
        public string RoleName { get; set; }
        public bool RememberMe { get; set; }
        public string ApplicationNo { get; set; }

        public List<RoleViewModel> RoleList { get; set; }
        public Nullable<bool> IsPerformanceDept { get; set; }
        public string errorMessage { get; set; }
        public int Admissionid { get; set; }
        public int ptaRegistrationInfoId { get; set; }
        public int ptaPilotRegistrationMasterId { get; set; }
    }
}
