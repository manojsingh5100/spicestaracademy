using SJModel;
using SJData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.Entity.SqlServer;

namespace SJService
{
    public class RegistrationService : InitialClogitory
    {
        SJStarERPEntities _context = null;
        public RegistrationService()
        {
            _context = new SJStarERPEntities();
        }

        public DataTableFilterModel GetProjectList(DataTableFilterModel filter, int SessionYr, string Tag = null)
        {
            var data = _context.RegistrationMasters.Where(r => r.IsActive).AsQueryable();
            if (string.IsNullOrEmpty(Tag))
            {
                if (SessionYr > 0)
                    data = data.Where(d => d.SessionMaster.SessionYr == SessionYr || (d.RegistrationDate.Value.Year == SessionYr));
            }
            else
            {
                if (Tag != null && Tag == "Screen")
                    data = data.Where(d => d.PaymentStatus || d.IsConsultantCandidate || d.IsHRCandidate);
                if (Tag != null && Tag == "Medical")
                    data = data.Where(d => d.IsScreenningClear != null && d.IsScreenningClear == true && !d.IsStandBy);
                if (Tag != null && Tag == "Refund")
                    data = data.Where(d => d.IsScreenningClear != null && d.IsScreenningClear == true && d.IsMedicalClear != null && d.IsMedicalClear == false);
                if (SessionYr > 0)
                {
                    if (filter.columns[10].search.value == "Withdrwan")
                        data = data.Where(t => t.AddmissionMasters.FirstOrDefault().AddmissionDate.Value.Year == SessionYr);
                    else
                        data = data.Where(d => d.SessionMaster.SessionYr == SessionYr);
                }
            }

            var info = data.Select(model => new RegistrationViewModel()
            {
                Id = model.Id,
                DOB = model.DOB,
                Email = model.Email.ToLower(),
                Gender = model.Gender == "M" ? "Male" : "Female",
                Mobile = model.Mobile,
                PaymentStatus = model.PaymentStatus,
                RegistartionNo = model.RegistartionNo,
                RegistrationDate = model.RegistrationDate,
                Fname = model.Fname,
                Lname = model.Lname,
                PaymentStatusStr = (model.IsConsultantCandidate ? "Consultent Candidate" : (model.IsHRCandidate ? "HR Candidate" : (model.PaymentStatus ? "Success" : "Pending"))),
                IsScreenningClear = model.IsScreenningClear,
                IsMedicalClear = model.IsMedicalClear,
                CourseName = model.CourseMaster.CourseName,
                MedicalRemark = model.MedicalRemark != null ? model.MedicalRemark : "",
                IsStandBy = model.IsStandBy,
                IsConsultantCandidate = model.IsConsultantCandidate,
                IsHRCandidate = model.IsHRCandidate,
                IsMedicalStandBy = model.IsMedicalStandBy,
                CourseId = model.CourseId,
                SessionId = model.SessionId,
                IsFeePayStandBy = model.IsFeePayStandBy,
                SourceOfCandidate = model.SourceOfCandidate != null ? model.SourceOfCandidate : "Empty",
                ModOfPayment = model.ModOfPayment,
                IsFeePayment = model.FeeDetails.Where(f => f.IsActive && f.FeeTypeDetail.FeeType.Name == "Admission").FirstOrDefault().FeePaymentDetails.Where(fpd => fpd.IsActive).FirstOrDefault().FeeCollections.Count > 0 ? true : false,
                MedicalStatus = model.AddmissionMasters.Count > 0 ? model.AddmissionMasters.FirstOrDefault().MedicalDetails.FirstOrDefault().MedicalStatus : "",
                AddMissionId = model.AddmissionMasters.Count > 0 ? model.AddmissionMasters.FirstOrDefault().Id : 0,
                BatchName = model.AddmissionMasters.Count > 0 ? model.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchMaster.Name : "",
                BatchId = model.AddmissionMasters.Count > 0 ? model.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchId.Value : 0,
                ShowMedicalConsultPopUp = model.IsMedicalClear.HasValue ? false : (model.IsScreenningClear.Value == true ? true : false)
            }).AsNoTracking().AsEnumerable();

            if (Tag != null && (Tag == "Medical"))
                info = info.Where(d => d.BatchId != 19);

            if (!string.IsNullOrWhiteSpace(filter.columns[7].search.value) && filter.columns[7].search.value != "")
            {
                int batch = Convert.ToInt32(filter.columns[7].search.value);
                info = info.Where(t => t.BatchId == batch);
                if (string.IsNullOrWhiteSpace(filter.search.value))
                    info = info.Where(t => !t.IsStandBy && t.IsScreenningClear == true);
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[9].search.value))
                info = info.Where(t => t.CourseName == filter.columns[9].search.value);

