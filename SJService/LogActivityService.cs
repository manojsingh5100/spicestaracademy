using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJData;
using SJModel;

namespace SJService
{
    public class LogActivityService
    {
        SJStarERPEntities _context = null;
        public LogActivityService()
        {
            _context = new SJStarERPEntities();
        }

        public List<LogActivityViewModel> GetUserActivities()
        {
            return _context.UserActivityLogs.Select(item => new LogActivityViewModel
            {
                Id = item.Id,
                RegistrationNo = item.RegistrationNo,
                Activity = item.Activity,
                Fname = item.UserLogin.Fname,
                Lname = item.UserLogin.LName,
                EnteredDate = item.EnteredDate,
                ModuleName = item.ModuleName,
                ActivityMessage = item.ActivityMessage
            }).OrderByDescending(o => o.Id).ToList();
        }

        public LogActivityViewModel CreateLogActivity(LogActivityViewModel Model)
        {
            UserActivityLog log = new UserActivityLog
            {
                ActioName = Model.ActioName,
                Activity = Model.Activity,
                ActivityMessage = Model.ActivityMessage,
                ControllerName = Model.ControllerName,
                EnteredBy = Model.EnteredBy,
                EnteredDate = Model.EnteredDate,
                ModuleName = Model.ModuleName,
                RegistrationNo = Model.RegistrationNo
            };
            _context.UserActivityLogs.Add(log);
            _context.SaveChanges();
            return Model;
        }

        public IEnumerable<LogActivityViewModel> GetTopFiveActities()
        {
            return _context.UserActivityLogs.OrderByDescending(o => o.EnteredDate).Take(5).Select(item => new LogActivityViewModel
            {
                Id = item.Id,
                ActioName = item.ActioName,
                Activity = item.Activity,
                ActivityMessage = item.ActivityMessage,
                ControllerName = item.ControllerName,
                Fname = item.UserLogin.Fname,
                Lname = item.UserLogin.LName,
                ModuleName = item.ModuleName,
                EnteredDate = item.EnteredDate
            }).AsEnumerable();
        }

        public IEnumerable<LogActivityViewModel> GetTopThreeActities(string ModuleName)
        {
            return _context.UserActivityLogs.Where(w=>w.ModuleName == ModuleName).OrderByDescending(o => o.EnteredDate).Take(3).Select(item => new LogActivityViewModel
            {
                Id = item.Id,
                ActioName = item.ActioName,
                Activity = item.Activity,
                ActivityMessage = item.ActivityMessage,
                ControllerName = item.ControllerName,
                Fname = item.UserLogin.Fname,
                Lname = item.UserLogin.LName,
                ModuleName = item.ModuleName,
                EnteredDate = item.EnteredDate
            }).AsEnumerable();
        }
    }
}
