using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJModel;
using SJData;
using System.Globalization;

namespace SJService
{
    public class ReportService
    {
        private SJStarERPEntities _context = null;
        public ReportService()
        {
            _context = new SJStarERPEntities();
        }

        public LineChartViewModel GetLineChartData(ReportFilterViewModel filterModel)
        {
            var data = _context.AddmissionMasters.Where(a => a.IsActive && (!a.RegistrationMaster.IsMedicalClear.HasValue || a.RegistrationMaster.IsMedicalClear.Value) && a.AddmissionDetails.FirstOrDefault().BatchMaster.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.Name != "Batch 0").Select(item => new
            {
                AdmissionId = item.Id,
                CourseId = item.CourseId,
                CourseName = item.CourseMaster.CourseName,
                AdmissionDate = item.AddmissionDate,
                BatchId = item.AddmissionDetails.FirstOrDefault().BatchId,
                BatchName = item.AddmissionDetails.FirstOrDefault().BatchMaster.Name,
                State = item.AddressDetails.FirstOrDefault().PerState,
                City = item.AddressDetails.FirstOrDefault().PerCity
            }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterModel.CourseId))
            {
                var courseArr = filterModel.CourseId.Split(',').Select(Int32.Parse).ToList();
                if (courseArr.Count > 0)
                    data = data.Where(t => courseArr.Any(c => c == t.CourseId));
            }
            if (!string.IsNullOrWhiteSpace(filterModel.BatchId))
            {
                var batchArr = filterModel.BatchId.Split(',').Select(Int32.Parse).ToList();
                if (batchArr.Count > 0)
                    data = data.Where(t => batchArr.Any(c => c == t.BatchId));
            }
            if (!string.IsNullOrEmpty(filterModel.State))
            {
                data = data.Where(x => x.State == filterModel.State);
                if (!string.IsNullOrEmpty(filterModel.City))
                    data = data.Where(x => x.City == filterModel.City);
            }
            LineChartViewModel Model = new LineChartViewModel();
            if (filterModel.Year > 0 && filterModel.Month == 0)
            {
                var AdmInfo = data.Where(d => d.AdmissionDate.Value.Year == filterModel.Year)
                   .GroupBy(x => new { x.CourseName, x.AdmissionDate.Value.Month }).Select(items => new
                   {
                       CourseName = items.Key.CourseName,
                       Month = items.Key.Month,
                       Count = items.Count()
                   }).OrderBy(o => o.Month).ToList();

                var BBAinfo = AdmInfo.Where(a => a.CourseName == "BBA").ToList();
                var MBAinfo = AdmInfo.Where(a => a.CourseName == "MBA").ToList();
                var PHTinfo = AdmInfo.Where(a => a.CourseName == "PHT").ToList();
                int[] BBABarX = new int[12];
                int[] MBABarY = new int[12];
                int[] PHTBarZ = new int[12];
                for (int i = 1; i <= 12; i++)
                {
                    var a = BBAinfo.Where(n => n.Month == i).FirstOrDefault();
                    var b = MBAinfo.Where(n => n.Month == i).FirstOrDefault();
                    var c = PHTinfo.Where(n => n.Month == i).FirstOrDefault();

                    if (a != null)
                        BBABarX[i - 1] = a.Count;
                    else
                        BBABarX[i - 1] = 0;

                    if (b != null)
                        MBABarY[i - 1] = b.Count;
                    else
                        MBABarY[i - 1] = 0;

                    if (c != null)
                        PHTBarZ[i - 1] = c.Count;
                    else
                        PHTBarZ[i - 1] = 0;
                }
                Model.line = new List<Line>
                {
                    new Line{ Label="BBA",value=BBABarX },
                    new Line{Label="MBA",value=MBABarY},
                    new Line{Label="PHT",value=PHTBarZ}
                };
                Model.LineChartLbl = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Set", "Oct", "Nov", "Dec" };
            }
            else if (filterModel.Month > 0)
            {
                var AdmInfo = data.Where(d => d.AdmissionDate.Value.Year == filterModel.Year && d.AdmissionDate.Value.Month == filterModel.Month)
                             .GroupBy(x => new { x.CourseName, x.AdmissionDate.Value.Day }).Select(items => new
                             {
                                 CourseName = items.Key.CourseName,
                                 Day = items.Key.Day,
                                 Count = items.Count()
                             }).OrderBy(o => o.Day).ToList();

                var BBAinfo = AdmInfo.Where(a => a.CourseName == "BBA").ToList();
                var MBAinfo = AdmInfo.Where(a => a.CourseName == "MBA").ToList();
                var PHTinfo = AdmInfo.Where(a => a.CourseName == "PHT").ToList();
                int[] BBABarX = new int[DateTime.DaysInMonth(filterModel.Year, filterModel.Month)];
                int[] MBABarY = new int[DateTime.DaysInMonth(filterModel.Year, filterModel.Month)];
                int[] PHTBarZ = new int[DateTime.DaysInMonth(filterModel.Year, filterModel.Month)];

                for (int i = 1; i <= DateTime.DaysInMonth(filterModel.Year, filterModel.Month); i++)
                {
                    var a = BBAinfo.Where(n => n.Day == i).FirstOrDefault();
                    var b = MBAinfo.Where(n => n.Day == i).FirstOrDefault();
                    var c = PHTinfo.Where(n => n.Day == i).FirstOrDefault();
                    if (a != null)
                        BBABarX[i - 1] = a.Count;
                    else
                        BBABarX[i - 1] = 0;

                    if (b != null)
                        MBABarY[i - 1] = b.Count;
                    else
                        MBABarY[i - 1] = 0;

                    if (c != null)
                        PHTBarZ[i - 1] = c.Count;
                    else
                        PHTBarZ[i - 1] = 0;
                }
                Model.line = new List<Line>
                {
                    new Line{ Label="BBA",value=BBABarX },
                    new Line{Label="MBA",value=MBABarY},
                    new Line{Label="PHT",value=PHTBarZ}
                };
                int daysMonth = DateTime.DaysInMonth(filterModel.Year, filterModel.Month);
                string[] days = new string[daysMonth];
                for (int i = 1; i <= daysMonth; i++)
                    days[i - 1] = i.ToString();
                Model.LineChartLbl = days;
            }
            else
            {
                var AdmInfo = data.GroupBy(x => new { x.CourseName, x.AdmissionDate.Value.Year }).Select(items => new
                {
                    CourseName = items.Key.CourseName,
                    Year = items.Key.Year,
                    Count = items.Count()
                }).OrderBy(o => o.Year).ToList();
                var BBAinfo = AdmInfo.Where(a => a.CourseName == "BBA").ToList();
                var MBAinfo = AdmInfo.Where(a => a.CourseName == "MBA").ToList();
                var PHTinfo = AdmInfo.Where(a => a.CourseName == "PHT").ToList();
                int[] BBABarX = new int[6];
                int[] MBABarY = new int[6];
                int[] PHTBarZ = new int[6];

                Model.LineChartLbl = new string[6];
                decimal[] BarY = new decimal[6];
                int j = 1;
                for (int i = (DateTime.Now.Year - 5); i <= DateTime.Now.Year; i++)
                {
                    Model.LineChartLbl[j - 1] = i.ToString();
                    var a = BBAinfo.Where(n => n.Year == i).FirstOrDefault();
                    var b = MBAinfo.Where(n => n.Year == i).FirstOrDefault();
                    var c = PHTinfo.Where(n => n.Year == i).FirstOrDefault();
                    if (a != null)
                        BBABarX[j - 1] = a.Count;
                    else
                        BBABarX[j - 1] = 0;

                    if (b != null)
                        MBABarY[j - 1] = b.Count;
                    else
                        MBABarY[j - 1] = 0;

                    if (c != null)
                        PHTBarZ[j - 1] = c.Count;
                    else
                        PHTBarZ[j - 1] = 0;

                    j++;
                }
                Model.line = new List<Line>
                {
                    new Line{ Label="BBA",value=BBABarX },
                    new Line{Label="MBA",value=MBABarY},
                    new Line{Label="PHT",value=PHTBarZ}
                };
            }
            return Model;
        }

