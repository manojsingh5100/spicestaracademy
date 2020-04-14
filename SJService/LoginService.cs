using SJData;
using SJModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJService
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
                Where(l => l.Role.IsPerformanceDept.HasValue &&
                l.Role.IsPerformanceDept.Value == model.IsPerformanceDept.Value);

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

        public bool SaveUserRegistration(LoginViewModel model)
        {
            bool status = false;
            if (model.Id == 0)
            {
                UserLogin login = new UserLogin
                {
                    Fname = model.Fname,
                    LName = model.LName,
                    Department = model.Department,
                    Designation = model.Designation,
                    Email = model.Email,
                    IsActive = true,
                    Password = model.Password,
                    EnteredDate = DateTime.Now,
                    RoleId = model.RoleId,
                };
                _context.UserLogins.Add(login);
                if (_context.SaveChanges() == 1)
                {
                    status = true;
                    //========= User Activity ===========
                    LogActivityViewModel log = new LogActivityViewModel();
                    log.EnteredBy = model.EnteredBy;
                    log.EnteredDate = DateTime.Now;
                    log.ActioName = "SaveUserRegistration";
                    log.ModuleName = "Create User";
                    log.ControllerName = "Login";
                    log.Activity = "Create User";
                    log.ActivityMessage = "Add new user '" + model.Fname + (string.IsNullOrEmpty(model.LName) ? "'." : (" " + model.LName + "'."));
                    LogActivityService logActivityService = new LogActivityService();
                    logActivityService.CreateLogActivity(log);
                    //====================================
                }
            }
            return status;
        }

        public List<RoleViewModel> GetRoleList()
        {
            return _context.Roles.Where(s => s.IsActive && !s.IsPerformanceDept.Value).Select(m => new RoleViewModel
            {
                Id = m.Id,
                Name = m.Name,
            }).OrderBy(o => o.Name).ToList();
        }

        public bool IsExistEmail(string Email)
        {
            return _context.UserLogins.Any(u => u.Email == Email);
        }
    }
}