            if (!string.IsNullOrWhiteSpace(filter.columns[10].search.value))
            {
                if (filter.columns[10].search.value == "Selected")
                    info = info.Where(t => t.IsStandBy == false && t.IsScreenningClear == true);
                else if (filter.columns[10].search.value == "Rejected")
                    info = info.Where(t => t.IsScreenningClear == false && t.IsStandBy == false);
                else if (filter.columns[10].search.value == "Stand-By")
                    info = info.Where(t => t.IsStandBy == true);
                else if (filter.columns[10].search.value == "Withdrwan")
                    info = info.Where(t => t.MedicalStatus == "Withdrawn");
                else
                    info = info.Where(t => !t.IsScreenningClear == null);
            }
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                if (Tag == "Screen" || Tag == "Medical")
                    info = info.Where(d => (d.RegistartionNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.PaymentStatusStr) && d.PaymentStatusStr.ToString().ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Mobile) && d.Mobile.ToString().ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.BatchName) && d.BatchName.ToString().ToLower().Contains(filter.search.value.ToLower()))));
                else
                    info = info.Where(d => (d.RegistartionNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Mobile) && d.Mobile.ToString().ToLower().Contains(filter.search.value.ToLower()))));
            }
            var o = filter.order[0];
            var name = filter.columns[filter.order[0].column].data;
            if (o.dir == "asc")
                info = info.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
            else
                info = info.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
            if (!string.IsNullOrWhiteSpace(filter.columns[8].search.value))
                info = info.Where(t => t.CourseName == filter.columns[8].search.value);

            if (Tag != null && Tag == "Screen")
            {
                if (!string.IsNullOrWhiteSpace(filter.columns[5].search.value) && filter.columns[5].search.value != "" && !string.IsNullOrWhiteSpace(filter.columns[6].search.value) && filter.columns[6].search.value != "")
                {
                    DateTime fromDate = DateTime.ParseExact(filter.columns[5].search.value, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    DateTime toDate = DateTime.ParseExact(filter.columns[6].search.value, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    info = info.Where(t => t.RegistrationDate.HasValue && t.RegistrationDate.Value.Date >= fromDate && t.RegistrationDate.Value.Date <= toDate);
                }
            }

            filter.recordsFiltered = info.Count();
            var dataFilter = filter.length > 0 ? info.Skip(filter.start).Take(filter.length).ToList() : info.ToList();
            string[] RegList = dataFilter.Select(i => i.Email.ToLower()).ToArray();
            if (RegList.Length > 0)
            {
                var filterdata = GetReRegistrationHistory(RegList);
                foreach (var item in dataFilter)
                {
                    item.DateOfBirthNew = item.DateOfBirth;
                    if (filterdata.Where(w => w.SessionName.ToLower() == item.Email.ToLower()).Any())
                        item.IsResistrationHistory = true;
                }
            }
            filter.data = dataFilter;
            return filter;
        }

        public IEnumerable<SessionMasterViewModel> GetReRegistrationHistory(string[] info)
        {
            var data = _context.RegistrationMasters.Where(w => info.Contains(w.Email.ToLower())).GroupBy(g => g.Email).Select(s => new SessionMasterViewModel
            {
                SessionName = s.Key,
                Id = s.Count()
            }).Where(w => w.Id > 1).AsEnumerable();
            return data;
        }

        public RegistrationViewModel IsStudentScreenningMedicalClearance(int Id, bool status, string Tag)
        {
            bool Reject = false;
            var info = _context.RegistrationMasters.Where(r => r.IsActive && r.Id == Id).FirstOrDefault();
            if (info != null)
            {
                if (Tag == "Screen")
                {
                    if (info.IsScreenningClear == null)
                        info.IsScreenningClear = status;
                }
                else if (Tag == "Medical")
                {
                    if (info.IsMedicalClear == null)
                        info.IsMedicalClear = status;
                    else if (info.IsMedicalClear.HasValue && !info.IsMedicalClear.Value)
                    {
                        info.IsMedicalClear = status;
                        info.MedicalRemark = null;
                        info.IsMedicalStandBy = false;
                        var admissionData = _context.AddmissionMasters.Where(a => a.RegistrationNo == info.RegistartionNo).FirstOrDefault();
                        if (admissionData != null)
                            admissionData.IsActive = true;
                    }
                    var medicalinfo = _context.MedicalDetails.Where(m => m.AddmissionMaster.RegistrationNo == info.RegistartionNo).FirstOrDefault();
                    if (medicalinfo != null)
                        medicalinfo.MedicalStatus = "FIT";
                }
                _context.SaveChanges();
                Reject = (Tag == "Screen" ? info.IsScreenningClear.Value : info.IsMedicalClear.Value);
            }
            return new RegistrationViewModel { Fname = info.Fname, Lname = info.Lname, RegistartionNo = info.RegistartionNo, Email = info.Email, IsRejected = Reject, ApplicationNo = info.ApplicationNo };
        }

        public List<CourseMasterViewModel> GetSourceList()
        {
            return _context.ptaLeadSourceMasters.Where(p => p.IsActive).Select(item => new CourseMasterViewModel
            {
                Id = item.Id,
                CourseName = item.Name
            }).ToList();
        }

        public RegistrationViewModel GetRegisterInfoById(int id)
        {
            RegistrationViewModel model = new RegistrationViewModel();
            model = _context.RegistrationMasters.Where(r => r.IsActive && r.Id == id).Select(m => new RegistrationViewModel
            {
                CorrespondenceAddress = m.CorrespondenceAddress,
                CorrespondenceCity = m.CorrespondenceCity,
                CorrespondenceState = m.CorrespondenceState,
                CorrespondenceZip = m.CorrespondenceZip,
                DOB = m.DOB,
                Email = m.Email,
                Fname = m.Fname,
                Lname = m.Lname,
                Mobile = m.Mobile,
                RegistartionNo = m.RegistartionNo,
                Gender = m.Gender,
                Height = m.Height,
                Width = m.Width,
                Id = m.Id,
                IsAppeared = m.IsAppeared,
                IsPassport = m.IsPassport,
                PermanentAddress = m.PermanentAddress,
                PermanentCity = m.PermanentCity,
                PermanentState = m.PermanentState,
                PermanentZip = m.PermanentZip,
                PaymentStatus = m.PaymentStatus,
                CourseId = m.CourseId,
                Education = m.Education == null ? "10+2" : m.Education.Trim(),
                ModOfPayment = m.ModOfPayment,
                IsConsultantCandidate = m.IsConsultantCandidate,
                IsHRCandidate = m.IsHRCandidate,
                SessionId = m.SessionId,
                SessionName = m.SessionMaster.SessionName,
                SessionYr = m.SessionMaster.SessionYr.HasValue ? m.SessionMaster.SessionYr : 0,
                CourseName = m.CourseMaster.CourseName,
                RegistrationDate = m.RegistrationDate,
                IsAddmission = m.IsAddmission,
                IsAnyCourseFeePay = m.FeeDetails.Count() > 0 ? true : false,
                SourceOfCandidate = m.SourceOfCandidate
            }).FirstOrDefault();
            //if (model != null && model.RegistrationDate.HasValue)
            //{
            //    model.RegisterDate = model.RegistrationDate.Value.Day.ToString().PadLeft(2, '0') + "/" + model.RegistrationDate.Value.Month.ToString().PadLeft(2, '0') + "/" + model.RegistrationDate.Value.Year;
            //    if (!string.IsNullOrEmpty(model.Mobile) && model.Mobile.Contains('-'))
            //        model.Mobile = model.Mobile.Split('-')[1];
            //}
            //if (model != null && model.DOB.HasValue)
            //    model.DateOfBirth = model.DOB.Value.Day.ToString().PadLeft(2, '0') + "/" + model.DOB.Value.Month.ToString().PadLeft(2, '0') + "/" + model.DOB.Value.Year;
            return model;
        }

        public RegistrationViewModel GetReRegisterDetailById(int id)
        {
            RegistrationViewModel model = new RegistrationViewModel();
            model = _context.RegistrationMasters.Where(r => r.IsActive && !r.IsReRegister && r.RegistartionNo == id).Select(m => new RegistrationViewModel
            {
                CorrespondenceAddress = m.CorrespondenceAddress,
                CorrespondenceCity = m.CorrespondenceCity,
                CorrespondenceState = m.CorrespondenceState,
                CorrespondenceZip = m.CorrespondenceZip,
                Email = m.Email,
                Fname = m.Fname,
                Lname = m.Lname,
                Mobile = m.Mobile,
                Gender = m.Gender,
                Height = m.Height,
                Width = m.Width,
                DOB = m.DOB,
                IsAppeared = m.IsAppeared,
                IsPassport = m.IsPassport,
                PermanentAddress = m.PermanentAddress,
                PermanentCity = m.PermanentCity,
                PermanentState = m.PermanentState,
                PermanentZip = m.PermanentZip,
                CourseId = m.CourseId,
                Education = m.Education == null ? "10+2" : m.Education.Trim(),
                SessionId = m.SessionId,
                SessionName = m.SessionMaster.SessionName,
                CourseName = m.CourseMaster.CourseName,
            }).FirstOrDefault();
            //if (model != null && model.DOB.HasValue)
            //    model.DateOfBirth = model.DOB.Value.Day.ToString().PadLeft(2, '0') + "/" + model.DOB.Value.Month.ToString().PadLeft(2, '0') + "/" + model.DOB.Value.Year;
            return model;
        }

        public RegistrationViewModel GetRegisterInfoByRegNo(int RegNo)
        {
            RegistrationViewModel model = new RegistrationViewModel();
            model = _context.RegistrationMasters.Join(
                _context.AddmissionMasters,
                rg => rg.RegistartionNo,
                ad => ad.RegistrationNo,
                (rg, ad) => new RegistrationViewModel
                {
                    RegistartionNo = rg.RegistartionNo,
                    Id = rg.Id,
                    MedicalRemark = rg.MedicalRemark,
                    IsMedicalStandBy = rg.IsMedicalStandBy,
                    IsMedicalClear = rg.IsMedicalClear,
                    Fname = rg.Fname,
                    Lname = rg.Lname,
                    ModOfPayment = ad.MedicalDetails.FirstOrDefault().MedicalStatus,
                    DateOfWithdrawal = ad.MedicalDetails.FirstOrDefault().DateOfWithdrawn,
                    UserFname = ad.MedicalDetails.FirstOrDefault().UserLogin.Fname,
                    UserLname = ad.MedicalDetails.FirstOrDefault().UserLogin.LName
                }).Where(w => w.RegistartionNo == RegNo).FirstOrDefault();
            return model;
        }

        public int MaxRegistartionNumber()
        {
            return _context.RegistrationMasters.Any() ? _context.RegistrationMasters.Max(r => r.RegistartionNo) + 1 : 1;
        }

        public RegistrationViewModel AddUpdate(RegistrationViewModel model)
        {
            RegistrationMaster obj = new RegistrationMaster();
            try
            {
                var data = _context.RegistrationMasters.Find(model.Id);
                if (data != null)
                {
                    obj = data;
                    var admData = _context.AddmissionMasters.Include("AddmissionDetails").Include("AddressDetails").Include("MedicalDetails").Where(a => a.RegistrationNo == data.RegistartionNo).FirstOrDefault();
                    if (admData != null)
                    {
                        admData.Fname = model.Fname;
                        admData.Lname = model.Lname;
                        admData.Email = model.Email;
                        admData.MobileNo = model.Mobile;
                        if (model.DateOfBirthNew != null && model.DateOfBirthNew != "")
                            admData.DOB = DateTime.ParseExact(model.DateOfBirthNew, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        admData.IsValidPassport = model.IsPassport;
                        admData.IsAppeared = model.IsAppeared;
                        admData.Education = model.Education;
                        admData.CourseId = model.CourseId.Value;
                        admData.AddmissionDetails.FirstOrDefault().SessionId = model.SessionId.Value;
                        admData.AddressDetails.FirstOrDefault().CopAddress = model.CorrespondenceAddress;
                        admData.AddressDetails.FirstOrDefault().CopCity = model.CorrespondenceCity;
                        admData.AddressDetails.FirstOrDefault().CopState = model.CorrespondenceState;
                        admData.AddressDetails.FirstOrDefault().CopZip = model.CorrespondenceZip;
                        admData.AddressDetails.FirstOrDefault().PerAddress = model.PermanentAddress;
                        admData.AddressDetails.FirstOrDefault().PerCity = model.PermanentCity;
                        admData.AddressDetails.FirstOrDefault().PerState = model.PermanentState;
                        admData.AddressDetails.FirstOrDefault().PerZip = model.PermanentZip;
                        _context.Entry(admData.AddressDetails.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;

                        admData.MedicalDetails.FirstOrDefault().Height = model.Height;
                        admData.MedicalDetails.FirstOrDefault().Weight = model.Width;
                        _context.Entry(admData.MedicalDetails.FirstOrDefault()).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                obj.Fname = model.Fname;
                obj.Lname = model.Lname;
                if (model.Mobile.Contains("+91-"))
                    obj.Mobile = model.Mobile;
                else
                    obj.Mobile = "+91-" + model.Mobile;
                obj.CorrespondenceAddress = model.CorrespondenceAddress;
                obj.CorrespondenceCity = model.CorrespondenceCity;
                obj.CorrespondenceState = model.CorrespondenceState;
                obj.CorrespondenceZip = model.CorrespondenceZip;
                obj.PermanentAddress = model.PermanentAddress;
                obj.PermanentCity = model.PermanentCity;
                obj.PermanentState = model.PermanentState;
                obj.PermanentZip = model.PermanentZip;
                if (model.DateOfBirthNew != null && model.DateOfBirthNew != "")
                    obj.DOB = DateTime.ParseExact(model.DateOfBirthNew, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj.Email = model.Email;
                obj.Gender = model.Gender;
                obj.Height = model.Height;
                obj.Width = model.Width;
                obj.RegistartionNo = model.RegistartionNo;
                obj.IsPassport = model.IsPassport;
                obj.IsAppeared = model.IsAppeared;
                obj.CourseId = model.CourseId;
                obj.Education = model.Education;
                obj.SessionId = model.SessionId;
                obj.IsConsultantCandidate = model.IsConsultantCandidate;
                obj.IsHRCandidate = model.IsHRCandidate;
                obj.SessionId = model.SessionId;
                obj.CourseId = model.CourseId;
                obj.IsActive = true;
                obj.IsAddmission = false;
                if (!string.IsNullOrEmpty(model.SourceOfCandidate))
                    obj.SourceOfCandidate = model.SourceOfCandidate;
                else
                    obj.SourceOfCandidate = null;
                //if (model.RegisterDate != null && model.RegisterDate != "")
                //    obj.RegistrationDate = DateTime.ParseExact(model.RegisterDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //else
                //    obj.RegistrationDate = DateTime.Now;

                if (obj.Id == 0)
                {
                    //  obj.RegistrationDate = DateTime.Now;
                    obj.RegistartionNo = MaxRegistartionNumber();
                    _context.RegistrationMasters.Add(obj);
                }

                if (model.PaymentStatus)
                {
                    obj.PaymentDate = obj.RegistrationDate;
                    obj.PaymentId = "RC#" + model.RegistartionNo;
                    obj.PaymentStatus = true;
                    obj.ModOfPayment = "Cash";
                    obj.IsConsultantCandidate = false;
                    obj.IsHRCandidate = false;
                }

                _context.SaveChanges();
                model.Id = obj.Id;
                model.RegistartionNo = obj.RegistartionNo;
                return model;
            }
            catch (Exception ex)
            {
                model.MedicalRemark = ex.Message + " " + ex.InnerException;
                return model;
            }
        }

        public MedicalStatusResponceViewModel UpdateWithStopWithdrwn(int RegNo, string MdlStatus, int Id, int UserId)
        {
            MedicalStatusResponceViewModel result = new MedicalStatusResponceViewModel();
            bool status = false;
            var data = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == RegNo && r.Id == Id).FirstOrDefault();
            if (data != null)
            {
                result.Email = data.Email;
                result.ApplicationNo = data.ApplicationNo;
                data.MedicalRemark = null;
                if (!string.IsNullOrEmpty(MdlStatus) && MdlStatus == "NotWithdrawn")
                {
                    var admissionData = _context.AddmissionMasters.Where(a => a.RegistrationNo == RegNo).FirstOrDefault();
                    if (admissionData != null)
                        admissionData.IsActive = true;
                    if (MdlStatus == "Withdrawn")
                        result.Message = "Drop withdrawal registration no " + data.RegistartionNo + ".";
                }
                status = true;
            }
            var medicalData = _context.MedicalDetails.Where(a => a.AddmissionMaster.RegistrationNo == RegNo).FirstOrDefault();
            if (medicalData != null)
            {
                medicalData.MedicalStatus = null;
                medicalData.DateOfWithdrawn = null;
                medicalData.EnteredBy = UserId;
            }
            _context.SaveChanges();
            result.MStatus = status;
            return result;
        }
        public MedicalStatusResponceViewModel UpdateMedicalRemark(int RegNo, string remark, string MdlStatus, int Id, int UserId, string tag = null)
        {
            MedicalStatusResponceViewModel result = new MedicalStatusResponceViewModel();
            bool status = false;
            var data = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == RegNo && r.Id == Id).FirstOrDefault();
            if (data != null)
            {
                result.Email = data.Email;
                result.ApplicationNo = data.ApplicationNo;
                data.MedicalRemark = remark;
                data.IsMedicalClear = false;
                data.IsMedicalStandBy = MdlStatus == "SBY" ? true : false;
                if (!string.IsNullOrEmpty(MdlStatus) && MdlStatus != "TMU")
                {
                    var admissionData = _context.AddmissionMasters.Where(a => a.RegistrationNo == RegNo).FirstOrDefault();
                    if (admissionData != null)
                        admissionData.IsActive = false;
                    if (MdlStatus == "Withdrawn")
                    {
                        admissionData.AddmissionDetails.FirstOrDefault().BatchId = 19;
                        result.Message = "Add registration no " + data.RegistartionNo + " as withdrawal candididate.";
                    }
                }
                status = true;
            }
            var medicalData = _context.MedicalDetails.Where(a => a.AddmissionMaster.RegistrationNo == RegNo).FirstOrDefault();
            if (medicalData != null)
            {
                medicalData.MedicalStatus = MdlStatus;
                medicalData.DateOfWithdrawn = DateTime.Now;
                medicalData.EnteredBy = UserId;
            }
            _context.SaveChanges();
            result.MStatus = status;
            return result;
        }

        //==============

        public string CalculateYourAge(DateTime? Dob)
        {
            if (!Dob.HasValue)
                return "";
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob.Value).Ticks).Year - 1;
            DateTime PastYearDate = Dob.Value.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            return String.Format("{0} Year(s) {1} Month(s)",
            Years, Months);
        }

        public ScreeningInfoViewModel MapingScreening(dynamic obj)
        {
            List<ScreeningParameterOptionViewModel> parameterList = new List<ScreeningParameterOptionViewModel>();
            List<ScreeningInterviewerResultViewModel> interviewerList = new List<ScreeningInterviewerResultViewModel>();
            int SessionId = Convert.ToInt32(obj.registerInfo.SessionId);
            int CourseId = Convert.ToInt32(obj.registerInfo.CourseId);
            int RegNo = Convert.ToInt32(obj.registerInfo.RegistartionNo);
            int SessionYr = Convert.ToInt32(obj.registerInfo.SessionMaster.SessionYr);
            int aregno = obj.registerInfo.RegistartionNo;
            ScreeningInfoViewModel model = new ScreeningInfoViewModel
            {
                RegistrationId = obj.registerInfo.RegistartionNo,
                RegId = obj.registerInfo.Id,
                FName = obj.registerInfo.Fname,
                LName = obj.registerInfo.Lname,
                Height = obj.registerInfo.Height,
                Weight = obj.registerInfo.Width,
                DOB = obj.registerInfo.DOB,
                Age = CalculateYourAge(obj.registerInfo.DOB),
                IsStandBy = obj.registerInfo.IsStandBy,
                IsScreeningClr = obj.registerInfo.IsScreenningClear,
                CourseId = CourseId,
                SessionYr = SessionYr,
                LeadSource = obj.registerInfo.SourceOfCandidate,
                IsWithdrawal = _context.AddmissionMasters.Where(a => a.RegistrationNo == aregno).Any() ? (_context.AddmissionMasters.Where(a => a.RegistrationNo == aregno).FirstOrDefault().MedicalDetails.FirstOrDefault().MedicalStatus == "Withdrawn" ? true : false) : false,
                SessionName = _context.SessionMasters.Where(s => s.IsActive && s.Id == SessionId).FirstOrDefault().SessionName,
                CourseName = _context.CourseMasters.Where(c => c.IsActive && c.Id == CourseId).FirstOrDefault().CourseName,
                BatchId = _context.AddmissionMasters.Any(a => a.RegistrationNo == RegNo) ? _context.AddmissionMasters.Where(b => b.RegistrationNo == RegNo).FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchId : 0,
                IsAnyCourseFeePay = _context.FeeDetails.Where(f => f.RegistrationNo == RegNo).Any()
            };

            model.ScreeningTest = new ScreeningTestViewModel();
            if (obj.ScreeningTestInfo != null)
            {
                model.ScreeningTest.Date = obj.ScreeningTestInfo.ScreeningDate.Day.ToString().PadLeft(2, '0') + "/" + obj.ScreeningTestInfo.ScreeningDate.Month.ToString().PadLeft(2, '0') + "/" + obj.ScreeningTestInfo.ScreeningDate.Year;
                model.ScreeningTest.EyeSightLeft = obj.ScreeningTestInfo.EyeSightLeft;
                model.ScreeningTest.EyeSightRight = obj.ScreeningTestInfo.EyeSightRight;
                model.ScreeningTest.DocumentMasterId = obj.ScreeningTestInfo.DocumentMasterId;
                model.ScreeningTest.FlyingExp = obj.ScreeningTestInfo.FlyingExp;
                model.ScreeningTest.Id = obj.ScreeningTestInfo.Id;
                model.ScreeningTest.Interviewer = obj.ScreeningTestInfo.Interviewer;

                model.ScreeningTest.PassportExpiryDate = obj.ScreeningTestInfo.PassportExpiryDate;
                model.ScreeningTest.PassportExpiryDateStr = obj.ScreeningTestInfo.PassportExpiryDate != null ? (obj.ScreeningTestInfo.PassportExpiryDate.Day.ToString().PadLeft(2, '0') + "/" + obj.ScreeningTestInfo.PassportExpiryDate.Month.ToString().PadLeft(2, '0') + "/" + obj.ScreeningTestInfo.PassportExpiryDate.Year) : null;
                model.ScreeningTest.PassportIssueDate = obj.ScreeningTestInfo.PassportIssueDate;
                model.ScreeningTest.PassportIssueDateStr = obj.ScreeningTestInfo.PassportIssueDate != null ? (obj.ScreeningTestInfo.PassportIssueDate.Day.ToString().PadLeft(2, '0') + "/" + obj.ScreeningTestInfo.PassportIssueDate.Month.ToString().PadLeft(2, '0') + "/" + obj.ScreeningTestInfo.PassportIssueDate.Year) : null;
                model.ScreeningTest.RegistrationId = obj.ScreeningTestInfo.RegistrationId;
                model.ScreeningTest.ReviewDate = obj.ScreeningTestInfo.ReviewDate;
                model.ScreeningTest.ReviewDateStr = obj.ScreeningTestInfo.ReviewDate != null ? (obj.ScreeningTestInfo.ReviewDate.Day.ToString().PadLeft(2, '0') + "/" + obj.ScreeningTestInfo.ReviewDate.Month.ToString().PadLeft(2, '0') + "/" + obj.ScreeningTestInfo.ReviewDate.Year) : null;
                model.ScreeningTest.TatooOrScarId = obj.ScreeningTestInfo.TatooOrScarId;
                model.ScreeningTest.Remark = obj.ScreeningTestInfo.Remark;

                if (obj.ScreeningTestInfo.ScreeningParameterOptions.Count > 0)
                {
                    foreach (var item in obj.ScreeningTestInfo.ScreeningParameterOptions)
                    {
                        ScreeningParameterOptionViewModel p = new ScreeningParameterOptionViewModel
                        {
                            Fair = item.Fair,
                            Good = item.Good,
                            Id = item.Id,
                            Poor = item.Poor,
                            Remark = item.Remark,
                            ScreeningParameterId = item.ScreeningParameterId,
                            ScreeningTestId = item.ScreeningTestId,
                            ParameterName = item.ScreeningParameter.ParameterName,
                            Interviewer = item.InterViewerName
                        };
                        parameterList.Add(p);
                    }
                }
                else
                {
                    parameterList = _context.ScreeningParameters.Where(p => p.IsActive).Select(s => new ScreeningParameterOptionViewModel
                    {
                        ScreeningParameterId = s.Id,
                        ParameterName = s.ParameterName,
                    }).ToList();
                }
                model.ParameterOption = parameterList;
            }
            else
            {
                var parameterData = _context.ScreeningParameters.Where(p => p.IsActive).Select(s => new ScreeningParameterOptionViewModel
                {
                    ScreeningParameterId = s.Id,
                    ParameterName = s.ParameterName,
                }).ToList();
                model.ParameterOption = parameterData;
            }
            return model;
        }

        public RoleViewModel GetSessionNameByCourseIdAndCurrDate(int CourseId, string CourseName, int Year, int RegistartionNo)
        {
            RoleViewModel model = new RoleViewModel();
            if (CourseId > 0)
            {
                var Sessiondata = _context.SessionMasters.Where(s => s.IsActive && s.CourseMaster.IsActive && s.CourseMasterId == CourseId && s.SessionYr == Year).FirstOrDefault();
                if (Sessiondata != null)
                {
                    model.Name = " ( " + Sessiondata.SessionName + " )";
                    model.Id = Sessiondata.Id;
                }
                //model.SessionListByCId = _context.SessionMasters.Where(s => s.IsActive && s.CourseMasterId == CourseId && s.SessionYr == Year).Select(item => new SemesterMasterViewModel
                //{
                //    SemesterName = item.SessionName,
                //    Id = item.Id
                //}).ToList();
                model.SessionListByCId = _context.SessionMasters.Where(s => s.IsActive && s.CourseMasterId == CourseId).Select(item => new SemesterMasterViewModel
                {
                    SemesterName = item.SessionName,
                    Id = item.Id
                }).ToList();
            }
            return model;
        }


        public string FillDashboardChart(int Month, int Year)
        {
            var T_mapList = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistrationDate.Value != null);
            T_mapList = T_mapList.Where(r => r.RegistrationDate.Value.Year == Year && r.RegistrationDate.Value.Month == Month);
            var MapList = T_mapList.OrderBy(o => o.RegistrationDate).GroupBy(g => g.RegistrationDate.Value.Day).Select(s => new AxisPoint
            {
                Y = s.Count(),
                X = s.Key
            }).ToList();
            return Axis.JConversion(Axis.BarPoints(MapList));
        }

        public LeadSourceHeadModel FillDashboardLeadChart(int Year)
        {
            LeadSourceHeadModel Model = new LeadSourceHeadModel();
            var T_mapList = _context.RegistrationMasters.Where(r => r.RegistrationDate.Value != null);
            T_mapList = T_mapList.Where(r => r.RegistrationDate.Value.Year == Year && r.SourceOfCandidate != null && r.SourceOfCandidate != "");
            var MapList = T_mapList.OrderBy(o => o.RegistrationDate).GroupBy(g => g.SourceOfCandidate).Select(s => new LeadSourceChartViewModel
            {
                Yaxix = s.Count(),
                Xaxis = s.Key
            }).ToList();
            Model.GetLeadSourceValues = MapList;
            return Model;
        }

        public List<RoleViewModel> GetLeadSourceList()
        {
            return _context.ptaLeadSourceMasters.Where(w => w.IsActive).Select(item => new RoleViewModel
            {
                Name = item.Name,
                Id = item.Id
            }).ToList();
        }

        public DesktopChartList GetDesktopData(int SessionYr)
        {
            int AdmissionCount = 0;
            DateTime today = DateTime.Today;
            var T_registration = _context.RegistrationMasters.AsQueryable();
            //====================================================================================================
            var T_screenning = (from st in _context.ScreeningTests
                                join rm in _context.RegistrationMasters on st.RegistrationId equals rm.RegistartionNo
                                where rm.IsActive == true && rm.IsScreenningClear != null && rm.IsStandBy == false
                                && (rm.PaymentStatus || rm.IsConsultantCandidate || rm.IsHRCandidate)
                                select new
                                {
                                    RegNo = st.RegistrationId,
                                    Date = st.ScreeningDate,
                                    IsSelect = rm.IsScreenningClear.Value
                                });
            var T_medical = _context.AddmissionMasters.Where(r => r.RegistrationMaster.IsActive && r.RegistrationMaster.IsScreenningClear.HasValue && r.RegistrationMaster.IsScreenningClear.Value && r.RegistrationMaster.IsMedicalClear.HasValue);

            var T_screenning_selected = T_screenning.Where(w => w.IsSelect);
            var T_screenning_rejected = T_screenning.Where(w => !w.IsSelect);
            var T_screening_pending = _context.RegistrationMasters.Where(r => r.IsActive && (r.PaymentStatus || r.IsConsultantCandidate || r.IsHRCandidate) && !r.IsScreenningClear.HasValue);
            var StandBy = _context.RegistrationMasters.Where(r => (r.PaymentStatus || r.IsConsultantCandidate || r.IsHRCandidate) && r.IsStandBy);
            var T_medical_selected = T_medical.Where(m => m.IsActive && m.RegistrationMaster.IsMedicalClear.Value);
            var T_medical_rejected = T_medical.Where(m => !m.IsActive && !m.RegistrationMaster.IsMedicalClear.Value);
            var T_medical_pending = _context.AddmissionMasters.Where(r => r.IsActive && r.RegistrationMaster.IsActive && r.RegistrationMaster.IsScreenningClear.HasValue && r.RegistrationMaster.IsScreenningClear.HasValue && !r.RegistrationMaster.IsMedicalClear.HasValue);
            var WithdrwanNo = _context.AddmissionMasters.Where(a => (a.RegistrationMaster.PaymentStatus || a.RegistrationMaster.IsConsultantCandidate || a.RegistrationMaster.IsHRCandidate) && a.MedicalDetails.FirstOrDefault().MedicalStatus == "Withdrawn").AsQueryable();
            //================
            if (SessionYr > 0)
            {
                T_registration = T_registration.Where(r => r.RegistrationDate.Value.Year == SessionYr);

                AdmissionCount = _context.AddmissionMasters.Where(a => a.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.IsActive && a.AddmissionDetails.FirstOrDefault().BatchId != 19 && a.RegistrationMaster.IsActive && (!a.RegistrationMaster.IsMedicalClear.HasValue || a.RegistrationMaster.IsMedicalClear.Value) && a.AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart.Value.Year == SessionYr).Count();
                //=====================
                T_screenning_selected = T_screenning_selected.Where(s => s.Date.Year == SessionYr);
                T_screenning_rejected = T_screenning_rejected.Where(s => s.Date.Year == SessionYr);
                T_screening_pending = T_screening_pending.Where(s => s.RegistrationDate.Value.Year == SessionYr);
                T_medical_selected = T_medical_selected.Where(s => s.AddmissionDate.Value.Year == SessionYr);
                T_medical_rejected = T_medical_rejected.Where(s => s.AddmissionDate.Value.Year == SessionYr);
                T_medical_pending = T_medical_pending.Where(s => s.AddmissionDate.Value.Year == SessionYr);
                T_medical = T_medical.Where(a => a.AddmissionDetails.FirstOrDefault().SessionMaster.SessionYr == SessionYr || a.AddmissionDate.Value.Year == SessionYr);
                StandBy = StandBy.Where(s => s.RegistrationDate.HasValue && s.RegistrationDate.Value.Year == SessionYr);
                WithdrwanNo = WithdrwanNo.Where(d => d.AddmissionDate.Value.Year == SessionYr);
            }
            else
            {
                AdmissionCount = _context.AddmissionMasters.Where(a => a.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.IsActive && a.AddmissionDetails.FirstOrDefault().BatchId != 19 && a.RegistrationMaster.IsActive && (!a.RegistrationMaster.IsMedicalClear.HasValue || a.RegistrationMaster.IsMedicalClear.Value)).Count();
            }

            DesktopChartList list = new DesktopChartList
            {
                TotalRegistartion = T_registration.Count(),
                TotalAdmission = AdmissionCount,
                TotalMedical = T_medical_pending.Count(),
                TotalScreenning = T_screening_pending.Count(),
                TotalScreenningSelected = T_screenning_selected.Count(),
                TotalScreenningRejected = T_screenning_rejected.Count(),
                TotalMedicalSelected = T_medical_selected.Count(),
                TotalMedicalRejected = T_medical_rejected.Count(),
                Withdrawn = WithdrwanNo.Count(),
                StandBy = StandBy.Count()
            };
            list.Map = FillDashboardChart(today.Month, today.Year);
            var InitialDate = Convert.ToDateTime("01/06/2018");
            var MonthWiseData = _context.RegistrationMasters.Where(w => w.IsActive && w.RegistrationDate.Value >= InitialDate);
            if (SessionYr > 0)
                MonthWiseData = MonthWiseData.Where(w => w.RegistrationDate.Value.Year == SessionYr);
            var ExactMonthWiseData = MonthWiseData.GroupBy(o => new
            {
                Month = o.RegistrationDate.Value.Month,
                Year = o.RegistrationDate.Value.Year
            }).Select(g => new
            {
                Month = g.Key.Month,
                Year = g.Key.Year,
                Total = g.Count()
            }).OrderByDescending(o => o.Year)
                .ThenByDescending(o => o.Month)
                .ToList();
            var TotalRegListByMonth = new List<RegisterByMonth>();
            foreach (var item in ExactMonthWiseData)
            {
                string Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Month);
                TotalRegListByMonth.Add(new RegisterByMonth
                {
                    MonthNum = item.Month,
                    NoOfRegister = item.Total,
                    Year = item.Year,
                    Month = Month + ", " + item.Year
                });
            };
            list.TotalRegByMonth = TotalRegListByMonth;
            SJService.LogActivityService log = new LogActivityService();
            list.GetActivityList = log.GetTopFiveActities();
            return list;
        }

        public DataTableFilterModel GetWithdrawalCandidateList(DataTableFilterModel filter, int SessionYr)
        {
            var data = _context.AddmissionMasters.Where(a => a.MedicalDetails.FirstOrDefault().MedicalStatus == "Withdrawn").AsQueryable();
            if (SessionYr > 0)
                data = data.Where(d => d.AddmissionDate.Value.Year == SessionYr);
            //var data = _context.RegistrationMasters.Where(d => d.IsActive && d.IsScreenningClear != null && d.IsScreenningClear == true && !d.IsStandBy).AsQueryable();
            //if (SessionYr > 0)
            //    data = data.Where(d => d.SessionMaster.SessionYr == SessionYr || (d.RegistrationDate.Value.Year == SessionYr));
            //if (string.IsNullOrWhiteSpace(filter.columns[7].search.value))
            //{
            //    data = data.Where(r => r.IsMedicalClear.HasValue && r.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchId == 19);
            //}
            //else
            //{
            //    data = data.Where(r => r.IsMedicalClear.HasValue && r.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchId != 19 && r.MedicalRemark != "Withdrawal");
            //}
            //var info = data.Select(model => new RegistrationViewModel()
            //{
            //    Id = model.Id,
            //    DOB = model.DOB,
            //    Email = model.Email,
            //    Gender = model.Gender == "M" ? "Male" : "Female",
            //    Mobile = model.Mobile,
            //    CourseName = model.CourseMaster.CourseName,
            //    Fname = model.Fname,
            //    Lname = model.Lname,
            //    RegistartionNo = model.RegistartionNo,
            //    RegistrationDate = model.RegistrationDate,
            //    MedicalRemark = model.MedicalRemark != null ? model.MedicalRemark : "",
            //}).AsEnumerable();

            var info = data.Select(model => new RegistrationViewModel()
            {
                Id = model.RegistrationMasterId.Value,
                DOB = model.DOB,
                Email = model.Email,
                Gender = model.Gender == "M" ? "Male" : "Female",
                Mobile = model.MobileNo,
                CourseName = model.CourseMaster.CourseName,
                Fname = model.Fname,
                Lname = model.Lname,
                RegistartionNo = model.RegistrationNo,
                RegistrationDate = model.MedicalDetails.FirstOrDefault().DateOfWithdrawn,
                MedicalRemark = model.RegistrationMaster.MedicalRemark != null ? model.RegistrationMaster.MedicalRemark : "",
                RefundStatus = model.RegistrationMaster.FeeDetails.Any() ? (model.RegistrationMaster.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(a => a.Id).FirstOrDefault().Amount == model.RegistrationMaster.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(a => a.Id).FirstOrDefault().PartWisePayments.Sum(s => s.Amount) ? "success" : (model.RegistrationMaster.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(a => a.Id).FirstOrDefault().PartWisePayments.Sum(s => s.Amount) > 0 ? "orange" : "")) : "",
                IsFeePayment = model.RegistrationMaster.FeeDetails.Where(w => w.IsActive).Any(a => a.RegistrationNo == model.RegistrationNo) ? (model.RegistrationMaster.FeeDetails.Any(a => a.FeeTypeDetail.IsActive && a.FeeTypeDetail.FeeTypeId == 1) ? true : false) : false
            }).AsEnumerable();

            var totalCount = info.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                info = info.Where(d => d.RegistartionNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Mobile) && d.Mobile.ToString().ToLower().Contains(filter.search.value.ToLower())));
            }
            if (!string.IsNullOrWhiteSpace(filter.columns[6].search.value))
                info = info.Where(t => t.CourseName == filter.columns[6].search.value);
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
            int draw = filter.start + 1;
            foreach (var i in dataFilter)
            {
                i.SrNo = draw;
                draw++;
            }
            filter.data = dataFilter;
            return filter;
        }

        //public DataTableFilterModel GetWithdrawalCandidateList(DataTableFilterModel filter, int SessionYr)
        //{
        //    var data = _context.RegistrationMasters.Where(r => r.IsActive && (r.PaymentStatus || r.IsConsultantCandidate || r.IsHRCandidate) && ((r.IsScreenningClear.HasValue && !r.IsScreenningClear.Value && !r.IsStandBy) || (r.IsMedicalClear.HasValue && !r.IsMedicalClear.Value && !r.IsMedicalStandBy)));
        //    if (SessionYr > 0)
        //        data = data.Where(d => d.SessionMaster.SessionYr == SessionYr);
        //    var info = data.Select(model => new RegistrationViewModel()
        //    {
        //        Id = model.Id,
        //        DOB = model.DOB,
        //        Email = model.Email,
        //        Gender = model.Gender == "M" ? "Male" : "Female",
        //        Mobile = model.Mobile,
        //        PaymentStatus = model.PaymentStatus,
        //        RegistartionNo = model.RegistartionNo,
        //        RegistrationDate = model.RegistrationDate,
        //        Fname = model.Fname,
        //        Lname = model.Lname,
        //        //DateOfBirth = "",
        //        PaymentStatusStr = (model.IsConsultantCandidate ? "Consultent Candidate" : (model.IsHRCandidate ? "HR Candidate" : (model.PaymentStatus ? "Success" : "Pending"))),
        //        IsScreenningClear = model.IsScreenningClear,
        //        IsMedicalClear = model.IsMedicalClear,
        //        IsFeePayment = _context.FIN_FeeReceiptMaster.Any(f => f.RegNo == model.RegistartionNo),
        //        IsRefunded = _context.FIN_FeeRefundMaster.Any(f => f.RegNo == model.RegistartionNo),
        //        CourseName = model.CourseMaster.CourseName,
        //        MedicalRemark = model.MedicalRemark != null ? model.MedicalRemark : "",
        //        IsConsultantCandidate = model.IsConsultantCandidate,
        //        IsHRCandidate = model.IsHRCandidate,
        //        BatchName = _context.AddmissionMasters.Any(a => a.RegistrationNo == model.RegistartionNo) ? _context.AddmissionDetails.Where(a => a.AddmissionMaster.RegistrationNo == model.RegistartionNo).FirstOrDefault().BatchMaster.Name : ""
        //    }).AsEnumerable();

        //    var check = info.ToList();

        //    var totalCount = info.Count();
        //    if (!string.IsNullOrWhiteSpace(filter.search.value))
        //    {
        //        info = info.Where(d => d.RegistartionNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.PaymentStatusStr) && d.PaymentStatusStr.ToString().ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Mobile) && d.Mobile.ToString().ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.BatchName) && d.BatchName.ToString().ToLower().Contains(filter.search.value.ToLower())));
        //    }

        //    if (!string.IsNullOrWhiteSpace(filter.columns[9].search.value))
        //        info = info.Where(t => t.CourseName == filter.columns[9].search.value);

        //    if (!string.IsNullOrWhiteSpace(filter.columns[10].search.value))
        //    {
        //        DateTime today = DateTime.Now;
        //        if (filter.columns[10].search.value == "threeMonth")
        //            info = info.Where(t => t.RegistrationDate.HasValue && t.RegistrationDate.Value.Date <= today.AddMonths(-3).Date);
        //        else if (filter.columns[10].search.value == "sixMonth")
        //            info = info.Where(t => t.RegistrationDate.HasValue && t.RegistrationDate.Value.Date <= today.AddYears(-6).Date);
        //    }

        //    var o = filter.order[0];
        //    var name = filter.columns[filter.order[0].column].data;
        //    if (o.dir == "asc")
        //    {
        //        info = info.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
        //    }
        //    else
        //    {
        //        info = info.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
        //    }

        //    var filteredCount = info.Count();
        //    filter.recordsTotal = totalCount;
        //    filter.recordsFiltered = filteredCount;
        //    var dataFilter = info.Skip(filter.start).Take(filter.length).ToList();
        //    int draw = filter.start + 1;
        //    foreach (var i in dataFilter)
        //    {
        //        //i.DateOfBirth = i.DOB.HasValue ? i.DOB.Value.ToString("dd-MM-yyyy") : "";
        //        //i.RegisterDate = i.RegistrationDate.HasValue ? i.RegistrationDate.Value.ToString("dd-MM-yyyy") : "";
        //        i.SrNo = draw;
        //        draw++;
        //    }
        //    filter.data = dataFilter;
        //    return filter;
        //}

        // Screening Test

        public List<RoleViewModel> GetScreeningDocument()
        {
            return _context.DocumentMasters.Where(d => d.IsActive && d.DepartmentMasterId == 1).Select(s => new RoleViewModel
            {
                Name = s.DocumentName,
                Id = s.Id
            }).ToList();
        }

        public List<RoleViewModel> GetScreeningTatooList()
        {
            return _context.TatooOrScars.Where(d => d.IsActive.HasValue && d.IsActive.Value).Select(s => new RoleViewModel
            {
                Name = s.Name,
                Id = s.Id
            }).ToList();
        }

        public bool IsScreeningStandBy(bool IsStandBy, int RegNo)
        {
            bool Status = false;
            var result = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == RegNo).FirstOrDefault();
            if (result != null)
            {
                result.IsStandBy = IsStandBy;
                Status = true;
            }
            return Status;
        }

        public ScreeningInfoViewModel GetScreeningTestInfo(int RegistrationId)
        {
            return MapingScreening(_context
                .RegistrationMasters
                .GroupJoin(
                _context.ScreeningTests,
                r => r.RegistartionNo,
                r1 => r1.RegistrationId,
                (r, r1) => new { S1 = r, S2 = r1.DefaultIfEmpty() })
                .SelectMany(obj => obj.S2.Select(b => new { registerInfo = obj.S1, ScreeningTestInfo = b }))
                .Where(f => f.registerInfo.IsActive && f.registerInfo.RegistartionNo.Equals(RegistrationId))
                .FirstOrDefault());
        }

        public ScreeningInfoViewModel GetScreeningTestInfoToShow(int RegistrationId)
        {
            return MapingScreening(_context
                .RegistrationMasters
                .GroupJoin(
                _context.ScreeningTests,
                r => r.RegistartionNo,
                r1 => r1.RegistrationId,
                (r, r1) => new { S1 = r, S2 = r1.DefaultIfEmpty() })
                .SelectMany(obj => obj.S2.Select(b => new { registerInfo = obj.S1, ScreeningTestInfo = b }))
                .Where(f => f.registerInfo.RegistartionNo.Equals(RegistrationId))
                .FirstOrDefault());
        }

        public bool SaveSendRejectedEmailInfo(SendingEmailForRejectedCandidateViewModel Model)
        {
            bool Status = false;

            if (Model.Email == null)
                return Status;

            SendingEmailForRejectedCandidate obj = null;
            if (Model.Id > 0)
            {
                obj = _context.SendingEmailForRejectedCandidates.Where(i => i.Id == Model.Id).FirstOrDefault();
                if (obj != null)
                {
                    obj.IsActive = false;
                    obj.IsSendEmail = true;
                }
            }
            else
            {
                var objcopy = _context.SendingEmailForRejectedCandidates.Where(i => i.Email == Model.Email && i.IsActive && i.RegistrationNo == Model.RegistrationNo).FirstOrDefault();
                if (objcopy != null)
                    objcopy.DateOfSending = Model.DateOfSending;
                else
                {
                    obj = new SendingEmailForRejectedCandidate
                    {
                        DateOfSending = Model.DateOfSending,
                        Email = Model.Email,
                        IsActive = Model.IsActive,
                        IsSendEmail = Model.IsSendEmail,
                        RegistrationNo = Model.RegistrationNo
                    };
                    _context.SendingEmailForRejectedCandidates.Add(obj);
                }
            }
            if (_context.SaveChanges() > 0)
                Status = true;
            return Status;
        }

        public List<SendingEmailForRejectedCandidateViewModel> SendRejectedEmailWithIn24Hours()
        {
            return _context.SendingEmailForRejectedCandidates.Where(s => s.IsActive).Select(item => new SendingEmailForRejectedCandidateViewModel
            {
                DateOfSending = item.DateOfSending,
                Id = item.Id,
                Email = item.Email,
                IsActive = item.IsActive,
                IsSendEmail = item.IsSendEmail,
                RegistrationNo = item.RegistrationNo
            }).ToList();
        }

        public ScreeningInfoViewModel ScreeningTestSubmit(ScreeningInfoViewModel Model)
        {
            int isSelectedCount = 0;
            if (Model.ScreeningTest.Id > 0)
            {
                var test = _context.ScreeningTests.Where(s => s.Id == Model.ScreeningTest.Id).FirstOrDefault();
                if (test != null)
                {
                    test.DocumentMasterId = Model.ScreeningTest.DocumentMasterId;
                    test.EyeSightLeft = Model.ScreeningTest.EyeSightLeft;
                    test.EyeSightRight = Model.ScreeningTest.EyeSightRight;
                    test.FlyingExp = Model.ScreeningTest.FlyingExp;
                    test.PassportExpiryDate = Model.ScreeningTest.PassportExpiryDateStr != null ? (DateTime.ParseExact(Model.ScreeningTest.PassportExpiryDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                    test.PassportIssueDate = Model.ScreeningTest.PassportIssueDateStr != null ? (DateTime.ParseExact(Model.ScreeningTest.PassportIssueDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                    test.ReviewDate = Model.ScreeningTest.ReviewDateStr != null ? (DateTime.ParseExact(Model.ScreeningTest.ReviewDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                    test.Interviewer = Model.ScreeningTest.Interviewer;
                    test.TatooOrScarId = Model.ScreeningTest.TatooOrScarId;
                    test.Remark = Model.ScreeningTest.Remark;
                    test.ScreeningDate = (!string.IsNullOrEmpty(Model.CreatedDate) ? DateTime.ParseExact(Model.CreatedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now);

                    if (Model.ParameterOption.Any(p => p.Good || p.Fair || p.Poor))
                    {
                        foreach (var item in Model.ParameterOption)
                        {
                            ScreeningParameterOption parameter;
                            parameter = _context.ScreeningParameterOptions.Where(sp => sp.Id == item.Id).FirstOrDefault();
                            if (parameter != null)
                            {
                                parameter.Fair = item.Fair;
                                parameter.Good = item.Good;
                                parameter.Poor = item.Poor;
                                parameter.Remark = item.Remark;
                                parameter.ScreeningParameterId = item.ScreeningParameterId;
                                parameter.InterViewerName = item.Interviewer;
                            }
                            else
                            {
                                parameter = new ScreeningParameterOption
                                {
                                    Fair = item.Fair,
                                    Good = item.Good,
                                    Poor = item.Poor,
                                    Remark = item.Remark,
                                    ScreeningParameterId = item.ScreeningParameterId,
                                    InterViewerName = item.Interviewer,
                                    ScreeningTestId = test.Id
                                };
                                _context.ScreeningParameterOptions.Add(parameter);
                            }
                            if (item.Poor)
                                isSelectedCount++;
                        }
                    }
                    else
                    {
                        isSelectedCount = 10;
                    }
                }
                Model.MStatus = true;
            }
            else
            {
                ScreeningTest test = new ScreeningTest
                {
                    DocumentMasterId = Model.ScreeningTest.DocumentMasterId,
                    EyeSightLeft = Model.ScreeningTest.EyeSightLeft,
                    EyeSightRight = Model.ScreeningTest.EyeSightRight,
                    FlyingExp = Model.ScreeningTest.FlyingExp,
                    RegistrationMasterId = Model.RegId,
                    PassportExpiryDate = Model.ScreeningTest.PassportExpiryDateStr != null ? (DateTime.ParseExact(Model.ScreeningTest.PassportExpiryDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null,
                    PassportIssueDate = Model.ScreeningTest.PassportIssueDateStr != null ? (DateTime.ParseExact(Model.ScreeningTest.PassportIssueDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null,
                    RegistrationId = Model.RegistrationId,
                    ReviewDate = Model.ScreeningTest.ReviewDateStr != null ? (DateTime.ParseExact(Model.ScreeningTest.ReviewDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null,
                    Interviewer = Model.ScreeningTest.Interviewer,
                    ScreeningDate = (!string.IsNullOrEmpty(Model.CreatedDate) ? DateTime.ParseExact(Model.CreatedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now),
                    // ScreeningDate = DateTime.Now,
                    TatooOrScarId = Model.ScreeningTest.TatooOrScarId,
                    Remark = Model.ScreeningTest.Remark
                };
                _context.ScreeningTests.Add(test);
                if (Model.ParameterOption.Any(p => p.Good || p.Fair || p.Poor))
                {
                    foreach (var item in Model.ParameterOption)
                    {
                        ScreeningParameterOption parameter = new ScreeningParameterOption
                        {
                            Fair = item.Fair,
                            Good = item.Good,
                            Poor = item.Poor,
                            Remark = item.Remark,
                            ScreeningParameterId = item.ScreeningParameterId,
                            InterViewerName = item.Interviewer,
                            ScreeningTestId = test.Id
                        };
                        if (item.Poor)
                            isSelectedCount++;
                        _context.ScreeningParameterOptions.Add(parameter);
                    }
                }
                else
                {
                    isSelectedCount = 10;
                }
                Model.MStatus = true;
            }
            var registartion = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == Model.RegistrationId).FirstOrDefault();
            bool? selectedStatus = isSelectedCount == 0 ? true : (isSelectedCount == 10 ? (bool?)null : false);
            Model.IsSelected = selectedStatus;
            if (registartion != null)
            {
                registartion.IsScreenningClear = selectedStatus;
                registartion.IsStandBy = Model.IsStandBy;
                registartion.Height = Model.Height;
                registartion.Width = Model.Weight;
                Model.Email = registartion.Email;
                Model.ApplicationNo = registartion.ApplicationNo;
            }
            if (_context.SaveChanges() >= 0)
            {
                var admissionData = _context.AddmissionMasters.Where(a => a.RegistrationNo == Model.RegistrationId).FirstOrDefault();
                if (admissionData != null)
                {
                    if (selectedStatus.HasValue && !selectedStatus.Value)
                        admissionData.IsActive = false;
                    else if (selectedStatus.HasValue && selectedStatus.Value)
                    {
                        admissionData.IsActive = true;
                        var admissionDetails = _context.AddmissionDetails.Where(a => a.AddmissionId == admissionData.Id).FirstOrDefault();
                        if (admissionDetails != null)
                            admissionDetails.BatchId = Model.BatchId;
                    }
                    _context.SaveChanges();
                }
                else
                {
                    if (selectedStatus.HasValue && selectedStatus.Value)
                    {
                        AddmissionService admissionService = new AddmissionService();
                        var RegisterNo = admissionService.CreateAddmission1(Model.RegistrationId, Model.CreatedBy, registartion.Id, Model.BatchId, Model.CreatedDate);
                    }
                }
            }
            if (Model.IsWithdrawal == true)
            {
                UpdateMedicalRemark(Model.RegistrationId, Model.ScreeningTest.Remark, "Withdrawn", Model.RegId, Model.CreatedBy);
            }
            else
            {
                UpdateWithStopWithdrwn(Model.RegistrationId, "NotWithdrawn", Model.RegId, Model.CreatedBy);
            }
            return Model;
        }

        public bool GetFeeTypeExist(int SessionId, int CourseId)
        {
            return _context.FeeTypeDetails.Any(a => a.IsActive && a.SessionMasterId == SessionId && a.CourseId == CourseId);
        }

        public override bool IsManualExistByEmail(string Email, string Mobile)
        {
            bool result = false;
            DateTime CurrentDate = DateTime.Now.Date;
            var data = _context.RegistrationMasters.Where(r => r.IsActive && (r.Email.ToLower() == Email.ToLower() || r.Mobile == Mobile)).Select(item => new SemesterMasterViewModel
            {
                BatchName = item.Email,
                SemesterName = item.Mobile,
                RegistrationDate = item.RegistrationDate
            }).FirstOrDefault();

            if (data != null)
            {
                int NoOfDays = GetMonthDifference(data.RegistrationDate.Value.Date, CurrentDate);
                if (NoOfDays > 180)
                    result = true;
            }
            return result;
        }

        public override string IsStudentExistByEmail(string Email, string Mobile)
        {
            var result = "";
            DateTime CurrentDate = DateTime.Now.Date;
            var data = _context.RegistrationMasters.Where(r => r.Email.ToLower() == Email.ToLower() || r.Mobile == Mobile).Select(item => new SemesterMasterViewModel
            {
                BatchName = item.Email,
                SemesterName = item.Mobile,
                RegistrationDate = item.RegistrationDate
            }).FirstOrDefault();
            if (data != null)
            {
                int NoOfDays = GetMonthDifference(data.RegistrationDate.Value.Date, CurrentDate);
                if (NoOfDays < 180)
                    result = "Registration for this candidate already exist. Kindly ensure 6 months have elapsed from date of registration!";
            }
            return result;
        }

        public string DeleteRegistrationByRegNo(int RegNo)
        {
            string Status = "";
            var data = _context.RegistrationMasters.Where(r => r.RegistartionNo == RegNo).FirstOrDefault();
            if (data != null)
            {
                if ((data.IsScreenningClear.HasValue && data.IsScreenningClear.Value) || data.IsStandBy)
                {
                    Status = "This candidate record can not delete because screenning test has been cleared!";
                }
                else
                {
                    data.IsActive = false;
                    _context.SaveChanges();
                }
            }
            return Status;
        }

        public bool DisableReregisterOldRecord(string Email, string Mobile)
        {
            var status = false;
            try
            {
                var data = _context.RegistrationMasters.Where(r => r.IsActive && (r.Email == Email || r.Mobile == Mobile)).ToList();
                foreach (var item in data)
                {
                    item.IsAddmission = false;
                    item.IsActive = false;
                    item.IsReRegister = true;
                }
                _context.SaveChanges();
                status = true;
                return status;
            }
            catch (Exception ex)
            { return status; }
        }

        public List<RegistrationViewModel> GetReRegisterHistory(string Email, string Mobile)
        {
            List<RegistrationViewModel> Result = new List<RegistrationViewModel>();
            if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Mobile))
                return Result;
            var data = _context.RegistrationMasters.Where(r => r.Email == Email || r.Mobile == Mobile);

            var list = data.Select(model => new RegistrationViewModel
            {
                Id = model.Id,
                DOB = model.DOB,
                Email = model.Email,
                Gender = model.Gender == "M" ? "Male" : "Female",
                Mobile = model.Mobile,
                PaymentStatus = model.PaymentStatus,
                RegistartionNo = model.RegistartionNo,
                RegistrationDate = model.RegistrationDate,
                Fname = model.Fname,
                Lname = model.Lname,
                //DateOfBirth = "",
                PaymentStatusStr = (model.IsConsultantCandidate ? "Consultant Candidate" : (model.IsHRCandidate ? "HR Candidate" : (model.PaymentStatus ? "Success" : "Pending"))),
                IsScreenningClear = model.IsScreenningClear,
                IsMedicalClear = model.IsMedicalClear,
                IsFeePayment = _context.FIN_FeeReceiptMaster.Any(f => f.RegNo == model.RegistartionNo),
                IsRefunded = _context.FIN_FeeRefundMaster.Any(f => f.RegNo == model.RegistartionNo),
                CourseName = model.CourseMaster.CourseName,
                MedicalRemark = model.MedicalRemark != null ? model.MedicalRemark : "",
                IsConsultantCandidate = model.IsConsultantCandidate,
                IsHRCandidate = model.IsHRCandidate,
                BatchName = _context.AddmissionMasters.Any(a => a.RegistrationNo == model.RegistartionNo) ? _context.AddmissionDetails.Where(a => a.AddmissionMaster.RegistrationNo == model.RegistartionNo).FirstOrDefault().BatchMaster.Name : "",
                IsReregister = model.IsReRegister,
                IsActive = model.IsActive
            }).OrderByDescending(o => o.RegistrationDate).ToList();

            if (list.Count > 0)
            {
                var info = list.Where(r => r.IsReregister && !r.IsActive).OrderByDescending(o => o.RegistartionNo).ToList();
                var list1 = list.Where(r => r.IsActive && !r.IsReregister && ((r.IsScreenningClear.HasValue && !r.IsScreenningClear.Value) || (r.IsMedicalClear.HasValue && !r.IsMedicalClear.Value))).FirstOrDefault();
                if (list1 != null)
                    Result.Add(list1);
                if (info.Count > 0)
                {
                    foreach (var item in info)
                    {
                        Result.Add(item);
                    }
                }
            }
            return Result;
        }

        public bool UpdateMobileNumber()
        {
            bool status = false;
            var data = _context.RegistrationMasters.Where(r => r.Mobile.Length == 10).ToList();
            if (data != null)
            {
                foreach (var d in data)
                {
                    d.Mobile = "+91-" + d.Mobile;
                }
                _context.SaveChanges();
                status = true;
            }
            return status;
        }


        public List<SessionMasterViewModel> GetSessionList()
        {
            return _context.SessionMasters.Where(s => s.IsActive).GroupBy(g => g.SessionYr).Select(item => new SessionMasterViewModel
            {
                Id = item.Key.Value
            }).OrderByDescending(o => o.Id).ToList();
        }

        public void AddRegistrationIdInAdmissionMaster()
        {
            var data = _context.AddmissionMasters.ToList();
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    var r = _context.RegistrationMasters.Where(a => a.RegistartionNo == i.RegistrationNo).FirstOrDefault();
                    if (r != null)
                    {
                        i.RegistrationMasterId = r.Id;
                    }
                }
                _context.SaveChanges();
            }
        }

        public List<RegistrationViewModel> GetScreenningExportData(ExportDataViewModel filter, int SessionYr)
        {
            var data = _context.RegistrationMasters.Where(r => r.IsActive).AsQueryable();
            data = data.Where(d => d.PaymentStatus || d.IsConsultantCandidate || d.IsHRCandidate);
            if (SessionYr > 0)
                data = data.Where(d => d.SessionMaster.SessionYr == SessionYr);
            var info = data.Select(model => new RegistrationViewModel()
            {
                Id = model.Id,
                DOB = model.DOB,
                Email = model.Email.ToLower(),
                Gender = model.Gender == "M" ? "Male" : "Female",
                Mobile = model.Mobile,
                PaymentStatus = model.PaymentStatus,
                RegistartionNo = model.RegistartionNo,
                RegistrationDate = model.RegistrationDate,
                Fname = model.Fname,
                Lname = model.Lname,
                PaymentStatusStr = (model.IsConsultantCandidate ? "Consultant Candidate" : (model.IsHRCandidate ? "HR Candidate" : (model.PaymentStatus ? "Success" : "Pending"))),
                ScreenningStatus = model.IsScreenningClear.HasValue ? (model.IsScreenningClear.Value ? "Selected" : "Rejected") : "Pending",
                CourseName = model.CourseMaster.CourseName,
                SourceOfCandidate = model.SourceOfCandidate,
                ModOfPayment = model.ModOfPayment,
                BatchName = model.AddmissionMasters.Count > 0 ? model.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchMaster.Name : "",
                BatchId = model.AddmissionMasters.Count > 0 ? model.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchId.Value : 0,
                IsStandBy = model.IsStandBy
            }).AsNoTracking().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                info = info.Where(d => (d.RegistartionNo.ToString().ToLower().Contains(filter.Search.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.Search.ToLower())) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(filter.Search.ToLower())) || (!string.IsNullOrEmpty(d.PaymentStatusStr) && d.PaymentStatusStr.ToString().ToLower().Contains(filter.Search.ToLower())) || (!string.IsNullOrEmpty(d.Mobile) && d.Mobile.ToString().ToLower().Contains(filter.Search.ToLower())) || (!string.IsNullOrEmpty(d.BatchName) && d.BatchName.ToString().ToLower().Contains(filter.Search.ToLower()))));
            }

            if (filter.BatchId > 0)
            {
                info = info.Where(t => t.BatchId == filter.BatchId);
            }

            if (!string.IsNullOrWhiteSpace(filter.CourseId))
                info = info.Where(t => t.CourseName == filter.CourseId);

            if (!string.IsNullOrWhiteSpace(filter.IsSelected))
            {
                if (filter.IsSelected == "Pending")
                    info = info.Where(t => t.ScreenningStatus == "Pending");
                else if (filter.IsSelected == "Selected")
                    info = info.Where(t => t.ScreenningStatus == "Selected" && !t.IsStandBy);
                else if (filter.IsSelected == "Rejected")
                    info = info.Where(t => t.ScreenningStatus == "Rejected" && !t.IsStandBy);
                else if (filter.IsSelected == "Stand-By")
                    info = info.Where(t => t.IsStandBy);
            }
            var Result = info.OrderByDescending(o => o.RegistartionNo).ToList();
            return Result;
        }

        public int IsOverBatchStrength(int BatchId)
        {
            var dd = _context.AddmissionMasters.Where(a => a.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.IsActive && a.AddmissionDetails.FirstOrDefault().BatchId == BatchId).Count();
            if (BatchId == 19)
                dd = 20;
            return dd;
        }

        public string ChangeLeadSource(int RegNo, string LeadSource)
        {
            string msg = "";
            var data = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == RegNo).FirstOrDefault();
            if (data != null)
            {
                if (LeadSource == "" || LeadSource == "Select")
                    data.SourceOfCandidate = null;
                else
                    data.SourceOfCandidate = LeadSource;
                msg = "Lead source succesfully changed";
                _context.SaveChanges();
            }
            return msg;
        }
    }
}
