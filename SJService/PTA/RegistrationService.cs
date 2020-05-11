using SJData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJModel.PTAModel;

namespace SJService.PTA
{
    public class RegistrationService
    {
        SJStarERPEntities _context = null;
        public RegistrationService()
        {
            _context = new SJStarERPEntities();
        }

        public IEnumerable<PilotRegistrationViewModel> GetPilotCandidateList()
        {
            return _context.ptaPilotRegistrationMasters.Where(p => p.IsActive).Select(item => new PilotRegistrationViewModel
            {
                Id = item.Id,
                Fname = item.Fname,
                Lname = item.Lname,
                PilotRegistartionNo = item.ptaRegistrationInfoes.FirstOrDefault().RegistartionNo,
                RegistartionNo = item.RegistrationNo,
                Email = item.Email,
                Mobile = item.Mobile,
                DOB = item.DOB,
                CourseName = item.CourseMaster.CourseName,
                RegistrationDate = item.RegistrationDate,
                SourceOfCandidate = item.ptaRegistrationInfoes.FirstOrDefault().ptaLeadSourceMaster.Name,
                Gender = item.ptaRegistrationInfoes.FirstOrDefault().ptaGenderMaster.Name,
                CreatedBy = item.ptaScreeningInfoes.FirstOrDefault().ptaScreeningTestResults.Where(e => e.IsPassed.HasValue && e.IsPassed.Value).Count(),
                IsActive = item.ptaScreeningInfoes.FirstOrDefault().ptaScreeningTestResults.Where(e => e.IsPassed.HasValue && !e.IsPassed.Value).Any(),
                ScreeningTestResultCount = item.ptaScreeningInfoes.FirstOrDefault().ptaScreeningTestResults.Count(),
                IsScrreningExamFeeEmailNotificationNo = item.ptaScreeningEmailNotificationLogs.Where(x => x.ExamTermNo == 1).Count(),
            }).AsEnumerable();
        }

        public PilotRegistrationViewModel GetPilotCandidateInfoByRegNo(int RegId)
        {
            return _context.ptaPilotRegistrationMasters.Where(p => p.IsActive && p.Id == RegId).Select(item => new PilotRegistrationViewModel
            {
                Id = item.Id,
                Fname = item.Fname,
                Lname = item.Lname,
                PilotRegistartionNo = item.ptaRegistrationInfoes.FirstOrDefault().RegistartionNo,
                RegistartionNo = item.RegistrationNo,
                Email = item.Email,
                Mobile = item.Mobile,
                DOB = item.DOB,
                CourseName = item.CourseMaster.CourseName,
                ExamTerm = item.ptaScreeningExamFeeInfoes.Count > 0 ? item.ptaScreeningExamFeeInfoes.OrderByDescending(o => o.ScreeningAmountTerm).FirstOrDefault().ScreeningAmountTerm : 0
            }).FirstOrDefault();
        }

        public bool SaveExamFeeNitificationLogs(PilotRegistrationViewModel Model)
        {
            bool status = false;
            ptaScreeningEmailNotificationLog log = new ptaScreeningEmailNotificationLog
            {
                Amount = Model.ExamAmount,
                CourseName = Model.CourseName,
                LogDate = DateTime.Now,
                CandidateName = Model.StudentName,
                Email = Model.Email,
                ExamTermNo = Model.ExamTerm,
                IsActive = Model.IsActive,
                IsSendEmail = Model.IsSendEmail,
                PTAPilotRegistrationMasterId = Model.Id
            };
            _context.ptaScreeningEmailNotificationLogs.Add(log);
            _context.SaveChanges();
            status = true;
            return status;
        }

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

        public List<PilotScreeningTestTypeViewModel> GetScreenTestList()
        {
            return _context.ptaScreeningTestTypes.Where(p => p.IsActive).Select(item => new PilotScreeningTestTypeViewModel
            {
                Id = item.Id,
                Name = item.Name,
                InitialMark = item.InitialMark
            }).ToList();
        }

