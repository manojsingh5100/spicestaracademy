using SJData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJModel;
using System.Net.Mail;
using System.Net;

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
                TotalAdmission = _context.ptaAdmissionMasters.Where(a => a.IsActive).Count(),
                TotalScreenning = _context.ptaScreeningInfoes.Count(),
                TotalMedical = _context.ptaDocumentDetails.GroupBy(g => g.ptaPilotRegistrationMasterId).Any() ? _context.ptaDocumentDetails.GroupBy(g => g.ptaPilotRegistrationMasterId).Select(item => new { Id = item.Key }).Count()
               : 0,
                Map = Axis.JConversion(Axis.BarPoints(T_mapList.OrderBy(o => o.RegistrationDate).GroupBy(g => g.RegistrationDate.Day).Select(s => new AxisPoint
                {
                    Y = s.Count(),
                    X = s.Key
                }).ToList()))
            };
        }


        public static string Email(string htmlString)
        {
            string msg = "";
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("priyankavrmma94@gmail.com");
                message.To.Add(new MailAddress("sandeep.kumar@interactive12.com"));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("priyankavrma94@gmail.com", "Pipa@143#v");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.Send(message);
                msg = "Successfull";
                return msg;
            }
            catch (Exception ex) { return ex.InnerException.Message; }
        }

    }
}
