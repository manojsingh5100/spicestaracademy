using SJData;
using SJModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJService.PTA
{
    public class LoginService
    {
        SJStarERPEntities _context = null;
        public LoginService()
        {
            _context = new SJStarERPEntities();
        }
        public LoginViewModel GetLoginInfo(LoginViewModel model)
        {
            var list = _context.UserLogins.
                Where(l => !l.Role.IsPerformanceDept.HasValue);
            return list.Where(x => x.IsActive && x.Role.IsActive && x.Email == model.Email && x.Password == model.Password)
            .Select(l => new LoginViewModel
            {
                Email = l.Email,
                Fname = l.Fname,
                LName = l.LName,
                Id = l.Id,
                RoleId = l.RoleId,
                Department = l.Department,
                Designation = l.Designation,
                RoleName = l.Role.Name,
                Password = l.Password
            }).FirstOrDefault();
        }
    }
}
