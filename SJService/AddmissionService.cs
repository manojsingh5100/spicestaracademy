using SJModel;
using SJData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using SJModel.Helper;

namespace SJService
{
    public class AddmissionService
    {
        public SJStarERPEntities _context = null;
        public AddmissionService()
        {
            _context = new SJStarERPEntities();
        }

        public List<SemesterMasterViewModel> GetBatchList()
        {
            return _context.BatchMasters.Where(s => s.IsActive).Select(m => new
            {
                Id = m.Id,
                BatchName = m.Name
            }).ToList().Select(item => new SemesterMasterViewModel
            {
                Id = item.Id,
                BatchName = item.BatchName,
                BatchId = Convert.ToInt32(item.BatchName.Split(' ')[1])
            }).OrderBy(o => o.BatchId).ToList();
        }

        public List<CourseMasterViewModel> GetCourseList()
        {
            return _context.CourseMasters.Where(s => s.IsActive && s.DepartmentMasterId == 1).Select(m => new CourseMasterViewModel
            {
                Id = m.Id,
                CourseName = m.CourseName
            }).ToList();
        }

        public List<SessionMasterViewModel> GetSessionList()
        {
            return _context.SessionMasters.Where(s => s.IsActive).Select(m => new SessionMasterViewModel
            {
                Id = m.Id,
                SessionName = m.SessionName
            }).ToList();
        }

