using SJData;
using SJModel.PTAModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJService.PTA
{
    public class AdmissionPilotService
    {
        SJStarERPEntities _context = null;
        public AdmissionPilotService()
        {
            _context = new SJStarERPEntities();
        }

        public bool CreatePilotAdmission(ptaPilotRegistrationMaster Model, int CreatedBy)
        {
            bool status = false;
            ptaAdmissionMaster admission = new ptaAdmissionMaster
            {
                Fname = Model.Fname,
                Lanme = Model.Lname,
                Email = Model.Email,
                Mobile = Model.Mobile,
                DOB = Model.DOB,
                Height = Model.Height,
                CourseId = Model.CourseId,
                SessionId = Model.SessionId,
                ApplicationNo =Model.ApplicationNo,
                AdmissionBy = CreatedBy,
                AdmissionDate = DateTime.Now,
                IsActive = true
            };
            _context.ptaAdmissionMasters.Add(admission);
            var data = _context.ptaRegistrationInfoes.Where(p => p.ptaPilotRegistrationMasterId == Model.Id).FirstOrDefault();
            if (data != null)
                data.AdmissionId = admission.Id;

            _context.SaveChanges();
            status = true;
            return status;
        }

        public IEnumerable<PilotRegistrationViewModel> GetPilotAdmissionCandidateList()
        {
            return _context.ptaAdmissionMasters.Where(p => p.IsActive).Select(item => new PilotRegistrationViewModel
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
                MedicalImagesCount = item.ptaDocumentDetails.Where(t=>t.DocumentMaster.IsActive && t.DocumentMaster.DepartmentMasterId == 2 && t.DocumentMaster.DocumentName != "Other").Count()
            }).AsEnumerable();
        }

    }
}
