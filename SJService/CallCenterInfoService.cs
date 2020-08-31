using SJData;
using SJModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJService
{
    public class CallCenterInfoService
    {
        SJStarERPEntities _context = null;
        public CallCenterInfoService()
        {
            _context = new SJStarERPEntities();
        }


        public DataTableFilterModel GetCallCenterRemarkList(DataTableFilterModel filter, int RegNo)
        {
            var info = _context.CallCenterRemarks.Where(c => c.RegistrationNo == RegNo).Select(model => new CallCenterRemarkViewModel()
            {
                Id = model.Id,
                Remarks = model.Remarks,
                EnteredByName = model.UserLogin.Fname + " " + model.UserLogin.LName,
                EnteredDate = model.EnteredDate
            }).AsEnumerable();

            var totalCount = info.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                info = info.Where(d => (d.Remarks.ToLower().Contains(filter.search.value.ToLower()) || d.EnteredByName.ToLower().Contains(filter.search.value.ToLower()) || d.EnteredDate.ToString().ToLower().Contains(filter.search.value.ToLower())));
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
                i.CreateDate = i.EnteredDate.ToString("dd-MM-yyyy");
            }
            filter.data = dataFilter;
            return filter;
        }

        public DataTableFilterModel GetCallCenterInfoList(DataTableFilterModel filter, int SessionYr)
        {
            var data = _context.RegistrationMasters.Where(r => r.IsActive).AsQueryable();
            //data = data.Where(r => r.AddmissionMasters.Any() && r.AddmissionMasters.FirstOrDefault().IsActive && (!r.IsMedicalClear.HasValue || r.IsMedicalClear.Value) && r.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchId != 19);

            data = data.Where(r => r.AddmissionMasters.Any() && (r.FeeDetails.Any() || !r.IsMedicalClear.HasValue || r.IsMedicalClear.Value) && ((r.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchId == 19 && r.FeeDetails.Any()) || (r.AddmissionMasters.FirstOrDefault().IsActive && r.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchId != 19)));

            if (SessionYr > 0)
                data = data.Where(d => d.AddmissionMasters.FirstOrDefault().AddmissionDetails.FirstOrDefault().BatchMaster.DateOfStart.Value.Year == SessionYr);

            var info = data.Select(model => new RegistrationViewModel()
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
                CourseId = model.CourseId,
                SessionId = model.SessionId,
                PaymentDate = model.PaymentDate,
                PaymentStatusStr = model.PaymentStatus ? "Success" : "Failed",
                IsScreenningClear = model.IsScreenningClear,
                IsMedicalClear = model.IsMedicalClear,
                IsFeePayment = _context.FeeDetails.Where(f => f.IsActive && f.RegistrationNo == model.RegistartionNo && f.FeeTypeDetail.FeeType.Name == "Admission").FirstOrDefault().FeePaymentDetails.Where(fpd => fpd.IsActive).FirstOrDefault().FeeCollections.Any(),  // _context.FIN_FeeReceiptMaster.Any(f => f.RegNo == model.RegistartionNo),
                IsRefunded = _context.FIN_FeeRefundMaster.Any(f => f.RegNo == model.RegistartionNo),
                CourseName = model.CourseMaster.CourseName,
                AddMissionId = _context.AddmissionMasters.Any(a => a.RegistrationNo == model.RegistartionNo) ? _context.AddmissionMasters.Where(a => a.RegistrationNo == model.RegistartionNo).FirstOrDefault().Id : 0,
                IsFeePayStandBy = model.IsFeePayStandBy,
                BatchName = _context.AddmissionMasters.Any(a => a.RegistrationNo == model.RegistartionNo) ? _context.AddmissionDetails.Where(a => a.AddmissionMaster.RegistrationNo == model.RegistartionNo).FirstOrDefault().BatchMaster.Name : "",
                BatchId = _context.AddmissionMasters.Any(a => a.RegistrationNo == model.RegistartionNo) ? _context.AddmissionDetails.Where(a => a.AddmissionMaster.RegistrationNo == model.RegistartionNo).FirstOrDefault().BatchId.Value : 0,

                RefundStatus = model.FeeDetails.Any() ? (model.FeeDetails.FirstOrDefault().FeeTypeDetail.Amount == _context.PartWisePayments.Where(p => p.FeeCollection.FeePaymentDetail.FeeDetail.IsActive && p.FeeCollection.FeePaymentDetail.FeeDetail.RegistrationNo == model.RegistartionNo).Sum(s => s.Amount) ? "success" : (_context.FeeCollections.Where(f => f.FeePaymentDetail.FeeDetail.IsActive && f.FeePaymentDetail.FeeDetail.RegistrationNo == model.RegistartionNo).Sum(s => s.Amount) > 0 ? "orange" : "")) : ""



                //RefundStatus = model.FeeDetails.Any() ? (model.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(a => a.Id).FirstOrDefault().Amount == model.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(a => a.Id).FirstOrDefault().PartWisePayments.Sum(s => s.Amount) ? "success" : (model.FeeDetails.FirstOrDefault().FeePaymentDetails.FirstOrDefault().FeeCollections.OrderByDescending(a => a.Id).FirstOrDefault().PartWisePayments.Sum(s => s.Amount) > 0 ? "orange" : "")) : ""

            }).Where(result => result.IsScreenningClear.HasValue && result.IsScreenningClear.Value).AsEnumerable();

            var totalCount = info.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                info = info.Where(d => (d.RegistartionNo.ToString().ToLower().Contains(filter.search.value.ToLower()) || (!string.IsNullOrEmpty(d.StudentName) && d.StudentName.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToString().ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Mobile) && d.Mobile.ToString().ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.PaymentStatusStr) && d.PaymentStatusStr.ToString().ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.BatchName) && d.BatchName.ToString().ToLower().Contains(filter.search.value.ToLower()))));
            }
            var o = filter.order[0];
            var name = filter.columns[filter.order[0].column].data;
            if (o.dir == "desc")
            {
                info = info.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
            }
            else
            {
                info = info.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[7].search.value))
            {
                int batch = Convert.ToInt32(filter.columns[7].search.value);
                info = info.Where(t => t.BatchId == batch);
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[8].search.value))
                info = info.Where(t => t.CourseName == filter.columns[8].search.value);

            if (!string.IsNullOrWhiteSpace(filter.columns[11].search.value))
            {
                if (filter.columns[11].search.value == "Fully")
                    info = info.Where(t => t.RefundStatus == "success");
                else if (filter.columns[11].search.value == "Partially")
                {
                    var sss = info.ToList();
                    info.Where(t => t.RefundStatus == "orange" && t.RefundStatus != "success" && t.RefundStatus != "");
                    var ss = info.ToList();
                }
                else if (filter.columns[11].search.value == "Unpaid")
                {
                    info.Where(t => t.RefundStatus == "" && t.RefundStatus != "orange" && t.RefundStatus != "success");
                }
            }

            var filteredCount = info.Count();
            filter.recordsTotal = totalCount;
            filter.recordsFiltered = filteredCount;
            var dataFilter = info.Skip(filter.start).Take(filter.length).ToList();
            //foreach (var i in dataFilter)
            //{
            //    i.DateOfBirth = i.DOB.HasValue ? i.DOB.Value.ToString("dd-MM-yyyy") : "";
            //    i.RegisterDate = i.RegistrationDate.HasValue? i.RegistrationDate.Value.ToString("dd-MM-yyyy") : "";
            //}
            filter.data = dataFilter;
            return filter;
        }

        public bool SaveCallCenterRemarks(int RegNo, string Remark, int UserId)
        {
            bool status = false;
            if (RegNo > 0)
            {
                CallCenterRemark model = new CallCenterRemark
                {
                    EnteredBy = UserId,
                    EnteredDate = DateTime.Now,
                    RegistrationNo = RegNo,
                    Remarks = Remark
                };
                _context.CallCenterRemarks.Add(model);
                _context.SaveChanges();
                status = true;
            }
            return status;
        }

    }
}