        public PilotScreeningViewModel IntialScreeningInfo(int RegNo)
        {
            PilotScreeningViewModel Model = new PilotScreeningViewModel
            {
                PilotRegistrationInfo = _context.ptaPilotRegistrationMasters.Where(p => p.RegistrationNo == RegNo && p.IsActive).Select(i => new PilotRegistrationViewModel
                {
                    Id = i.Id,
                    PilotRegistartionNo = i.ptaRegistrationInfoes.FirstOrDefault().RegistartionNo,
                    RegistartionNo = i.RegistrationNo,
                    Fname = i.Fname,
                    Lname = i.Lname,
                    Email = i.Email,
                    Mobile = i.Mobile,
                    CourseName = i.CourseMaster.CourseName,
                    RegistrationDate = i.RegistrationDate,
                    Gender = i.ptaRegistrationInfoes.FirstOrDefault().ptaGenderMaster.Name,
                    DOB = i.DOB,
                    Height = i.Height,
                    SessionName = i.SessionMaster.SessionName,
                    TenthCGPA = i.ptaRegistrationInfoes.FirstOrDefault().ptaEducationDetails.Where(e => e.Class == "10th").FirstOrDefault().PercentageOrCGPA,
                    TwelveCGPA = i.ptaRegistrationInfoes.FirstOrDefault().ptaEducationDetails.Where(e => e.Class == "12th").FirstOrDefault().PercentageOrCGPA,
                    SourceOfCandidate = i.ptaRegistrationInfoes.FirstOrDefault().ptaLeadSourceMaster.Name,
                    ScreeningInfoId = _context.ptaScreeningInfoes.Where(p => p.PTARegistrationNo == i.RegistrationNo).Any() ? _context.ptaScreeningInfoes.Where(p => p.PTARegistrationNo == i.RegistrationNo).FirstOrDefault().Id : 0
                }).FirstOrDefault(),

                GetPilotTrainingTestTypeList = _context.ptaScreeningTestTypes.Where(p => p.IsActive).Select(i => new PilotScreeningTestTypeViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    InitialMark = i.InitialMark,
                    ptaScreenningTestTypeId = i.ptaScreeningTestResults.Any() ? i.ptaScreeningTestResults.FirstOrDefault().PTAScreeningTestTypeId : 0
                }).ToList()
            };

            var data = _context.ptaScreeningInfoes.Where(s => s.IsActive && s.PTARegistrationNo == RegNo).FirstOrDefault();
            if (data != null)
                Model.Remark = data.Remark;
            if (data != null && data.ptaScreeningTestResults.Count() > 0)
            {
                foreach (var item in data.ptaScreeningTestResults)
                {
                    var d = Model.GetPilotTrainingTestTypeList.Where(g => g.Id == item.PTAScreeningTestTypeId).FirstOrDefault();
                    if (d != null)
                    {
                        d.IsSelected = item.IsPassed;
                        d.ObtainMark = item.ObtainMark;
                        if (!item.IsPassed.HasValue)
                            d.AgainExam = "yes";
                    }
                }
            }
            return Model;
        }

        public PilotMedicalInfoViewModel InitialMedicalInfo(int Id)
        {
            PilotMedicalInfoViewModel result = new PilotMedicalInfoViewModel();
            result.PilotRegistrationNo = Id;
            var data = _context.ptaDocumentDetails.Where(p => p.ptaPilotRegistrationMasterId == Id && p.DocumentMaster.DocumentName != "Other").ToList();
            var initialDocList = _context.DocumentMasters.Where(d => d.IsActive && d.DepartmentMasterId == 2 && d.DocumentName != "Other").Select(item => new PilotDocumentMasterViewModel
            {
                DocumentMasterId = item.Id,
                DocumentName = item.DocumentName,
                //DocumentPath = item.ptaDocumentDetails.Count > 0 ? item.ptaDocumentDetails.FirstOrDefault().DocumentPath : "",
                //Extention = item.ptaDocumentDetails.Count > 0 ? item.ptaDocumentDetails.FirstOrDefault().Extention : ""
            }).ToList();
            foreach (var i in initialDocList)
            {
                if (data != null)
                {
                    foreach (var j in data)
                    {
                        if (i.DocumentMasterId == j.DocumentMasterId)
                        {
                            i.DocumentPath = j.DocumentPath;
                            i.Extention = j.Extention;
                        }
                    }
                }
            }
            result.Documents = initialDocList;
            return result;
        }

