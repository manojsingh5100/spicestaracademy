using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJModel;
using SJData;
using System.Globalization;
using Newtonsoft.Json;

namespace SJService
{
    public class FeeManagementService
    {
        SJStarERPEntities _context = null;
        public FeeManagementService()
        {
            _context = new SJStarERPEntities();
        }
        public bool UpdateRegistrationFee(int Id, string PaymentId, decimal Fee, string MOP)
        {
            bool status = false;
            var data = _context.RegistrationMasters.Where(r => r.IsActive && r.Id == Id).FirstOrDefault();
            if (data != null)
            {
                data.PaymentDate = DateTime.Now;
                data.PaymentId = PaymentId;
                data.PaymentStatus = true;
                data.ModOfPayment = MOP;
                data.IsConsultantCandidate = false;
                data.IsHRCandidate = false;
                _context.SaveChanges();
                status = true;
            }
            return status;
        }

        public List<RoleViewModel> GetFeeTypeList()
        {
            return _context.FeeTypes.Where(f => f.IsActive).Select(i => new RoleViewModel
            {
                Id = i.Id,
                Name = i.Name
            }).ToList();
        }


        public List<RoleViewModel> GetSessionList()
        {
            return _context.SessionMasters.Where(f => f.IsActive).Select(i => new RoleViewModel
            {
                Id = i.Id,
                Name = i.SessionName
            }).ToList();
        }

        public List<RoleViewModel> GetCourseList()
        {
            return _context.CourseMasters.Where(f => f.IsActive).Select(i => new RoleViewModel
            {
                Id = i.Id,
                Name = i.CourseName
            }).ToList();
        }

