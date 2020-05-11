using SJData;
using SJModel.PTAModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJService.PTA
{
    public class NotificationService
    {
        SJStarERPEntities _context = null;
        public NotificationService()
        {
            _context = new SJStarERPEntities();
        }

        //******************************PilotScreeningList****************************************//
        public IEnumerable<PilotRegistrationViewModel> GetPilotCandidateScreenningList()
        {
            return _context.ptaPilotRegistrationMasters.Where(p => p.IsActive).Select(item => new PilotRegistrationViewModel
            {
                Id = item.Id,
                Fname = item.Fname,
                Lname = item.Lname,
                PilotRegistartionNo = item.ptaRegistrationInfoes.FirstOrDefault().RegistartionNo,
                RegistartionNo = item.RegistrationNo,
                ApplicationNo = item.ApplicationNo,
                Email = item.Email,
                Mobile = item.Mobile,
                DOB = item.DOB,
                CourseName = item.CourseMaster.CourseName,
                RegistrationDate = item.RegistrationDate,
                SourceOfCandidate = item.ptaRegistrationInfoes.FirstOrDefault().ptaLeadSourceMaster.Name,
                Gender = item.ptaRegistrationInfoes.FirstOrDefault().ptaGenderMaster.Name,
                CreatedBy = item.ptaScreeningInfoes.FirstOrDefault().ptaScreeningTestResults.Where(e => e.IsPassed.HasValue && e.IsPassed.Value).Count(),
                IsActive = item.ptaScreeningInfoes.FirstOrDefault().ptaScreeningTestResults.Where(e => e.IsPassed.HasValue && !e.IsPassed.Value).Any(),
                ScreeningExamFeeNo = item.ptaScreeningExamFeeInfoes.Count(),
                LastExamFailedStatus = item.ptaScreeningExamFeeInfoes.Count() == 0 ? true : (item.ptaScreeningExamFeeInfoes.Count() < 4 ? (item.ptaScreeningInfoes.FirstOrDefault().ptaScreeningTestResults.OrderByDescending(o => o.Id).Where(w => w.IsPassed.HasValue).FirstOrDefault().IsPassed.Value == false ? true : false) : false),
                IsAppeared = item.ptaScreeningEmailNotificationLogs.Count() == (item.ptaScreeningExamFeeInfoes.Count() - 1) ? false : true
            }).AsEnumerable();
        }


        //*******************************GetAdmissionList************************************//
        public IEnumerable<PilotRegistrationViewModel> GetPilotAdmissionList()
        {
            return _context.ptaAdmissionMasters.Where(a => a.IsActive).Select(item => new PilotRegistrationViewModel
            {
                Id = item.Id,
                Fname = item.Fname,
                Lname = item.Lanme,
                PilotRegistartionNo = item.ptaRegistrationInfoes.FirstOrDefault().RegistartionNo,
                RegistartionNo = item.ptaRegistrationInfoes.FirstOrDefault().ptaPilotRegistrationMaster.RegistrationNo,
                ApplicationNo = item.ApplicationNo,
                Email = item.Email,
                Mobile = item.Mobile,
                DOB = item.DOB,
                CourseName = item.CourseMaster.CourseName,
                RegistrationDate = item.AdmissionDate,
                Gender = item.ptaRegistrationInfoes.FirstOrDefault().ptaGenderMaster.Name,
                PilotRegistrationId = item.ptaRegistrationInfoes.FirstOrDefault().ptaPilotRegistrationMaster.Id,

            }).AsEnumerable();
        }
    }
}