        public PilotMedicalInfoViewModel InitialOtherUploadDocsInfo(int Id, int AdmissionId)
        {
            PilotMedicalInfoViewModel result = new PilotMedicalInfoViewModel();
            result.PilotRegistrationNo = Id;
            result.AdmissionId = AdmissionId;
            var data = _context.ptaDocumentDetails.Where(p => p.ptaPilotRegistrationMasterId == Id && p.DocumentMaster.IsActive && p.DocumentMaster.DepartmentMasterId == 2 && p.DocumentMaster.DocumentName == "Other" && p.IsActive).Select(item => new PilotDocumentMasterViewModel
            {
                DocumentMasterId = item.Id,
                DocumentName = item.DocumentPath,
                Extention = item.Extention,
                DocumentPath = item.DocumentPath,
            }).ToList();
            result.Documents = data;
            return result;
        }


        public MessageForSucccessful SaveUpdatePilotScreeningInfo(PilotScreeningViewModel Model)
        {
            MessageForSucccessful message = new MessageForSucccessful();
            int screenInfoId = 0;
            var registerData = _context.ptaPilotRegistrationMasters.Where(r => r.IsActive && r.RegistrationNo == Model.PTARegistrationNo).FirstOrDefault();
            if (registerData != null)
            {
                PTAExamLogViewModel failedlog = new PTAExamLogViewModel();
                failedlog.PTAPilotRegistrationMasterId = registerData.Id;
                if (Model.PilotScreeningInfoId == 0)
                {
                    ptaScreeningInfo screeninfo = new ptaScreeningInfo
                    {
                        PTAPilotRegistrationMasterId = registerData.Id,
                        PTARegistrationNo = Model.PTARegistrationNo,
                        Remark = Model.Remark,
                        EnteredDate = DateTime.Now,
                        EnteredBy = Model.EnteredBy,
                        IsActive = true
                    };
                    _context.ptaScreeningInfoes.Add(screeninfo);

                    if (registerData.ptaScreeningExamFeeInfoes.Count == 1)
                        registerData.ptaScreeningExamFeeInfoes.FirstOrDefault().PTAScreeningInfoId = screeninfo.Id;
                    _context.SaveChanges();
                    screenInfoId = screeninfo.Id;
                }
                else
                    screenInfoId = _context.ptaScreeningInfoes.Where(p => p.IsActive && p.PTARegistrationNo == Model.PTARegistrationNo).FirstOrDefault().Id;
                foreach (var Item in Model.GetPilotTrainingTestTypeList)
                {
                    var exist = _context.ptaScreeningInfoes.Where(p => p.IsActive && p.PTARegistrationNo == Model.PTARegistrationNo && p.ptaScreeningTestResults.Where(t => t.PTAScreeningTestTypeId == Item.Id).Any()).FirstOrDefault();
                    if (exist == null)
                    {
                        ptaScreeningTestResult TestResult = new ptaScreeningTestResult
                        {
                            PTAScreeningInfoId = screenInfoId,
                            PTAScreeningTestTypeId = Item.Id,
                            IsActive = true,
                            ObtainMark = Item.ObtainMark,
                            IsPassed = Item.IsPassed
                        };
                        _context.ptaScreeningTestResults.Add(TestResult);

                    }
                    else
                    {
                        var aa = _context.ptaScreeningTestResults.Where(t => t.PTAScreeningTestTypeId == Item.Id && t.PTAScreeningInfoId == screenInfoId).FirstOrDefault();
                        if (aa != null && aa.IsActive)
                        {
                            aa.IsPassed = Item.IsPassed;
                            aa.ObtainMark = Item.ObtainMark;
                        }
                    }
                    if (!Item.IsPassed)
                    {
                        failedlog.PTAScreeningInfoId = screenInfoId;
                        failedlog.PTAScreeningExamFeeInfoId = registerData.ptaScreeningExamFeeInfoes.OrderByDescending(o => o.ScreeningAmountTerm).FirstOrDefault().Id;
                        failedlog.PTAScreeningTestTypeId = Item.Id;
                        failedlog.EnteredBy = Model.EnteredBy;

                        ptaScreeningExamfailedLog log = new ptaScreeningExamfailedLog
                        {
                            PTAPilotRegistrationMasterId = failedlog.PTAPilotRegistrationMasterId,
                            PTAScreeningExamFeeInfoId = failedlog.PTAScreeningExamFeeInfoId,
                            PTAScreeningInfoId = failedlog.PTAScreeningInfoId,
                            PTAScreeningTestTypeId = failedlog.PTAScreeningTestTypeId,
                            EnteredBy = failedlog.EnteredBy,
                            ExamStatus = "Failed",
                            EnteredDate = DateTime.Now
                        };
                        _context.ptaScreeningExamfailedLogs.Add(log);
                    }
                }
                _context.SaveChanges();
                if (Model.GetPilotTrainingTestTypeList.Where(g => g.IsPassed == true).Count() == 4)
                {
                    AdmissionPilotService s = new AdmissionPilotService();
                    s.CreatePilotAdmission(registerData, Model.EnteredBy);
                }
                if (Model.GetPilotTrainingTestTypeList.LastOrDefault() != null)
                {
                    int d = Model.GetPilotTrainingTestTypeList.Where(g => g.IsPassed).Count();
                    message.Message = (Model.GetPilotTrainingTestTypeList.LastOrDefault().IsPassed ? (d == 4 ? "Candidate is selected." : "Candidate is selected") : "Candidate is rejected");
                }
                message.IsSuccess = true;
            }
            message.RegNo = Model.PTARegistrationNo.ToString();
            return message;
        }