        public string AddUpdateFeeTypeDetail(FeeTypeDetailViewModel model)
        {
            string Message = "";
            try
            {
                FeeTypeDetail obj;
                //=============User Activity ========================
                LogActivityViewModel log = new LogActivityViewModel();
                log.EnteredBy = model.EnteredBy;
                log.EnteredDate = DateTime.Now;
                log.ActioName = "AddUpdateFeeTypeDetail";
                log.ModuleName = "Add fee type";
                log.ControllerName = "FeeManagement";
                //===================================================
                if (model.Id > 0)
                {
                    obj = _context.FeeTypeDetails.Where(f => f.Id == model.Id).FirstOrDefault();
                    Message = "Successfully Updated!";
                    log.Activity = "Updated";
                    log.ActivityMessage = "Add fee type is updated for";
                }
                else
                {
                    var data = _context.FeeTypeDetails
                    .Where(f => f.CourseId == model.CourseId
                    && f.SessionMasterId == model.SessionMasterId
                    && f.FeeTypeId == model.FeeTypeId).FirstOrDefault();
                    if (data != null)
                    {
                        Message = "This FeeType Detail record already exist!";
                        return Message;
                    }
                    obj = new FeeTypeDetail();
                    Message = "Successfully Inserted!";
                    log.Activity = "Updated";
                    log.ActivityMessage = "Add fee type is updated for";
                }
                obj.CourseId = model.CourseId;
                obj.Amount = model.Amount;
                obj.FeeTypeId = model.FeeTypeId;
                obj.IsActive = model.IsActive;
                obj.SessionMasterId = model.SessionMasterId;
                if (model.Id == 0)
                    _context.FeeTypeDetails.Add(obj);
                _context.SaveChanges();
                return Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public AddmissionMasterViewModel GetFeeDataByRegId(int RegNo)
        {
            var data = _context.FIN_FeeReceiptMaster.Where(f => f.RegNo == RegNo).Select(d => new AddmissionMasterViewModel
            {
                Id = d.Id,
                Remark = d.Remarks,
                ModOfPayment = d.PaymentMode,
                FeePaymentList = d.FIN_FeeReceiptDetail.Select(r => new FeePaymentViewModel
                {
                    FeeName = r.FeeName,
                    Id = r.Id,
                    Fee = r.FeeAmount.Value,
                    PaymentMode = r.ModOfPayment
                }).ToList(),

            }).FirstOrDefault();
            return data;
        }

        public AddmissionMasterViewModel GetRefundedFeeDataByRegId(int RegNo)
        {
            var data = _context.FIN_FeeRefundMaster.Where(f => f.RegNo == RegNo).Select(d => new AddmissionMasterViewModel
            {
                Id = d.Id,
                Remark = d.Remarks,
                FeePaymentList = d.FIN_FeeRefundDetail.Select(r => new FeePaymentViewModel
                {
                    FeeName = r.FeeName,
                    Id = r.Id,
                    Fee = r.FeeAmount.Value
                }).ToList()
            }).FirstOrDefault();
            return data;
        }

        public DataTableFilterModel GetRefundDetailsList(DataTableFilterModel filter)
        {
            //var data = _context.ExceededFeeAmountOnCourseChanges.Select(item => new FeeRefundViewModel
            //{
            //    Id = item.Id,
            //    RegNo = item.AddmissionMaster.RegistrationNo,
            //    Amount = item.ExceedAmount,
            //    Course = item.AddmissionMaster.CourseMaster.CourseName,
            //    Email = item.AddmissionMaster.Email,
            //    Fname = item.AddmissionMaster.Fname,
            //    Lname = item.AddmissionMaster.Lname,
            //    Mobile = item.AddmissionMaster.MobileNo,
            //    Status = item.RefundInformations.Any() ? item.RefundInformations.FirstOrDefault().Status : "Pending",
            //    AdmissionId = item.AdmissionId,
            //    Remark = item.RefundInformations.Any() ? item.RefundInformations.FirstOrDefault().StatusLine : "",
            //}).AsEnumerable();
            var data = _context.RefundInformations.Select(item => new FeeRefundViewModel
            {
                Id = item.Id,
                RegNo = item.AddmissionMaster.RegistrationNo,
                Amount = item.ExceededFeeAmountOnCourseChange == null ? 0 : item.ExceededFeeAmountOnCourseChange.ExceedAmount,
                Course = item.AddmissionMaster.CourseMaster.CourseName,
                Fname = item.AddmissionMaster.Fname,
                Lname = item.AddmissionMaster.Lname,
                Mobile = item.AddmissionMaster.MobileNo,
                AdmissionId = item.AddmissionMaster.Id,
                Status = item.Status != "" ? item.Status : "Pending",
                Remark = item.StatusLine
            }).AsEnumerable();

            var totalCount = data.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                data = data.Where(d => d.RegNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Mobile) && d.Mobile.ToString().ToLower().Contains(filter.search.value.ToLower())));
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

        public DataTableFilterModel GetFeeTypeDetailList(DataTableFilterModel filter)
        {
            var data = _context.FeeTypeDetails
               .Select(a => new FeeTypeDetailViewModel
               {
                   Id = a.Id,
                   FeeTypeName = a.FeeType.Name,
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

        public FeeTypeDetailViewModel GetFeeTypeDetailById(int FeeTypeDetailId)
        {
            return _context.FeeTypeDetails.Where(f => f.Id == FeeTypeDetailId).Select(t => new FeeTypeDetailViewModel
            {
                Id = t.Id,
                Amount = t.Amount,
                FeeTypeId = t.FeeTypeId,
                IsActive = t.IsActive,
                SessionMasterId = t.SessionMasterId,
                CourseId = t.CourseId,
                CourseName = t.CourseMaster.CourseName,
                CourseSessionName = t.SessionMaster.SessionName,
                FeeTypeName = t.FeeType.Name,
                SessionName = t.SessionMaster.SessionName
            }).FirstOrDefault();
        }

        public Nullable<decimal> GetAmountByFeeTypeDetail(int FeeTypeDetailId)
        {
            decimal Amount = 0;
            var feeTypeDetails = _context.FeeTypeDetails.Where(c => c.IsActive && c.Id == FeeTypeDetailId).SingleOrDefault();
            if (feeTypeDetails != null)
                Amount = feeTypeDetails.Amount.Value;
            return Amount;
        }

        public CheckBalanceViewModel CapitalAndPiecePaymentBalance(int RegNo, int PaymentTypeId, int FeeTypeDetailId, int? InstallmentId)
        {
            CheckBalanceViewModel Model = new CheckBalanceViewModel();
            bool status = true;
            var feeCollectionData = _context.FeeCollections.Where(f => f.IsActive && f.FeePaymentDetail.FeeDetail.RegistrationNo == RegNo && f.FeePaymentDetail.FeeDetail.IsActive);
            if (InstallmentId.HasValue)
                feeCollectionData = feeCollectionData.Where(fc => fc.InstallmentMasterId == InstallmentId);
            if (feeCollectionData.FirstOrDefault() != null)
            {
                var parameterList = _context.PartWisePayments.Where(b => b.FeeCollectionId == feeCollectionData.FirstOrDefault().Id && b.FeeCollection.FeePaymentDetail.FeeDetail.IsActive).ToList();
                decimal BankAmount = parameterList.Count > 0 ? parameterList.Sum(s => s.Amount).Value : 0;
                decimal feeCollect = feeCollectionData.FirstOrDefault().Amount;
                if (BankAmount > 0)
                {
                    if (BankAmount < feeCollect)
                    {
                        status = false;
                        Model.PartWisePaymentList = parameterList.Select(item => new BankAndAmountDetailViewModel
                        {
                            Amount = item.Amount.HasValue ? item.Amount.Value : 0,
                            FeeCollectionId = item.FeeCollectionId
                        }).ToList();
                        Model.PaidAmount = feeCollect;
                    }
                    Model.RemainderPartWiseAmount = feeCollect - BankAmount;
                }
            }
            Model.BalanceStatus = status;
            return Model;
        }

        public bool SaveFeeCollectionAndPartWisePayment(FeePaymentDetailViewModel Model)
        {
            try
            {
                PartWisePayment payment = new PartWisePayment
                {
                    Amount = Model.BankDetail.Amount,
                    FeeCollectionId = Model.hdnFeeDetailId,
                    RecieptNo = "RCFC#" + (_context.PartWisePayments.ToList().Count() + 1),
                    PaymentModeId = Model.PaymentModeId,
                    EnteredBy = Model.EnteredBy,
                    EnteredDate = Model.EnteredDate
                };
                _context.PartWisePayments.Add(payment);

                BankAndAmountDetail bankDetail = new BankAndAmountDetail();
                if (Model.FeePayModeName != "" && (Model.FeePayModeName == "DD" || Model.FeePayModeName == "RTGS"))
                {
                    bankDetail.DDRTGSNo = Model.BankDetail.DDRTGSNo;
                    bankDetail.EnteredDate = Model.EnteredDate;
                    bankDetail.EnteredBy = Model.EnteredBy;
                    bankDetail.PartWisePaymentId = payment.Id;
                    _context.BankAndAmountDetails.Add(bankDetail);
                }

                var registerData = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == Model.FeeDetail.RegistrationNo).FirstOrDefault();
                if (registerData != null && Model.FeeName == "Admission")
                    registerData.IsFeePayStandBy = false;
                _context.SaveChanges();
                if (Model.FeeName == "Admission")
                    IsFeePaymentStandBy(false, Model.FeeDetail.RegistrationNo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveFeeCollectionAndPayment(FeePaymentDetailViewModel model)
        {
            try
            {
                FeeDetail feeDetail = new FeeDetail();
                if (model.FeeDetail.Id == 0)
                {
                    feeDetail.PaymentTypeId = model.FeeDetail.PaymentTypeId;
                    feeDetail.FeeTypeDetailId = model.FeeDetail.FeeTypeDetailId;
                    feeDetail.RegistrationMasterId = model.FeeDetail.RegistrationMasterId;
                    feeDetail.RegistrationNo = model.FeeDetail.RegistrationNo;
                    feeDetail.IsActive = true;
                    feeDetail.EnteredBy = model.EnteredBy;
                    feeDetail.EnteredDate = model.EnteredDate;
                    _context.FeeDetails.Add(feeDetail);
                }

                FeePaymentDetail feePaymentDetail = new FeePaymentDetail
                {
                    FeeDetailId = model.FeeDetail.Id == 0 ? feeDetail.Id : model.FeeDetail.Id,
                    PaymentModeId = model.PaymentModeId,
                    TransectionId = model.TransectionId,
                    FIN_BankDetailId = (int?)null,
                    RecieptNo = model.RecieptNo,
                    IsActive = true,
                    EnteredBy = model.EnteredBy,
                    EnteredDate = model.EnteredDate
                };
                _context.FeePaymentDetails.Add(feePaymentDetail);

                FeeCollection feeCollection = new FeeCollection
                {
                    FeePaymentDetailId = feePaymentDetail.Id,
                    Amount = model.FeeCollection.Amount,
                    EnteredBy = model.EnteredBy,
                    EnteredDate = model.EnteredDate,
                    IsActive = true,
                    PanNumber = model.FeeCollection.PanNumber,
                    InstallmentMasterId = model.FeeCollection.Installment == 0 ? (int?)null : model.FeeCollection.Installment,
                    RecieptNo = "RC" + model.FeeDetail.RegistrationNo + "#" + (_context.FeeCollections.ToList().Count() + 1)
                };
                _context.FeeCollections.Add(feeCollection);


                PartWisePayment payment = new PartWisePayment
                {
                    Amount = model.BankDetail.Amount,
                    FeeCollectionId = feeCollection.Id,
                    RecieptNo = "RCFC#" + (_context.PartWisePayments.ToList().Count() + 1),
                    PaymentModeId = model.PaymentModeId,
                    EnteredBy = model.EnteredBy,
                    EnteredDate = model.EnteredDate
                };
                _context.PartWisePayments.Add(payment);


                BankAndAmountDetail bankDetail = new BankAndAmountDetail();
                if (model.FeePayModeName != "" && (model.FeePayModeName == "DD" || model.FeePayModeName == "RTGS"))
                {
                    bankDetail.DDRTGSNo = model.BankDetail.DDRTGSNo;
                    bankDetail.EnteredDate = model.EnteredDate;
                    bankDetail.EnteredBy = model.EnteredBy;
                    bankDetail.PartWisePaymentId = payment.Id;
                    _context.BankAndAmountDetails.Add(bankDetail);
                }

                var registerData = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == model.FeeDetail.RegistrationNo).FirstOrDefault();
                if (registerData != null && model.FeeName == "Admission")
                    registerData.IsFeePayStandBy = false;
                _context.SaveChanges();
                if (model.FeeName == "Admission")
                    IsFeePaymentStandBy(false, model.FeeDetail.RegistrationNo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public FeePaymentViewModel IsAdmissionFeePay(int RegistrationNo, int FeeTypeDetailId, int SessionMasterId, int CourseId)
        {
            FeePaymentViewModel result = new FeePaymentViewModel();
            var data = _context
                .FeeCollections
                .Where(f => f.FeePaymentDetail.FeeDetail.FeeTypeDetail.Id == FeeTypeDetailId
                && f.FeePaymentDetail.FeeDetail.RegistrationNo == RegistrationNo
                && f.FeePaymentDetail.FeeDetail.FeeTypeDetail.SessionMasterId == SessionMasterId
                && f.FeePaymentDetail.FeeDetail.FeeTypeDetail.CourseId == CourseId
                && f.IsActive && f.FeePaymentDetail.IsActive && f.FeePaymentDetail.FeeDetail.IsActive
                && f.FeePaymentDetail.FeeDetail.FeeTypeDetail.IsActive).Select(item => new FeeDetailViewModel
                {
                    IsAdmissionFeePay = item.FeePaymentDetail.FeeDetail.FeeTypeDetail.FeeTypeId == 1 ? true : false,
                    PaymentTypeId = item.FeePaymentDetail.FeeDetail.PaymentTypeId
                }).FirstOrDefault();
            result.FeeInfoDetail = data;
            return result;
        }

        public List<InstallmentMasterViewModel> GetFeeCollectionRecordByRecieptNo(string RecieptNo, int SessionYr, int CourseId, int FeeDetailId, DateTime? BatchStartTime)
        {
            var data = _context.SessionInstallmentMasters.Where(w => w.FeeTypeDetail.SessionMaster.SessionYr == SessionYr && w.FeeTypeDetail.CourseId == CourseId).Select(item => new InstallmentMasterViewModel
            {
                Id = item.InstallmentMaster.Id,
                Name = item.InstallmentMaster.Name,
                InstallMentId = item.InstallmentMasterId,
                Amount = item.Amount
            }).ToList();

            if (data.Count > 0)
            {
                DateTime Today = DateTime.Now;
                int AddMonth = 0;
                foreach (var item in data)
                {
                    var collection = _context.FeeCollections.Where(f => f.FeePaymentDetail.RecieptNo == RecieptNo && f.InstallmentMasterId == item.InstallMentId).FirstOrDefault();
                    if (collection != null)
                    {
                        decimal? amt = collection.PartWisePayments.Sum(s => s.Amount);
                        if (amt.HasValue)
                        {
                            if (amt.Value < item.Amount)
                            {
                                item.TakeFeeInstallmentStatus = true;
                                item.IsFeeCollection = true;
                                break;
                            }
                            else if (amt.Value >= item.Amount)
                            {
                                item.IsFeeCollection = true;
                            }
                        }
                    }
                    else
                    {
                        if (FeeDetailId == 0)
                        {
                            item.IsFeeCollection = true;
                            item.Temp = true;
                            item.TakeFeeInstallmentStatus = true;
                            break;
                        }

                        if (BatchStartTime.HasValue && Today.Date >= BatchStartTime.Value.AddMonths(AddMonth).Date)
                        {
                            item.TakeFeeInstallmentStatus = true;
                            item.IsFeeCollection = true;
                            if (collection != null && collection.PartWisePayments.Count > 0)
                                item.Temp = false;
                            else
                                item.Temp = true;
                            break;
                        }
                    }
                    AddMonth += 11;
                }
            }
            return data;
        }

        public string GetInstallmentInfo(int SessionYr, int CourseId)
        {
            string abc = "";
            var data = _context.SessionInstallmentMasters.Where(f => f.FeeTypeDetail.SessionMaster.SessionYr == SessionYr && f.FeeTypeDetail.CourseId == CourseId).Select(item => new
            {
                Installment = item.InstallmentMasterId,
                Amount = item.Amount
            }).OrderBy(o => o.Installment).ToList();
            if (data.Count() > 0)
                abc = JsonConvert.SerializeObject(data);
            return abc;
        }

        public List<AddFeeCollectionViewModel> GetDepositFeeByCourseYr(int RegistrationNo, int FeeTypeDetailId, int SessionMasterId, int CourseId)
        {
            return _context
                .FeeCollections
                .Where(f => f.FeePaymentDetail.FeeDetail.FeeTypeDetail.Id == FeeTypeDetailId
                && f.FeePaymentDetail.FeeDetail.RegistrationNo == RegistrationNo
                && f.FeePaymentDetail.FeeDetail.FeeTypeDetail.SessionMasterId == SessionMasterId
                && f.FeePaymentDetail.FeeDetail.FeeTypeDetail.CourseId == CourseId
                && f.IsActive && f.FeePaymentDetail.IsActive && f.FeePaymentDetail.FeeDetail.IsActive
                && f.FeePaymentDetail.FeeDetail.FeeTypeDetail.IsActive)
                .Select(item => new AddFeeCollectionViewModel
                {
                    PanNumber = item.PanNumber,
                    Amount = item.Amount
                }).ToList();
        }

        public IEnumerable<RoleViewModel> GetSessionListByCourseId(int CourseId)
        {
            return _context.SessionMasters.Where(s => s.IsActive && s.CourseMasterId == CourseId)
                .Select(item => new RoleViewModel
                {
                    Id = item.Id,
                    Name = item.SessionName
                }).AsEnumerable();
        }

        public FeePaymentDetailViewModel FeeCollectionPaymentDetail(int RegistrationId)
        {
            var resultInfo = _context.RegistrationMasters
                .GroupJoin(_context.AddmissionMasters,
                r => r.RegistartionNo, a => a.RegistrationNo
                , (r, a) => new { R = r, A = a.DefaultIfEmpty() })
                .SelectMany(obj => obj.A.Select(result => new { registration = obj.R, admission = result }))
                .Where(c => c.registration.IsActive && c.registration.RegistartionNo.Equals(RegistrationId))
                .Select(o => new FeePaymentDetailViewModel
                {
                    RegistrationInfo = new RegistrationViewModel
                    {
                        RegistartionNo = o.registration.RegistartionNo,
                        Id = o.registration.Id,
                        AddMissionId = o.admission != null ? o.admission.Id : (int?)null,
                        Fname = o.registration.Fname,
                        Lname = o.registration.Lname,
                        CourseId = o.registration.CourseId,
                        SessionId = o.registration.SessionId,
                        Email = o.registration.Email,
                        Mobile = o.registration.Mobile,
                        Gender = o.registration.Gender,
                        CourseName = o.registration.CourseMaster.CourseName,
                        SessionName = o.registration.SessionMaster.SessionName,
                        AmityEnNo = o.admission.AmityEnrollMentId,
                        IsFeePayStandBy = o.registration.IsFeePayStandBy,
                        SessionYr = o.registration.SessionMaster.SessionYr,
                        BatchCreationDate = o.admission.AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart,
                        IsAnyCourseFeePay = _context.FeeDetails.Where(f => f.RegistrationNo == RegistrationId).Any(),
                        ExceedPaymentRefundId = o.admission.ExceededFeeAmountOnCourseChanges.Any() ? o.admission.ExceededFeeAmountOnCourseChanges.FirstOrDefault().Id : 0,
                        RefundStatus = o.admission.ExceededFeeAmountOnCourseChanges.Any() ? (o.admission.ExceededFeeAmountOnCourseChanges.FirstOrDefault().RefundInformations.Any() ? o.admission.ExceededFeeAmountOnCourseChanges.FirstOrDefault().RefundInformations.FirstOrDefault().Status : "Pending") : "",
                        RefundRemark = o.admission.RefundInformations.Any() ? o.admission.RefundInformations.FirstOrDefault().Status : ""
                    },
                    PaymentTypeList = _context.PaymentTypes.Where(p => p.IsActive).Select(r => new RoleViewModel
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).AsEnumerable(),
                    PaymentModeList = _context.PaymentModes.Where(p => p.IsActive).Select(r => new RoleViewModel
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).AsEnumerable(),
                    FeeDetail = new FeeDetailViewModel
                    {
                        Id = _context.FeeDetails.Any(f => f.RegistrationNo == RegistrationId && f.IsActive) ? _context.FeeDetails.Where(f => f.RegistrationNo == RegistrationId && f.IsActive).FirstOrDefault().Id : 0
                    },
                    FeeTypeDetailList = _context.FeeTypeDetails.
                    Where(ft => ft.IsActive && ft.SessionMasterId == o.registration.SessionId && ft.CourseId == o.registration.CourseId)
                    .Select(item => new RoleViewModel
                    {
                        Id = item.Id,
                        Name = item.FeeType.Name
                    }).AsEnumerable()
                }).FirstOrDefault();
            if (resultInfo != null)
                resultInfo.SessionInstallmentJson = GetInstallmentInfo(resultInfo.RegistrationInfo.SessionYr.Value, resultInfo.RegistrationInfo.CourseId.Value);
            resultInfo.GetNotification = GetNotificatioInfo(RegistrationId);
            return resultInfo;
        }

        public bool IsFeePaymentStandBy(bool IsFeePayStandBy, int RegNo)
        {
            bool Status = false;
            var Result = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == RegNo).FirstOrDefault();
            if (Result != null)
            {
                Result.IsFeePayStandBy = IsFeePayStandBy;
                _context.SaveChanges();
                Status = true;
            }
            return Status;
        }

        public dynamic GetEmailNotificatioInfo(int BatchId)
        {
            var GetResultInfo = (from am in _context.AddmissionMasters
                                 join f in _context.FeeDetails
                                 on am.RegistrationNo equals f.RegistrationNo
                                 into G
                                 from obj in G.DefaultIfEmpty()
                                 where am.IsActive && am.AddmissionDetails.FirstOrDefault().BatchId == BatchId
                                 && obj.FeeTypeDetail.FeeType.Name == "Admission"
                                 select new
                                 {
                                     Id = am.Id,
                                     RegNo = am.RegistrationNo,
                                     FName = am.Fname,
                                     LName = am.Lname,
                                     RegisterEmail = am.RegisterEmail,
                                     Mobile = am.MobileNo,
                                     CourseName = am.CourseMaster.CourseName,
                                     AdmissionDate = am.AddmissionDate,
                                     BatchName = am.AddmissionDetails.FirstOrDefault().BatchMaster.Name,
                                     BtachId = am.AddmissionDetails.FirstOrDefault().BatchId,
                                     BatchCreatedDate = am.AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart,
                                     SessionName = am.AddmissionDetails.FirstOrDefault().SessionMaster.SessionName,
                                     SessionYr = am.AddmissionDetails.FirstOrDefault().SessionMaster.SessionYr,
                                     FeeDetailInfo = new
                                     {
                                         CapitalAmount = obj.FeeTypeDetail.Amount,
                                         FeeCollectionInfo = obj.FeePaymentDetails.FirstOrDefault().FeeCollections.Select(fc => new
                                         {
                                             InstallmentName = fc.InstallmentMaster.Name,
                                             Amount = fc.Amount,
                                             PartwiseInfo = fc.PartWisePayments.Select(pp => new
                                             {
                                                 Amount = pp.Amount,
                                                 PaymentName = pp.PaymentMode.Name
                                             }).ToList()
                                         }).ToList()
                                     }
                                 }).ToList();

            if (GetResultInfo.Count > 0)
            {
                foreach (var item in GetResultInfo)
                {
                    if (item.FeeDetailInfo != null)
                    {
                        foreach (var col in item.FeeDetailInfo.FeeCollectionInfo)
                        {
                        }
                    }
                }
            }
            return GetResultInfo;
        }

        public GetNotificationViewModel GetNotificatioInfo(int Id)
        {
            if (_context.FeeDetails.Where(f => f.IsActive && f.RegistrationNo == Id).Any())
            {
                decimal paidFee = 0;
                GetNotificationViewModel model = new GetNotificationViewModel();
                DateTime CurrentYr = DateTime.Now;
                var info = (from r in _context.RegistrationMasters
                            join f in _context.FeeCollections
                            on r.RegistartionNo equals f.FeePaymentDetail.FeeDetail.RegistrationNo into G
                            from obj in G.DefaultIfEmpty()
                            where r.RegistartionNo == Id && r.IsActive && obj.FeePaymentDetail.FeeDetail.IsActive
                            select new
                            {
                                Register = r,
                                collection = obj,
                                partwiseList = obj.PartWisePayments
                            }).ToList();
                model.NoOfCollection = info.Count();
                List<int> ArrSessionYr = new List<int>();
                var sessionId = info.FirstOrDefault().Register.SessionId;
                var courseId = info.FirstOrDefault().Register.CourseId;
                var feeTypeDetail = _context.FeeTypeDetails.
                Where(f => f.FeeType.Name == "Admission" && f.SessionMasterId == sessionId && f.CourseId == courseId).FirstOrDefault();
                decimal capitalAmout = feeTypeDetail.Amount.HasValue ? feeTypeDetail.Amount.Value : 0;

                var data = info.Where(i => i.collection != null && i.collection.FeePaymentDetail.FeeDetail.FeeTypeDetail.FeeType.Name == "Admission").ToList();

                if (data.Count > 0)
                {
                    foreach (var a in data)
                    {
                        if (a.partwiseList.Count > 0)
                            paidFee += a.partwiseList.Where(w => w.Amount.HasValue).Sum(p => p.Amount.Value);
                        else
                            paidFee += a.collection.Amount;
                    }
                    // paidFee = data.Sum(s => s.collection.Amount);
                }

                if (feeTypeDetail != null)
                {
                    if (paidFee == 0)
                        model.DueAdmissionFee = "Due admission fee : " + capitalAmout + " INR."; // capitalAmout
                    else if (paidFee == capitalAmout)
                        model.DueAdmissionFee = "Admission fee has no due.";// 
                    else if (paidFee > 0)
                        model.DueAdmissionFee = "Due admission fee : " + (capitalAmout - paidFee) + " INR.";
                }

                if (info != null && info.Count > 0)
                {
                    var SessionName = info.FirstOrDefault().Register.SessionMaster.SessionName;
                    if (SessionName != "")
                    {
                        string[] ArrSessionPrePostYr = SessionName.Split('-');
                        if (ArrSessionPrePostYr.Length > 0)
                        {
                            for (int i = Convert.ToInt32(ArrSessionPrePostYr[0]); i < Convert.ToInt32(ArrSessionPrePostYr[1]); i++)
                                ArrSessionYr.Add(i);
                        }
                    }
                    if (ArrSessionYr.Count > 0)
                    {
                        if (data.Count > 0)
                        {
                            if (paidFee == capitalAmout)
                            {
                                model.Message = "Your admission fee " + capitalAmout + " INR already has been paid!";
                                model.IsAddAdmFee = true;
                            }
                            else if (data.FirstOrDefault().collection.InstallmentMasterId != null)
                            {
                                for (int i = 0; i < ArrSessionYr.Count; i++)
                                {
                                    DateTime PreYr = DateTime.Parse("07/01/" + ArrSessionYr[i]);
                                    //DateTime TillYr = PreYr.AddMonths(11);
                                    DateTime TillYr = PreYr.AddMonths(10);
                                    DateTime MsgDelTillDate = PreYr.AddMonths(11);
                                    if (PreYr <= CurrentYr && TillYr >= CurrentYr && PreYr <= data[i].collection.EnteredDate && TillYr >= data[i].collection.EnteredDate) { }
                                    else
                                    {
                                        if (MsgDelTillDate < TillYr)
                                        {
                                            model.Message = "Your Installment-" + (i + 2) + " is pending kindy make the payments within the stipulated deadline!!";
                                            model.IsAddAdmFee = true;
                                        }
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                model.IsAddAdmFee = true;
                            }
                        }
                        else
                        {
                            model.Message = "Please pay Admission Fee " + capitalAmout + " INR.";
                            model.IsAddAdmFee = true;
                        }
                    }
                }
                return model;
            }
            else
            {
                decimal paidFee = 0;
                GetNotificationViewModel model = new GetNotificationViewModel();
                DateTime CurrentYr = DateTime.Now;
                var info = (from r in _context.RegistrationMasters
                            join f in _context.FeeCollections
                            on r.RegistartionNo equals f.FeePaymentDetail.FeeDetail.RegistrationNo into G
                            from obj in G.DefaultIfEmpty()
                            where r.RegistartionNo == Id && r.IsActive
                            select new
                            {
                                Register = r,
                                collection = obj,
                                partwiseList = obj.PartWisePayments
                            }).ToList();
                List<int> ArrSessionYr = new List<int>();
                var sessionId = info.FirstOrDefault().Register.SessionId;
                var courseId = info.FirstOrDefault().Register.CourseId;
                var feeTypeDetail = _context.FeeTypeDetails.
                Where(f => f.FeeType.Name == "Admission" && f.SessionMasterId == sessionId && f.CourseId == courseId).FirstOrDefault();
                decimal capitalAmout = feeTypeDetail.Amount.HasValue ? feeTypeDetail.Amount.Value : 0;

                var data = info.Where(i => i.collection != null && i.collection.FeePaymentDetail.FeeDetail.FeeTypeDetail.FeeType.Name == "Admission").ToList();

                if (data.Count > 0)
                {
                    foreach (var a in data)
                    {
                        if (a.partwiseList.Count > 0)
                            paidFee += a.partwiseList.Where(w => w.Amount.HasValue).Sum(p => p.Amount.Value);
                        else
                            paidFee += a.collection.Amount;
                    }
                    // paidFee = data.Sum(s => s.collection.Amount);
                }

                if (feeTypeDetail != null)
                {
                    if (paidFee == 0)
                        model.DueAdmissionFee = "Due admission fee : " + capitalAmout + " INR."; // capitalAmout
                    else if (paidFee == capitalAmout)
                        model.DueAdmissionFee = "Admission fee has no due.";// 
                    else if (paidFee > 0)
                        model.DueAdmissionFee = "Due admission fee : " + (capitalAmout - paidFee) + " INR.";
                }

                if (info != null && info.Count > 0)
                {
                    var SessionName = info.FirstOrDefault().Register.SessionMaster.SessionName;
                    if (SessionName != "")
                    {
                        string[] ArrSessionPrePostYr = SessionName.Split('-');
                        if (ArrSessionPrePostYr.Length > 0)
                        {
                            for (int i = Convert.ToInt32(ArrSessionPrePostYr[0]); i < Convert.ToInt32(ArrSessionPrePostYr[1]); i++)
                                ArrSessionYr.Add(i);
                        }
                    }
                    if (ArrSessionYr.Count > 0)
                    {
                        if (data.Count > 0)
                        {
                            if (paidFee == capitalAmout)
                            {
                                model.Message = "Your admission fee " + capitalAmout + " INR already has been paid!";
                                model.IsAddAdmFee = true;
                            }
                            else if (data.FirstOrDefault().collection.InstallmentMasterId != null)
                            {
                                for (int i = 0; i < ArrSessionYr.Count; i++)
                                {
                                    DateTime PreYr = DateTime.Parse("07/01/" + ArrSessionYr[i]);
                                    //DateTime TillYr = PreYr.AddMonths(11);
                                    DateTime TillYr = PreYr.AddMonths(10);
                                    DateTime MsgDelTillDate = PreYr.AddMonths(11);
                                    if (PreYr <= CurrentYr && TillYr >= CurrentYr && PreYr <= data[i].collection.EnteredDate && TillYr >= data[i].collection.EnteredDate) { }
                                    else
                                    {
                                        if (MsgDelTillDate < TillYr)
                                        {
                                            model.Message = "Your Installment-" + (i + 2) + " is pending kindy make the payments within the stipulated deadline!!";
                                            model.IsAddAdmFee = true;
                                        }
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                model.IsAddAdmFee = true;
                            }
                        }
                        else
                        {
                            model.Message = "Please pay Admission Fee " + capitalAmout + " INR.";
                            model.IsAddAdmFee = true;
                        }
                    }
                }
                return model;
            }
        }

        public AddmissionMasterViewModel GetFeeDetailList(int RegistrationNo)
        {
            var result = _context.RegistrationMasters.Where(r => r.IsActive && r.RegistartionNo == RegistrationNo).
                Select(i => new AddmissionMasterViewModel
                {
                    RegNo = i.RegistartionNo,
                    Fname = i.Fname,
                    Lname = i.Lname,
                    SessionName = i.SessionMaster.SessionName,
                    CourseName = i.CourseMaster.CourseName,
                    MobileNo = i.Mobile,
                    Email = i.Email,
                    PaymentDetailList = _context.FeeCollections.Where(f => f.FeePaymentDetail.FeeDetail.RegistrationNo == RegistrationNo && f.IsActive
                    && f.FeePaymentDetail.FeeDetail.IsActive).
            Select(item => new PaymentInfoViewModel
            {
                Amount = item.Amount,
                FeeName = item.FeePaymentDetail.FeeDetail.FeeTypeDetail.FeeType.Name,
                InstallmentName = item.InstallmentMaster.Name,
                PaymentType = item.FeePaymentDetail.FeeDetail.PaymentType.Name,
                PaymentMode = item.FeePaymentDetail.PaymentMode.Name,
                RecieptNo = item.RecieptNo,
                Date = item.EnteredDate,
                IsShowPartWisePayment = item.PartWisePayments.Count == 1 ? (item.PartWisePayments.FirstOrDefault().Amount == item.Amount ? false : true) : true,
                PartWiseInfoList = item.PartWisePayments.Select(j => new PartWiseInfoViewModel
                {
                    Amount = j.Amount,
                    PaymentMode = j.PaymentMode.Name,
                    RecieptNo = j.RecieptNo,
                    Date = j.EnteredDate,
                    DDRTGSNo = j.BankAndAmountDetails.FirstOrDefault().DDRTGSNo
                }).ToList()
            }).ToList()
                }).FirstOrDefault();

            if (result != null && result.PaymentDetailList.Count > 0)
            {
                List<PaymentInfoViewModel> MultipleData = new List<PaymentInfoViewModel>();
                foreach (var item in result.PaymentDetailList)
                {
                    //if (!item.IsShowPartWisePayment)
                    //    continue;
                    if (item.PartWiseInfoList != null && item.PartWiseInfoList.Count > 0)
                    {
                        if (!item.IsShowPartWisePayment && item.PartWiseInfoList.Count == 1)
                        {
                            item.DDRTGSNo = item.PartWiseInfoList[0].DDRTGSNo;
                            item.PaymentMode = item.PartWiseInfoList[0].PaymentMode;
                        }
                        foreach (var i in item.PartWiseInfoList)
                        {
                            i.DateStr = i.Date.ToString("dd/MM/yyyy");
                        }
                    }

                    PaymentInfoViewModel model = new PaymentInfoViewModel
                    {
                        Amount = item.Amount,
                        FeeName = item.FeeName,
                        InstallmentName = item.InstallmentName,
                        PartwiseNo = "",
                        PaymentType = item.PaymentType,
                        PaymentMode = (item.PartWiseInfoList.Count == 1 ? item.PaymentMode : ""),
                        RecieptNo = item.RecieptNo,
                        Date = item.Date,
                        PartWiseInfoList = item.PartWiseInfoList,
                        IsShowPartWisePayment = item.IsShowPartWisePayment,
                        DDRTGSNo = item.DDRTGSNo,
                        JsonInfo = Newtonsoft.Json.JsonConvert.SerializeObject(item.PartWiseInfoList)
                    };
                    MultipleData.Add(model);
                }

                result.PaymentDetailList = MultipleData;

                foreach (var item in result.PaymentDetailList)
                {
                    item.DateStr = item.Date.Day.ToString().PadLeft(2, '0') + "/" + item.Date.Month.ToString().PadLeft(2, '0') + "/" + item.Date.Year;
                }
            }
            return result;
        }

        public int GetSessionIdByCourse(int CourseId, int Year, string CourseName)
        {
            var result = "";
            if (CourseName == "BBA")
                result = Year + "-" + (Year + 3);
            else if (CourseName == "MBA")
                result = Year + "-" + (Year + 2);
            else if (CourseName == "PHT")
                result = Year + "-" + (Year + 1);

            var Data = _context.SessionMasters.Where(s => s.IsActive && s.CourseMasterId == CourseId && s.SessionName == result).FirstOrDefault();
            return Data != null ? Data.Id : 0;
        }

        public FeeEmailNotificationViewModel GetFeeInfoByRegId(int RegId)
        {
            return _context.FeeDetails.Where(f => f.IsActive && f.RegistrationNo == RegId)
                .Select(g => new FeeEmailNotificationViewModel
                {
                    RegNo = g.RegistrationNo,
                    FullAmount = g.FeeTypeDetail.Amount,
                    CourseName = g.FeeTypeDetail.CourseMaster.CourseName,
                    SessionName = g.FeeTypeDetail.SessionMaster.SessionName,
                    PaymentType = g.PaymentType.Name,
                    FeeCollection = g.FeePaymentDetails.Select(item => new FeeCollectionViewModel
                    {
                        Amount = item.FeeCollections.FirstOrDefault().Amount,
                        IsActive = item.FeeCollections.FirstOrDefault().IsActive,
                        InstallmentId = item.FeeCollections.FirstOrDefault().InstallmentMasterId,
                        InstallmentName = item.FeeCollections.FirstOrDefault().InstallmentMaster.Name,
                        EnterDate = item.FeeCollections.FirstOrDefault().EnteredDate
                    }).OrderBy(o => o.InstallmentId).ToList()
                }).FirstOrDefault();
        }

        public bool UpdateFeeEmailNotification(int Id)
        {
            bool Status = false;
            var result = _context.FeeEmailNotifacationLogs.Where(w => w.Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsActive = false;
                result.IsSenEmail = true;
            }
            _context.SaveChanges();
            Status = true;
            return Status;
        }

        public List<FeeEmailNotificationViewModel> SendFeeEmailOneByOne()
        {
            int[] ArrYr = new int[3];
            ArrYr[0] = DateTime.Now.Year;
            ArrYr[1] = ArrYr[0] - 1;
            ArrYr[2] = ArrYr[1] - 1;
            return _context.FeeDetails.Where(f => f.IsActive && ArrYr.Contains(f.FeeTypeDetail.SessionMaster.SessionYr.Value))
                .Select(g => new
                {
                    RegNo = g.RegistrationNo,
                    FullAmount = g.FeeTypeDetail.Amount,
                    CourseName = g.FeeTypeDetail.CourseMaster.CourseName,
                    SessionId = g.FeeTypeDetail.SessionMasterId,
                    SessionName = g.FeeTypeDetail.SessionMaster.SessionName,
                    AdmInfo = _context.AddmissionMasters.Where(a => a.IsActive && a.RegistrationNo == g.RegistrationNo).Select(i => new
                    {
                        AdmId = i.Id,
                        Fname = i.Fname,
                        Lname = i.Lname,
                        Email = i.RegisterEmail,
                        //BatchId = i.AddmissionDetails.FirstOrDefault().BatchId,
                        //BatchStartDate = i.AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart,
                        //BatchName = i.AddmissionDetails.FirstOrDefault().BatchMaster.Name
                        BatchId = i.AddmissionDetails.FirstOrDefault().BatchMaster.BatchHistoryDetails.Count() > 0 ? i.AddmissionDetails.FirstOrDefault().BatchMaster.BatchHistoryDetails.OrderBy(o => o.Id).FirstOrDefault().BatchMaster.Id : i.AddmissionDetails.FirstOrDefault().BatchMaster.Id,
                        BatchStartDate = i.AddmissionDetails.FirstOrDefault().BatchMaster.BatchHistoryDetails.Count() > 0 ? i.AddmissionDetails.FirstOrDefault().BatchMaster.BatchHistoryDetails.OrderBy(o => o.Id).FirstOrDefault().BatchMaster.DateOfStart : i.AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart,
                        BatchName = i.AddmissionDetails.FirstOrDefault().BatchMaster.BatchHistoryDetails.Count() > 0 ? i.AddmissionDetails.FirstOrDefault().BatchMaster.BatchHistoryDetails.OrderBy(o => o.Id).FirstOrDefault().BatchMaster.Name : i.AddmissionDetails.FirstOrDefault().BatchMaster.Name
                    }).FirstOrDefault(),
                    FeeCollection = g.FeePaymentDetails.AsEnumerable().
                    Select(item => new FeeCollectionViewModel
                    {
                        PartWise = item.FeeCollections.FirstOrDefault().PartWisePayments.AsEnumerable(),
                        Amount = item.FeeCollections.FirstOrDefault().Amount,
                        IsActive = item.FeeCollections.FirstOrDefault().IsActive,
                        InstallmentId = item.FeeCollections.FirstOrDefault().InstallmentMasterId,
                        InstallmentName = item.FeeCollections.FirstOrDefault().InstallmentMaster.Name,
                        EnterDate = item.EnteredDate
                    }).OrderByDescending(o => o.EnterDate).AsEnumerable()
                })
                .Where(w => w.FeeCollection.Any() && w.FeeCollection.Sum(s => s.Amount) < w.FullAmount)
                .Select(item => new FeeEmailNotificationViewModel
                {
                    RegNo = item.RegNo,
                    FullAmount = item.FullAmount,
                    CourseName = item.CourseName,
                    SessionId = item.SessionId,
                    SessionName = item.SessionName,
                    FeeCollection = item.FeeCollection,
                    Fname = item.AdmInfo.Fname,
                    Lname = item.AdmInfo.Lname,
                    RegisterEmail = item.AdmInfo.Email,
                    BatchId = item.AdmInfo.BatchId,
                    BatchName = item.AdmInfo.BatchName,
                    BatchStartDate = item.AdmInfo.BatchStartDate
                }).Where(i => i.RegisterEmail != null).ToList();
        }

        public string GetCourseChangeFeeReminder(int RegNo, string CourseName, string FeeInstallmentName)

        {
            var admData = _context.AddmissionMasters.Where(a => a.IsActive && a.RegistrationNo == RegNo && a.CourseMaster.CourseName == CourseName).FirstOrDefault();
            decimal Amount = 0;
            if (admData != null)
            {
                int SessionId = admData.AddmissionDetails.FirstOrDefault().SessionId;
                Amount = _context.FeeTypeDetails.Where(f => f.IsActive && f.SessionMasterId == SessionId && f.CourseId == admData.CourseId).FirstOrDefault().SessionInstallmentMasters.Where(s => s.InstallmentMaster.Name == FeeInstallmentName).FirstOrDefault().Amount;

                var data = _context.FeeCollections.Where(x => x.FeePaymentDetail.FeeDetail.IsActive && x.FeePaymentDetail.FeeDetail.RegistrationNo == RegNo && x.InstallmentMaster.Name == FeeInstallmentName).FirstOrDefault();
                if (data != null)
                    Amount = Amount - data.PartWisePayments.Sum(s => s.Amount).Value;
            }
            return Amount.ToString();
        }

        public List<FeeEmailNotifacationLogViewModel> GetFeeEmailNotifyList()
        {
            return _context.FeeEmailNotifacationLogs.Where(w => w.IsActive && !w.IsSenEmail).Select(item => new FeeEmailNotifacationLogViewModel
            {
                CandidateName = item.CandidateName,
                CourseName = item.CourseName,
                EndDateofSendingMail = item.EndDateofSendingMail,
                FeeInstallmentName = item.FeeInstallmentName,
                Id = item.Id,
                IsActive = item.IsActive,
                IsSenEmail = item.IsSenEmail,
                LogDate = item.LogDate,
                RegNo = item.RegNo,
                StartDateOfSendingMail = item.StartDateOfSendingMail,
                Email = item.Email
            }).ToList();
        }

        public bool GetFeeEmailNotifyDetailBiId(FeeEmailNotifacationLogViewModel Model)
        {
            bool Status = true;
            var data = _context.FeeEmailNotifacationLogs.Where(w => w.RegNo == Model.RegNo && w.CourseName == Model.CourseName && w.FeeInstallmentName == Model.FeeInstallmentName && w.StartDateOfSendingMail == Model.StartDateOfSendingMail).FirstOrDefault();
            if (data != null)
                Status = false;
            return Status;
        }

        public bool AddFeeNotifaicationLog(FeeEmailNotifacationLogViewModel Model)
        {
            try
            {
                var data = _context.FeeEmailNotifacationLogs.Where(w => w.IsActive == true && w.IsSenEmail == false && w.FeeInstallmentName == Model.FeeInstallmentName).ToList();
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        item.IsActive = false;
                    }
                }
                FeeEmailNotifacationLog obj = new FeeEmailNotifacationLog
                {
                    CandidateName = Model.CandidateName,
                    CourseName = Model.CourseName,
                    EndDateofSendingMail = Model.EndDateofSendingMail,
                    StartDateOfSendingMail = Model.StartDateOfSendingMail,
                    FeeInstallmentName = Model.FeeInstallmentName,
                    LogDate = DateTime.Now,
                    RegNo = Model.RegNo,
                    Email = Model.Email,
                    IsSenEmail = false,
                    IsActive = true
                };
                _context.FeeEmailNotifacationLogs.Add(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string CandidateCourseChange(int CourseId, int SessionYr)
        {
            return _context.SessionMasters.Where(s => s.SessionYr == SessionYr && s.CourseMasterId == CourseId).FirstOrDefault().SessionName;
        }

        public string SaveSessionChange(int RegNo, int EnteredBy, int SessionId)
        {
            string Msg = "Some problem occur!";
            if (SessionId > 0)
            {
                var registerdata = _context.RegistrationMasters.Where(w => w.RegistartionNo == RegNo).FirstOrDefault();
                if (registerdata != null)
                {
                    registerdata.SessionId = SessionId;
                    var admissiondata = _context.AddmissionDetails.Where(w => w.AddmissionMaster.RegistrationNo == RegNo).FirstOrDefault();
                    if (admissiondata != null)
                        admissiondata.SessionId = SessionId;
                    _context.SaveChanges();
                    Msg = "Session has been changed succesfully";
                }
            }
            return Msg;
        }

        public string SaveCourseChange(int CourseId, int SessionYr, int RegNo, int EnteredBy, int FeeDetailId, string OldCourse, string Remark)
        {
            string Msg = "Some problem occur!";
            int SessionId = _context.SessionMasters.Where(s => s.IsActive && s.SessionYr == SessionYr && s.CourseMasterId == CourseId).FirstOrDefault().Id;
            int CourseChangeHistoryId = 0;
            if (SessionId > 0)
            {
                CourseChangeHistoryDetail ch = new CourseChangeHistoryDetail();
                ch.Description = Remark;
                ch.EnteredBy = EnteredBy;
                ch.CourseTo = CourseId;
                ch.SessionTo = SessionId;
                ch.RegistrationNo = RegNo;
                ch.EnteredDate = DateTime.Now;
                var olddata = _context.SessionMasters.Where(s => s.IsActive && s.SessionYr == SessionYr && s.CourseMaster.CourseName == OldCourse).FirstOrDefault();
                if (olddata != null)
                {
                    ch.CourseFrom = olddata.CourseMasterId;
                    ch.SessionFrom = olddata.Id;
                }
                var data = _context.RegistrationMasters.Where(w => w.RegistartionNo == RegNo).FirstOrDefault();
                if (data != null)
                {
                    data.SessionId = SessionId;
                    data.CourseId = CourseId;
                    if (data.AddmissionMasters.Count() > 0)
                    {
                        var admMaster = data.AddmissionMasters.FirstOrDefault();
                        var admDetail = data.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault();
                        admMaster.CourseId = CourseId;
                        admDetail.SessionId = SessionId;
                        ch.AdmissionMasterId = admMaster.Id;
                    }
                    _context.CourseChangeHistoryDetails.Add(ch);
                    Msg = "Course has been changed successfully!";
                }
                _context.SaveChanges();
                CourseChangeHistoryId = ch.Id;
            }
            if (FeeDetailId > 0 && OldCourse == "PHT")
                BalanceCourseFeeFromPHTtoMBA(RegNo, FeeDetailId, CourseId, SessionYr, EnteredBy, CourseChangeHistoryId);
            if (FeeDetailId > 0 && (OldCourse == "MBA" || OldCourse == "BBA"))
                BalanceCourseFeeFromMBAtoPHT(RegNo, FeeDetailId, CourseId, SessionYr, EnteredBy, CourseChangeHistoryId);
            return Msg;
        }

        public void CaptureCourseFeeSettlementLog(int CourseChangeHistoryDetailId, int FeeDetailIdFrom, int FeeDetailTo, int UserId)
        {
            FeeSettlementOnCourseChnageLog Log = new FeeSettlementOnCourseChnageLog
            {
                CourseChangeHistoryDetailId = CourseChangeHistoryDetailId,
                FeeDetailFrom = FeeDetailIdFrom,
                FeeDetailIdTo = FeeDetailTo,
                EnteredBy = UserId,
                EnteredDate = DateTime.Now
            };
            _context.FeeSettlementOnCourseChnageLogs.Add(Log);
            _context.SaveChanges();
        }


        public void UpdateFeeEmailNotificationForCourseChange(int RegNO, string CourseName)
        {
            var data = _context.FeeEmailNotifacationLogs.Where(w => w.RegNo == RegNO && w.IsActive == true && w.IsSenEmail == false).FirstOrDefault();
            if (data != null)
                data.CourseName = CourseName;
            _context.SaveChanges();
        }

        public void BalanceCourseFeeFromMBAtoPHT(int RegNo, int FeeDetailId, int CourseId, int SessionYr, int UserId, int CourseChangeLogId)
        {
            var feeDetailData = _context.FeeDetails.Where(f => f.IsActive && f.RegistrationNo == RegNo && f.Id == FeeDetailId).FirstOrDefault();
            //if (feeDetailData != null && feeDetailData.FeePaymentDetails.Count() == 1)
            if (feeDetailData != null && feeDetailData.FeePaymentDetails.Count() > 0)
            {
                var feetypedetail = _context.FeeTypeDetails.Where(f => f.FeeTypeId == 1 && f.SessionMaster.SessionYr == SessionYr && f.CourseId == CourseId).FirstOrDefault();
                FeeDetail feeDetail = new FeeDetail
                {
                    PaymentTypeId = 2,
                    FeeTypeDetailId = feetypedetail.Id,
                    RegistrationMasterId = feeDetailData.RegistrationMasterId,
                    RegistrationNo = feeDetailData.RegistrationNo,
                    IsActive = true,
                    EnteredBy = UserId,
                    EnteredDate = feeDetailData.EnteredDate
                };
                _context.FeeDetails.Add(feeDetail);

                FeePaymentDetail feePaymentDetail = new FeePaymentDetail
                {
                    FeeDetailId = feeDetail.Id,
                    PaymentModeId = feeDetailData.FeePaymentDetails.FirstOrDefault().PaymentModeId,
                    TransectionId = feeDetailData.FeePaymentDetails.FirstOrDefault().TransectionId,
                    RecieptNo = "RC#" + RegNo,
                    IsActive = true,
                    EnteredBy = UserId,
                    EnteredDate = feeDetailData.EnteredDate
                };
                _context.FeePaymentDetails.Add(feePaymentDetail);

                FeeCollection feeCollection = new FeeCollection
                {
                    FeePaymentDetailId = feePaymentDetail.Id,
                    Amount = feetypedetail.Amount.Value,
                    EnteredBy = UserId,
                    EnteredDate = feeDetailData.EnteredDate,
                    IsActive = true,
                    PanNumber = feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PanNumber,
                    RecieptNo = "RC" + RegNo + "#1"
                };
                _context.FeeCollections.Add(feeCollection);

                feeDetailData.IsActive = false;
                _context.SaveChanges();

                if (feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PartWisePayments.Count() > 0)
                {
                    decimal Amount = 0;
                    foreach (var item in feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PartWisePayments)
                    {
                        decimal amt = item.Amount.Value;
                        Amount += item.Amount.Value;
                        if (Amount > feetypedetail.Amount.Value)
                            amt = feetypedetail.Amount.Value - (Amount - item.Amount.Value);

                        PartWisePayment paymentFirst = new PartWisePayment
                        {
                            Amount = amt,
                            FeeCollectionId = feeCollection.Id,
                            RecieptNo = item.RecieptNo,
                            PaymentModeId = item.PaymentMode.Id,
                            EnteredBy = UserId,
                            EnteredDate = DateTime.Now
                        };
                        _context.PartWisePayments.Add(paymentFirst);

                        BankAndAmountDetail bankDetail = new BankAndAmountDetail
                        {
                            DDRTGSNo = item.BankAndAmountDetails.FirstOrDefault().DDRTGSNo,
                            EnteredDate = item.BankAndAmountDetails.FirstOrDefault().EnteredDate,
                            EnteredBy = item.BankAndAmountDetails.FirstOrDefault().EnteredBy,
                            PartWisePaymentId = paymentFirst.Id
                        };
                        _context.BankAndAmountDetails.Add(bankDetail);
                        _context.SaveChanges();
                        if (Amount > feetypedetail.Amount.Value)
                            break;
                    }
                }
                //===================================================
                decimal CapitalAmount = feetypedetail.Amount.Value;
                decimal? PaidAmount = feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PartWisePayments.Sum(p => p.Amount);
                if (PaidAmount.Value > CapitalAmount)
                {
                    decimal ExceedAmount = PaidAmount.Value - CapitalAmount;
                    ExceedFeeLogCourseChangeViewModel exceed = new ExceedFeeLogCourseChangeViewModel
                    {
                        EnteredBy = UserId,
                        ExceedAmount = ExceedAmount,
                        OldFeeDetailId = feeDetailData.Id,
                        LogDate = DateTime.Now,
                        RegistrationNo = feeDetailData.RegistrationNo,
                        FeeDetailId = feeDetail.Id
                    };
                    ExeedPaymentLogDetailOnCourseChange(exceed);
                }
                //===================================================
                CaptureCourseFeeSettlementLog(CourseChangeLogId, feeDetailData.Id, feeDetail.Id, UserId);
                IsFeePaymentStandBy(false, RegNo);
            }
        }

        public void BalanceCourseFeeFromPHTtoMBA(int RegNo, int FeeDetailId, int CourseId, int SessionYr, int UserId, int CourseChangeLogId)
        {
            var feeDetailData = _context.FeeDetails.Where(f => f.IsActive && f.RegistrationNo == RegNo && f.Id == FeeDetailId).FirstOrDefault();
            //if (feeDetailData != null && feeDetailData.FeePaymentDetails.Count() == 1)
            if (feeDetailData != null && feeDetailData.FeePaymentDetails.Count() > 0)
            {
                var feetypedetailId = _context.FeeTypeDetails.Where(f => f.IsActive && f.FeeTypeId == 1 && f.CourseId == CourseId && f.SessionMaster.SessionYr == SessionYr).FirstOrDefault();
                FeeDetail feeDetail = new FeeDetail
                {
                    PaymentTypeId = 1,
                    FeeTypeDetailId = feetypedetailId.Id,
                    RegistrationMasterId = feeDetailData.RegistrationMasterId,
                    RegistrationNo = feeDetailData.RegistrationNo,
                    IsActive = true,
                    EnteredBy = UserId,
                    EnteredDate = feeDetailData.EnteredDate
                };
                _context.FeeDetails.Add(feeDetail);

                FeePaymentDetail feePaymentDetail = new FeePaymentDetail
                {
                    FeeDetailId = feeDetail.Id,
                    PaymentModeId = feeDetailData.FeePaymentDetails.FirstOrDefault().PaymentModeId,
                    TransectionId = feeDetailData.FeePaymentDetails.FirstOrDefault().TransectionId,
                    RecieptNo = "RC#" + RegNo,
                    IsActive = true,
                    EnteredBy = UserId,
                    EnteredDate = feeDetailData.EnteredDate
                };
                _context.FeePaymentDetails.Add(feePaymentDetail);

                FeeCollection feeCollection = new FeeCollection
                {
                    FeePaymentDetailId = feePaymentDetail.Id,
                    Amount = feetypedetailId.SessionInstallmentMasters.Where(s => s.InstallmentMasterId == 1).FirstOrDefault().Amount,
                    EnteredBy = UserId,
                    EnteredDate = feeDetailData.EnteredDate,
                    IsActive = true,
                    PanNumber = feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PanNumber,
                    InstallmentMasterId = 1,
                    RecieptNo = "RC" + RegNo + "#1"
                };
                _context.FeeCollections.Add(feeCollection);
                feeDetailData.IsActive = false;
                _context.SaveChanges();

                decimal? FeePayAmount = feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PartWisePayments.Sum(s => s.Amount);
                if (FeePayAmount.HasValue)
                {
                    if (FeePayAmount.Value <= feetypedetailId.SessionInstallmentMasters.Where(s => s.InstallmentMasterId == 1).FirstOrDefault().Amount)
                    {
                        if (feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PartWisePayments.Count() > 0)
                        {
                            foreach (var item in feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PartWisePayments)
                            {
                                PartWisePayment paymentFirst = new PartWisePayment
                                {
                                    Amount = item.Amount,
                                    FeeCollectionId = feeCollection.Id,
                                    RecieptNo = "RCFC#" + (_context.PartWisePayments.ToList().Count() + 1),
                                    PaymentModeId = item.PaymentMode.Id,
                                    EnteredBy = UserId,
                                    EnteredDate = DateTime.Now
                                };
                                _context.PartWisePayments.Add(paymentFirst);

                                BankAndAmountDetail bankDetail = new BankAndAmountDetail
                                {
                                    DDRTGSNo = item.BankAndAmountDetails.FirstOrDefault().DDRTGSNo,
                                    EnteredDate = item.BankAndAmountDetails.FirstOrDefault().EnteredDate,
                                    EnteredBy = item.BankAndAmountDetails.FirstOrDefault().EnteredBy,
                                    PartWisePaymentId = paymentFirst.Id
                                };
                                _context.BankAndAmountDetails.Add(bankDetail);
                                _context.SaveChanges();
                            }
                        }
                    }
                    else if (FeePayAmount.Value > feetypedetailId.SessionInstallmentMasters.Where(s => s.InstallmentMasterId == 1).FirstOrDefault().Amount)
                    {
                        decimal AmountByAmount = 0;
                        bool SecondFeeCollectionStatus = false;
                        int secondFeecollectionId = 0;
                        bool FirstComplete = false;
                        foreach (var item in feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PartWisePayments)
                        {
                            AmountByAmount += item.Amount.Value;
                            if (AmountByAmount <= feetypedetailId.SessionInstallmentMasters.Where(s => s.InstallmentMasterId == 1).FirstOrDefault().Amount)
                            {
                                PartWisePayment paymentFirst = new PartWisePayment
                                {
                                    Amount = item.Amount,
                                    FeeCollectionId = feeCollection.Id,
                                    RecieptNo = "RCFC#" + (_context.PartWisePayments.ToList().Count() + 1),
                                    PaymentModeId = item.PaymentMode.Id,
                                    EnteredBy = UserId,
                                    EnteredDate = DateTime.Now
                                };
                                _context.PartWisePayments.Add(paymentFirst);

                                BankAndAmountDetail bankDetail = new BankAndAmountDetail
                                {
                                    DDRTGSNo = item.BankAndAmountDetails.FirstOrDefault().DDRTGSNo,
                                    EnteredDate = item.BankAndAmountDetails.FirstOrDefault().EnteredDate,
                                    EnteredBy = item.BankAndAmountDetails.FirstOrDefault().EnteredBy,
                                    PartWisePaymentId = paymentFirst.Id
                                };
                                _context.BankAndAmountDetails.Add(bankDetail);
                                _context.SaveChanges();
                                if (AmountByAmount == feetypedetailId.SessionInstallmentMasters.Where(s => s.InstallmentMasterId == 1).FirstOrDefault().Amount)
                                    FirstComplete = true;
                            }
                            else if (AmountByAmount > feetypedetailId.SessionInstallmentMasters.Where(s => s.InstallmentMasterId == 1).FirstOrDefault().Amount)
                            {
                                if (!SecondFeeCollectionStatus)
                                {
                                    decimal part1 = AmountByAmount - feetypedetailId.SessionInstallmentMasters.Where(s => s.InstallmentMasterId == 1).FirstOrDefault().Amount;
                                    decimal part2 = item.Amount.Value - part1;

                                    //============== First Collection =================
                                    if (!FirstComplete)
                                    {
                                        PartWisePayment paymentFirst = new PartWisePayment
                                        {
                                            Amount = part2,
                                            FeeCollectionId = feeCollection.Id,
                                            RecieptNo = "RCFC#" + (_context.PartWisePayments.ToList().Count() + 1),
                                            PaymentModeId = item.PaymentMode.Id,
                                            EnteredBy = UserId,
                                            EnteredDate = DateTime.Now
                                        };
                                        _context.PartWisePayments.Add(paymentFirst);

                                        BankAndAmountDetail bankDetail = new BankAndAmountDetail
                                        {
                                            DDRTGSNo = item.BankAndAmountDetails.FirstOrDefault().DDRTGSNo,
                                            EnteredDate = item.BankAndAmountDetails.FirstOrDefault().EnteredDate,
                                            EnteredBy = item.BankAndAmountDetails.FirstOrDefault().EnteredBy,
                                            PartWisePaymentId = paymentFirst.Id
                                        };
                                        _context.BankAndAmountDetails.Add(bankDetail);
                                        _context.SaveChanges();
                                    }
                                    //===================================================

                                    //================= Second Collection ===============

                                    FeePaymentDetail feePaymentDetailSecond = new FeePaymentDetail
                                    {
                                        FeeDetailId = feeDetail.Id,
                                        PaymentModeId = item.PaymentMode.Id,
                                        RecieptNo = "RC#" + RegNo,
                                        IsActive = true,
                                        EnteredBy = UserId,
                                        EnteredDate = DateTime.Now
                                    };
                                    _context.FeePaymentDetails.Add(feePaymentDetailSecond);

                                    FeeCollection feeCollectionSecond = new FeeCollection
                                    {
                                        FeePaymentDetailId = feePaymentDetailSecond.Id,
                                        Amount = feetypedetailId.SessionInstallmentMasters.Where(s => s.InstallmentMasterId == 2).FirstOrDefault().Amount,
                                        EnteredBy = UserId,
                                        EnteredDate = DateTime.Now,
                                        IsActive = true,
                                        PanNumber = feeDetailData.FeePaymentDetails.FirstOrDefault().FeeCollections.FirstOrDefault().PanNumber,
                                        InstallmentMasterId = 2,
                                        RecieptNo = "RC" + RegNo + "#2"
                                    };
                                    _context.FeeCollections.Add(feeCollectionSecond);

                                    PartWisePayment paymentSecond = new PartWisePayment
                                    {
                                        Amount = FirstComplete ? item.Amount : part1,
                                        FeeCollectionId = feeCollectionSecond.Id,
                                        RecieptNo = "RCFC#" + (_context.PartWisePayments.ToList().Count() + 1),
                                        PaymentModeId = item.PaymentMode.Id,
                                        EnteredBy = UserId,
                                        EnteredDate = DateTime.Now
                                    };
                                    _context.PartWisePayments.Add(paymentSecond);

                                    BankAndAmountDetail bankDetailSecond = new BankAndAmountDetail
                                    {
                                        DDRTGSNo = item.BankAndAmountDetails.FirstOrDefault().DDRTGSNo,
                                        EnteredDate = item.BankAndAmountDetails.FirstOrDefault().EnteredDate,
                                        EnteredBy = item.BankAndAmountDetails.FirstOrDefault().EnteredBy,
                                        PartWisePaymentId = paymentSecond.Id
                                    };
                                    _context.BankAndAmountDetails.Add(bankDetailSecond);
                                    _context.SaveChanges();

                                    //===================================================
                                    secondFeecollectionId = feeCollectionSecond.Id;
                                    SecondFeeCollectionStatus = true;
                                }
                                else
                                {
                                    PartWisePayment paymentFirst = new PartWisePayment
                                    {
                                        Amount = item.Amount,
                                        FeeCollectionId = secondFeecollectionId,
                                        RecieptNo = "RCFC#" + (_context.PartWisePayments.ToList().Count() + 1),
                                        PaymentModeId = item.PaymentMode.Id,
                                        EnteredBy = UserId,
                                        EnteredDate = DateTime.Now
                                    };
                                    _context.PartWisePayments.Add(paymentFirst);

                                    BankAndAmountDetail bankDetail = new BankAndAmountDetail
                                    {
                                        DDRTGSNo = item.BankAndAmountDetails.FirstOrDefault().DDRTGSNo,
                                        EnteredDate = item.BankAndAmountDetails.FirstOrDefault().EnteredDate,
                                        EnteredBy = item.BankAndAmountDetails.FirstOrDefault().EnteredBy,
                                        PartWisePaymentId = paymentFirst.Id
                                    };
                                    _context.BankAndAmountDetails.Add(bankDetail);
                                    _context.SaveChanges();
                                }
                                //===================================================
                            }
                        }
                        IsFeePaymentStandBy(false, RegNo);
                    }
                }
                CaptureCourseFeeSettlementLog(CourseChangeLogId, feeDetailData.Id, feeDetail.Id, UserId);
                UpdateFeeEmailNotificationForCourseChange(RegNo, feetypedetailId.CourseMaster.CourseName);
            }
        }

        public bool ExeedPaymentLogDetailOnCourseChange(ExceedFeeLogCourseChangeViewModel Model)
        {
            bool Status = false;
            var admissionData = _context.AddmissionMasters.Where(w => w.RegistrationNo == Model.RegistrationNo).FirstOrDefault();
            if (admissionData != null)
            {
                ExceededFeeAmountOnCourseChange info = new ExceededFeeAmountOnCourseChange
                {
                    LogDate = DateTime.Now,
                    AdmissionId = admissionData.Id,
                    EnteredBy = Model.EnteredBy,
                    ExceedAmount = Model.ExceedAmount,
                    FeeDetailId = Model.FeeDetailId,
                    OldFeeDetailId = Model.OldFeeDetailId,
                    RegistrationNo = Model.RegistrationNo
                };
                _context.ExceededFeeAmountOnCourseChanges.Add(info);
                _context.SaveChanges();
                Status = true;
            }
            return Status;
        }

        public string GetCourseNameByCourseId(int CourseId)
        {
            return _context.CourseMasters.Where(w => w.IsActive && w.Id == CourseId).FirstOrDefault().CourseName;
        }

        public dynamic GetRecieptDetail(string RecieptNo)
        {
            var data = _context.FeeCollections.Where(f => f.FeePaymentDetail.FeeDetail.IsActive && f.RecieptNo == RecieptNo && f.IsActive).Select(item => new
            {
                FeeType = item.FeePaymentDetail.FeeDetail.FeeTypeDetail.FeeType.Name,
                CAmount = item.Amount,
                RecieptNo = item.RecieptNo,
                PaymentType = item.InstallmentMasterId != null ? "Annual Installments" : "One Time Payment",
                InstallementNo = item.InstallmentMasterId != null ? item.InstallmentMasterId.Value : 0,
                PaidAmount = item.PartWisePayments.Sum(s => s.Amount),
                PartwiseInfo = item.PartWisePayments.Select(i => new
                {
                    Amount = i.Amount,
                    RecieptNo = i.RecieptNo,
                    PaymentMode = i.PaymentMode.Name,
                    DDRTGSNO = i.BankAndAmountDetails.FirstOrDefault().DDRTGSNo,
                    Date = i.EnteredDate
                }).ToList()
            }).FirstOrDefault();
            return data;
        }

        public string SaveUpdateInstallment(SessionInstallmentMasterViewModel Model)
        {
            try
            {
                string msg = "";
                SessionInstallmentMaster Master = new SessionInstallmentMaster
                {
                    Amount = Model.Amount,
                    FeeTypeDetailId = Model.FeeTypeDetailId,
                    InstallmentMasterId = Model.InstallmentMasterId,
                    EneteredDate = DateTime.Now,
                    EnteredBy = Model.EnteredBy
                };
                _context.SessionInstallmentMasters.Add(Master);
                _context.SaveChanges();
                msg = "Scuucessfully Created!";
                return msg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public SessionInstallmentViewModel GetInstallmetsByFeetypedetailId(int FeeTypeDetailId)
        {
            return _context.FeeTypeDetails.Where(w => w.Id == FeeTypeDetailId).Select(item => new SessionInstallmentViewModel
            {
                InstallmentList = item.SessionInstallmentMasters.Select(a => new SessionInstallmentMasterViewModel
                {
                    Amount = a.Amount,
                    FeeTypeDetailId = a.FeeTypeDetailId,
                    InstallmentName = a.InstallmentMaster.Name,
                    Id = a.Id
                }).ToList(),
                CapitalAmount = item.Amount.Value,
                FeeTypeDeatilId = item.Id,
                GetInstallmentList = _context.InstallmentMasters.Select(s => new SemesterMasterViewModel
                {
                    Id = s.Id,
                    BatchName = s.Name
                }).ToList()
            }).FirstOrDefault();
        }

        public RefundInformationViewModel OpenRefundPopup(int Id, int? AdmissionId)
        {
            RefundInformationViewModel model = new RefundInformationViewModel();
            model.AdmissionId = AdmissionId;
            if (AdmissionId.HasValue)
            {
                int regNo = _context.AddmissionMasters.Where(a => a.Id == AdmissionId.Value).FirstOrDefault().RegistrationNo;
                model.TotalAmount = _context.PartWisePayments.Where(w => w.FeeCollection.FeePaymentDetail.FeeDetail.IsActive && w.FeeCollection.FeePaymentDetail.FeeDetail.RegistrationMaster.RegistartionNo == regNo).Sum(s => s.Amount);
            }
            var data = _context.RefundInformations.Where(w => w.Id == Id).FirstOrDefault();

            if (data != null)
            {
                model.Id = data.Id;
                model.RefundAmount = data.RefundAmount;
                model.Status = data.Status;
                model.StatusRemark = data.StatusLine;
                model.CaseClosed = data.CaseClosed;
                model.CurrentDate = data.CurrDate.HasValue ? data.CurrDate.Value.Day.ToString().PadLeft(2, '0') + "/" + data.CurrDate.Value.Month.ToString().PadLeft(2, '0') + "/" + data.CurrDate.Value.Year : DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
            }

            if (data != null && data.ExceededFeeAmountOnCourseChange != null)
            {
                model.ExeedAmount = data.ExceededFeeAmountOnCourseChange.ExceedAmount;
                model.ExceedFeeAmountOnCourseChangeId = data.ExceededFeeAmountOnCourseChangeId.Value;
            }
            else
                model.ExeedAmount = 0;

            //model.ExceedFeeAmountOnCourseChangeId = Id;
            //var data = _context.ExceededFeeAmountOnCourseChanges.Where(w => w.Id == Id).FirstOrDefault();
            //if (data != null)
            //    model.ExeedAmount = data.ExceedAmount;
            //if (data != null && data.RefundInformations.Any())
            //{
            //    model.Id = data.RefundInformations.FirstOrDefault().Id;
            //    model.RefundAmount = data.RefundInformations.FirstOrDefault().RefundAmount;
            //    model.Status = data.RefundInformations.FirstOrDefault().Status;
            //    model.StatusRemark = data.RefundInformations.FirstOrDefault().StatusLine;
            //    model.CaseClosed = data.RefundInformations.FirstOrDefault().CaseClosed;
            //    model.CurrentDate = data.RefundInformations.FirstOrDefault().CurrDate.HasValue ? data.RefundInformations.FirstOrDefault().CurrDate.Value.Day.ToString().PadLeft(2, '0') + "/" + data.RefundInformations.FirstOrDefault().CurrDate.Value.Month.ToString().PadLeft(2, '0') + "/" + data.RefundInformations.FirstOrDefault().CurrDate.Value.Year : DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
            //}
            return model;
        }

        public bool SaveApprovedAndReject(RefundInformationViewModel Model)
        {
            bool bit = false;
            string Status = "";
            if (Model.RefundAmount > 0 && Model.Header == "Approved")
                Status = "Case closed and approved with " + Model.RefundAmount + " INR.";
            else if (Model.Header == "Reject")
                Status = "Case closed with reject";
            if (Status == "")
            {

                RefundInformation obj = new RefundInformation
                {
                    CaseClosed = Model.CaseClosed,
                    EnteredDate = DateTime.Now,
                    RefundAmount = Model.RefundAmount,
                    ExceededFeeAmountOnCourseChangeId = Model.Id,
                    StatusLine = Model.StatusRemark,
                    EnteredBy = Model.EnteredBy,
                    CurrDate = DateTime.ParseExact(Model.CurrentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date,
                    Status = "",
                    AddmissionMasterId = Model.AdmissionId
                };
                if (Model.Id == 0)
                    obj.ExceededFeeAmountOnCourseChangeId = (int?)null;
                _context.RefundInformations.Add(obj);
                _context.SaveChanges();
                bit = true;
            }
            else
            {
                var dd = _context.RefundInformations.Where(w => w.Id == Model.Id).FirstOrDefault();
                if (dd != null)
                {
                    dd.Status = Status;
                }
                _context.SaveChanges();
                bit = true;
            }
            return bit;
        }
    }
}
