using SJData;
using SJModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJService.PTA
{
    public class StudentLoginService
    {
        SJStarERPEntities _context = null;
        public StudentLoginService()
        {
            _context = new SJStarERPEntities();
        }
        public LoginViewModel GetStudentLogin(LoginViewModel model)
        {
            var list = _context.ptaStudentLogins.
                Where(l => !l.Role.IsPerformanceDept.HasValue);
            return list.Where(x => x.IsActive && x.Role.IsActive && x.RoleId == 6 && x.Email == model.Email && x.Password == model.Password)
            .Select(l => new LoginViewModel
            {
                Email = l.Email,
                Fname = l.Fname,
                LName = l.Lname,
                Id = l.Id,
                RoleId = l.RoleId,
                RoleName = l.Role.Name,
                Password = l.Password,
                ApplicationNo = l.ApplicationNo,
                Admissionid = l.ptaRegistrationInfo.AdmissionId.HasValue ? l.ptaRegistrationInfo.AdmissionId.Value : 0,
                ptaRegistrationInfoId = l.ptaRegistrationInfo.Id,
                ptaPilotRegistrationMasterId = l.ptaRegistrationInfo.ptaPilotRegistrationMasterId
            }).FirstOrDefault();
        }
    }
}
