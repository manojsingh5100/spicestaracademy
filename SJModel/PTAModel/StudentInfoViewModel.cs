using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJModel.PTAModel;

namespace SJModel.PTAModel
{
    public class StudentInfoViewModel
    {
        public int Id { get; set; }
        public string ApplicationNo { get; set; }
        public string CandidateName { get; set; }
        public string SessionName { get; set; }
        public string CourseName { get; set; }
        public string Education { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string Mobile { get; set; }
        public string Batch { get; set; }
        public bool ValidPassport { get; set; }
        public int BoardandUniversityInfoId10th { get; set; }
        public int BoardandUniversityInfoId12th { get; set; }
        public int BoardandUniversityInfoIdUG { get; set; }
        public int BoardandUniversityInfoIdPG { get; set; }
        public int EvaluationType10th { get; set; }
        public int EvaluationType12th { get; set; }
        public int EvaluationTypeUG { get; set; }
        public int EvaluationTypePG { get; set; }
        public int StreamModel { get; set; }
        public int ResultStatusID { get; set; }


        public int FeeTypeId { get; set; }
        public int CourseDurationId { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public int SessionMasterId { get; set; }

        public string FeeTypeName { get; set; }
        public string CourseSessionName { get; set; }
        //public string SessionName { get; set; }
        public int CourseId { get; set; }
        //public string CourseName { get; set; }
        public List<ptaFeeTypeInfoViewModel> FeeTypeList { get; set; }
        public List<ptaFeeTypeInfoViewModel> SessionNameList { get; set; }
        public List<ptaFeeTypeInfoViewModel> CourseList { get; set; }

        public List<PTAEducationViewModel> EducationInfo { get; set; }
        public List<GetDDListInfoViewModel> GetBoardAndUniversityList { get; set; }
        public List<GetDDListInfoViewModel> GetEvaluationTypeList { get; set; }
        public List<GetDDListInfoViewModel> GetBoardAndUniversityListUG { get; set; }
        public List<GetDDListInfoViewModel> GetStream { get; set; }
        public PTAAddressInfoViewModel AddressInfo { get; set; }
        //public List<PTAEducationViewModel> EducationInfo { get; set; }
        public PTABoardandUniversityViewModel BoardandUniversityInfo { get; set; }
        public PTAUGBoardInfoViewModel UGBoardInfo { get; set; }
        public PTAStreamInfoViewModel StreamInfo { get; set; }
        public PTAEduEvaluationInfoViewModel EduEvaluationInfo { get; set; }
        public List<GetDDListInfoViewModel> Resultstatusinfo { get; set; }
        public PTARegistrationInfoViewModel RegistraionInfo { get; set; }
        public PTAEducationSubjectMarks SubjectMarks { get; set; }
        public List<CityCountryInfoViewModel> CountryList { get; set; }
        public List<CityCountryInfoViewModel> StateList { get; set; }
        public List<CityCountryInfoViewModel> DistrictList { get; set; }
        public List<CityCountryInfoViewModel> CityList { get; set; }
        public List<ptaDocumentsType> DocumentsUpload { get; set; }
        public List<ptaDocumentDetails> ptaDocumentDetails { get; set; }
        public ptaDocumentMaster ptaDocumentMaster { get; set; }

    }
    public class PTAAddressInfoViewModel
    {
        public int Id { get; set; }
        public string CorCountry { get; set; }
        public string CorState { get; set; }
        public string CorDistrict { get; set; }
        public string CorCity { get; set; }
        public string CorAddress1 { get; set; }
        public string CorAddress2 { get; set; }
        public string CorPin { get; set; }
        public bool IsPermanent { get; set; }
        public string PerCountry { get; set; }
        public string PerState { get; set; }
        public string PerDistrict { get; set; }
        public string PerCity { get; set; }
        public string PerAddress1 { get; set; }
        public string PerAddress2 { get; set; }
        public string PerPin { get; set; }
        public string ApplicationNo { get; set; }
    }

    public class PTAEducationViewModel
    {
        public int Id { get; set; }
        public string Class { get; set; }
        public string NameOfInstitute { get; set; }
        public int? ptaBoardAndUniversityMasterId { get; set; }
        public int? ptaUGBoardAndUniversityMasterId { get; set; }
        public int? ptaStreamMasterId { get; set; }
        public int? ptaDegreeMasterId { get; set; }
        public string YearOfPassing { get; set; }
        public int? ptaEduEvaluationTypeMasterId { get; set; }
        public double? PercentageOrCGPA { get; set; }
        public int? ptaResultStatusMasterId { get; set; }
        public int ptaRegistrationInfoId { get; set; }
        public List<PTAEducationSubjectMarks> SubjectMarks { get; set; }
    }

    public class PTABoardandUniversityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class PTAUGBoardInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class PTAStreamInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class PTAEduEvaluationInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class PTAResultStatusMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class PTARegistrationInfoViewModel
    {
        public int Id { get; set; }
        public string RegistrationNo { get; set; }
        public int ptaTitleMasterId { get; set; }
        public int ptaGenderMasterId { get; set; }
        public int ptaNationalMasterId { get; set; }
        public int ptaLeadSourceMasterId { get; set; }
        public int ptaMedicalStatusMasterId { get; set; }
        public int ptaPilotRegistraionMasterId { get; set; }
        public int ptaAdmiisionId { get; set; }
    }
    public class PTAEducationSubjectMarks
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public double? MaximumMarks { get; set; }
        public double? ObtainedMarks { get; set; }
        public double? Percentage { get; set; }
        public int PtaEducationDetailsId { get; set; }
    }
    public class PtaCountryCityInfo
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
    }
    public class ptaDocumentsType
    {
        public int id { get; set; }
        public string DocumentType { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
    }
    public class ptaDocumentDetails
    {
        public int Id { get; set; }
        public int DocumentMasterId { get; set; }
        public string DocumentPath { get; set; }
        public string Extension { get; set; }
        public bool IsActive { get; set; }
        public int EnteredBy { get; set; }
        public DateTime EneteredDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int ptaPilotRegistrationMasterId { get; set; }
        public int ptaAdmissionMasterId { get; set; }
        public string ApplicationNo { get; set; }
        public int? EnteredStudentBy { get; set; }
        public int? UpdatedStudentBy { get; set; }
        public string DocumentName { get; set; }
    }

    public class ptaDocumentMaster
    {
        public int id { get; set; }
        public string DocumentName { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentMasterId { get; set; }
    }

    public class CourseMasterViewModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
    public class SessionMasterViewModel
    {
        public int Id { get; set; }
        public string SessionName { get; set; }
        public bool IsActive { get; set; }
        public int CourseId { get; set; }
    }
    public class ptaFeeType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

}
