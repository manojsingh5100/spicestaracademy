using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SJData;
using SJModel.PTAModel;

namespace SJService.PTA
{
    public class StudentProfileService
    {
        private SJStarERPEntities _context = null;
        public StudentProfileService()
        {
            _context = new SJStarERPEntities();
        }

        public StudentInfoViewModel GetStudentProfile(string ApplicationNo)
        {
            var Data = _context.ptaPilotRegistrationMasters.Where(x => x.IsActive && x.ApplicationNo == ApplicationNo).FirstOrDefault();
            StudentInfoViewModel Model = new StudentInfoViewModel();
            if (Data != null)
            {
                Model.ApplicationNo = Data.ApplicationNo;
                Model.SessionName = Data.SessionMaster.SessionName;
                Model.CourseName = Data.CourseMaster.CourseName;
                Model.Education = Data.ptaRegistrationInfoes.FirstOrDefault().ptaEducationDetails.OrderByDescending(o => o.Id).FirstOrDefault().Class;
                Model.Email = Data.Email;
                Model.DOB = Data.DOB.HasValue ? Data.DOB.Value.Day.ToString().PadLeft(2, '0') + "/" + Data.DOB.Value.Month.ToString().PadLeft(2, '0') + "/" + Data.DOB.Value.Year : "";
                Model.Mobile = Data.Mobile;
                Model.Batch = "Batch1";
                Model.EducationInfo = Data.ptaRegistrationInfoes.FirstOrDefault().ptaEducationDetails.Select(item => new PTAEducationViewModel
                {
                    Class = item.Class,
                    Id = item.Id,
                    NameOfInstitute = item.NameOfInstitute,
                    PercentageOrCGPA = item.PercentageOrCGPA,
                    ptaBoardAndUniversityMasterId = item.ptaBoardAndUniversityMasterId,
                    ptaUGBoardAndUniversityMasterId = item.ptaUGBoardAndUniversityMasterId,
                    ptaDegreeMasterId = item.ptaDegreeMasterId,
                    ptaStreamMasterId = item.ptaStreamMasterId,
                    YearOfPassing = item.YearOfPassing,
                    ptaEduEvaluationTypeMasterId = item.ptaEduEvaluationTypeMasterId,
                    ptaResultStatusMasterId = item.ptaResultStatusMasterId,
                    ptaRegistrationInfoId = item.ptaRegistrationInfoId,
                    SubjectMarks = item.ptaEducationSubjectMarks.Select(x => new PTAEducationSubjectMarks
                    {
                        Id = x.Id,
                        MaximumMarks = x.MaximumMark,
                        ObtainedMarks = x.ObtainMark,
                        Percentage = x.Percentage,
                        SubjectName = x.SubjectName,
                        PtaEducationDetailsId = x.ptaEducationDetailId
                    }).ToList()
                }).ToList();
                Model.AddressInfo = Data.ptaRegistrationInfoes.FirstOrDefault().ptaAddressDetails.Select(item => new PTAAddressInfoViewModel
                {
                    Id = item.Id,
                    CorAddress1 = item.Cor_Address1,
                    CorAddress2 = item.Cor_Address2,
                    CorCity = RemoveWhiteSpaces(!string.IsNullOrEmpty(item.Cor_City) ? item.Cor_City : ""),
                    CorCountry = RemoveWhiteSpaces(!string.IsNullOrEmpty(item.Cor_Country) ? item.Cor_Country : ""),
                    CorDistrict = RemoveWhiteSpaces(!string.IsNullOrEmpty(item.Cor_District) ? item.Cor_District : ""),
                    CorPin = item.Cor_PinCode,
                    CorState = RemoveWhiteSpaces(!string.IsNullOrEmpty(item.Cor_State) ? item.Cor_State : ""),
                    PerAddress1 = item.Per_Address1,
                    PerAddress2 = item.Per_Address2,
                    IsPermanent = item.IsPermanent,
                    PerCity = RemoveWhiteSpaces(!string.IsNullOrEmpty(item.Per_City) ? item.Per_City : ""),
                    PerCountry = RemoveWhiteSpaces(!string.IsNullOrEmpty(item.Per_Country) ? item.Per_Country : ""),
                    PerDistrict = RemoveWhiteSpaces(!string.IsNullOrEmpty(item.Per_District) ? item.Per_District : ""),
                    PerPin = item.Per_PinCode,
                    PerState = RemoveWhiteSpaces(!string.IsNullOrEmpty(item.Per_State) ? item.Per_State : "")
                }).FirstOrDefault();
                Model.CountryList = GetCityCountryInfoList("Country");
            }
            return Model;
        }

