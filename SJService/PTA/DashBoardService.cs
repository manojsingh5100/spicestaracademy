using SJData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJModel;

namespace SJService.PTA
{
    public class DashBoardService
    {
        SJStarERPEntities _context = null;
        public DashBoardService()
        {
            _context = new SJStarERPEntities();
        }

        public DesktopChartList GetDashBoardData()
        {
            DateTime today = DateTime.Today;
            var T_mapList = _context.ptaPilotRegistrationMasters.Where(r => r.IsActive && r.RegistrationDate != null && r.RegistrationDate.Year == today.Year && r.RegistrationDate.Month == today.Month);
            return new DesktopChartList
            {
               TotalRegistartion = _context.ptaPilotRegistrationMasters.Count(),
               TotalAdmission = _context.ptaAdmissionMasters.Where(a=>a.IsActive).Count(),
               TotalScreenning = _context.ptaScreeningInfoes.Count(),
               TotalMedical = _context.ptaDocumentDetails.GroupBy(g => g.ptaPilotRegistrationMasterId).Any() ? _context.ptaDocumentDetails.GroupBy(g => g.ptaPilotRegistrationMasterId).Select(item => new { Id = item.Key }).Count()
               : 0,
               Map =  Axis.JConversion(Axis.BarPoints(T_mapList.OrderBy(o => o.RegistrationDate).GroupBy(g => g.RegistrationDate.Day).Select(s => new AxisPoint
               {
                   Y = s.Count(),
                   X = s.Key
               }).ToList()))
            };
        }
    }
}