        public List<RoleViewModel> GetLeadSourceList()
        {
            return _context.ptaLeadSourceMasters.Where(w => w.IsActive).Select(item => new RoleViewModel
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }

        public int CreateAddmission1(int RegNo, int CreatedBy, int Id, int? BatchId, string CreatedDate)
        {
            var data = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == RegNo).FirstOrDefault();
            AddmissionMaster addmissionMaster = null;
            if (data != null)
            {
                addmissionMaster = new AddmissionMaster
                {
                    CourseId = data.CourseId.Value,
                    CreatedBy = CreatedBy,
                    DOB = data.DOB,
                    Education = data.Education,
                    Email = data.Email,
                    Fname = data.Fname,
                    Lname = data.Lname,
                    Gender = data.Gender,
                    IsAppeared = data.IsAppeared,
                    IsValidPassport = data.IsPassport,
                    MobileNo = data.Mobile,
                    PassingYear = data.PassingYear,
                    RegistrationNo = data.RegistartionNo,
                    AddmissionDate = (!string.IsNullOrEmpty(CreatedDate) ? DateTime.ParseExact(CreatedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now),
                    IsActive = true,
                    RegistrationMasterId = Id
                };

                AddressDetail addressDetail = new AddressDetail
                {
                    AddmissionId = addmissionMaster.Id,
                    CopAddress = data.CorrespondenceAddress,
                    CopCity = data.CorrespondenceCity,
                    CopState = data.CorrespondenceState,
                    CopZip = data.CorrespondenceZip,
                    PerAddress = data.PermanentAddress,
                    PerCity = data.PermanentCity,
                    PerState = data.PermanentState,
                    PerZip = data.PermanentZip
                };

                MedicalDetail medicalDetail = new MedicalDetail
                {
                    AddmissionId = addmissionMaster.Id,
                    Height = data.Height,
                    Weight = data.Width
                };

                AddmissionDetail addmissionDetail = new AddmissionDetail
                {
                    AddmissionId = addmissionMaster.Id,
                    CreatedBy = CreatedBy,
                    CreatedDate = (!string.IsNullOrEmpty(CreatedDate) ? DateTime.ParseExact(CreatedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now),
                    SessionId = data.SessionId.Value,
                    BatchId = BatchId
                };
                _context.AddmissionMasters.Add(addmissionMaster);
                _context.AddmissionDetails.Add(addmissionDetail);
                _context.AddressDetails.Add(addressDetail);
                _context.MedicalDetails.Add(medicalDetail);
                data.IsAddmission = true;
            }
            _context.SaveChanges();
            return addmissionMaster.RegistrationNo;
        }

        public int CreateAddmission(RegistrationViewModel model)
        {
            var data = _context.RegistrationMasters.Where(r => r.IsActive && r.Id == model.Id).FirstOrDefault();
            AddmissionMaster addmissionMaster = null;
            if (data != null)
            {
                addmissionMaster = new AddmissionMaster
                {
                    AddmissionDate = DateTime.Now,
                    CourseId = model.CourseId.Value,
                    CreatedBy = model.CreatedBy,
                    DOB = data.DOB,
                    Education = "10+2",
                    Email = data.Email,
                    Fname = data.Fname,
                    Lname = data.Lname,
                    Gender = data.Gender,
                    IsAppeared = data.IsAppeared,
                    IsValidPassport = data.IsPassport,
                    MobileNo = data.Mobile,
                    PassingYear = data.PassingYear,
                    RegistrationNo = data.RegistartionNo,
                    AmityEnrollMentId = model.AmityEnNo
                };

                AddressDetail addressDetail = new AddressDetail
                {
                    AddmissionId = addmissionMaster.Id,
                    CopAddress = data.CorrespondenceAddress,
                    CopCity = data.CorrespondenceCity,
                    CopState = data.CorrespondenceState,
                    CopZip = data.CorrespondenceZip,
                    PerAddress = data.PermanentAddress,
                    PerCity = data.PermanentCity,
                    PerState = data.PermanentState,
                    PerZip = data.PermanentZip
                };

                MedicalDetail medicalDetail = new MedicalDetail
                {
                    AddmissionId = addmissionMaster.Id,
                    Height = data.Height,
                    Weight = data.Width
                };

                AddmissionDetail addmissionDetail = new AddmissionDetail
                {
                    AddmissionId = addmissionMaster.Id,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.Now,
                    SessionId = model.SessionId.Value
                };
                _context.AddmissionMasters.Add(addmissionMaster);
                _context.AddmissionDetails.Add(addmissionDetail);
                _context.AddressDetails.Add(addressDetail);
                _context.MedicalDetails.Add(medicalDetail);
                data.IsAddmission = true;
            }
            _context.SaveChanges();
            return addmissionMaster.Id;
        }

        public bool IsAdmissionOfCandidate(int Id)
        {
            return _context.RegistrationMasters
                .Join(_context.AddmissionMasters,
                r => r.RegistartionNo, a => a.RegistrationNo,
                (r, a) => new { r, a }).Join
                (_context.AddmissionDetails, combinedEntity => combinedEntity.a.Id,
                ad => ad.AddmissionId, (combinedEntity, ad) => new { RegistrationId = combinedEntity.r.Id })
                .Where(res => res.RegistrationId.Equals(Id)).Any();
        }

        public DataTableFilterModel GetAddmissionViewList(DataTableFilterModel filter, int SessionYr)
        {
            var result = _context.AddmissionMasters.Where(a => a.IsActive && a.RegistrationMaster.IsActive && ((a.MedicalDetails.FirstOrDefault().MedicalStatus == "TMU" && a.RegistrationMaster.FeeDetails.Any()) || !a.RegistrationMaster.IsMedicalClear.HasValue || a.RegistrationMaster.IsMedicalClear.Value) && a.AddmissionDetails.FirstOrDefault().BatchMaster.IsActive && a.AddmissionDetails.FirstOrDefault().BatchId != 19);
            if (SessionYr > 0)
                result = result.Where(d => d.AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart.Value.Year == SessionYr);
            var data = result.Select(a => new AddmissionMasterViewModel
            {
                Id = a.Id,
                RegNo = a.RegistrationNo,
                AddmissionDate = a.AddmissionDate,
                DOB = a.DOB,
                Email = a.Email,
                CourseName = a.CourseMaster.CourseName,
                BatchName = a.AddmissionDetails.FirstOrDefault().BatchMaster.Name,
                BatchId = a.AddmissionDetails.FirstOrDefault().BatchId,
                BatchStartDate = a.AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart,
                SessionName = a.AddmissionDetails.FirstOrDefault().SessionMaster.SessionName,
                Fname = a.Fname,
                Lname = a.Lname,
                MobileNo = a.MobileNo,
                IsValidPassport = a.IsValidPassport,
                Gender = a.Gender,
                MedicalStatus = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == a.RegistrationNo).FirstOrDefault().MedicalRemark,
                IsBatchHistory = a.BatchHistoryDetails.Any(),
                IsTermResgCandidate = a.CandidateTerminationResignationInfoes.Any() ? (a.CandidateTerminationResignationInfoes.FirstOrDefault().IsActive ? a.CandidateTerminationResignationInfoes.FirstOrDefault().CandidateActionMaster.Name : (a.CandidateTerminationResignationInfoes.FirstOrDefault().CandidateTRInfoes.OrderByDescending(e => e.Id).FirstOrDefault().IsActive ? "P-" + a.CandidateTerminationResignationInfoes.FirstOrDefault().CandidateTRInfoes.OrderByDescending(f => f.Id).FirstOrDefault().ActionName : "")) : ""
            }).AsEnumerable();

            var totalCount = data.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                if (filter.search.value.Contains("'"))
                {
                    filter.search.value = filter.search.value.Replace(@"'", "");
                    data = data.Where(d => filter.search.value.Split(',').Where(a => a.ToLower() == d.BatchName.ToLower()).Any());
                    filter.columns[filter.order[0].column].data = "BatchName";
                }
                else
                {
                    data = data.Where(d => d.RegNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.MobileNo) && d.MobileNo.ToString().ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.BatchName) && d.BatchName.ToString().ToLower().Contains(filter.search.value.ToLower())));
                }
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[7].search.value) && filter.columns[7].search.value != "")
            {
                int batch = Convert.ToInt32(filter.columns[7].search.value);
                data = data.Where(t => t.BatchId == batch);
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[5].search.value))
                data = data.Where(t => t.CourseName == filter.columns[5].search.value);

            if (!string.IsNullOrWhiteSpace(filter.columns[3].search.value) && (filter.columns[3].search.value == "Termination" || filter.columns[3].search.value == "Resignation"))
                data = data.Where(t => t.IsTermResgCandidate == filter.columns[3].search.value);

            var o = filter.order[0];
            var name = filter.columns[filter.order[0].column].data;

            if (o.dir == "asc")
            {
                data = data.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
            }
            else
            {
                data = data.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
            }

            var filteredCount = data.Count();
            filter.recordsTotal = totalCount;
            filter.recordsFiltered = filteredCount;
            var dataFilter = data.Skip(filter.start).Take(filter.length).ToList();
            foreach (var i in dataFilter)
            {
                i.DateOfBirth = i.DOB.HasValue ? i.DOB.Value.ToString("dd-MM-yyyy") : "";
                i.DateOfAddmission = i.BatchStartDate.HasValue ? i.BatchStartDate.Value.ToString("dd-MM-yyyy") : "";
            }
            filter.data = dataFilter;
            return filter;
        }

        public DataTableFilterModel GetApprovedStudentList(DataTableFilterModel filter)
        {
            var data = _context.RegistrationMasters.Where(r => r.IsActive).AsQueryable();
            data = data.Where(r => !r.IsAddmission && r.IsScreenningClear.HasValue && r.IsScreenningClear.Value && r.IsMedicalClear.HasValue && r.IsMedicalClear.Value);
            var info = data.Select(model => new RegistrationViewModel()
            {
                Id = model.Id,
                DOB = model.DOB,  // DateTime.ParseExact(model.DOB, "dd-MM-yyyy", CultureInfo.InvariantCulture) ,
                Email = model.Email,
                Gender = model.Gender == "M" ? "Male" : "Female",
                Mobile = model.Mobile,
                RegistartionNo = model.RegistartionNo,
                Fname = model.Fname,
                Lname = model.Lname,
                //DateOfBirth = "",
                IsAddmission = model.IsAddmission,
                CourseName = model.CourseMaster.CourseName
            }).AsEnumerable();

            var totalCount = info.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                info = info.Where(d => (d.Email.ToLower().Contains(filter.search.value.ToLower()) || d.RegistartionNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || d.Mobile.ToLower().Contains(filter.search.value.ToLower()) || d.Gender.ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower()))));
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[7].search.value))
                info = info.Where(t => t.CourseName == filter.columns[7].search.value);

            var o = filter.order[0];
            var name = filter.columns[filter.order[0].column].data;
            if (o.dir == "asc")
            {
                info = info.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
            }
            else
            {
                info = info.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
            }

            var filteredCount = info.Count();
            filter.recordsTotal = totalCount;
            filter.recordsFiltered = filteredCount;
            var dataFilter = info.Skip(filter.start).Take(filter.length).ToList();
            //foreach (var i in dataFilter)
            //{
            //    i.DateOfBirth = i.DOB.HasValue ? i.DOB.Value.ToString("dd-MM-yyyy") : "";
            //}
            filter.data = dataFilter;
            return filter;
        }

        public AddmissionMasterViewModel GetAddmissionMasterInfo(int AdmissionId)
        {
            return _context.AddmissionMasters.Where(a => a.Id == AdmissionId).Select(m => new AddmissionMasterViewModel
            {
                AddmissionDate = m.AddmissionDate,
                CourseId = m.CourseId,
                CourseName = m.CourseMaster.CourseName,
                DOB = m.DOB,
                Email = m.Email,
                Fname = m.Fname,
                Lname = m.Lname,
                Gender = m.Gender,
                IsValidPassport = m.IsValidPassport,
                IsAppeared = m.IsAppeared,
                MobileNo = m.MobileNo,
                Id = m.Id,
                //PassingYear = m.PassingYear,
                ImageUrl = m.ImageUrl,
                AmityEnNo = m.AmityEnrollMentId,
                RegNo = m.RegistrationNo,
                SessionName = m.AddmissionDetails.FirstOrDefault().SessionMaster.SessionName,
                Education = m.Education,
                ResumeUrl = m.ResumeUrl
            }).FirstOrDefault();
        }

        public AdmissionInfoViewModel GetAddmissionDetail(int AdmissionId)
        {
            FeeManagementService feeManagement = new FeeManagementService();
            var data = _context.AddmissionMasters.Where(a => a.Id == AdmissionId).Select(m => new AdmissionInfoViewModel
            {
                AdmissionDetail = new AddmissionMasterViewModel
                {
                    AddmissionDate = m.AddmissionDate,
                    CourseId = m.CourseId,
                    CourseName = m.CourseMaster.CourseName,
                    DOB = m.DOB,
                    Email = m.Email,
                    Fname = m.Fname,
                    Lname = m.Lname,
                    Gender = m.Gender,
                    IsValidPassport = m.IsValidPassport,
                    IsAppeared = m.IsAppeared,
                    MobileNo = m.MobileNo,
                    Id = m.Id,
                    //PassingYear = m.PassingYear,
                    ImageUrl = m.ImageUrl,
                    AmityEnNo = m.AmityEnrollMentId,
                    RegNo = m.RegistrationNo,
                    SessionName = m.AddmissionDetails.FirstOrDefault().SessionMaster.SessionName,
                    Education = m.Education,
                    ResumeUrl = m.ResumeUrl,
                    RegisterEmail = m.RegisterEmail,
                    AdmissionId = m.Id,
                    FeeCapitalAmount = _context.FeeTypeDetails.Where(f => f.IsActive && f.CourseId == m.CourseId).FirstOrDefault().Amount
                },
                MedicalDetail = new MedicalDetailViewModel
                {
                    Height = m.MedicalDetails.FirstOrDefault().Height,
                    Weight = m.MedicalDetails.FirstOrDefault().Weight,
                    Id = m.MedicalDetails.FirstOrDefault().Id,
                    VisionLeft = m.MedicalDetails.FirstOrDefault().VisionLeft,
                    VisionRight = m.MedicalDetails.FirstOrDefault().VisionRight,
                    MedicalCenter = m.MedicalDetails.FirstOrDefault().MedicalCenter,
                    MedicalStatus = m.MedicalDetails.FirstOrDefault().MedicalStatus,
                    MDate = m.MedicalDetails.FirstOrDefault().MedicalDate,
                    FDate = m.MedicalDetails.FirstOrDefault().FitnessDate,
                    JDate = m.MedicalDetails.FirstOrDefault().DateOfJoining
                },
                AddressDetail = new AddressDetailViewModel
                {
                    Id = m.AddressDetails.FirstOrDefault().Id,
                    CopAddress = m.AddressDetails.FirstOrDefault().CopAddress,
                    CopCity = m.AddressDetails.FirstOrDefault().CopCity,
                    CopState = m.AddressDetails.FirstOrDefault().CopState,
                    CopZip = m.AddressDetails.FirstOrDefault().CopZip,
                    PerAddress = m.AddressDetails.FirstOrDefault().PerAddress,
                    PerCity = m.AddressDetails.FirstOrDefault().PerCity,
                    PerState = m.AddressDetails.FirstOrDefault().PerState,
                    PerZip = m.AddressDetails.FirstOrDefault().PerZip
                },
            }).FirstOrDefault();
            if (data != null)
            {
                data.GetFeeDetailOfCandidate = feeManagement.GetFeeInfoByRegId(data.AdmissionDetail.RegNo);
                if (data.AdmissionDetail.DOB.HasValue)
                    data.AdmissionDetail.DateOfBirth = data.AdmissionDetail.DOB.Value.Day.ToString().PadLeft(2, '0') + "/" + data.AdmissionDetail.DOB.Value.Month.ToString().PadLeft(2, '0') + "/" + data.AdmissionDetail.DOB.Value.Year;
                data.BatchList = GetBatchList();
                var admissionData = _context.AddmissionDetails.Where(a => a.AddmissionId == data.AdmissionDetail.Id).FirstOrDefault();
                if (admissionData != null)
                {
                    data.AdmissionDetail.BatchId = admissionData.BatchId;
                    data.AdmissionDetail.BatchfromId = admissionData.BatchId.Value;
                }
                if (data.MedicalDetail.MDate.HasValue)
                    data.MedicalDetail.MedicalDate = data.MedicalDetail.MDate.Value.Day.ToString().PadLeft(2, '0') + "/" + data.MedicalDetail.MDate.Value.Month.ToString().PadLeft(2, '0') + "/" + data.MedicalDetail.MDate.Value.Year;
                if (data.MedicalDetail.FDate.HasValue)
                    data.MedicalDetail.FitnessDate = data.MedicalDetail.FDate.Value.Day.ToString().PadLeft(2, '0') + "/" + data.MedicalDetail.FDate.Value.Month.ToString().PadLeft(2, '0') + "/" + data.MedicalDetail.FDate.Value.Year;
                if (data.MedicalDetail.JDate.HasValue)
                    data.MedicalDetail.JoiningDate = data.MedicalDetail.JDate.Value.Day.ToString().PadLeft(2, '0') + "/" + data.MedicalDetail.JDate.Value.Month.ToString().PadLeft(2, '0') + "/" + data.MedicalDetail.JDate.Value.Year;
            }
            return data;
        }

        public bool UpdateImagePath(int id, string path = null)
        {
            bool status = false;
            var data = _context.AddmissionMasters.Where(a => a.Id == id).FirstOrDefault();
            if (data != null)
            {
                data.ImageUrl = path;
                _context.SaveChanges();
                status = true;
            }
            return status;
        }

        public bool UpdateResumePath(int id, string path = null)
        {
            bool status = false;
            var data = _context.AddmissionMasters.Where(a => a.Id == id).FirstOrDefault();
            if (data != null)
            {
                data.ResumeUrl = path;
                _context.SaveChanges();
                status = true;
            }
            return status;
        }

        public AddmissionMasterViewModel UpdateAdmissionBasicDetail(AddmissionMasterViewModel model)
        {
            if (model != null && model.Id > 0)
            {
                var data = _context.AddmissionMasters.Where(a => a.Id == model.Id).FirstOrDefault();
                if (data != null)
                {
                    model.ResumeUrl = data.ResumeUrl;
                    model.RegNo = data.RegistrationNo;
                    data.Email = model.Email;
                    data.Fname = model.Fname;
                    data.Lname = model.Lname;
                    if (model.DateOfBirth != null && model.DateOfBirth != "")
                        data.DOB = DateTime.ParseExact(model.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    data.MobileNo = model.MobileNo;
                    data.RegisterEmail = model.RegisterEmail;
                    data.IsValidPassport = model.IsValidPassport;
                    data.IsAppeared = model.IsAppeared;
                    data.AmityEnrollMentId = model.AmityEnNo;
                    if (model.IsAppeared)
                        data.PassingYear = null;
                    else
                        data.PassingYear = model.PassingYear;
                    data.UpdatedBy = model.UpdatedBy;

                    if (data.RegistrationMaster != null)
                    {
                        data.RegistrationMaster.Fname = model.Fname;
                        data.RegistrationMaster.Lname = model.Lname;
                        data.RegistrationMaster.Email = model.Email;
                        data.RegistrationMaster.Mobile = model.MobileNo;
                        data.RegistrationMaster.IsPassport = model.IsValidPassport;
                        data.RegistrationMaster.IsAppeared = model.IsAppeared;
                        if (model.DateOfBirth != null && model.DateOfBirth != "")
                            data.RegistrationMaster.DOB = DateTime.ParseExact(model.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        if (model.IsAppeared)
                            data.RegistrationMaster.PassingYear = null;
                        else
                            data.RegistrationMaster.PassingYear = model.PassingYear;
                    }

                    var admissionData = _context.AddmissionDetails.Where(a => a.AddmissionId == data.Id).FirstOrDefault();
                    if (admissionData != null)
                        admissionData.BatchId = model.BatchId;

                    if (model.BatchId != model.BatchfromId)
                    {
                        BatchHistoryDetail batchHistory = new BatchHistoryDetail
                        {
                            AdmissionId = data.Id,
                            BatchFromId = model.BatchfromId,
                            BatchToId = model.BatchId.Value,
                            Description = model.BatchDescription,
                            EnteredDate = DateTime.Now,
                            EnteredBy = model.UpdatedBy.Value
                        };
                        _context.BatchHistoryDetails.Add(batchHistory);
                    }
                    _context.SaveChanges();
                }
            }
            return model;
        }

        public bool UpdateMedicalDetail(MedicalDetailViewModel model)
        {
            bool status = false;
            if (model != null && model.Id > 0)
            {
                var data = _context.MedicalDetails.Where(m => m.Id == model.Id).FirstOrDefault();
                if (data != null)
                {
                    data.Height = model.Height;
                    data.Weight = model.Weight;
                    data.VisionLeft = model.VisionLeft;
                    data.VisionRight = model.VisionRight;
                    data.MedicalCenter = model.MedicalCenter;
                    data.MedicalStatus = model.MedicalStatus;
                    if (!string.IsNullOrEmpty(model.MedicalDate))
                        data.MedicalDate = DateTime.ParseExact(model.MedicalDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(model.FitnessDate))
                        data.FitnessDate = DateTime.ParseExact(model.FitnessDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(model.JoiningDate))
                        data.DateOfJoining = DateTime.ParseExact(model.JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    data.AddmissionMaster.RegistrationMaster.Height = model.Height;
                    data.AddmissionMaster.RegistrationMaster.Width = model.Weight;

                    status = true;
                    _context.SaveChanges();
                }
            }
            return status;
        }

        public bool UpdateAddressDetail(AddressDetailViewModel model)
        {
            bool status = false;
            if (model != null && model.Id > 0)
            {
                var data = _context.AddressDetails.Where(m => m.Id == model.Id).FirstOrDefault();
                if (data != null)
                {
                    data.CopAddress = model.CopAddress;
                    data.CopCity = model.CopCity;
                    data.CopState = model.CopState;
                    data.CopZip = model.CopZip;
                    data.PerAddress = model.PerAddress;
                    data.PerCity = model.PerCity;
                    data.PerState = model.PerState;
                    data.PerZip = model.PerZip;

                    data.AddmissionMaster.RegistrationMaster.PermanentAddress = model.PerAddress;
                    data.AddmissionMaster.RegistrationMaster.PermanentCity = model.PerCity;
                    data.AddmissionMaster.RegistrationMaster.PermanentState = model.PerState;
                    data.AddmissionMaster.RegistrationMaster.PermanentZip = model.PerZip;
                    data.AddmissionMaster.RegistrationMaster.CorrespondenceAddress = model.CopAddress;
                    data.AddmissionMaster.RegistrationMaster.CorrespondenceCity = model.CopCity;
                    data.AddmissionMaster.RegistrationMaster.CorrespondenceState = model.CopState;
                    data.AddmissionMaster.RegistrationMaster.CorrespondenceZip = model.CopZip;

                    status = true;
                    _context.SaveChanges();
                }
            }
            return status;
        }

        public DataTableFilterModel GetCandidateReport(DataTableFilterModel filter, int SessionYr)
        {
            var result = _context.AddmissionMasters.Where(a => a.IsActive);
            if (SessionYr > 0)
                result = result.Where(d => d.AddmissionDetails.FirstOrDefault().SessionMaster.SessionYr == SessionYr);
            var info = result.Select(model => new RegistrationViewModel()
            {
                RegistartionNo = model.RegistrationNo,
                Fname = model.Fname,
                Lname = model.Lname,
                DOB = model.DOB,
                Email = model.Email,
                Mobile = model.MobileNo,
                Gender = model.Gender,
                Education = model.Education,
                CourseName = model.CourseMaster.CourseName,
                CourseId = model.CourseId,
                PermanentState = model.AddressDetails.Any(ad => ad.AddmissionId == model.Id) ? model.AddressDetails.Where(add => add.AddmissionId == model.Id).FirstOrDefault().PerState : "",
                CorrespondenceState = model.AddressDetails.Any(ad => ad.AddmissionId == model.Id) ? model.AddressDetails.Where(add => add.AddmissionId == model.Id).FirstOrDefault().CopState : "",
                PermanentCity = model.AddressDetails.Any(ad => ad.AddmissionId == model.Id) ? model.AddressDetails.Where(add => add.AddmissionId == model.Id).FirstOrDefault().PerCity : "",
                CorrespondenceCity = model.AddressDetails.Any(ad => ad.AddmissionId == model.Id) ? model.AddressDetails.Where(add => add.AddmissionId == model.Id).FirstOrDefault().CopCity : "",
                BatchName = model.AddmissionDetails.Any(ad => ad.AddmissionId == model.Id) ? model.AddmissionDetails.Where(add => add.AddmissionId == model.Id).FirstOrDefault().BatchMaster.Name : "",
                BatchId = model.AddmissionDetails.Any(ad => ad.AddmissionId == model.Id) ? model.AddmissionDetails.Where(add => add.AddmissionId == model.Id).FirstOrDefault().BatchMaster.Id : 0,
                AdmissionDate = model.AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart.Value,
                IsFeePayStandBy = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == model.RegistrationNo).Any() ? _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == model.RegistrationNo).FirstOrDefault().IsFeePayStandBy : false,
                IsFeePayment = model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).Any(a => a.RegistrationNo == model.RegistrationNo) ? (model.RegistrationMaster.FeeDetails.Any(a => a.FeeTypeDetail.IsActive && a.FeeTypeDetail.FeeTypeId == 1) ? true : false) : false,
                SourceOfCandidate = model.RegistrationMaster.SourceOfCandidate != null ? model.RegistrationMaster.SourceOfCandidate : "",
                FeeTotalAmount = model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().FeeTypeDetail.Amount,
                FeeDueAmount = (model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().FeeTypeDetail.Amount - model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().FeePaymentDetails.Sum(s => s.FeeCollections.Sum(a => a.PartWisePayments.Sum(p => p.Amount)))),
                FirstInstallment = model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().PaymentTypeId == 1 ? (model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.Sum(s => s.PartWisePayments.Sum(q => q.Amount))) : (model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.Sum(s => s.PartWisePayments.Sum(q => q.Amount))),
                SecondInstallment = _context.PartWisePayments.Where(w=> w.FeeCollection.FeePaymentDetail.FeeDetail.IsActive && w.FeeCollection.InstallmentMasterId == 2 &&  w.FeeCollection.FeePaymentDetail.FeeDetail.RegistrationNo == model.RegistrationNo).Sum(s=>s.Amount),
                ThirdInstallment = _context.PartWisePayments.Where(w => w.FeeCollection.FeePaymentDetail.FeeDetail.IsActive && w.FeeCollection.InstallmentMasterId == 3 && w.FeeCollection.FeePaymentDetail.FeeDetail.RegistrationNo == model.RegistrationNo).Sum(s => s.Amount),

                //SecondInstallment = model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().PaymentTypeId == 1 ? (model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().FeePaymentDetails.
                //Where(w=>w.FeeCollections.Where(a=>a.InstallmentMasterId == 2).Any()).FirstOrDefault().FeeCollections.Where(w => w.InstallmentMasterId == 2).Sum(s => s.PartWisePayments.Sum(q => q.Amount))) : 0,
                //ThirdInstallment = model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().PaymentTypeId == 1 ? (model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.Where(w => w.InstallmentMasterId == 3).Sum(s => s.PartWisePayments.Sum(q => q.Amount))) : 0,


                RefundStatus = model.RegistrationMaster.FeeDetails.Any() ? (model.RegistrationMaster.FeeDetails.FirstOrDefault().FeeTypeDetail.Amount == _context.PartWisePayments.Where(p => p.FeeCollection.FeePaymentDetail.FeeDetail.IsActive && p.FeeCollection.FeePaymentDetail.FeeDetail.RegistrationNo == model.RegistrationNo).Sum(s => s.Amount) ? "success" : (_context.FeeCollections.Where(f => f.FeePaymentDetail.FeeDetail.IsActive && f.FeePaymentDetail.FeeDetail.RegistrationNo == model.RegistrationNo).Sum(s => s.Amount) > 0 ? "orange" : "")) : "",

                //RefundStatus = model.RegistrationMaster.FeeDetails.Any() ? (model.RegistrationMaster.FeeDetails.FirstOrDefault ().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(a => a.Id).FirstOrDefault().Amount == model.RegistrationMaster.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(a => a.Id).FirstOrDefault().PartWisePayments.Sum(s => s.Amount) ? "success" : (model.RegistrationMaster.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(a => a.Id).FirstOrDefault().PartWisePayments.Sum(s => s.Amount) > 0 ? "orange" : "")) : "",
                BatchCommencementDate = model.AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart
            }).Where(a => a.BatchId != 19).AsEnumerable();

            var totalCount = info.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                info = info.Where(d => d.RegistartionNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[6].search.value))
            {
                int yr = Convert.ToInt32(filter.columns[6].search.value);
                info = info.Where(t => t.AdmissionDate.Year == yr);

                if (!string.IsNullOrWhiteSpace(filter.columns[7].search.value))
                {
                    int mon = Convert.ToInt32(filter.columns[7].search.value);
                    info = info.Where(t => t.AdmissionDate.Year == yr && t.AdmissionDate.Month == mon);
                }
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[2].search.value) && filter.columns[2].search.value != "" && !string.IsNullOrWhiteSpace(filter.columns[3].search.value) && filter.columns[3].search.value != "")
            {
                DateTime fromDate = DateTime.ParseExact(filter.columns[2].search.value, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                DateTime toDate = DateTime.ParseExact(filter.columns[3].search.value, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                info = info.Where(t => t.AdmissionDate.Date >= fromDate && t.AdmissionDate.Date <= toDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[8].search.value))
            {
                var courseArr = filter.columns[8].search.value.Split(',');
                if (courseArr.Length > 0)
                    info = info.Where(t => courseArr.Any(c => c == t.CourseId.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[11].search.value))
            {
                var batchArr = filter.columns[11].search.value.Split(',');
                if (batchArr.Length > 0)
                    info = info.Where(t => batchArr.Any(c => c == t.BatchId.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[14].search.value))
            {
                bool status = filter.columns[14].search.value == "Deposited" ? true : false;
                info = info.Where(t => t.IsFeePayment == status);
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[10].search.value))
            {
                info = info.Where(t => t.PermanentState == filter.columns[10].search.value);
                if (!string.IsNullOrWhiteSpace(filter.columns[12].search.value))
                    info = info.Where(t => t.PermanentCity == filter.columns[12].search.value);
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[5].search.value))
            {
                info = info.Where(t => t.SourceOfCandidate == filter.columns[5].search.value);
            }

            var o = filter.order[0];
            var name = filter.columns[filter.order[0].column].data;
            if (o.dir == "asc")
            {
                info = info.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
            }
            else
            {
                info = info.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
            }
            var filteredCount = info.Count();
            filter.recordsTotal = totalCount;
            filter.recordsFiltered = filteredCount;
            var dataFilter = info.Skip(filter.start).Take(filter.length).ToList();
            foreach (var i in dataFilter)
            {
                DateTime today = DateTime.Now;
                if (today < i.BatchCommencementDate.Value.AddMonths(11))
                    i.FeeDueDate = i.BatchCommencementDate.Value.AddMonths(11).Day.ToString().PadLeft(2, '0') + "/" + i.BatchCommencementDate.Value.AddMonths(11).Month.ToString().PadLeft(2, '0') + "/" + i.BatchCommencementDate.Value.AddMonths(11).Year;
                else
                {
                    if (today < i.BatchCommencementDate.Value.AddMonths(22))
                        i.FeeDueDate = i.BatchCommencementDate.Value.AddMonths(22).Day.ToString().PadLeft(2, '0') + "/" + i.BatchCommencementDate.Value.AddMonths(22).Month.ToString().PadLeft(2, '0') + "/" + i.BatchCommencementDate.Value.AddMonths(22).Year;
                    else
                        i.FeeDueDate = i.BatchCommencementDate.Value.AddMonths(33).Day.ToString().PadLeft(2, '0') + "/" + i.BatchCommencementDate.Value.AddMonths(33).Month.ToString().PadLeft(2, '0') + "/" + i.BatchCommencementDate.Value.AddMonths(33).Year;
                }
            }
            filter.data = dataFilter;
            return filter;
        }
        public List<RoleViewModel> BatchHistoryOpenModelPopup(int Adm)
        {
            return _context.BatchHistoryDetails.Where(w => w.AdmissionId == Adm).Select(item => new RoleViewModel
            {
                ActiveStr = item.BatchMaster.Name,
                Name = item.BatchMaster1.Name,
                Description = item.Description,
                EnteredDate = item.EnteredDate
            }).OrderByDescending(o => o.EnteredDate).ToList();
        }

        public string RemoveResume(int AdmissionId)
        {
            string url = "";
            var data = _context.AddmissionMasters.Where(a => a.Id == AdmissionId).FirstOrDefault();
            if (data != null)
            {
                url = data.ResumeUrl;
                data.ResumeUrl = null;
            }
            _context.SaveChanges();
            return url;
        }

        public string RemovePhoto(int AdmissionId)
        {
            string url = "";
            var data = _context.AddmissionMasters.Where(a => a.Id == AdmissionId).FirstOrDefault();
            if (data != null)
            {
                url = data.ImageUrl;
                data.ImageUrl = null;
            }
            _context.SaveChanges();
            return url;
        }

        public DataTableFilterModel GetCandidateListForTermination(DataTableFilterModel filter)
        {
            var data = _context.RegistrationMasters.Where(r => r.IsActive).AsQueryable();
            data = data.Where(r => !r.IsAddmission && r.IsScreenningClear.HasValue && r.IsScreenningClear.Value && r.IsMedicalClear.HasValue && r.IsMedicalClear.Value);
            var info = data.Select(model => new RegistrationViewModel()
            {
                Id = model.Id,
                DOB = model.DOB,  // DateTime.ParseExact(model.DOB, "dd-MM-yyyy", CultureInfo.InvariantCulture) ,
                Email = model.Email,
                Gender = model.Gender == "M" ? "Male" : "Female",
                Mobile = model.Mobile,
                RegistartionNo = model.RegistartionNo,
                Fname = model.Fname,
                Lname = model.Lname,
                //DateOfBirth = "",
                IsAddmission = model.IsAddmission,
                CourseName = model.CourseMaster.CourseName
            }).AsEnumerable();

            var totalCount = info.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                info = info.Where(d => (d.Email.ToLower().Contains(filter.search.value.ToLower()) || d.RegistartionNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || d.Mobile.ToLower().Contains(filter.search.value.ToLower()) || d.Gender.ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower()))));
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[7].search.value))
                info = info.Where(t => t.CourseName == filter.columns[7].search.value);

            var o = filter.order[0];
            var name = filter.columns[filter.order[0].column].data;
            if (o.dir == "asc")
            {
                info = info.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
            }
            else
            {
                info = info.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
            }

            var filteredCount = info.Count();
            filter.recordsTotal = totalCount;
            filter.recordsFiltered = filteredCount;
            var dataFilter = info.Skip(filter.start).Take(filter.length).ToList();
            //foreach (var i in dataFilter)
            //{
            //    i.DateOfBirth = i.DOB.HasValue ? i.DOB.Value.ToString("dd-MM-yyyy") : "";
            //}
            filter.data = dataFilter;
            return filter;
        }

        public int GetCandidateADMStatusByName(string Name)
        {
            int Id = 0;
            Id = _context.CandidateActionMasters.Where(w => w.IsActive && w.Name == Name).FirstOrDefault().Id;
            return Id;
        }

        public TerminationViewModel TerminationResignationPopupInfo(int AdmissionId, string ActionName)
        {
            TerminationViewModel model = new TerminationViewModel();
            model.CandidateActionName = ActionName;
            var data1 = _context.AddmissionMasters.Include("AddmissionDetails").Include("CandidateTerminationResignationInfoes").Include("CandidateTerminationResignationInfoes.CandidateTRInfoes").Where(a => a.IsActive && a.Id == AdmissionId).FirstOrDefault();
            if (data1 != null)
            {
                model.AddmissionMasterId = data1.Id;
                model.RegNo = data1.RegistrationNo;
                model.StudentName = data1.Fname + " " + data1.Lname;
                var feedetail = _context.FeeDetails.Where(f => f.IsActive && f.RegistrationNo == data1.RegistrationNo).FirstOrDefault();
                if (feedetail != null)
                {
                    var capitalAmount = feedetail.FeeTypeDetail.Amount;
                    var totalAmount = feedetail.FeePaymentDetails.Sum(p => p.FeeCollections.Sum(c => c.PartWisePayments.Sum(w => w.Amount)));
                    if (capitalAmount.HasValue && totalAmount.HasValue)
                    {
                        string money = "No Due";
                        var outstandAmount = capitalAmount.Value - totalAmount.Value;
                        if (outstandAmount > 0)
                            money = outstandAmount.ToString() + " Rs.";
                        model.OutstandingFeeAmount = money;
                    }
                }
                else
                {
                    int sessionNo = data1.AddmissionDetails.FirstOrDefault().SessionId;
                    int courseNo = data1.CourseId;
                    var feeAmount = _context.FeeTypeDetails.Where(w => w.IsActive && w.SessionMasterId == sessionNo && w.CourseId == courseNo).FirstOrDefault();
                    if (feeAmount != null)
                        model.OutstandingFeeAmount = feeAmount.Amount.ToString();
                }
                if (data1.CandidateTerminationResignationInfoes.FirstOrDefault() != null)
                {
                    model.CandidateTerminationResignationInfoId = data1.CandidateTerminationResignationInfoes.FirstOrDefault().Id;
                    model.CandidateTerminationResignationIsActive = data1.CandidateTerminationResignationInfoes.FirstOrDefault().IsActive;
                    var dd = data1.CandidateTerminationResignationInfoes.FirstOrDefault().CandidateTRInfoes.OrderByDescending(o => o.Id).FirstOrDefault();
                    if (dd != null)
                    {
                        model.SchedulingDate = dd.SchedulingDate.HasValue ? (dd.SchedulingDate.Value.Day.ToString().PadLeft(2, '0') + "/" + dd.SchedulingDate.Value.Month.ToString().PadLeft(2, '0') + "/" + dd.SchedulingDate.Value.Year) : "";
                        model.NoticePeriod = dd.ResignNoticePeriod.HasValue ? dd.ResignNoticePeriod.ToString() : null;
                        if (dd.IsClosed)
                        {
                            model.IsClosed = true;
                            model.ClosedComment = dd.ClosedComment;
                        }
                        model.StatusInfo = dd.StatusInfo;
                        model.IsActive = dd.IsActive;
                        model.TerminationOrResignationDate = dd.TerminationOrResignationDate.HasValue ? (dd.TerminationOrResignationDate.Value.Day.ToString().PadLeft(2, '0') + "/" + dd.TerminationOrResignationDate.Value.Month.ToString().PadLeft(2, '0') + "/" + dd.TerminationOrResignationDate.Value.Year) : "";
                    }
                }
            }
            return model;
        }

        public string AddTerminationResignationInfo(TerminationViewModel Model)
        {
            string Message = "";
            try
            {
                LogActivityViewModel log1 = new LogActivityViewModel();
                log1.EnteredBy = Model.EnteredBy;
                log1.EnteredDate = DateTime.Now;
                log1.ActioName = "AddTerminationResignationInfo";
                log1.ModuleName = "Admission";
                log1.ControllerName = "Addmission";
                log1.RegistrationNo = Model.RegNo;
                if (Model.CandidateTerminationResignationInfoId == 0)
                {
                    CandidateTerminationResignationInfo t = new CandidateTerminationResignationInfo
                    {
                        AddmissionMasterId = Model.AddmissionMasterId,
                        IsActive = false,
                        CandidateActionMasterId = Model.CandidateActionMasterId
                    };
                    _context.CandidateTerminationResignationInfoes.Add(t);

                    CandidateTRInfo log = new CandidateTRInfo
                    {
                        CandidateTerminationResignationInfoId = t.Id,
                        StatusInfo = Model.StatusInfo,
                        IsActive = Model.IsActive,
                        EnteredDate = DateTime.Now,
                        EnteredBy = Model.EnteredBy,
                        ActionName = Model.CandidateActionName
                    };

                    if (Model.IsClosed)
                    {
                        log.IsClosed = true;
                        log.ClosedComment = Model.ClosedComment;
                    }

                    if (!string.IsNullOrEmpty(Model.CandidateActionName))
                    {
                        if (Model.CandidateActionName == "Termination")
                        {
                            if (!string.IsNullOrEmpty(Model.SchedulingDate))
                                log.SchedulingDate = DateTime.ParseExact(Model.SchedulingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            log1.Activity = "Terminate";
                            log1.ActivityMessage = "Candidate registration no " + Model.RegNo + " is terminated.";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Model.TerminationOrResignationDate))
                            {
                                int Days = Convert.ToInt32(Model.NoticePeriod);
                                var scheduleDate = DateTime.ParseExact(Model.TerminationOrResignationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(Days);
                                log.SchedulingDate = scheduleDate;
                                log.ResignNoticePeriod = Days;
                                log1.Activity = "Resignation";
                                log1.ActivityMessage = "Candidate registration no " + Model.RegNo + " is resigned.";
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(Model.TerminationOrResignationDate))
                        log.TerminationOrResignationDate = DateTime.ParseExact(Model.TerminationOrResignationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    _context.CandidateTRInfoes.Add(log);
                }
                else
                {
                    var CandidateTerminationResignation = _context.CandidateTerminationResignationInfoes.Where(c => c.Id == Model.CandidateTerminationResignationInfoId).FirstOrDefault();
                    if (CandidateTerminationResignation != null)
                    {
                        CandidateTerminationResignation.IsActive = false;
                        CandidateTerminationResignation.CandidateActionMasterId = Model.CandidateActionMasterId;
                        var data = _context.CandidateTRInfoes.Where(s => s.CandidateTerminationResignationInfoId == Model.CandidateTerminationResignationInfoId).ToList();
                        if (data.Count() > 0)
                        {
                            foreach (var obj in data)
                                obj.IsActive = false;
                        }
                        CandidateTRInfo log = new CandidateTRInfo
                        {
                            CandidateTerminationResignationInfoId = CandidateTerminationResignation.Id,
                            StatusInfo = Model.StatusInfo,
                            IsActive = Model.IsActive,
                            EnteredDate = DateTime.Now,
                            EnteredBy = Model.EnteredBy,
                            ActionName = Model.CandidateActionName
                        };

                        if (Model.IsClosed)
                        {
                            log.IsClosed = true;
                            log.ClosedComment = Model.ClosedComment;
                            CandidateTerminationResignation.IsActive = true;
                        }
                        //if (!string.IsNullOrEmpty(Model.SchedulingDate))
                        //    log.SchedulingDate = DateTime.ParseExact(Model.SchedulingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (!string.IsNullOrEmpty(Model.CandidateActionName))
                        {
                            if (Model.CandidateActionName == "Termination")
                            {
                                if (!string.IsNullOrEmpty(Model.SchedulingDate))
                                    log.SchedulingDate = DateTime.ParseExact(Model.SchedulingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                log1.Activity = "Terminate";
                                log1.ActivityMessage = "Candidate registration no " + Model.RegNo + " is terminated.";
                            }
                            else
                            {
                                int Days = Convert.ToInt32(Model.NoticePeriod);
                                var scheduleDate = DateTime.ParseExact(Model.TerminationOrResignationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(Days);
                                log.SchedulingDate = scheduleDate;
                                log.ResignNoticePeriod = Days;
                                log1.Activity = "Resignation";
                                log1.ActivityMessage = "Candidate registration no " + Model.RegNo + " is resigned.";
                            }
                        }

                        if (!string.IsNullOrEmpty(Model.TerminationOrResignationDate))
                            log.TerminationOrResignationDate = DateTime.ParseExact(Model.TerminationOrResignationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        _context.CandidateTRInfoes.Add(log);
                    }
                }
                _context.SaveChanges();
                LogActivityService logActivityService = new LogActivityService();
                logActivityService.CreateLogActivity(log1);
                Message = "Status successfully saved";
                return Message;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return Message;
            }
        }

        public List<TerminationViewModel> GetSheduledTRinfo()
        {
            var CurrentDateTo = DateTime.Now.Date;
            return _context.CandidateTRInfoes.Where(c => c.IsActive && c.SchedulingDate != null && c.ActionName != "").Select(item => new TerminationViewModel
            {
                SchedulingDatestr = item.SchedulingDate,
                CandidateActionName = item.ActionName,
                IsActive = item.IsActive,
                Id = item.Id,
                AddmissionMasterId = item.CandidateTerminationResignationInfo.AddmissionMasterId,
            }).Where(w => w.SchedulingDatestr.HasValue && (w.SchedulingDatestr.Value < CurrentDateTo || w.SchedulingDatestr.Value == CurrentDateTo)).ToList();
        }

        public bool AddEndTRSheduledInfo(TerminationViewModel info)
        {
            var Status = false;
            var data = _context.CandidateTerminationResignationInfoes.Where(w => !w.IsActive && w.AddmissionMasterId == info.AddmissionMasterId).FirstOrDefault();
            if (data != null)
            {
                data.IsActive = true;
                var subData = _context.CandidateTRInfoes.Where(w => w.IsActive && w.Id == info.Id).FirstOrDefault();
                if (subData != null)
                    subData.IsActive = false;
                ScheduledStatusLogForTerminateResignation model = new ScheduledStatusLogForTerminateResignation
                {
                    CandidateTRInfoId = info.Id,
                    EnteredDate = DateTime.Now,
                };
                _context.ScheduledStatusLogForTerminateResignations.Add(model);
                _context.SaveChanges();
                Status = true;
            }
            return Status;
        }

        public List<TerminationViewModel> GetTerminationResignationInfo()
        {
            //var info = _context.CandidateTRInfoes.GroupBy(g=>g.CandidateTerminationResignationInfoId)Select(item => new
            //{

            //    TerminationOrResignationDate = item.TerminationOrResignationDate,
            //    StatusInfo = item.StatusInfo,
            //    DoneBy = item.UserLogin.Fname + " " + item.UserLogin.LName,
            //    SchedulingDate = item.SchedulingDate,
            //    CandidateActionName = item.ActionName,
            //    NoticePeriod = item.ResignNoticePeriod,
            //    RegNo = item.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationNo,
            //    Fname = item.CandidateTerminationResignationInfo.AddmissionMaster.Fname,
            //    Lname = item.CandidateTerminationResignationInfo.AddmissionMaster.Lname
            //}).AsEnumerable();

            var info = _context.CandidateTRInfoes.GroupBy(g => g.CandidateTerminationResignationInfoId).Select(item => new
            {

                KeyItem = item.Key,
                Info = item.OrderByDescending(o => o.Id).FirstOrDefault()
            }).AsEnumerable();

            var data = info.Select(a => new TerminationViewModel
            {
                TerminationOrResignationDate = a.Info.TerminationOrResignationDate.HasValue ? (a.Info.TerminationOrResignationDate.Value.Day.ToString().PadLeft(2, '0') + "/" + a.Info.TerminationOrResignationDate.Value.Month.ToString().PadLeft(2, '0') + "/" + a.Info.TerminationOrResignationDate.Value.Year) : "",
                StatusInfo = a.Info.StatusInfo,
                StudentName = a.Info.CandidateTerminationResignationInfo.AddmissionMaster.Fname + " " + a.Info.CandidateTerminationResignationInfo.AddmissionMaster.Lname,
                DoneBy = a.Info.UserLogin.Fname + " " + a.Info.UserLogin.LName,
                SchedulingDate = a.Info.SchedulingDate.HasValue ? (a.Info.SchedulingDate.Value.Day.ToString().PadLeft(2, '0') + "/" + a.Info.SchedulingDate.Value.Month.ToString().PadLeft(2, '0') + "/" + a.Info.SchedulingDate.Value.Year) : "",
                CandidateActionName = a.Info.ActionName,
                NoticePeriod = a.Info.ResignNoticePeriod.ToString(),
                RegNo = a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationNo,
                FeeDueAmount = (a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().FeeTypeDetail.Amount - a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationMaster.FeeDetails.Where(w => w.IsActive).FirstOrDefault().FeePaymentDetails.Sum(s => s.FeeCollections.Sum(b => b.PartWisePayments.Sum(p => p.Amount)))),
                RefundStatus = a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationMaster.FeeDetails.Any() ? (a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationMaster.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(o => o.Id).FirstOrDefault().Amount == a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationMaster.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(o => o.Id).FirstOrDefault().PartWisePayments.Sum(s => s.Amount) ? "success" : (a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationMaster.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(o => o.Id).FirstOrDefault().PartWisePayments.Sum(s => s.Amount) > 0 ? "orange" : "")) : "",
                IsFeePayment = a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationMaster.FeeDetails.Where(w => w.IsActive).Any(b => b.RegistrationNo == a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationMaster.RegistartionNo) ? (a.Info.CandidateTerminationResignationInfo.AddmissionMaster.RegistrationMaster.FeeDetails.Any(c => c.FeeTypeDetail.IsActive && c.FeeTypeDetail.FeeTypeId == 1) ? true : false) : false
            }).ToList();

            foreach (var i in data)
            {
                if (i.IsFeePayment)
                {
                    if (i.RefundStatus == "success")
                        i.FundStatus = "Fully paid";
                    else if (i.RefundStatus == "orange")
                        i.FundStatus = "Partially paid";
                    else
                        i.FundStatus = "Unpaid";
                }
                else
                {
                    i.FundStatus = "Unpaid";
                }
            }
            return data;
        }
    }
}