        public List<ptaDocumentDetails> GetDocumentDetails(string applNo)
        {
            return (from pdd in _context.ptaDocumentDetails
                    join dmm in _context.DocumentMasters on pdd.DocumentMasterId equals dmm.Id
                    where pdd.DocumentMasterId == 7 && pdd.IsActive == true
                    select new
                    { pdd.Id, pdd.EnteredDate, pdd.Extention, dmm.DocumentName, pdd.DocumentMasterId, pdd.DocumentPath }).ToList().Select(item => new ptaDocumentDetails
                    {
                        Id = item.Id,
                        DocumentMasterId = item.DocumentMasterId,
                        DocumentName = item.DocumentName,
                        EneteredDate = item.EnteredDate,
                        Extension = item.Extention,
                        DocumentPath = item.DocumentPath
                    }).ToList(); ;
        }

        public object DeleteFile(int documentId)
        {
            var data = _context.ptaDocumentDetails.Where(x => x.Id == documentId).FirstOrDefault();
            _context.ptaDocumentDetails.Remove(data);
            _context.SaveChanges();
            return data;

        }

        public List<CityCountryInfoViewModel> GetCityCountryInfoList(string str, string CountryName = null, string State = null, string District = null, string City = null)
        {
            var Data = _context.ptaCountryCityInfoes.ToList();
            if (str == "Country")
            {
                return Data.Select(i => i.Country).Distinct().Select(x => new CityCountryInfoViewModel
                {
                    Id = RemoveWhiteSpaces(x),
                    Name = x
                }).ToList();
            }
            else if (str == "State")
            {
                return Data.Where(x => x.Country == CountryName).Select(a => a.State).Distinct().Select(x => new CityCountryInfoViewModel
                {
                    Id = RemoveWhiteSpaces(x),
                    Name = x
                }).ToList();
            }
            else if (str == "District")
            {
                return Data.Where(x => x.Country == CountryName && x.State == State).Select(a => a.District).Distinct().Select(x => new CityCountryInfoViewModel
                {
                    Id = RemoveWhiteSpaces(x),
                    Name = x
                }).ToList();
            }
            else
            {
                return Data.Where(x => x.Country == CountryName && x.State == State && x.District == District).Select(a => a.City).Distinct().Select(x => new CityCountryInfoViewModel
                {
                    Id = RemoveWhiteSpaces(x),
                    Name = x
                }).ToList();
            }

        }

        public string RemoveWhiteSpaces(string str)
        {
            return Regex.Replace(str, @"\s+", String.Empty);
        }