        public BarChartViewModel GetBarChartData(ReportFilterViewModel filterModel)
        {
            var data = _context.FeeCollections.Where(f => f.IsActive)
                .Select(item => new
                {
                    EnteredDate = item.EnteredDate,
                    Amount = item.PartWisePayments.Count > 0 ? item.PartWisePayments.Sum(p => p.Amount).Value : item.Amount,
                    AdmInfo = _context.AddmissionDetails.Where(a => a.AddmissionMaster.RegistrationNo == item.FeePaymentDetail.FeeDetail.RegistrationNo).Select(s => new
                    {
                        BatchId = s.BatchId,
                        State = s.AddmissionMaster.AddressDetails.FirstOrDefault().PerState,
                        city = s.AddmissionMaster.AddressDetails.FirstOrDefault().PerCity
                    }).FirstOrDefault(),
                    CourseId = item.FeePaymentDetail.FeeDetail.FeeTypeDetail.CourseId
                }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterModel.CourseId))
            {
                var courseArr = filterModel.CourseId.Split(',').Select(Int32.Parse).ToList();
                if (courseArr.Count > 0)
                    data = data.Where(t => courseArr.Any(c => c == t.CourseId));
            }
            if (!string.IsNullOrWhiteSpace(filterModel.BatchId))
            {
                var batchArr = filterModel.BatchId.Split(',').Select(Int32.Parse).ToList();
                if (batchArr.Count > 0)
                    data = data.Where(t => batchArr.Any(c => c == t.AdmInfo.BatchId));
            }
            if (!string.IsNullOrEmpty(filterModel.State))
            {
                data = data.Where(x => x.AdmInfo.State == filterModel.State);
                if (!string.IsNullOrEmpty(filterModel.City))
                    data = data.Where(x => x.AdmInfo.city == filterModel.City);
            }
            BarChartViewModel model = new BarChartViewModel();
            if (filterModel.Month > 0)
            {
                if (filterModel.Year > 0)
                    data = data.Where(d => d.EnteredDate.Year == filterModel.Year);
                var feeInfo = data.Where(d => d.EnteredDate.Month == filterModel.Month)
                    .GroupBy(x => x.EnteredDate.Day, (name, items) => new
                    {
                        Day = name,
                        Amount = items.Sum(i => i.Amount)
                    }).OrderBy(o => o.Day).ToList();
                decimal[] BarY = new decimal[DateTime.DaysInMonth(filterModel.Year, filterModel.Month)];
                for (int i = 1; i <= DateTime.DaysInMonth(filterModel.Year, filterModel.Month); i++)
                {
                    var a = feeInfo.Where(n => n.Day == i).FirstOrDefault();
                    if (a != null)
                        BarY[i - 1] = a.Amount;
                    else
                        BarY[i - 1] = 0;
                }
                int daysMonth = DateTime.DaysInMonth(filterModel.Year, filterModel.Month);
                string[] days = new string[daysMonth];
                for (int i = 1; i <= daysMonth; i++)
                    days[i - 1] = i.ToString();
                model.BarChartLbl = days;
                model.DataY = BarY;
            }
            else if (filterModel.Year > 0)
            {
                var feeInfo = data.Where(d => d.EnteredDate.Year == filterModel.Year)
                  .GroupBy(x => x.EnteredDate.Month, (name, items) => new
                  {
                      Month = name,
                      Amount = items.Sum(i => i.Amount)
                  }).OrderBy(o => o.Month).ToList();
                decimal[] BarY = new decimal[12];
                for (int i = 1; i <= 12; i++)
                {
                    var a = feeInfo.Where(n => n.Month == i).FirstOrDefault();
                    if (a != null)
                        BarY[i - 1] = a.Amount;
                    else
                        BarY[i - 1] = 0;
                }
                model.BarChartLbl = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "Setember", "October", "November", "December" };
                model.DataY = BarY;
            }
            else
            {
                var feeInfo = data.GroupBy(x => x.EnteredDate.Year, (name, items) => new
                {
                    Year = name,
                    Amount = items.Sum(i => i.Amount)
                }).OrderBy(o => o.Year).ToList();
                decimal[] BarY = new decimal[6];
                string[] chLbl = new string[6];
                int j = 1;
                for (int i = (DateTime.Now.Year - 5); i <= DateTime.Now.Year; i++)
                {
                    chLbl[j - 1] = i.ToString();
                    var a = feeInfo.Where(n => n.Year == i).FirstOrDefault();
                    if (a != null)
                        BarY[j - 1] = a.Amount;
                    else
                        BarY[j - 1] = 0;
                    j++;
                }
                model.BarChartLbl = chLbl;
                model.DataY = BarY;
            }
            return model;
        }

        public int[] GetDonutChartData(ReportFilterViewModel filterModel)
        {
            int RejectedCandidate = 0;
            int PendingCandidate = 0;
            int AdmissionCandidate = 0;
            int[] result = new int[3];
            int minYear = DateTime.Now.AddYears(-5).Year;
            int currentYear = DateTime.Now.Year;
            var registerData = _context.RegistrationMasters.Where(r => r.RegistrationDate.Value.Year >= minYear && r.RegistrationDate.Value.Year <= currentYear).AsQueryable();

            var admissionData = _context.AddmissionMasters.Where(a => a.IsActive && (!a.RegistrationMaster.IsMedicalClear.HasValue || a.RegistrationMaster.IsMedicalClear.Value) && a.AddmissionDetails.FirstOrDefault().BatchMaster.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.Name != "Batch 0");
            admissionData = admissionData.Where(a => a.AddmissionDate.Value.Year >= minYear && a.AddmissionDate.Value.Year <= currentYear);

            if (filterModel.Year > 0)
            {
                registerData = registerData.Where(d => d.RegistrationDate.Value.Year == filterModel.Year);
                admissionData = admissionData.Where(d => d.AddmissionDate.Value.Year == filterModel.Year);
            }

            if (!string.IsNullOrWhiteSpace(filterModel.CourseId))
            {
                var courseArr = filterModel.CourseId.Split(',').Select(Int32.Parse).ToList();
                if (courseArr.Count > 0)
                {
                    registerData = registerData.Where(t => courseArr.Any(c => c == t.CourseId));
                    admissionData = admissionData.Where(t => courseArr.Any(c => c == t.CourseId));
                }
            }
            if (!string.IsNullOrEmpty(filterModel.State))
            {
                registerData = registerData.Where(x => x.PermanentState == filterModel.State);
                admissionData = admissionData.Where(x => x.AddressDetails.Any() && x.AddressDetails.FirstOrDefault().PerState == filterModel.State);
                if (!string.IsNullOrEmpty(filterModel.City))
                {
                    registerData = registerData.Where(x => x.PermanentCity == filterModel.City);
                    admissionData = admissionData.Where(x => x.AddressDetails.Any() && x.AddressDetails.FirstOrDefault().PerCity == filterModel.City);
                }
            }

            if (filterModel.Month > 0)
            {
                registerData = registerData.Where(d => d.RegistrationDate.Value.Month == filterModel.Month);
                admissionData = admissionData.Where(d => d.AddmissionDate.Value.Month == filterModel.Month);
            }
            RejectedCandidate = registerData.Where(r => (r.IsScreenningClear.HasValue && !r.IsScreenningClear.Value) || (r.IsMedicalClear.HasValue && !r.IsMedicalClear.Value)).Count();
            PendingCandidate = registerData.Where(r =>r.IsActive && (!r.IsScreenningClear.HasValue || !r.IsMedicalClear.HasValue)).Count();
            AdmissionCandidate = admissionData.Count();
            result[0] = PendingCandidate;
            result[1] = RejectedCandidate;
            result[2] = AdmissionCandidate;
            return result;
        }

        public RevenueReportViewModel GetFilterizeChartData(ReportFilterViewModel filterModel)
        {
            var RevenueReport = new RevenueReportViewModel();
            RevenueReport.BarChartData = GetBarChartData(filterModel);
            RevenueReport.DonutChartData = GetDonutChartData(filterModel);
            RevenueReport.LineChartData = GetLineChartData(filterModel);
            return RevenueReport;
        }

        public DataTableFilterModel GetSSAReportList(DataTableFilterModel filter , int SessionYr)
        {
            var info = ((_context.AddmissionMasters.Join(_context.CourseMasters,
                am => am.CourseId, cm => cm.Id, (am, cm) => new { am, cm })).Join(
                (_context.AddmissionDetails.Join(_context.BatchMasters, ad => ad.BatchId,
                  bm => bm.Id, (ad, bm) => new { ad, bm }).Where(a => a.bm.IsActive).Join(
                  _context.SessionMasters, comb => comb.ad.SessionId, sm => sm.Id,
                  (comb, sm) => new { comb, sm }).Where(i => i.sm.IsActive).Join
                (_context.AddressDetails.DefaultIfEmpty(), combined => combined.comb.ad.AddmissionId,
               add => add.AddmissionId, (combined, add) => new
               {
                   AdmissionId = combined.comb.ad.AddmissionId,
                   AdmissionActive = combined.comb.ad.AddmissionMaster.IsActive,
                   BatchId = combined.comb.bm.Id,
                   SessionId = combined.sm.Id,
                   SessionYr = combined.sm.SessionYr,
                   BatchName = combined.comb.bm.Name,
                   SessionName = combined.sm.SessionName,
                   SessionActive = combined.sm.IsActive,
                   Address1 = add.CopAddress,
                   Address2 = add.CopCity,
                   Address3 = add.CopState,
                   Pincode = add.CopZip,
                   ValidPassport = combined.comb.ad.AddmissionMaster.IsValidPassport,
                   MedicalCenter = combined.comb.ad.AddmissionMaster.MedicalDetails.FirstOrDefault().MedicalCenter,
                   MedicalStatus = combined.comb.ad.AddmissionMaster.MedicalDetails.FirstOrDefault().MedicalStatus,
                   MedicalDate = combined.comb.ad.AddmissionMaster.MedicalDetails.FirstOrDefault().MedicalDate,
                   FitnessDate = combined.comb.ad.AddmissionMaster.MedicalDetails.FirstOrDefault().FitnessDate,
                   JoiningDate = combined.comb.ad.AddmissionMaster.MedicalDetails.FirstOrDefault().DateOfJoining
               }).Where(i => i.AdmissionActive && i.SessionActive && i.BatchId != 19)), info1 => info1.am.Id, info2 => info2.AdmissionId,
                (info1, info2) => new { info1, info2 })).Join(_context.ScreeningTests,
                res1 => res1.info1.am.RegistrationNo, res2 => res2.RegistrationId,
                (res1, res2) => new { res1 = res1, res2 = res2 }).GroupBy(g => g.res1.info1.am.Id).Select(item => new ClientReportViewModel
                {
                    Id = item.FirstOrDefault().res1.info1.am.Id,
                    BatchId = item.FirstOrDefault().res1.info2.BatchId,
                    BatchName = item.FirstOrDefault().res1.info2.BatchName,
                    InterViewDate = item.FirstOrDefault().res2.ScreeningDate,
                    Gender = item.FirstOrDefault().res1.info1.am.Gender,
                    Fname = item.FirstOrDefault().res1.info1.am.Fname,
                    Lname = item.FirstOrDefault().res1.info1.am.Lname,
                    DOB = item.FirstOrDefault().res1.info1.am.DOB,
                    AdmissionDate = item.FirstOrDefault().res1.info1.am.AddmissionDate,
                    Addresee1 = item.FirstOrDefault().res1.info2.Address1,
                    Addresee2 = item.FirstOrDefault().res1.info2.Address2,
                    Addresee3 = item.FirstOrDefault().res1.info2.Address3,
                    PinCode = item.FirstOrDefault().res1.info2.Pincode,
                    Contact = item.FirstOrDefault().res1.info1.am.MobileNo,
                    Email = item.FirstOrDefault().res1.info1.am.Email,
                    Designation = "TCC",
                    DepartMent = "IFSD",
                    Location = "GGN",
                    MedicalCenter = item.FirstOrDefault().res1.info2.MedicalCenter,
                    MedicalStatus = item.FirstOrDefault().res1.info2.MedicalStatus,
                    MDate = item.FirstOrDefault().res1.info2.MedicalDate,
                    FDate = item.FirstOrDefault().res1.info2.FitnessDate,
                    JDate = item.FirstOrDefault().res1.info2.JoiningDate,
                    CourseName = item.FirstOrDefault().res1.info1.am.CourseMaster.CourseName,
                    CourseId = item.FirstOrDefault().res1.info1.am.CourseId,
                    IsValidPassport = item.FirstOrDefault().res1.info2.ValidPassport,
                    SessionYr = item.FirstOrDefault().res1.info2.SessionYr
                }).AsEnumerable();

            if (SessionYr > 0)
                info = info.Where(d => d.SessionYr == SessionYr);

            var totalCount = info.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
                info = info.Where(d => (!string.IsNullOrEmpty(d.FullName) && d.FullName.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Email) && d.Email.ToLower().Contains(filter.search.value.ToLower())) || (!string.IsNullOrEmpty(d.Contact) && d.Contact.ToLower().Contains(filter.search.value.ToLower())));

            if (!string.IsNullOrWhiteSpace(filter.columns[6].search.value))
            {
                int yr = Convert.ToInt32(filter.columns[6].search.value);
                info = info.Where(t => t.AdmissionDate.Value.Year == yr);

                if (!string.IsNullOrWhiteSpace(filter.columns[7].search.value))
                {
                    int mon = Convert.ToInt32(filter.columns[7].search.value);
                    info = info.Where(t => t.AdmissionDate.Value.Year == yr && t.AdmissionDate.Value.Month == mon);
                }
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[2].search.value) && filter.columns[2].search.value != "" && !string.IsNullOrWhiteSpace(filter.columns[3].search.value) && filter.columns[3].search.value != "")
            {
                DateTime fromDate = DateTime.ParseExact(filter.columns[2].search.value, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                DateTime toDate = DateTime.ParseExact(filter.columns[3].search.value, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                info = info.Where(t => t.AdmissionDate.Value.Date >= fromDate && t.AdmissionDate.Value.Date <= toDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[14].search.value))
            {
                var courseArr = filter.columns[14].search.value.Split(',');
                if (courseArr.Length > 0)
                    info = info.Where(t => courseArr.Any(c => c == t.CourseId.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[1].search.value))
            {
                var batchArr = filter.columns[1].search.value.Split(',');
                if (batchArr.Length > 0)
                    info = info.Where(t => batchArr.Any(c => c == t.BatchId.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(filter.columns[10].search.value))
                info = info.Where(t => t.Addresee3 == filter.columns[10].search.value);


            var o = filter.order[0];
            var name = filter.columns[filter.order[0].column].data;
            if (o.dir == "asc")
                info = info.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
            else
                info = info.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
            var filteredCount = info.Count();
            filter.recordsTotal = totalCount;
            filter.recordsFiltered = filteredCount;
            var dataFilter = info.Skip(filter.start).Take(filter.length).ToList();
            foreach (var i in dataFilter)
            {
                i.Title = i.Gender == "M" ? "Mr" : "Ms";
                i.DateOfBirth = i.DOB.HasValue ? i.DOB.Value.ToString("dd-MM-yyyy") : "";
                i.DateOfInterView = i.InterViewDate.ToString("dd-MM-yyyy");
                if (i.MDate.HasValue)
                    i.MedicalDate = i.MDate.Value.Day.ToString().PadLeft(2, '0') + "/" + i.MDate.Value.Month.ToString().PadLeft(2, '0') + "/" + i.MDate.Value.Year;
                if (i.FDate.HasValue)
                    i.FitnessDate = i.FDate.Value.Day.ToString().PadLeft(2, '0') + "/" + i.FDate.Value.Month.ToString().PadLeft(2, '0') + "/" + i.FDate.Value.Year;
                if (i.JDate.HasValue)
                    i.JoiningDate = i.JDate.Value.Day.ToString().PadLeft(2, '0') + "/" + i.JDate.Value.Month.ToString().PadLeft(2, '0') + "/" + i.JDate.Value.Year;
                i.NOC_PP = i.IsValidPassport ? "Passport" : "";
            }
            filter.data = dataFilter;
            return filter;
        }

        public List<RoleViewModel> GetYearList()
        {
            List<RoleViewModel> years = new List<RoleViewModel>();
            for (int i = -2; i <= 5; ++i)
            {
                string yr = DateTime.Now.AddYears(i).ToString("yyyy");
                RoleViewModel model = new RoleViewModel
                {
                    Id = Convert.ToInt32(yr),
                    Name = yr
                };
                years.Add(model);
            }
            return years;
        }
    }
}
