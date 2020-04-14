using SJData;
using SJModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJService.PTA
{
    public class FeeCollectionService
    {
        SJStarERPEntities _context = null;
        public FeeCollectionService()
        {
            _context = new SJStarERPEntities();
        }

        public List<RoleViewModel> GetFeeTypeList()
        {
            return _context.ptaFeeTypes.Where(f => f.IsActive).Select(i => new RoleViewModel
            {
                Id = i.Id,
                Name = i.Name
            }).ToList();
        }

        public List<RoleViewModel> GetSessionList()
        {
            return _context.SessionMasters.Where(f => f.IsActive && f.CourseMaster.DepartmentMasterId == 2).Select(i => new RoleViewModel
            {
                Id = i.Id,
                Name = i.SessionName
            }).ToList();
        }

        public List<RoleViewModel> GetCourseList()
        {
            return _context.CourseMasters.Where(f => f.IsActive && f.DepartmentMasterId == 2).Select(i => new RoleViewModel
            {
                Id = i.Id,
                Name = i.CourseName
            }).ToList();
        }

        public DataTableFilterModel GetFeeTypeDetailList(DataTableFilterModel filter)
        {
            var data = _context.ptaFeeTypeDetails
               .Select(a => new FeeTypeDetailViewModel
               {
                   Id = a.Id,
                   FeeTypeName = a.ptaFeeType.Name,
                   Amount = a.Amount,
                   SessionName = a.SessionMaster.SessionName,
                   CourseName = a.CourseMaster.CourseName,
                   IsActive = a.IsActive
               }).AsEnumerable();

            var totalCount = data.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                data = data.Where(d => (d.FeeTypeName.ToLower().Contains(filter.search.value.ToLower()) || d.SessionName.ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.CourseName) && d.CourseName.ToLower().Contains(filter.search.value.ToLower()))));
            }

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
            filter.data = dataFilter;
            return filter;
        }

        public IEnumerable<RoleViewModel> GetSessionListByCourseId(int CourseId)
        {
            return _context.SessionMasters.Where(s => s.IsActive && s.CourseMaster.DepartmentMasterId == 2 && s.CourseMasterId == CourseId)
                .Select(item => new RoleViewModel
                {
                    Id = item.Id,
                    Name = item.SessionName
                }).AsEnumerable();
        }

        public string AddUpdateFeeTypeDetail(FeeTypeDetailViewModel model)
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
                obj.Amount = model.Amount.Value;
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

        public FeeTypeDetailViewModel GetFeeTypeDetailById(int FeeTypeDetailId)
        {
            return _context.ptaFeeTypeDetails.Where(f => f.Id == FeeTypeDetailId).Select(t => new FeeTypeDetailViewModel
            {
                Id = t.Id,
                Amount = t.Amount,
                FeeTypeId = t.PTAFeeTypeId,
                IsActive = t.IsActive,
                SessionMasterId = t.PTASessionMasterId,
                CourseId = t.PTACourseMasterId,
                CourseName = t.CourseMaster.CourseName,
                CourseSessionName = t.SessionMaster.SessionName,
                FeeTypeName = t.ptaFeeType.Name,
                SessionName = t.SessionMaster.SessionName
            }).FirstOrDefault();
        }

        public FeePaymentDetailViewModel FeeCollectionPaymentDetail(int AdmissionId)
        {
            var result = _context.ptaRegistrationInfoes.Where(p => p.ptaAdmissionMaster.IsActive && p.ptaAdmissionMaster.Id == AdmissionId).Select(o => new FeePaymentDetailViewModel
            {
                RegistrationInfo = new RegistrationViewModel
                {
                    RegistrationNoStr = o.RegistartionNo,
                    Id = o.ptaPilotRegistrationMasterId,
                    AddMissionId = o.AdmissionId,
                    Fname = o.ptaAdmissionMaster.Fname,
                    Lname = o.ptaAdmissionMaster.Lanme,
                    CourseId = o.ptaAdmissionMaster.CourseId,
                    SessionId = o.ptaAdmissionMaster.SessionId,
                    Email = o.ptaAdmissionMaster.Email,
                    Mobile = o.ptaAdmissionMaster.Mobile,
                    Gender = o.ptaGenderMaster.Name,
                    CourseName = o.ptaAdmissionMaster.CourseMaster.CourseName,
                    SessionName = o.ptaAdmissionMaster.SessionMaster.SessionName,
                    AmityEnNo = o.ptaAdmissionMaster.ApplicationNo,
                },
                PaymentTypeList = _context.ptaPaymentTypes.Where(p => p.IsActive).Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                }).AsEnumerable(),
                PaymentModeList = _context.ptaPaymentModes.Where(p => p.IsActive).Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                }).AsEnumerable(),
                FeeDetail = new FeeDetailViewModel
                {
                    Id = o.ptaAdmissionMaster.ptaFeeDetails.Any() ? o.ptaAdmissionMaster.ptaFeeDetails.FirstOrDefault().Id : 0
                },
                FeeTypeDetailList = _context.ptaFeeTypeDetails.
                    Where(ft => ft.IsActive && ft.PTASessionMasterId == o.ptaAdmissionMaster.SessionId && ft.PTACourseMasterId == o.ptaAdmissionMaster.CourseId)
                    .Select(item => new RoleViewModel
                    {
                        Id = item.Id,
                        Name = item.ptaFeeType.Name
                    }).AsEnumerable(),
            }).FirstOrDefault();
            return result;
        }

        public Nullable<decimal> GetAmountByFeeTypeDetail(int FeeTypeDetailId)
        {
            decimal Amount = 0;
            var feeTypeDetails = _context.ptaFeeTypeDetails.Where(c => c.IsActive && c.Id == FeeTypeDetailId).SingleOrDefault();
            if (feeTypeDetails != null)
                Amount = feeTypeDetails.Amount;
            return Amount;
        }

        public FeePaymentViewModel IsAdmissionFeePay(int AdmissionId, int FeeTypeDetailId, int SessionMasterId, int CourseId)
        {
            FeePaymentViewModel result = new FeePaymentViewModel();
            var data = _context
                .ptaFeeCollections
                .Where(f => f.ptaFeePaymentDetail.ptaFeeDetail.ptaFeeTypeDetail.Id == FeeTypeDetailId
                && f.ptaFeePaymentDetail.ptaFeeDetail.PTAAdmissionMasterId == AdmissionId
                && f.ptaFeePaymentDetail.ptaFeeDetail.ptaFeeTypeDetail.PTASessionMasterId == SessionMasterId
                && f.ptaFeePaymentDetail.ptaFeeDetail.ptaFeeTypeDetail.PTACourseMasterId == CourseId
                && f.IsActive && f.ptaFeePaymentDetail.IsActive && f.ptaFeePaymentDetail.ptaFeeDetail.IsActive
                && f.ptaFeePaymentDetail.ptaFeeDetail.ptaFeeTypeDetail.IsActive).Select(item => new FeeDetailViewModel
                {
                    IsAdmissionFeePay = item.ptaFeePaymentDetail.ptaFeeDetail.ptaFeeTypeDetail.PTAFeeTypeId == 1 ? true : false,
                    PaymentTypeId = item.ptaFeePaymentDetail.ptaFeeDetail.PTAPaymentTypeId
                }).FirstOrDefault();
            result.FeeInfoDetail = data;
            return result;
        }

        public List<AddFeeCollectionViewModel> GetDepositFeeByCourseYr(int AdmissionId, int FeeTypeDetailId, int SessionMasterId, int CourseId)
        {
            return _context
                .ptaFeeCollections
                .Where(f => f.ptaFeePaymentDetail.ptaFeeDetail.ptaFeeTypeDetail.Id == FeeTypeDetailId
                && f.ptaFeePaymentDetail.ptaFeeDetail.PTAAdmissionMasterId == AdmissionId
                && f.ptaFeePaymentDetail.ptaFeeDetail.ptaFeeTypeDetail.PTASessionMasterId == SessionMasterId
                && f.ptaFeePaymentDetail.ptaFeeDetail.ptaFeeTypeDetail.PTACourseMasterId == CourseId
                && f.IsActive && f.ptaFeePaymentDetail.IsActive && f.ptaFeePaymentDetail.ptaFeeDetail.IsActive
                && f.ptaFeePaymentDetail.ptaFeeDetail.ptaFeeTypeDetail.IsActive)
                .Select(item => new AddFeeCollectionViewModel
                {
                    PanNumber = item.PanNumber,
                    Amount = item.Amount
                }).ToList();
        }

        public List<InstallmentMasterViewModel> GetFeeCollectionRecordByRecieptNo(string RecieptNo)
        {
            return _context.ptaInstallmentMasters.Where(i => i.IsActive).Select(item => new InstallmentMasterViewModel
            {
                Id = item.Id,
                Name = item.Name,
                IsFeeCollection = _context.ptaFeeCollections.Any(f => f.ptaFeePaymentDetail.RecieptNo == RecieptNo && f.PTAInstallmentMasterId == item.Id)
            }).ToList();
        }
    }
}
