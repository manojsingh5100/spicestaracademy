using SJModel;
using SJModel.PerformanceModel;
using SJData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Globalization;

namespace SJService
{
    public class PerformanceService
    {
        SJStarERPEntities _context = null;
        public PerformanceService()
        {
            _context = new SJStarERPEntities();
        }

        public List<ReviewMasterViewModel> GetReViewList()
        {
            return _context.tblReviewMasters.Where(t => t.IsActive).Select(item => new ReviewMasterViewModel
            {
                Id = item.Id,
                Name = item.Name,
                IsActive = item.IsActive
            }).ToList();
        }

        public List<ReviewMasterViewModel> GetWeeklyTermTypeListById(int Id)
        {
            return _context.tblWeeklyReviewMasters.Where(t => t.IsActive && t.tblReviewMasterId == Id).Select(item => new ReviewMasterViewModel
            {
                Id = item.Id,
                Name = item.Name,
                IsActive = item.IsActive
            }).ToList();
        }

        public DataTableFilterModel GetCandidateList(DataTableFilterModel filter)
        {
            var info = _context.AddmissionMasters.Where(a => a.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.Name != "Batch 0").Select(item => new RegistrationViewModel
            {
                Id = item.Id,
                RegistartionNo = item.RegistrationNo,
                Fname = item.Fname + (string.IsNullOrEmpty(item.Lname) ? "" : (" " + item.Lname)), //+ " " + item.Lname,
                Email = item.Email,
                Mobile = item.MobileNo,
                CourseName = item.CourseMaster.CourseName,
                BatchName = item.AddmissionDetails.FirstOrDefault().BatchMaster.Name,
                AdmissionDate = item.AddmissionDate.Value,
                IsResistrationHistory = item.BatchHistoryDetails.Any(),
                PerformanceCardList = _context.tblPerformanceEntryMasters.Where(t => t.RegistrationNo == item.RegistrationNo && t.BatchId == item.AddmissionDetails.FirstOrDefault().BatchId).Select(i => new PerformanceCardListDetail
                {
                    ReviewId = i.ReviewId,
                    WeeklyTermId = i.tblWeeklyReviewMaster,
                    ReviewName = i.tblReviewMaster.Name,
                    WeeklyTermName = i.tblWeeklyReviewMaster1.Name
                }).ToList()
            }).AsEnumerable();
            var totalCount = info.Count();

            if (!string.IsNullOrWhiteSpace(filter.columns[7].search.value) && !string.IsNullOrWhiteSpace(filter.columns[8].search.value))
            {
                DateTime ToDate = DateTime.ParseExact(filter.columns[7].search.value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime FromDate = DateTime.ParseExact(filter.columns[8].search.value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (ToDate > FromDate)
                    info = info.Where(t => t.AdmissionDate >= FromDate && t.AdmissionDate <= ToDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.search.value))
                info = info.Where(d => !string.IsNullOrEmpty(d.Fname) && d.Fname.ToLower().Contains(filter.search.value.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.columns[5].search.value))
                info = info.Where(t => t.CourseName == filter.columns[5].search.value);

            if (!string.IsNullOrWhiteSpace(filter.columns[6].search.value))
                info = info.Where(t => t.BatchName == filter.columns[6].search.value);

            var ss = info.ToList();

            var o = filter.order[0];
            var name = filter.columns[filter.order[0].column].data;
            if (o.dir == "asc")
                info = info.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
            else
                info = info.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
            var filteredCount = info.Count();
            filter.recordsTotal = totalCount;
            filter.recordsFiltered = filteredCount;
            filter.data = info.Skip(filter.start).Take(filter.length).ToList();
            return filter;
        }

        public ResultViewModel SavePerformanceCardDetail(PerformanceCardViewModel Model)
        {
            ResultViewModel error = new ResultViewModel();
            try
            {
                tblPerformanceEntryMaster entryMaster = new tblPerformanceEntryMaster
                {
                    RegistrationNo = Model.RegNo,
                    ReviewId = Model.ReviewId,
                    tblPerformanceMasterId = Model.PerformanceMasterId,
                    EnteredDate = DateTime.Now,
                    EnteredBy = Model.UserId,
                    BatchId = Model.BatchId.Value,
                    AddmissionMasterId = Model.AdmissionId,
                    tblWeeklyReviewMaster = Model.WeeklyTermId > 0 ? Model.WeeklyTermId : (int?)null,
                    Percentage = Model.Percentage,
                    PerformanceReview = Model.PerformanceReview
                };
                _context.tblPerformanceEntryMasters.Add(entryMaster);
                foreach (var item in Model.ParameterTypeInfoList)
                {
                    tblPerformanceParameterTypeResultMaster parameterType = new tblPerformanceParameterTypeResultMaster
                    {
                        tblPerformanceEntryMasterId = entryMaster.Id,
                        tblPerformanceMasterId = item.PerformanceMasterId,
                        ParameterTypeId = item.ParameterTypeId,
                        EnteredBy = Model.UserId,
                        EnteredDate = DateTime.Now
                    };
                    _context.tblPerformanceParameterTypeResultMasters.Add(parameterType);
                    if (item.IsApplicable)
                    {
                        foreach (var obj in item.ParameterInfoList)
                        {
                            tblPerformanceParameterResultMaster parameter = new tblPerformanceParameterResultMaster
                            {
                                Rating = obj.Rating,
                                Remarks = obj.Remarks,
                                tblPerformanceParameterTypeResultMasterId = parameterType.Id,
                                tblParameterId = obj.TblParameterId,
                                EnteredBy = Model.UserId,
                                EnteredDate = DateTime.Now,
                                IsNotApplicable = false
                            };
                            _context.tblPerformanceParameterResultMasters.Add(parameter);
                        }
                    }
                    else
                    {
                        tblPerformanceParameterResultMaster parameter = new tblPerformanceParameterResultMaster
                        {
                            Rating = 0,
                            tblPerformanceParameterTypeResultMasterId = parameterType.Id,
                            tblParameterId = item.PtId,
                            EnteredBy = Model.UserId,
                            EnteredDate = DateTime.Now,
                            IsNotApplicable = true
                        };
                        _context.tblPerformanceParameterResultMasters.Add(parameter);
                    }
                    _context.SaveChanges();
                }
                error.IsSuccess = true;
                error.Massage = "Candidate performance card detail saved successfully!";
                return error;
            }
            catch (Exception ex)
            {
                error.IsSuccess = false;
                error.Massage = ex.Message + " " + ex.InnerException;
                return error;
            }
        }

        public ResultViewModel DisablePerformanceOption(int RegNo,int BatchId)
        {
            ResultViewModel res = new ResultViewModel();
            var Result = _context.tblPerformanceEntryMasters.Where(t => t.RegistrationNo == RegNo && t.BatchId == BatchId).ToList();
            res.WeeklyArr = string.Join(",", Result.Where(d => d.tblWeeklyReviewMaster.HasValue).Select(i => i.tblWeeklyReviewMaster).ToArray());
            res.ReviewArr = string.Join(",", Result.Where(d => !d.tblWeeklyReviewMaster.HasValue).Select(i => i.ReviewId).ToArray());
            return res;
        }

        public bool IsParmeterKeyExist(string KeyId)
        {
            return _context.tblParameterLists.Any(t => t.KeyId == KeyId);
        }

        public PerformanceCardViewModel GetCandidatePerformanceIntialPageInfo(int RegNo, string Review)
        {
            PerformanceCardViewModel result = new PerformanceCardViewModel();
            if (string.IsNullOrEmpty(Review))
            {
                result = _context.AddmissionMasters.Where(r => r.RegistrationNo == RegNo).Select(item => new PerformanceCardViewModel
                {
                    BatchName = item.AddmissionDetails.FirstOrDefault().BatchMaster.Name,
                    BatchId = item.AddmissionDetails.FirstOrDefault().BatchId,
                    RegNo = item.RegistrationNo,
                    StudentName = item.Fname + " " + item.Lname,
                    Fname = item.Fname,
                    Lname = item.Lname,
                    PhotoPath = item.ImageUrl,
                    Email = item.Email,
                    Mobile = item.MobileNo,
                    AdmissionDate = item.AddmissionDate,
                    AdmissionId = item.Id,
                    PercentageCritiriaInfo = item.AddmissionDetails.FirstOrDefault().BatchMaster.tblPerformancePercentageMasters.Select(s => new PercentageInfoViewModel
                    {
                        BatchId = s.BatchId,
                        FromPercentage = s.FromPercentage,
                        TillPercentage = s.TillPercentage,
                        tblReviewMasterId = s.tblReviewMasterId,
                        tblPerformanceMasterId = s.tblPerformanceMasterId
                    }).ToList(),
                    ParmeterTypeList = _context.tblParameterTypes.Where(t => t.IsActive).Select(p => new ParameterTypeViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        IsActive = p.IsActive,
                        ParameterList = p.tblParameterLists.Where(pl => pl.IsActive).Select(p1 => new ParameterListViewModel
                        {
                            ParameterId = p1.Id,
                            KeyId = p1.KeyId,
                            Name = p1.Name,
                            IsActive = p1.IsActive,
                            IsApplicable = p1.Gender == null ? true : (item.Gender == p1.Gender ? true : false)
                        }).ToList()
                    }).ToList()
                }).FirstOrDefault();
            }
            else
            {
                int RevIdNew = Convert.ToInt32(Review);
                result = _context.AddmissionMasters.Where(r => r.RegistrationNo == RegNo).Select(item => new PerformanceCardViewModel
                {
                    BatchName = item.AddmissionDetails.FirstOrDefault().BatchMaster.Name,
                    BatchId = item.AddmissionDetails.FirstOrDefault().BatchId,
                    RegNo = item.RegistrationNo,
                    StudentName = item.Fname + " " + item.Lname,
                    Fname = item.Fname,
                    Lname = item.Lname,
                    PhotoPath = item.ImageUrl,
                    Email = item.Email,
                    Mobile = item.MobileNo,
                    AdmissionDate = item.AddmissionDate,
                    AdmissionId = item.Id,
                    PercentageCritiriaInfo = item.AddmissionDetails.FirstOrDefault().BatchMaster.tblPerformancePercentageMasters.Select(s => new PercentageInfoViewModel
                    {
                        BatchId = s.BatchId,
                        FromPercentage = s.FromPercentage,
                        TillPercentage = s.TillPercentage,
                        tblReviewMasterId = s.tblReviewMasterId,
                        tblPerformanceMasterId = s.tblPerformanceMasterId
                    }).ToList(),
                    ParmeterTypeList = item.tblPerformanceEntryMasters.Where(w => w.ReviewId == RevIdNew).FirstOrDefault().tblPerformanceParameterTypeResultMasters.GroupBy(g => g.ParameterTypeId).Select(p => new ParameterTypeViewModel
                    {
                        Id = p.Key,
                        Name = p.FirstOrDefault().tblParameterType.Name,
                        IsActive = p.FirstOrDefault().tblParameterType.IsActive,
                        ParameterList = p.Select(p1 => new ParameterListViewModel
                        {
                            ParameterId = p1.tblPerformanceParameterResultMasters.FirstOrDefault().tblParameterId,
                            KeyId = p1.tblPerformanceParameterResultMasters.FirstOrDefault().tblParameterList.KeyId,
                            Name = p1.tblPerformanceParameterResultMasters.FirstOrDefault().tblParameterList.Name,
                            IsActive = p1.tblPerformanceParameterResultMasters.FirstOrDefault().tblParameterList.IsActive,
                            IsApplicable = p1.tblPerformanceParameterResultMasters.FirstOrDefault().tblParameterList.Gender == null ? true : (item.Gender == p1.tblPerformanceParameterResultMasters.FirstOrDefault().tblParameterList.Gender ? true : false)
                        }).ToList()
                    }).ToList()
                }).FirstOrDefault();
            }
            if (result != null)
            {
                result.UIN = GetCandidateKey(result.BatchName, result.RegNo, result.AdmissionDate);
                result.StudentName = result.Fname + " " + result.Lname;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                result.PercentageCritiriaJsonInfo = serializer.Serialize(result.PercentageCritiriaInfo.Select(a => new
                {
                    PeformanceId = a.tblPerformanceMasterId,
                    FromPercenage = a.FromPercentage,
                    ToPercentage = a.TillPercentage
                }).ToList());
            }
            if (!string.IsNullOrEmpty(Review))
            {
                var data = _context.tblPerformanceEntryMasters.Where(t => t.RegistrationNo == RegNo);
                string[] arr = Review.Split('-');
                int RevId = Convert.ToInt32(arr[0]);
                data = data.Where(d => d.ReviewId == RevId);
                if (arr.Length == 2)
                {
                    int WeeklyId = Convert.ToInt32(arr[1]);
                    data = data.Where(d => d.tblWeeklyReviewMaster == WeeklyId);
                }
                result.PerformanceReview = data.FirstOrDefault().PerformanceReview;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                result.GetRatingData = serializer.Serialize(data.FirstOrDefault().tblPerformanceParameterTypeResultMasters.Select(item => new ParameterTypeViewModel
                {
                    Id = item.ParameterTypeId,
                    Name = item.tblParameterType.Name,
                    ParameterList = item.tblPerformanceParameterResultMasters.Select(p => new ParameterListViewModel
                    {
                        KeyId = p.tblParameterList.KeyId,
                        ParameterId = p.tblParameterId,
                        Name = p.tblParameterList.Name,
                        Remark = p.Remarks,
                        Rating = p.Rating,
                        IsApplicable = p.IsNotApplicable
                    }).ToList()
                }).ToList());
            }
            return result;
        }

        public string GetCandidateKey(string Batch, int RegNo, DateTime? AdmDate)
        {
            string res = "SSA";
            if (!string.IsNullOrEmpty(Batch) && AdmDate.HasValue)
            {

                string[] b = Batch.Split(' ');
                if (b.Length > 0)
                    res += "|" + "B" + (b[1].Length == 1 ? "0" + b[1] : b[1]);
                res += "|" + AdmDate.Value.ToString("MM") + AdmDate.Value.ToString("yy");
                res += "|" + RegNo;
                return res;
            }
            else
                return "";
        }

        public List<PercentageInfoViewModel> GetPercentageCritiria(int BatchId, int ReviewId)
        {
            return _context.tblPerformancePercentageMasters.Where(t => t.BatchMaster.IsActive && t.BatchId == BatchId && t.tblReviewMasterId == ReviewId).OrderBy(o => o.Id).Select(item => new PercentageInfoViewModel
            {
                Id = item.Id,
                FromPercentage = item.FromPercentage,
                TillPercentage = item.TillPercentage
            }).ToList();
        }

        public dynamic UpdatePercentageCritiria(int BatchId, int ReviewId)
        {
            return _context.tblPerformancePercentageMasters.Where(t => t.BatchMaster.IsActive && t.BatchId == BatchId && t.tblReviewMasterId == ReviewId).OrderBy(o => o.Id).Select(item => new
            {
                PeformanceId = item.tblPerformanceMasterId,
                FromPercenage = item.FromPercentage,
                ToPercentage = item.TillPercentage
            }).ToList();
        }

        public List<ReligateInfoViewModel> GetBatchReligateDetails(int RegistratiNo)
        {
            List<ReligateInfoViewModel> Responce = new List<ReligateInfoViewModel>();
            var data = _context.BatchHistoryDetails.Where(b => b.AddmissionMaster.RegistrationNo == RegistratiNo).Select(item => new ReligateInfoViewModel
            {
                FromBatchName = item.BatchMaster.Name,
                ToBatchName = item.BatchMaster1.Name,
                AdmissionId = item.AdmissionId,
                Id = item.Id,
                FromBatchId = item.BatchFromId,
                ToBatchId = item.BatchToId
            }).OrderBy(o => o.Id).ToList();
            if (data.Count() > 0)
            {
                int no = 1;
                for (int i = 0; i < data.Count(); i++)
                {
                    int FromBatchId = data[i].FromBatchId;
                    Responce.Add(new ReligateInfoViewModel
                    {
                        FromBatchName = data[i].FromBatchName,
                        Id = no,
                        RegistrationNo = RegistratiNo,
                        PerformanceCardList = _context.tblPerformanceEntryMasters.Where(t => t.RegistrationNo == RegistratiNo && t.BatchId == FromBatchId).Select(g => new PerformanceCardListDetail
                        {
                            ReviewId = g.ReviewId,
                            WeeklyTermId = g.tblWeeklyReviewMaster,
                            ReviewName = g.tblReviewMaster.Name,
                            WeeklyTermName = g.tblWeeklyReviewMaster1.Name
                        }).ToList()
                    });
                    no++;
                    if (i == (data.Count() - 1))
                    {
                        int ToBatchId = data[i].ToBatchId;
                        Responce.Add(new ReligateInfoViewModel
                        {
                            FromBatchName = data[i].ToBatchName,
                            Id = no,
                            RegistrationNo = RegistratiNo,
                            PerformanceCardList = _context.tblPerformanceEntryMasters.Where(t => t.RegistrationNo == RegistratiNo && t.BatchId == ToBatchId).Select(g => new PerformanceCardListDetail
                            {
                                ReviewId = g.ReviewId,
                                WeeklyTermId = g.tblWeeklyReviewMaster,
                                ReviewName = g.tblReviewMaster.Name,
                                WeeklyTermName = g.tblWeeklyReviewMaster1.Name
                            }).ToList()
                        });
                    }
                }
            }
            return Responce.OrderByDescending(o => o.Id).ToList();
        }
    }
}