        public List<GetDDListInfoViewModel> GetResultStatusView()
        {
            return _context.ptaResultStatusMasters.Where(p => p.IsActive).Select(item => new GetDDListInfoViewModel
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }

        public List<GetDDListInfoViewModel> GetStreamView()
        {
            return _context.ptaStreamMasters.Where(p => p.IsActive).Select(item => new GetDDListInfoViewModel
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }

        public List<GetDDListInfoViewModel> GetBoardAndUniversityListUG()
        {
            return _context.ptaUGBoardAndUniversityMasters.Where(p => p.IsActive).Select(item => new GetDDListInfoViewModel
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }

        public List<GetDDListInfoViewModel> GetEvaluationList()
        {
            return _context.ptaEduEvaluationTypeMasters.Where(p => p.IsActive).Select(item => new GetDDListInfoViewModel
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }

        public string UpdateAddressDetails(PTAAddressInfoViewModel Model)
        {
            string responce = "";
            var data = _context.ptaAddressDetails.Where(a => a.ptaRegistrationInfo.ptaPilotRegistrationMaster.ApplicationNo == Model.ApplicationNo).FirstOrDefault();
            if (data != null)
            {
                data.Cor_Address1 = Model.CorAddress1;
                data.Cor_Address2 = Model.CorAddress2;
                data.Cor_City = Model.CorCity;
                data.Cor_Country = Model.CorCountry;
                data.Cor_District = Model.CorDistrict;
                data.Cor_State = Model.CorState;
                data.Cor_PinCode = Model.PerPin;
                data.Per_Address1 = Model.PerAddress1;
                data.Per_Address2 = Model.PerAddress2;
                data.Per_City = Model.PerCity;
                data.Per_Country = Model.PerCountry;
                data.Per_District = Model.PerDistrict;
                data.Per_State = Model.PerState;
                data.Per_PinCode = Model.PerPin;
                _context.SaveChanges();
                responce = "Address updated successfully!";
            }
            return responce;
        }

        public object EducationUploadFiles(List<PilotDocumentMasterViewModel> Model)
        {

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
                                ptaPilotRegistrationMasterId = item.ptaPilotRegistrationMasterId,
                                ptaAdmissionMasterId = item.PilotAdmissionId,
                                EnteredBy = 11,
                                EnteredDate = DateTime.Now
                            };
                            _context.ptaDocumentDetails.Add(info);
                        }
                        _context.SaveChanges();
                        Massage = "Other document successfully uploaded.";
                    }
                    return Massage;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public string UpdateBasicDetails(PilotRegistrationViewModel model)
        {
            string response = "";
            var data = _context.ptaPilotRegistrationMasters.Where(x => x.ApplicationNo == model.ApplicationNo).FirstOrDefault();
            if (data != null)
            {
                data.ApplicationNo = model.ApplicationNo;
                data.Email = model.Email;
                data.Mobile = model.Mobile;
                data.DOB = (!string.IsNullOrEmpty(model.DOBStr) ? DateTime.ParseExact(model.DOBStr, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null);
                _context.SaveChanges();
                response = "Basics Details Updated!";
            }
            return response;
        }


        public List<GetDDListInfoViewModel> GetBoardAndUniversityList()
        {
            return _context.ptaBoardAndUniversityMasters.Where(p => p.IsActive).Select(item => new GetDDListInfoViewModel
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }


        //*******************Upload Docs(17/03/20)**************//

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
                        data.UpdatedStudentBy = item.UpdatedStudentBy;
                    }
                    else
                    {
                        if (item.EnteredStudentBy > 0)
                        {
                            var ad = _context.ptaAdmissionMasters.Where(p => p.ptaRegistrationInfoes.FirstOrDefault().ptaPilotRegistrationMasterId == item.ptaPilotRegistrationMasterId).FirstOrDefault();
                            if (ad != null)
                            {
                                ptaDocumentDetail info = new ptaDocumentDetail
                                {
                                    DocumentMasterId = item.DocumentMasterId,
                                    DocumentPath = item.DocumentPath,
                                    IsActive = true,
                                    Extention = item.Extention,
                                    ptaPilotRegistrationMasterId = item.ptaPilotRegistrationMasterId,
                                    ptaAdmissionMasterId = ad.Id,
                                    EnteredDate = DateTime.Now,
                                    EnteredStudentBy = item.EnteredStudentBy
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

        public List<ptaFeeTypeInfoViewModel> GetFeeTypeList()
        {
            return _context.FeeTypes.Where(f => f.IsActive).Select(i => new ptaFeeTypeInfoViewModel
            {
                Id = i.Id,
                Name = i.Name
            }).ToList();
        }


        public List<ptaFeeTypeInfoViewModel> GetSessionList()
        {
            return _context.SessionMasters.Where(f => f.IsActive).Select(i => new ptaFeeTypeInfoViewModel
            {
                Id = i.Id,
                Name = i.SessionName
            }).ToList();
        }

        public List<ptaFeeTypeInfoViewModel> GetCourseList()
        {
            return _context.CourseMasters.Where(f => f.IsActive && f.DepartmentMasterId == 2).Select(i => new ptaFeeTypeInfoViewModel
            {
                Id = i.Id,
                Name = i.CourseName
            }).ToList();
        }
        public IEnumerable<ptaFeeTypeInfoViewModel> GetSessionListByCourseId(int CourseId)
        {
            return _context.SessionMasters.Where(s => s.IsActive && s.CourseMasterId == CourseId)
                .Select(item => new ptaFeeTypeInfoViewModel
                {
                    Id = item.Id,
                    Name = item.SessionName
                }).AsEnumerable();
        }

        public string AddUpdateFeeTypeDetail(StudentInfoViewModel model)
        {
            string Message = "";
            try
            {
                ptaFeeTypeDetail obj;
                if (model.Id > 0)
                {
                    obj = _context.ptaFeeTypeDetails.Where(f => f.Id == model.Id).FirstOrDefault();
                    Message = "Successfully Updated!";
                }
                else
                {
                    var data = _context.ptaFeeTypeDetails
                    .Where(f => f.PTACourseMasterId == model.CourseId
                    && f.PTASessionMasterId == model.SessionMasterId
                    && f.PTAFeeTypeId == model.FeeTypeId).FirstOrDefault();
                    if (data != null)
                    {
                        Message = "This FeeType Detail record already exist!";
                        return Message;
                    }
                    obj = new ptaFeeTypeDetail();
                    Message = "Successfully Inserted!";
                }
                obj.PTACourseMasterId = model.CourseId;
                obj.Amount = model.Amount;
                obj.PTAFeeTypeId = model.FeeTypeId;
                obj.IsActive = model.IsActive;
                obj.PTASessionMasterId = model.SessionMasterId;
                if (model.Id == 0)
                    _context.ptaFeeTypeDetails.Add(obj);
                _context.SaveChanges();
                return Message;
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
                            ptaPilotRegistrationMasterId = item.ptaPilotRegistrationMasterId,
                            ptaAdmissionMasterId = item.PilotAdmissionId,
                            EnteredStudentBy = item.EnteredStudentBy,
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

        public PilotMedicalInfoViewModel InitialMedicalInfo(int Id)
        {
            PilotMedicalInfoViewModel result = new PilotMedicalInfoViewModel();
            result.PilotRegistrationNo = Id;
            var data = _context.ptaDocumentDetails.Where(p => p.ptaPilotRegistrationMasterId == Id && p.DocumentMaster.DocumentName != "Other").ToList();
            var initialDocList = _context.DocumentMasters.Where(d => d.IsActive && d.DepartmentMasterId == 2 && d.DocumentName != "Other").Select(item => new PilotDocumentMasterViewModel
            {
                DocumentMasterId = item.Id,
                DocumentName = item.DocumentName,
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

        //*****************************************************************//
    }
}
