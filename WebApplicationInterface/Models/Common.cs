using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WebApplicationInterface.DataLayer;

namespace WebApplicationInterface.Models
{
    public class Common
    {
        SJStarERPEntities _context = null;
        public Common()
        {
            _context = new SJStarERPEntities();
        }
        public RegistrationViewModel WebInterfaceAddReg(RegistrationViewModel model)
        {
            RegistrationMaster obj = new RegistrationMaster();
            obj.Fname = model.Fname;
            obj.Lname = model.Lname;
            obj.Mobile = model.Mobile;
            obj.CorrespondenceAddress = model.CorrespondenceAddress;
            obj.CorrespondenceCity = model.CorrespondenceCity;
            obj.CorrespondenceState = model.CorrespondenceState;
            obj.CorrespondenceZip = model.CorrespondenceZip;
            obj.PermanentAddress = model.PermanentAddress;
            obj.PermanentCity = model.PermanentCity;
            obj.PermanentState = model.PermanentState;
            obj.PermanentZip = model.PermanentZip;
            if (model.DateOfBirth != null && model.DateOfBirth != "")
                obj.DOB = DateTime.ParseExact(model.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            obj.Email = model.Email;
            obj.Gender = model.Gender;
            obj.Height = model.Height;
            obj.Width = model.Width;
            obj.RegistartionNo = MaxRegistartionNumber();
            obj.IsPassport = model.IsPassport;
            obj.IsAppeared = model.IsAppeared;
            obj.Education = model.Education;
            obj.IsConsultantCandidate = model.IsConsultantCandidate;
            obj.IsHRCandidate = model.IsHRCandidate;
            obj.RegistrationDate = DateTime.Now;
            obj.PaymentDate = DateTime.Now;
            obj.PaymentId = model.PaymentId;
            if (string.IsNullOrEmpty(model.CourseName))
                obj.PaymentStatus = false;
            else
                obj.PaymentStatus = model.PaymentStatus;
            obj.ModOfPayment = model.ModOfPayment;
            obj.IsActive = true;
            obj.PassingYear = model.PassingYear;
            if (model.CurrentYear > 0 && !string.IsNullOrEmpty(model.CourseName))
            {
                var sessionCourseData = GetSessionIdByCourse(model.CurrentYear, model.CourseName);
                if (sessionCourseData != null)
                {
                    obj.SessionId = sessionCourseData.SessionId;
                    obj.CourseId = sessionCourseData.CourseId;
                }
            }
            obj.OrderId = model.OrderId;
            obj.ApplicationNo = model.ApplicationNo;
            obj.SerialNo = model.SerialNo;
            if (!string.IsNullOrEmpty(model.LeadType))
            {
                if (model.LeadType == "Consultant")
                    obj.IsConsultantCandidate = true;
                else if (model.LeadType == "SpiceJet HR")
                    obj.IsHRCandidate = true;
            }
            obj.SourceOfCandidate = model.LeadType;
            if (!string.IsNullOrEmpty(model.IsAadharCard) && model.IsAadharCard == "Yes")
                obj.IsAadharCard = true;
            obj.AadharCardNo = model.AadharCardNo;
            obj.DepartmentMasterId = 1;
            var list = _context.RegistrationMasters.Where(w => (w.Email == model.Email || w.Mobile == model.Mobile) && !w.IsReRegister).ToList();
            if (list.Count() > 0)
            {
                foreach (var i in list)
                {
                    i.IsActive = false;
                    i.IsReRegister = true;
                }
            }
            _context.RegistrationMasters.Add(obj);
            _context.SaveChanges();
            model.RegistartionNo = obj.RegistartionNo;
            return model;
        }

        public PtaScreeningExamFeeInfoViewModel AddScreeningExamInfo(PtaScreeningExamFeeInfoViewModel Model)
        {
            var data = _context.ptaScreeningInfoes.Where(p => p.IsActive && p.ptaPilotRegistrationMaster.ApplicationNo == Model.ApplicationNo).FirstOrDefault();
            if (data != null && !_context.ptaScreeningExamFeeInfoes.Where(p => p.PTAScreeningInfoId == data.Id && p.ScreeningAmountTerm == Model.ScreeningAmountTerm).Any())
            {
                ptaScreeningExamFeeInfo info = new ptaScreeningExamFeeInfo
                {
                    PaymentId = Model.PaymentId,
                    ScreeningAmount = Model.ScreeningAmount,
                    ScreeningAmountTerm = Model.ScreeningAmountTerm,
                    PTAScreeningInfoId = data.Id,
                    PTAPilotRegistrationMaterId = data.PTAPilotRegistrationMasterId,
                    DateOfAmount = DateTime.ParseExact(Model.DateOfAmount, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EnteredDate = DateTime.Now
                };
                _context.ptaScreeningExamFeeInfoes.Add(info);

                var result = _context.ptaScreeningTestResults.Where(p => p.PTAScreeningInfoId == data.Id).OrderByDescending(o => o.PTAScreeningTestTypeId).ToList();
                if (result.Count > 0)
                {
                    var d = result.Where(r => r.IsPassed.HasValue && !r.IsPassed.Value).FirstOrDefault();
                    if (d != null)
                    {
                        d.IsPassed = null;
                        d.ObtainMark = 0;
                    }
                }
                _context.SaveChanges();
                Model.Id = info.Id;
            }
            else
            {
                var d = _context.ptaPilotRegistrationMasters.Where(p => p.IsActive && p.ApplicationNo == Model.ApplicationNo).FirstOrDefault();
                if (d != null)
                {
                    ptaScreeningExamFeeInfo info = new ptaScreeningExamFeeInfo
                    {
                        PaymentId = Model.PaymentId,
                        ScreeningAmount = Model.ScreeningAmount,
                        ScreeningAmountTerm = Model.ScreeningAmountTerm,
                        PTAPilotRegistrationMaterId = d.Id,
                        DateOfAmount = DateTime.ParseExact(Model.DateOfAmount, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        EnteredDate = DateTime.Now
                    };
                    _context.ptaScreeningExamFeeInfoes.Add(info);
                    _context.SaveChanges();
                    Model.Id = info.Id;
                }
            }
            return Model;
        }

        public PilotRegistrationViewModel CreatePilotTrainingRegistration(PilotRegistrationViewModel model)
        {
            ptaPilotRegistrationMaster register = new ptaPilotRegistrationMaster();
            if (!string.IsNullOrEmpty(model.CourseApplied))
            {
                var courseInfo = _context.CourseMasters.Where(c => c.IsActive && c.DepartmentMasterId == 2 && c.CourseName == model.CourseApplied).FirstOrDefault();
                if (courseInfo != null)
                {
                    int PilotSessionId = GetPilotSessionIdByCourse(courseInfo.Id, DateTime.Now.Year, courseInfo.CourseName);
                    register.CourseId = courseInfo.Id;
                    register.SessionId = PilotSessionId;
                }
            }
            register.Fname = model.FirstName;
            register.Lname = model.Lastname;
            register.Email = model.Email;
            register.Mobile = model.MobileNo;
            if (model.DateOfBirth != null && model.DateOfBirth != "")
                register.DOB = DateTime.ParseExact(model.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            register.Height = model.Height;
            register.DepartmentMasterId = 2;
            register.RegistrationDate = DateTime.Now;
            register.RegistrationNo = PilotMaxRegistartionNumber();
            register.IsActive = true;
            register.ApplicationNo = model.ApplicationNo;
            _context.ptaPilotRegistrationMasters.Add(register);


            ptaRegistrationInfo PilotRegister = new ptaRegistrationInfo();
            PilotRegister.ptaPilotRegistrationMasterId = register.Id;
            PilotRegister.RegistartionNo = "PTA" + register.RegistrationNo;
            if (!string.IsNullOrEmpty(model.Title))
                PilotRegister.ptaTitleMasterId = Convert.ToInt32((Title)Enum.Parse(typeof(Title), model.Title));
            if (!string.IsNullOrEmpty(model.Gender))
                PilotRegister.ptaGenderMasterId = Convert.ToInt32((Gender)Enum.Parse(typeof(Gender), model.Gender.Replace(" ", string.Empty)));
            if (!string.IsNullOrEmpty(model.Nationality))
            {
                if (model.Nationality == "Overseas Citizen of India (OCI)")
                    PilotRegister.ptaNatioalityMasterId = 2;
                else
                    PilotRegister.ptaNatioalityMasterId = Convert.ToInt32((Nationality)Enum.Parse(typeof(Nationality), model.Nationality.Replace(" ", string.Empty)));
            }
            if (!string.IsNullOrEmpty(model.LeadSource))
                PilotRegister.ptaLeadSourceMasterId = Convert.ToInt32((LeadSource)Enum.Parse(typeof(LeadSource), model.LeadSource));
            if (!string.IsNullOrEmpty(model.MedicalStatus))
                PilotRegister.ptaMedicalStatusMasterId = Convert.ToInt32((MedicalStatus)Enum.Parse(typeof(MedicalStatus), model.MedicalStatus.Replace(" ", string.Empty)));

            _context.ptaRegistrationInfoes.Add(PilotRegister);


            if (!string.IsNullOrEmpty(model.PassportNo))
            {
                ptaPassport passport = new ptaPassport();
                passport.PassportCountry = model.PassportCountry;
                passport.PassportNo = model.PassportNo;
                passport.PlaceOfIssue = model.PassportPlaceOfIssue;
                if (!string.IsNullOrEmpty(model.PassportDateOfExpiry))
                    passport.DateOfExpiry = DateTime.ParseExact(model.PassportDateOfExpiry, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(model.PassportDateOfIssue))
                    passport.DateOfIssue = DateTime.ParseExact(model.PassportDateOfIssue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                passport.ptaRegistrationInfoId = PilotRegister.Id;
                _context.ptaPassports.Add(passport);
            }

            if (!string.IsNullOrEmpty(model.GuardianName))
            {
                ptaGuardian Guardian = new ptaGuardian();
                Guardian.Name = model.GuardianName;
                Guardian.Mobile = model.GuardianMobNo;
                Guardian.Relation = model.GuardianRelation;
                Guardian.ptaRegistrationInfoId = PilotRegister.Id;
                _context.ptaGuardians.Add(Guardian);
            }

            if (!string.IsNullOrEmpty(model.CorrespondenceCountry))
            {
                ptaAddressDetail Address = new ptaAddressDetail();
                Address.Cor_Country = model.CorrespondenceCountry;
                Address.Cor_State = model.CorrespondenceState;
                Address.Cor_District = model.CorrespondenceDistrict;
                Address.Cor_City = model.CorrespondenceCity;
                Address.Cor_Address1 = model.CorrespondenceAddress1;
                Address.Cor_Address2 = model.CorrespondenceAddress2;
                Address.Cor_PinCode = model.CorrespondencePinCode;
                Address.IsPermanent = model.SameAsCorrespondance;
                if (!string.IsNullOrEmpty(model.PermanentCountry))
                {
                    Address.Per_Country = model.PermanentCountry;
                    Address.Per_State = model.PermanentState;
                    Address.Per_District = model.PermanentDistrict;
                    Address.Per_City = model.PermanentCity;
                    Address.Per_Address1 = model.PermanentAddress1;
                    Address.Per_Address2 = model.PermanentAddress2;
                    Address.Per_PinCode = model.PermanentPinCode;
                }
                Address.ptaRegistrationInfoId = PilotRegister.Id;
                _context.ptaAddressDetails.Add(Address);
            }

            if (!string.IsNullOrEmpty(model.ApplicantName))
            {
                ptaDeclaration Delaration = new ptaDeclaration();
                Delaration.ApplicantName = model.ApplicantName;
                Delaration.ParentName = model.ParentName;
                if (!string.IsNullOrEmpty(model.Date))
                    Delaration.Date = DateTime.ParseExact(model.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Delaration.ptaRegistrationInfoId = PilotRegister.Id;
                _context.ptaDeclarations.Add(Delaration);
            }

            var BoardOrUniversity = _context.ptaBoardAndUniversityMasters.Where(u => u.IsActive).ToList();
            if (!string.IsNullOrEmpty(model.Tenth_BoardAndUniversity))
            {
                ptaEducationDetail HighSchoolInfo = new ptaEducationDetail();
                HighSchoolInfo.Class = "10th";
                HighSchoolInfo.NameOfInstitute = model.Tenth_NameOfInstitute;
                HighSchoolInfo.ptaBoardAndUniversityMasterId = BoardOrUniversity.Where(b => b.Name == model.Tenth_BoardAndUniversity).FirstOrDefault().Id;
                HighSchoolInfo.YearOfPassing = model.Tenth_YearOfPassing;
                HighSchoolInfo.ptaEduEvaluationTypeMasterId = Convert.ToInt32((EvaluationType)Enum.Parse(typeof(EvaluationType), model.Tenth_EvaluationType.Replace(" ", string.Empty)));
                HighSchoolInfo.PercentageOrCGPA = model.Tenth_PercentageOrCGPA;
                HighSchoolInfo.ptaRegistrationInfoId = PilotRegister.Id;
                _context.ptaEducationDetails.Add(HighSchoolInfo);
            }

            ptaEducationDetail InterMediateInfo = new ptaEducationDetail();
            if (!string.IsNullOrEmpty(model.Twelve_BoardAndUniversity))
            {
                InterMediateInfo.Class = "12th";
                InterMediateInfo.NameOfInstitute = model.Twelve_NameOfInstitute;
                InterMediateInfo.ptaBoardAndUniversityMasterId = BoardOrUniversity.Where(b => b.Name == model.Twelve_BoardAndUniversity).FirstOrDefault().Id;
                InterMediateInfo.ptaStreamMasterId = Convert.ToInt32((Stream)Enum.Parse(typeof(Stream), model.Twelve_Stream));
                InterMediateInfo.YearOfPassing = model.Twelve_YearOfPassing;
                if (!string.IsNullOrEmpty(model.Twelve_EvaluationType))
                    InterMediateInfo.ptaEduEvaluationTypeMasterId = Convert.ToInt32((EvaluationType)Enum.Parse(typeof(EvaluationType), model.Twelve_EvaluationType.Replace(" ", string.Empty)));
                if (model.Twelve_PercentageOrCGPA > 0)
                    InterMediateInfo.PercentageOrCGPA = model.Twelve_PercentageOrCGPA;
                if (!string.IsNullOrEmpty(model.Twelve_ResultStatus))
                    InterMediateInfo.ptaResultStatusMasterId = Convert.ToInt32((ResultStatus)Enum.Parse(typeof(ResultStatus), model.Twelve_ResultStatus));
                InterMediateInfo.ptaRegistrationInfoId = PilotRegister.Id;
                _context.ptaEducationDetails.Add(InterMediateInfo);
            }

            if (!string.IsNullOrEmpty(model.UG_BoardAndUniversity))
            {
                ptaEducationDetail UGInfo = new ptaEducationDetail();
                UGInfo.Class = "UG";
                UGInfo.NameOfInstitute = model.UG_NameOfInstitute;
                if (!string.IsNullOrEmpty(model.UG_BoardAndUniversity))
                    UGInfo.ptaUGBoardAndUniversityMasterId = _context.ptaUGBoardAndUniversityMasters.Where(b => b.IsActive && b.Name == model.UG_BoardAndUniversity).FirstOrDefault().Id;
                if (!string.IsNullOrEmpty(model.UG_YearOfPassing))
                    UGInfo.YearOfPassing = model.UG_YearOfPassing;
                if (!string.IsNullOrEmpty(model.UG_EvaluationType))
                    UGInfo.ptaEduEvaluationTypeMasterId = Convert.ToInt32((EvaluationType)Enum.Parse(typeof(EvaluationType), model.UG_EvaluationType.Replace(" ", string.Empty)));
                UGInfo.PercentageOrCGPA = model.UG_PercentageOrCGPA;
                if (!string.IsNullOrEmpty(model.UG_ResultStatus))
                    UGInfo.ptaResultStatusMasterId = Convert.ToInt32((ResultStatus)Enum.Parse(typeof(ResultStatus), model.UG_ResultStatus));
                UGInfo.ptaRegistrationInfoId = PilotRegister.Id;
                _context.ptaEducationDetails.Add(UGInfo);
            }
            _context.SaveChanges();

            if (InterMediateInfo.Id > 0)
            {
                ptaEducationSubjectMark Physics = new ptaEducationSubjectMark
                {
                    SubjectName = "Physics",
                    MaximumMark = model.MaxMarksOfPhysics,
                    ObtainMark = model.ObtainMarksOfPhysics,
                    Percentage = model.PercentageOfPhysics,
                    ptaEducationDetailId = InterMediateInfo.Id
                };
                _context.ptaEducationSubjectMarks.Add(Physics);

                ptaEducationSubjectMark English = new ptaEducationSubjectMark
                {
                    SubjectName = "English",
                    MaximumMark = model.MaxMarksOfEnglish,
                    ObtainMark = model.ObtainMarksOfEnglish,
                    Percentage = model.PercentageOfEnglish,
                    ptaEducationDetailId = Physics.ptaEducationDetailId
                };
                _context.ptaEducationSubjectMarks.Add(English);

                ptaEducationSubjectMark Math = new ptaEducationSubjectMark
                {
                    SubjectName = "Math",
                    MaximumMark = model.MaxMarksOfMath,
                    ObtainMark = model.ObtainMarksOfMath,
                    Percentage = model.PercentageOfMath,
                    ptaEducationDetailId = English.ptaEducationDetailId
                };
                _context.ptaEducationSubjectMarks.Add(Math);
                _context.SaveChanges();
            }
            model.RegistrationNo = PilotRegister.RegistartionNo;
            return model;
        }

        public int MaxRegistartionNumber()
        {
            return _context.RegistrationMasters.Where(r => r.DepartmentMasterId == 1).Any() ? _context.RegistrationMasters.Where(r => r.DepartmentMasterId == 1).Max(r => r.RegistartionNo) + 1 : 1;
        }

        public int PilotMaxRegistartionNumber()
        {
            return _context.ptaPilotRegistrationMasters.Any() ? _context.ptaPilotRegistrationMasters.Max(r => r.RegistrationNo) + 1 : 1000001;
        }

        public string IsExistEmailOrMobile(string Email, string Mobile)
        {
            var result = "";
            var data = _context.RegistrationMasters.Where(r => r.IsActive && r.DepartmentMasterId == 1 && (r.Email.ToLower() == Email.ToLower() || r.Mobile == Mobile)).Select(item => new EmailMobileExistViewModel
            {
                Email = item.Email,
                Mobile = item.Mobile
            }).FirstOrDefault();
            if (data != null)
            {
                if (data.Email.ToLower() == Email.ToLower())
                    result = "Email already exist!";
                else if (data.Mobile == Mobile)
                    result = "Mobile number already exist!";
            }
            return result;
        }

        public EmailMobileExistViewModel GetSessionIdByCourse(int Year, string CourseName)
        {
            Year = DateTime.Now.Year;
            return _context.SessionMasters.Where(s => s.IsActive && s.CourseMaster.CourseName == CourseName && s.SessionYr == Year).Select(item => new EmailMobileExistViewModel
            {
                CourseId = item.CourseMasterId,
                SessionId = item.Id
            }).FirstOrDefault();
        }

        public int GetPilotSessionIdByCourse(int CourseId, int Year, string CourseName)
        {
            var result = "";
            if (CourseName == "Cadet Pilot Programme")
                result = Year + "-" + (Year + 1);
            else if (CourseName == "Cadet Pilot Programme with BBA")
                result = Year + "-" + (Year + 3);
            else if (CourseName == "Cadet Pilot Programme with MBA")
                result = Year + "-" + (Year + 2);

            var Data = _context.SessionMasters.Where(s => s.IsActive && s.CourseMasterId == CourseId && s.SessionName == result).FirstOrDefault();
            return Data != null ? Data.Id : 0;
        }

        public bool UpdatePaymentDetail(int RegNo, string PaymentId)
        {
            var Status = false;
            if (RegNo > 0 && !string.IsNullOrEmpty(PaymentId))
            {
                var data = _context.RegistrationMasters.Where(r => r.RegistartionNo == RegNo && r.DepartmentMasterId == 1).FirstOrDefault();
                if (data != null)
                {
                    data.PaymentId = PaymentId;
                    data.PaymentStatus = true;
                    data.PaymentDate = DateTime.Now;
                    data.ModOfPayment = "Online";
                    data.IsActive = true;
                    _context.SaveChanges();
                    Status = true;
                }
            }
            return Status;
        }
    }

    public class EmailMobileExistViewModel
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int CourseId { get; set; }
        public int SessionId { get; set; }
    }

    public class RegistrationViewModel
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Width { get; set; }
        public string CorrespondenceAddress { get; set; }
        public string CorrespondenceCity { get; set; }
        public string CorrespondenceState { get; set; }
        public string CorrespondenceZip { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentState { get; set; }
        public string PermanentZip { get; set; }
        public bool IsAppeared { get; set; }
        public bool IsPassport { get; set; }
        public string PaymentId { get; set; }
        public bool PaymentStatus { get; set; }
        public int RegistartionNo { get; set; }
        public string Education { get; set; }
        public bool IsConsultantCandidate { get; set; }
        public string StudentName { get { return string.Format("{0} {1}", Fname, Lname); } }
        public string DateOfBirth { get; set; }
        public bool IsRejected { get; set; }
        public bool IsStandBy { get; set; }
        public int SessionId { get; set; }
        public int CourseId { get; set; }
        public bool IsFeePayment { get; set; }
        public string ModOfPayment { get; set; }
        public string CourseName { get; set; }
        public bool IsMedicalStandBy { get; set; }
        public bool IsFeePayStandBy { get; set; }
        public bool IsHRCandidate { get; set; }
        public int CurrentYear { get; set; }
        public string PassingYear { get; set; }
        public string Msg { get; set; }
        public string OrderId { get; set; }
        public string ApplicationNo { get; set; }
        public string SerialNo { get; set; }
        public string LeadType { get; set; }
        public string AadharCardNo { get; set; }
        public string IsAadharCard { get; set; }
    }

    public class PilotRegistrationViewModel
    {
        public int Id { get; set; }
        public string CourseApplied { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Nullable<double> Height { get; set; }
        public string Nationality { get; set; }
        public string LeadSource { get; set; }

        public string PassportNo { get; set; }
        public string PassportCountry { get; set; }
        public string PassportPlaceOfIssue { get; set; }
        public string PassportDateOfIssue { get; set; }
        public string PassportDateOfExpiry { get; set; }

        public string GuardianName { get; set; }
        public string GuardianMobNo { get; set; }
        public string GuardianRelation { get; set; }

        public string CorrespondenceCountry { get; set; }
        public string CorrespondenceState { get; set; }
        public string CorrespondenceDistrict { get; set; }
        public string CorrespondenceCity { get; set; }
        public string CorrespondenceAddress1 { get; set; }
        public string CorrespondenceAddress2 { get; set; }
        public string CorrespondencePinCode { get; set; }
        public bool SameAsCorrespondance { get; set; }
        public string PermanentCountry { get; set; }
        public string PermanentState { get; set; }
        public string PermanentDistrict { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentAddress1 { get; set; }
        public string PermanentAddress2 { get; set; }
        public string PermanentPinCode { get; set; }

        public string Tenth_NameOfInstitute { get; set; }
        public string Tenth_BoardAndUniversity { get; set; }
        public string Tenth_YearOfPassing { get; set; }
        public string Tenth_EvaluationType { get; set; }
        public float Tenth_PercentageOrCGPA { get; set; }

        public string Twelve_ResultStatus { get; set; }
        public string Twelve_NameOfInstitute { get; set; }
        public string Twelve_BoardAndUniversity { get; set; }
        public string Twelve_Stream { get; set; }
        public string Twelve_YearOfPassing { get; set; }
        public string Twelve_EvaluationType { get; set; }
        public float Twelve_PercentageOrCGPA { get; set; }

        public int MaxMarksOfPhysics { get; set; }
        public float ObtainMarksOfPhysics { get; set; }
        public float PercentageOfPhysics { get; set; }

        public int MaxMarksOfMath { get; set; }
        public float ObtainMarksOfMath { get; set; }
        public float PercentageOfMath { get; set; }

        public int MaxMarksOfEnglish { get; set; }
        public float ObtainMarksOfEnglish { get; set; }
        public float PercentageOfEnglish { get; set; }

        public string UG_NameOfInstitute { get; set; }
        public string UG_BoardAndUniversity { get; set; }
        public string UG_Degree { get; set; }
        public string UG_YearOfPassing { get; set; }
        public string UG_ResultStatus { get; set; }
        public string UG_EvaluationType { get; set; }
        public float UG_PercentageOrCGPA { get; set; }

        public string MedicalStatus { get; set; }

        public string ApplicantName { get; set; }
        public string ParentName { get; set; }
        public string Date { get; set; }


        public string RegistrationNo { get; set; }
        public string ApplicationNo { get; set; }
    }

    public class PtaScreeningExamFeeInfoViewModel
    {
        public int Id { get; set; }
        //public int PTAScreeningInfoId { get; set; } 
        public string ApplicationNo { get; set; }
        public int ScreeningAmountTerm { get; set; }
        public int ScreeningAmount { get; set; }
        public string DateOfAmount { get; set; }
        public string PaymentId { get; set; }
        //public string ExamResult { get; set; }
        //public int PTAScreeningTestTypeId { get; set; }
        public DateTime EnteredDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }
}