        public string SaveMedicalUploadFiles(List<PilotDocumentMasterViewModel> Model)
        {
            string Massage = "";
            try
            {
                foreach (var item in Model)
                {
                    var data = _context.ptaDocumentDetails.Where(p => p.IsActive && p.ptaPilotRegistrationMasterId == item.Id && p.DocumentMasterId == item.DocumentMasterId).FirstOrDefault();
                    if (data != null && string.IsNullOrEmpty(item.IsPreviousExist))
                    {
                        data.DocumentPath = item.DocumentPath;
                        data.Extention = item.Extention;
                        data.UpdatedDate = DateTime.Now;
                        data.UpdatedBy = item.EnteredBy;
                    }
                    else
                    {
                        if (item.EnteredBy > 0)
                        {
                            var ad = _context.ptaAdmissionMasters.Where(p => p.ptaRegistrationInfoes.FirstOrDefault().ptaPilotRegistrationMasterId == item.Id).FirstOrDefault();
                            if (ad != null)
                            {
                                ptaDocumentDetail info = new ptaDocumentDetail
                                {
                                    DocumentMasterId = item.DocumentMasterId,
                                    DocumentPath = item.DocumentPath,
                                    IsActive = true,
                                    Extention = item.Extention,
                                    ptaPilotRegistrationMasterId = item.Id,
                                    ptaAdmissionMasterId = ad.Id,
                                    EnteredBy = item.EnteredBy,
                                    EnteredDate = DateTime.Now
                                };
                                _context.ptaDocumentDetails.Add(info);
                            }
                        }
                    }
                }
                _context.SaveChanges();
                Massage = "Medical document successfully uploaded.";
                return Massage;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SaveOtherFilesUploadData(List<PilotDocumentMasterViewModel> Model)
        {
            string Massage = "";
            try
            {
                int DocId = _context.DocumentMasters.Where(d => d.IsActive && d.DocumentName == "Other").FirstOrDefault().Id;
                if (DocId > 0)
                {
                    foreach (var item in Model)
                    {
                        ptaDocumentDetail info = new ptaDocumentDetail
                        {
                            DocumentMasterId = DocId,
                            DocumentPath = item.DocumentPath,
                            IsActive = true,
                            Extention = item.Extention,
                            ptaPilotRegistrationMasterId = item.Id,
                            ptaAdmissionMasterId = item.PilotAdmissionId,
                            EnteredBy = item.EnteredBy,
                            EnteredDate = DateTime.Now
                        };
                        _context.ptaDocumentDetails.Add(info);
                    }
                    _context.SaveChanges();
                    Massage = "Medical document successfully uploaded.";
                }
                return Massage;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
