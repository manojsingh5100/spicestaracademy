using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJData;
using SJModel;
using SJModel.PerformanceModel;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Data;
using System.Reflection;

namespace SJService
{
    public class PerformanceReportService
    {
        private SJStarERPEntities _context = null;
        public PerformanceReportService()
        {
            _context = new SJStarERPEntities();
        }

        public decimal GetPercentage(decimal CountNum, decimal Rate)
        {
            return (Rate * 100) / CountNum;
            // return (Rate * 100) / (CountNum * 3);
        }

        public decimal GetBarChartPercentage(decimal CountNum, decimal Rate)
        {
            return (Rate * 100) / (CountNum * 3);
        }

        public BarChartViewModel GetAssesmentCompareChartInfo(ReportFilterViewModel obj)
        {
            BarChartViewModel model = new BarChartViewModel();
            return model;
        }

        public dynamic GetBatchChartData(ReportFilterViewModel obj)
        {
            int[] BatchIds = null;
            string[] BatchRangeArray = new string[3] { ("Batch " + obj.BatchRangeArray[0]), ("Batch " + obj.BatchRangeArray[1]), ("Batch " + obj.BatchRangeArray[2]) };
            var BatchChartInfo = _context.tblPerformanceParameterTypeResultMasters.Where(a => a.tblPerformanceEntryMaster.ReviewId != 1 && !a.tblPerformanceParameterResultMasters.FirstOrDefault().IsNotApplicable).AsQueryable();
            if (obj.ReviewId > 0)
                BatchChartInfo = BatchChartInfo.Where(a => a.tblPerformanceEntryMaster.ReviewId == obj.ReviewId);
            if (BatchRangeArray.Length > 0)
                BatchIds = GetBatchArrIds(BatchRangeArray);
            var data = BatchChartInfo.Where(a => BatchIds.Contains(a.tblPerformanceEntryMaster.BatchId.Value) && a.tblPerformanceEntryMaster.BatchId == a.tblPerformanceEntryMaster.AddmissionMaster.AddmissionDetails.FirstOrDefault().BatchId).GroupBy(g => g.tblPerformanceEntryMaster.BatchMaster.Name).Select(item => new
            {
                BatchName = item.Key,
                Percentage = Math.Round((item.Sum(s => s.tblPerformanceParameterResultMasters.Sum(a => a.Rating)) * 100) / (item.Count() * 3.00), 2),
            }).ToList();
            return data;
        }

        public dynamic GetBatchCandidateDataByBatch(ReportFilterViewModel obj)
        {
            var BatchChartInfo = _context.tblPerformanceParameterTypeResultMasters.Where(a => a.tblPerformanceEntryMaster.ReviewId != 1 && a.tblPerformanceEntryMaster.BatchMaster.Name == obj.BatchId && !a.tblPerformanceParameterResultMasters.FirstOrDefault().IsNotApplicable).AsQueryable();
            if (obj.ReviewId > 0)
                BatchChartInfo = BatchChartInfo.Where(a => a.tblPerformanceEntryMaster.ReviewId == obj.ReviewId);
            var data = BatchChartInfo.GroupBy(g => g.tblPerformanceEntryMaster.AddmissionMaster.RegistrationNo).Select(item => new
            {
                RegistrationNo = item.Key,
                Fname = item.FirstOrDefault().tblPerformanceEntryMaster.AddmissionMaster.Fname,
                Lname = item.FirstOrDefault().tblPerformanceEntryMaster.AddmissionMaster.Lname,
                Percentage = Math.Round((item.Sum(s => s.tblPerformanceParameterResultMasters.Sum(a => a.Rating)) * 100) / (item.Count() * 3.00), 2)
            }).OrderBy(o => o.Fname).ToList();
            return data;
        }

        public BarChartViewModel Getdatabycandidatename(string BatchName, string CandidateName, int ReviewId)
        {
            BarChartViewModel model = new BarChartViewModel();
            int BatchId = 0;
            var data = _context.tblPerformanceParameterTypeResultMasters.Where(w => !w.tblPerformanceParameterResultMasters.FirstOrDefault().IsNotApplicable).AsQueryable();
            if (BatchName != null)
            {
                BatchId = GetBatchIdByName(BatchName);
                data = data.Where(w => w.tblPerformanceEntryMaster.BatchId == BatchId);
            }
            var PieChartInfo = data;
            if (CandidateName != null)
            {
                data = data.Where(t => (t.tblPerformanceEntryMaster.AddmissionMaster.Fname + " " + t.tblPerformanceEntryMaster.AddmissionMaster.Lname) == CandidateName || t.tblPerformanceEntryMaster.AddmissionMaster.Fname == CandidateName);
            }
            if (ReviewId > 0)
                data = data.Where(t => t.tblPerformanceEntryMaster.ReviewId == ReviewId);

            var result = data.Select(item => new ParameterTypeInfoList
            {
                BatchId = _context.AddmissionDetails.Where(a => a.AddmissionMaster.RegistrationNo == item.tblPerformanceEntryMaster.RegistrationNo).FirstOrDefault().BatchId,
                ParameterType = item.tblParameterType.Name,
                PerformanceMasterId = item.tblPerformanceMasterId,
                RegNo = item.tblPerformanceEntryMaster.RegistrationNo,
                ParameterInfoList = item.tblPerformanceParameterResultMasters.Select(ob => new ParameterInfoList
                {
                    Rating = ob.Rating,
                    Remarks = ob.tblParameterList.Name,
                    TblParameterId = ob.tblParameterId,
                    ReviewName = ob.tblPerformanceParameterTypeResultMaster.tblPerformanceEntryMaster.tblReviewMaster.Name
                }).ToList(),
                ReviewId = item.tblPerformanceEntryMaster.ReviewId
            }).ToList();

            if (result.Count() > 0)
                model.RegistraionNo = result.FirstOrDefault().RegNo;

            decimal[] BarY = null;
            decimal[] BarX = null;
            if (result.Count > 0)
            {
                model.BarChartLbl = result.Select(d => d.ParameterType).Distinct().ToArray();
                BarY = new decimal[model.BarChartLbl.Length];
                BarX = new decimal[model.BarChartLbl.Length];
            }
            else
            {
                model.BarChartLbl = _context.tblParameterTypes.Where(p => p.IsActive).Select(d => d.Name).Distinct().ToArray();
                BarY = new decimal[model.BarChartLbl.Length];
                BarX = new decimal[model.BarChartLbl.Length];
            }

            if (ReviewId == 0)
            {
                model.SubchartString = JsonConvert.SerializeObject(result.GroupBy(x => x.ParameterType, (name, items) => new
                {
                    Name = name,
                    list = items.Select(sa => sa.ParameterInfoList).ToList()
                }).ToList());

                var result1 = result.Where(r => r.ReviewId == 2).ToList();
                var result2 = result.Where(r => r.ReviewId == 3).ToList();
                for (int i = 0; i < model.BarChartLbl.Count(); i++)
                {
                    int res1Count = result1.Where(r => r.ParameterType == model.BarChartLbl[i]).Count();
                    int res2Count = result2.Where(r => r.ParameterType == model.BarChartLbl[i]).Count();
                    int res1Sum = result1.Where(r => r.ParameterType == model.BarChartLbl[i]).Sum(s => s.ParameterInfoList.Sum(p => p.Rating));
                    int res2Sum = result2.Where(r => r.ParameterType == model.BarChartLbl[i]).Sum(s => s.ParameterInfoList.Sum(p => p.Rating));
                    BarY[i] = res1Count > 0 ? Math.Round(GetBarChartPercentage(res1Count, res1Sum), 2) : 0;
                    BarX[i] = res2Count > 0 ? Math.Round(GetBarChartPercentage(res2Count, res2Sum), 2) : 0;
                }
            }
            else if (ReviewId == 2)
            {
                var result1 = result.Where(r => r.ReviewId == 2).ToList();
                model.SubchartString = JsonConvert.SerializeObject(result1.GroupBy(x => x.ParameterType, (name, items) => new
                {
                    Name = name,
                    list = items.Select(sa => sa.ParameterInfoList).ToList()
                }).ToList());
                for (int i = 0; i < model.BarChartLbl.Count(); i++)
                {
                    int res1Count = result1.Where(r => r.ParameterType == model.BarChartLbl[i]).Count();
                    int res1Sum = result1.Where(r => r.ParameterType == model.BarChartLbl[i]).Sum(s => s.ParameterInfoList.Sum(p => p.Rating));
                    BarY[i] = res1Count > 0 ? Math.Round(GetBarChartPercentage(res1Count, res1Sum), 2) : 0;
                    //BarY[i] = result1.Count() > 0 ? GetPercentage(result1[i].ParameterInfoList.Count, result1[i].ParameterInfoList.Sum(s => s.Rating)) : 0;
                }
                BarX = null;
            }
            else if (ReviewId == 3)
            {
                var result2 = result.Where(r => r.ReviewId == 3).ToList();
                model.SubchartString = JsonConvert.SerializeObject(result2.GroupBy(x => x.ParameterType, (name, items) => new
                {
                    Name = name,
                    list = items.Select(sa => sa.ParameterInfoList).ToList()
                }).ToList());
                for (int i = 0; i < model.BarChartLbl.Count(); i++)
                {
                    int res2Count = result2.Where(r => r.ParameterType == model.BarChartLbl[i]).Count();
                    int res2Sum = result2.Where(r => r.ParameterType == model.BarChartLbl[i]).Sum(s => s.ParameterInfoList.Sum(p => p.Rating));
                    BarX[i] = res2Count > 0 ? Math.Round(GetBarChartPercentage(res2Count, res2Sum), 2) : 0;
                }
                BarY = null;
            }
            model.DataY = BarY;
            model.DataX = BarX;

            // Do nut chart
            var PieChartData = PieChartInfo.Where(p => p.tblPerformanceEntryMaster.ReviewId == 3).ToList();
            decimal HighPercentage = 0;
            decimal MidPercentage = 0;
            decimal LowPercentage = 0;
            decimal[] PieData = new decimal[3];
            List<ParameterListViewModel> ParameterListModel = new List<ParameterListViewModel>();
            if (PieChartData.Count > 0)
            {
                foreach (var m in PieChartData)
                {
                    ParameterListModel.Add(m.tblPerformanceParameterResultMasters.Select(s => new ParameterListViewModel
                    {
                        Rating = s.Rating
                    }).FirstOrDefault());
                }
                var High = ParameterListModel.Where(h => h.Rating == 3).ToList();
                var Mid = ParameterListModel.Where(h => h.Rating == 2).ToList();
                var Low = ParameterListModel.Where(h => h.Rating == 1).ToList();

                HighPercentage = High.Count > 0 ? GetPercentage(High.Count, High.Sum(h => h.Rating)) : 0;
                MidPercentage = Mid.Count > 0 ? GetPercentage(Mid.Count, Mid.Sum(h => h.Rating)) : 0;
                LowPercentage = Low.Count > 0 ? GetPercentage(Low.Count, Low.Sum(h => h.Rating)) : 0;
            }
            PieData[0] = HighPercentage;
            PieData[1] = MidPercentage;
            PieData[2] = LowPercentage;
            model.PieChartData = PieData;
            return model;
        }

        public int[] GetBatchArrIds(string[] Batches)
        {
            return _context.BatchMasters.Where(m => Batches.Contains(m.Name)).Select(item => item.Id).ToArray();
        }

        public List<PieChartViewModel> GetPieChartData(ReportFilterViewModel obj)
        {
            string[] BatchRangeArray = new string[obj.BatchRangeArray.Length];
            int[] BatchIds = null;
            for (int i = 0; i < obj.BatchRangeArray.Length; i++)
            {
                BatchRangeArray[i] = "Batch " + obj.BatchRangeArray[i];
            }
            var info = _context.tblPerformanceEntryMasters.Include("AddmissionMaster").Include("AddmissionMaster.AddmissionDetail").Where(w => w.ReviewId != 1 && w.tblPerformanceParameterTypeResultMasters.Count() > 0 && !w.tblPerformanceParameterTypeResultMasters.FirstOrDefault().tblPerformanceParameterResultMasters.FirstOrDefault().IsNotApplicable).AsQueryable();
            if (obj.ReviewId > 0)
                info = _context.tblPerformanceEntryMasters.Where(w => w.ReviewId == obj.ReviewId);
            if (BatchRangeArray.Length > 0)
                BatchIds = GetBatchArrIds(BatchRangeArray);

            var data = info.Select(item => new PerformanceEntryMasterViewModel
            {
                AdmBatchId = item.AddmissionMaster.AddmissionDetails.FirstOrDefault().BatchId,
                BatchId = item.BatchId,
                AddmissionMasterId = item.AddmissionMasterId,
                EnteredBy = item.EnteredBy,
                EnteredDate = item.EnteredDate,
                Id = item.Id,
                Percentage = item.Percentage,
                RegistrationNo = item.RegistrationNo,
                ReviewId = item.ReviewId,
                tblPerformanceMasterId = item.tblPerformanceMasterId,
                tblWeeklyReviewMasterId = item.tblWeeklyReviewMasterId,
                PerformanceReview = item.PerformanceReview,
                tblWeeklyReviewMaster = item.tblWeeklyReviewMaster
            }).Where(w => BatchIds.Contains(w.BatchId.Value) && w.AdmBatchId == w.BatchId).ToList();

            List<PieChartViewModel> Model = new List<PieChartViewModel>();
            decimal Number = data.Count();
            int HighStudent = 0;
            int MidStudent = 0;
            int LowStudent = 0;
            PieChartViewModel m1 = new PieChartViewModel();
            PieChartViewModel m2 = new PieChartViewModel();
            PieChartViewModel m3 = new PieChartViewModel();
            m1.Type = "HighPerformers";
            m2.Type = "MidPerformers";
            m3.Type = "HighPerformers";
            if (Number > 0)
            {
                PerformanceService performanceService = new PerformanceService();
                foreach (var item in data)
                {
                    if (!string.IsNullOrEmpty(item.Percentage))
                    {
                        var percentage = performanceService.GetPercentageCritiria(item.BatchId.Value, item.ReviewId);
                        decimal d = Convert.ToDecimal(item.Percentage.Split(' ')[0]);
                        if (d >= Convert.ToDecimal(percentage[2].FromPercentage))
                            HighStudent = HighStudent + 1;
                        else if (d < Convert.ToDecimal(percentage[2].FromPercentage) && d >= Convert.ToDecimal(percentage[1].FromPercentage))
                            MidStudent = MidStudent + 1;
                        else
                            LowStudent = LowStudent + 1;
                    }
                }
                if (HighStudent > 0)
                {
                    m1.NoOfStudent = HighStudent;
                    m1.Percentage = Math.Round((HighStudent * 100) / (Number), 2);
                }
                if (MidStudent > 0)
                {
                    m2.NoOfStudent = MidStudent;
                    m2.Percentage = Math.Round((MidStudent * 100) / (Number), 2);
                }
                if (LowStudent > 0)
                {
                    m3.NoOfStudent = LowStudent;
                    m3.Percentage = Math.Round((LowStudent * 100) / (Number), 2);
                }
            }
            Model.Add(m1);
            Model.Add(m2);
            Model.Add(m3);
            return Model;
        }

        public dynamic GetSelectedTopCandidates(ReportFilterViewModel obj)
        {
            string[] BatchRangeArray = new string[obj.BatchRangeArray.Length];
            int[] BatchIds = null;
            for (int i = 0; i < obj.BatchRangeArray.Length; i++)
            {
                BatchRangeArray[i] = "Batch " + obj.BatchRangeArray[i];
            }
            var info = _context.tblPerformanceEntryMasters.Where(w => w.ReviewId != 1).AsQueryable();
            if (obj.ReviewId > 0)
                info = _context.tblPerformanceEntryMasters.Where(w => w.ReviewId == obj.ReviewId);
            if (BatchRangeArray.Length > 0)
                BatchIds = GetBatchArrIds(BatchRangeArray);
            DashBoardTopCandidateViewModel Model = new DashBoardTopCandidateViewModel
            {
                TheadTitle = _context.tblParameterTypes.Select(i => new
                {
                    PName = i.Name,
                    PId = i.Id
                }).OrderBy(o => o.PName).ToList(),
                info = info.Where(w => BatchIds.Contains(w.BatchId.Value)).Select(item => new
                {
                    //Name = item.AddmissionMaster.Fname + " " + item.AddmissionMaster.Lname,
                    Fname = item.AddmissionMaster.Fname,
                    Lname = item.AddmissionMaster.Lname,
                    BatchName = item.BatchMaster.Name,
                    TotalPercentage = item.Percentage,
                    PerformanceData = item.tblPerformanceParameterTypeResultMasters.GroupBy(g => g.tblParameterType.Name).Select(i => new
                    {
                        ParameterName = i.Key,
                        performanceData = i.Select(s => new
                        {
                            Rating = s.tblPerformanceParameterResultMasters.FirstOrDefault().Rating
                        })
                    }).Select(i => new
                    {
                        ParameterName = i.ParameterName,
                        Percentage = (i.performanceData.Sum(s => s.Rating) * 100) / (i.performanceData.Count() * 3)
                    }).OrderBy(o => o.ParameterName).ToList()
                }).OrderBy(a => a.Fname).ToList()
            };
            return Model;
            // ============== Old Code ===========

            //PerformanceData = item.tblPerformanceParameterTypeResultMasters.Where(t => !t.tblPerformanceParameterResultMasters.FirstOrDefault().IsNotApplicable).GroupBy(g => g.tblParameterType.Name).Select(i => new
            //{
            //    ParameterName = i.Key,
            //    performanceData = i.Select(s => new
            //    {
            //        Rating = s.tblPerformanceParameterResultMasters.FirstOrDefault().Rating
            //    })
            //}).Select(i => new
            //{
            //    ParameterName = i.ParameterName,
            //    Percentage = (i.performanceData.Sum(s => s.Rating) * 100) / (i.performanceData.Count() * 3)
            //}).OrderBy(o => o.ParameterName).ToList()
            //}).OrderBy(a => a.Name).ToList();

        }

        public dynamic GetSelectedCandidateOnConsolidatedpage(int RegistraionNo, int BatchId, int ReviewId)
        {
            var list = _context.tblPerformanceEntryMasters.Where(w=>w.BatchId == w.AddmissionMaster.AddmissionDetails.FirstOrDefault().BatchId).AsQueryable();
            if (BatchId > 0)
                list = list.Where(w => w.BatchId == BatchId);
            if (ReviewId > 0)
                list = list.Where(w => w.ReviewId == ReviewId);
            if (RegistraionNo > 0)
                list = list.Where(w => w.RegistrationNo == RegistraionNo);
            return list.Select(item => new
            {
                Fname = item.AddmissionMaster.Fname,
                Lname = item.AddmissionMaster.Lname,
                TotalPercentage = item.Percentage,
                PerformanceData = item.tblPerformanceParameterTypeResultMasters.Where(w => !w.tblPerformanceParameterResultMasters.FirstOrDefault().IsNotApplicable).GroupBy(g => g.tblParameterType.Name).Select(i => new
                {
                    ParameterName = i.Key,
                    performanceData = i.Select(s => new
                    {
                        Rating = s.tblPerformanceParameterResultMasters.FirstOrDefault().Rating
                    })
                })
            }).Select(l => new
            {
                Fname = l.Fname,
                Lname = l.Lname,
                TotalPercentage = l.TotalPercentage,
                PerformanceData = l.PerformanceData.Select(i => new
                {
                    ParameterName = i.ParameterName,
                    Percentage = (i.performanceData.Sum(s => s.Rating) * 100) / (i.performanceData.Count() * 3)
                }).ToList()
            }).FirstOrDefault();
        }

        public BarChartViewModel GetBarChartData(ReportFilterViewModel obj)
        {
            BarChartViewModel model = new BarChartViewModel();
            var data = _context.tblPerformanceParameterTypeResultMasters.Where(w => !w.tblPerformanceParameterResultMasters.FirstOrDefault().IsNotApplicable).AsQueryable();
            if (obj.BatchNo > 0)
                data = data.Where(t => t.tblPerformanceEntryMaster.BatchId == obj.BatchNo);
            var PieChartInfo = data;
            if (obj.RegNo > 0)
                data = data.Where(t => t.tblPerformanceEntryMaster.RegistrationNo == obj.RegNo);
            if (obj.ReviewId > 0)
                data = data.Where(t => t.tblPerformanceEntryMaster.ReviewId == obj.ReviewId);

            if (obj.ReviewId == 1)
            {
                if (obj.ddlWeeklyId > 0)
                    data = data.Where(t => t.tblPerformanceEntryMaster.tblWeeklyReviewMaster == obj.ddlWeeklyId);
            }

            var result = data.Select(item => new ParameterTypeInfoList
            {
                BatchId = _context.AddmissionDetails.Where(a => a.AddmissionMaster.RegistrationNo == item.tblPerformanceEntryMaster.RegistrationNo).FirstOrDefault().BatchId,
                ParameterType = item.tblParameterType.Name,
                PerformanceMasterId = item.tblPerformanceMasterId,
                ParameterInfoList = item.tblPerformanceParameterResultMasters.Select(ob => new ParameterInfoList
                {
                    Rating = ob.Rating,
                    Remarks = ob.tblParameterList.Name,
                    TblParameterId = ob.tblParameterId,
                    ReviewName = ob.tblPerformanceParameterTypeResultMaster.tblPerformanceEntryMaster.tblReviewMaster.Name
                }).ToList(),
                ReviewId = item.tblPerformanceEntryMaster.ReviewId
            }).ToList();

            decimal[] BarY = null;
            decimal[] BarX = null;
            if (result.Count > 0)
            {
                model.BarChartLbl = result.Select(d => d.ParameterType).Distinct().ToArray();
                BarY = new decimal[model.BarChartLbl.Length];
                BarX = new decimal[model.BarChartLbl.Length];
            }
            else
            {
                model.BarChartLbl = _context.tblParameterTypes.Where(p => p.IsActive).Select(d => d.Name).Distinct().ToArray();
                BarY = new decimal[model.BarChartLbl.Length];
                BarX = new decimal[model.BarChartLbl.Length];
            }

            if (obj.ReviewId == 0)
            {
                model.SubchartString = JsonConvert.SerializeObject(result.GroupBy(x => x.ParameterType, (name, items) => new
                {
                    Name = name,
                    list = items.Select(sa => sa.ParameterInfoList).ToList()
                }).ToList());

                var result1 = result.Where(r => r.ReviewId == 2).ToList();
                var result2 = result.Where(r => r.ReviewId == 3).ToList();
                for (int i = 0; i < model.BarChartLbl.Count(); i++)
                {
                    int res1Count = result1.Where(r => r.ParameterType == model.BarChartLbl[i]).Count();
                    int res2Count = result2.Where(r => r.ParameterType == model.BarChartLbl[i]).Count();
                    int res1Sum = result1.Where(r => r.ParameterType == model.BarChartLbl[i]).Sum(s => s.ParameterInfoList.Sum(p => p.Rating));
                    int res2Sum = result2.Where(r => r.ParameterType == model.BarChartLbl[i]).Sum(s => s.ParameterInfoList.Sum(p => p.Rating));
                    BarY[i] = res1Count > 0 ? GetBarChartPercentage(res1Count, res1Sum) : 0;
                    BarX[i] = res2Count > 0 ? GetBarChartPercentage(res2Count, res2Sum) : 0;
                }
            }
            else if (obj.ReviewId == 2)
            {
                var result1 = result.Where(r => r.ReviewId == 2).ToList();
                model.SubchartString = JsonConvert.SerializeObject(result1.GroupBy(x => x.ParameterType, (name, items) => new
                {
                    Name = name,
                    list = items.Select(sa => sa.ParameterInfoList).ToList()
                }).ToList());
                for (int i = 0; i < model.BarChartLbl.Count(); i++)
                {
                    int res1Count = result1.Where(r => r.ParameterType == model.BarChartLbl[i]).Count();
                    int res1Sum = result1.Where(r => r.ParameterType == model.BarChartLbl[i]).Sum(s => s.ParameterInfoList.Sum(p => p.Rating));
                    BarY[i] = res1Count > 0 ? GetBarChartPercentage(res1Count, res1Sum) : 0;
                    //BarY[i] = result1.Count() > 0 ? GetPercentage(result1[i].ParameterInfoList.Count, result1[i].ParameterInfoList.Sum(s => s.Rating)) : 0;
                }
                BarX = null;
            }
            else if (obj.ReviewId == 3)
            {
                var result2 = result.Where(r => r.ReviewId == 3).ToList();
                model.SubchartString = JsonConvert.SerializeObject(result2.GroupBy(x => x.ParameterType, (name, items) => new
                {
                    Name = name,
                    list = items.Select(sa => sa.ParameterInfoList).ToList()
                }).ToList());
                for (int i = 0; i < model.BarChartLbl.Count(); i++)
                {
                    int res2Count = result2.Where(r => r.ParameterType == model.BarChartLbl[i]).Count();
                    int res2Sum = result2.Where(r => r.ParameterType == model.BarChartLbl[i]).Sum(s => s.ParameterInfoList.Sum(p => p.Rating));
                    BarX[i] = res2Count > 0 ? GetBarChartPercentage(res2Count, res2Sum) : 0;
                }
                BarY = null;
            }
            else if (obj.ReviewId == 1)
            {
                var result3 = result.Where(r => r.ReviewId == 1).ToList();
                model.SubchartString = JsonConvert.SerializeObject(result3.GroupBy(x => x.ParameterType, (name, items) => new
                {
                    Name = name,
                    list = items.Select(sa => sa.ParameterInfoList).ToList()
                }).ToList());
                for (int i = 0; i < model.BarChartLbl.Count(); i++)
                {
                    int res3Count = result3.Where(r => r.ParameterType == model.BarChartLbl[i]).Count();
                    int res3Sum = result3.Where(r => r.ParameterType == model.BarChartLbl[i]).Sum(s => s.ParameterInfoList.Sum(p => p.Rating));
                    BarX[i] = res3Count > 0 ? GetBarChartPercentage(res3Count, res3Sum) : 0;
                    //BarX[i] = result1.Count() > 0 ? GetPercentage(result3[i].ParameterInfoList.Count, result3[i].ParameterInfoList.Sum(s => s.Rating)) : 0;
                }
                BarY = null;
            }
            model.DataY = BarY;
            model.DataX = BarX;

            // Do nut chart
            var PieChartData = PieChartInfo.Where(p => p.tblPerformanceEntryMaster.ReviewId == 3).ToList();
            decimal HighPercentage = 0;
            decimal MidPercentage = 0;
            decimal LowPercentage = 0;
            decimal[] PieData = new decimal[3];
            List<ParameterListViewModel> ParameterListModel = new List<ParameterListViewModel>();
            if (PieChartData.Count > 0)
            {
                foreach (var m in PieChartData)
                {
                    ParameterListModel.Add(m.tblPerformanceParameterResultMasters.Select(s => new ParameterListViewModel
                    {
                        Rating = s.Rating
                    }).FirstOrDefault());
                }
                var High = ParameterListModel.Where(h => h.Rating == 3).ToList();
                var Mid = ParameterListModel.Where(h => h.Rating == 2).ToList();
                var Low = ParameterListModel.Where(h => h.Rating == 1).ToList();

                HighPercentage = High.Count > 0 ? GetPercentage(High.Count, High.Sum(h => h.Rating)) : 0;
                MidPercentage = Mid.Count > 0 ? GetPercentage(Mid.Count, Mid.Sum(h => h.Rating)) : 0;
                LowPercentage = Low.Count > 0 ? GetPercentage(Low.Count, Low.Sum(h => h.Rating)) : 0;
            }
            PieData[0] = HighPercentage;
            PieData[1] = MidPercentage;
            PieData[2] = LowPercentage;
            model.PieChartData = PieData;
            return model;
        }

        public int GetPerformanceValueByText(string Text)
        {
            int result = 0;
            if (Text == "HIGH PERFORMERS")
                result = 3;
            else if (Text == "MID PERFORMERS")
                result = 2;
            else if (Text == "LOW PERFORMERS")
                result = 1;
            return result;
        }

        public List<ParameterListViewModel> GetSubParameterData(string ParameterTypeName)
        {
            return _context.tblParameterLists.Where(p => p.tblParameterType.Name == ParameterTypeName && p.IsActive).
                Select(item => new ParameterListViewModel
                {
                    Name = item.Name,
                    ParameterId = item.Id
                }).ToList();
        }

        public List<RoleViewModel> GetCandidateListByBatchId(int BatchId)
        {
            return _context.AddmissionMasters.Where(item => item.AddmissionDetails.FirstOrDefault().BatchId == BatchId).Select(m => new RoleViewModel
            {
                Id = m.RegistrationNo,
                Name = m.Fname + " " + m.Lname
            }).ToList();
        }

        public List<RoleViewModel> GetCandidateListByBatchIdForReport(int BatchId)
        {
            var Data = _context.AddmissionMasters.AsQueryable();
            int capacity = 0;
            if (BatchId > 0)
            {
                Data = Data.Where(item => item.AddmissionDetails.FirstOrDefault().BatchId == BatchId);
                capacity = _context.AddmissionDetails.Where(a => a.BatchId == BatchId).Count();
            }
            return Data.Select(m => new RoleViewModel
            {
                Id = m.RegistrationNo,
                Name = m.Fname + " " + m.Lname,
                Capacity = capacity
            }).ToList();
        }

        public int MonthName(string m)
        {
            int res;
            switch (m)
            {
                case "Jan":
                    res = 1;
                    break;
                case "Feb":
                    res = 2;
                    break;
                case "Mar":
                    res = 3;
                    break;
                case "Apr":
                    res = 4;
                    break;
                case "May":
                    res = 5;
                    break;
                case "Jun":
                    res = 6;
                    break;
                case "Jul":
                    res = 7;
                    break;
                case "Aug":
                    res = 8;
                    break;
                case "Sep":
                    res = 9;
                    break;
                case "Oct":
                    res = 10;
                    break;
                case "Nov":
                    res = 11;
                    break;
                case "Dec":
                    res = 12;
                    break;
                default:
                    res = 0;
                    break;
            }
            return res;
        }

        public int[] GetQuarterMonthArr(string[] str)
        {
            int[] arr = new int[3];
            for (int i = 0; i < str.Length; i++)
            {
                arr[i] = MonthName(str[i]);
            }
            return arr;
        }


        public RoleViewModel GetInfoByRegNo(int RegNo)
        {
            return _context.AddmissionMasters.Where(a => a.RegistrationNo == RegNo).Select(item => new RoleViewModel
            {
                BatchId = item.AddmissionDetails.FirstOrDefault().BatchId.Value,
                Id = item.RegistrationNo
            }).FirstOrDefault();
        }

        public List<RoleViewModel> GetYearList()
        {
            List<RoleViewModel> years = new List<RoleViewModel>();
            for (int i = -1; i <= 5; ++i)
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

        public int GetNumberOfStudentBySession(int SessionYr)
        {
            if (SessionYr > 0)
                return _context.AddmissionMasters.Where(a => a.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.Name != "Batch 0" && (a.RegistrationMaster.IsMedicalClear.HasValue && a.RegistrationMaster.IsMedicalClear.Value) && a.AddmissionDetails.FirstOrDefault().SessionMaster.SessionYr == SessionYr).Count();
            else
                return _context.AddmissionMasters.Where(a => a.IsActive && a.AddmissionDetails.FirstOrDefault().BatchMaster.Name != "Batch 0" && a.AddmissionDetails.FirstOrDefault().BatchMaster.IsActive).Count();
        }

        public int GetNumberOfCourse()
        {
            return _context.CourseMasters.Where(c => c.IsActive && c.DepartmentMasterId == 1).Count();
        }

        public int GetNumberOfBatch()
        {
            return _context.BatchMasters.Where(b => b.IsActive && b.Name != "Batch 0").Count();
        }

        public string GetBatchIdByNameId(string Id)
        {
            return _context.BatchMasters.Where(b => b.IsActive && b.Name == ("Batch " + Id)).FirstOrDefault().Id.ToString();
        }

        public int GetBatchIdByName(string Batch)
        {
            return _context.BatchMasters.Where(b => b.IsActive && b.Name == Batch).FirstOrDefault().Id;
        }

        public int[] GetBatchListArray()
        {
            int[] result = new int[2];
            result[0] = 0;
            var data = _context.BatchMasters.Where(s => s.IsActive && s.Name != "Batch 0").ToList().Select(s => new { Batch = Convert.ToInt32(s.Name.Split(' ')[1]) }).OrderBy(o => o.Batch).ToList();
            if (data.Count > 0)
            {
                result[0] = data.LastOrDefault().Batch;
                result[1] = data.FirstOrDefault().Batch;
            }
            else
            {
                result[0] = 1;
                result[1] = 1;
            }
            return result;
        }

        public Mainclass GetPerformancReport(string batch, long type)
        {
            DataTable dt = new DataTable();
            dt = ToDataTable(_context.Sp_GetPerformancePWithParamRepots(batch, type).ToList());
            List<BarGraph> ss = new List<BarGraph>();
            Mainclass mn = new Mainclass();
            if (dt.Rows.Count != 0)
            {
                DataView view = new DataView(dt);
                string[] sss;
                DataTable distinctValues = view.ToTable(true, "ParameterName");
                sss = new string[distinctValues.Rows.Count];
                for (int q = 0; q < distinctValues.Rows.Count; q++)
                {

                    sss[q] = (distinctValues.Rows[q]["ParameterName"]).ToString();
                    mn.labels = sss;
                }
                Array.Sort(mn.labels, StringComparer.InvariantCulture);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BarGraph bar = new BarGraph();
                if (mn.datasets == null)
                {

                    bar.label = (dt.Rows[i]["BatchName"]).ToString();
                    bar.backgroundColor = "#FF6000";
                    var strExpr = "Id =" + Convert.ToInt32(dt.Rows[i]["Id"]);
                    dt.DefaultView.RowFilter = strExpr;
                    decimal[] sss;
                    DataTable dts = (dt.DefaultView).ToTable();
                    sss = new decimal[dts.Rows.Count];
                    foreach (var p in mn.labels)
                    {
                        for (int q = 0; q < dts.Rows.Count; q++)
                        {
                            if (Convert.ToString(dts.Rows[q]["ParameterName"]) == p)
                            {
                                sss[q] = Convert.ToDecimal(dts.Rows[q]["Result"]);
                                bar.data = sss;
                            }
                        }
                    }
                    ss.Add(bar);
                    mn.datasets = ss;
                }
                else
                {
                    string djfkjd = (dt.Rows[i]["BatchName"]).ToString();
                    List<BarGraph> ssss = (from s in mn.datasets where s.label == (dt.Rows[i]["BatchName"]).ToString() select s).ToList();
                    if (ssss.Count == 0)
                    {
                        bar.label = (dt.Rows[i]["BatchName"]).ToString();
                        if (ssss.Count == 2)
                        {
                            bar.backgroundColor = "#00ffad";
                        }
                        else if (ssss.Count == 3)
                        {
                            bar.backgroundColor = "#00BDC9";
                        }
                        var strExpr = "Id =" + Convert.ToInt32(dt.Rows[i]["Id"]);
                        dt.DefaultView.RowFilter = strExpr;
                        decimal[] sss;
                        DataTable dts = (dt.DefaultView).ToTable();
                        sss = new decimal[dts.Rows.Count];
                        foreach (var p in mn.labels)
                        {
                            for (int q = 0; q < dts.Rows.Count; q++)
                            {
                                if (Convert.ToString(dts.Rows[q]["ParameterName"]) == p)
                                {
                                    sss[q] = Convert.ToDecimal(dts.Rows[q]["Result"]);
                                    bar.data = sss;
                                }
                            }
                        }
                        ss.Add(bar);
                        mn.datasets = ss;
                    }
                }
            }
            if (mn.datasets != null)
            {
                if (mn.datasets.Count == 3)
                {
                    mn.datasets[2].backgroundColor = "#a1db32";
                }
            }
            return mn;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public List<SemesterMasterViewModel> GetParameterList()
        {
            return _context.tblParameterTypes.Where(t => t.IsActive).Select(item => new SemesterMasterViewModel
            {
                Id = item.Id,
                SemesterName = item.Name
            }).ToList();
        }

        public dynamic GetPerformanceReportList(int BatchId, int ReviewId)
        {
            var list = _context.tblPerformanceEntryMasters.
                Select(item => new
                {
                    Fname = item.AddmissionMaster.Fname,
                    Lname = item.AddmissionMaster.Lname,
                    BatchId = item.BatchId,
                    CurrentBatchId = item.AddmissionMaster.AddmissionDetails.FirstOrDefault().BatchId,
                    ReviewId = item.ReviewId,
                    TotalPercentage = item.Percentage,
                    PerformanceData = item.tblPerformanceParameterTypeResultMasters.Where(a => !a.tblPerformanceParameterResultMasters.FirstOrDefault().IsNotApplicable).GroupBy(g => g.tblParameterType.Name).Select(i => new
                    {
                        ParameterName = i.Key,
                        performanceData = i.Select(s => new
                        {
                            Rating = s.tblPerformanceParameterResultMasters.FirstOrDefault().Rating
                        })
                    })
                }).Where(i => i.BatchId == i.CurrentBatchId &&  i.ReviewId == ReviewId && i.BatchId == BatchId).ToList().Select(l => new
                 {
                     Fname = l.Fname,
                     Lname = l.Lname,
                     BatchId = l.BatchId,
                     TotalPercentage = l.TotalPercentage,
                     PerformanceData = l.PerformanceData.Select(i => new
                     {
                         ParameterName = i.ParameterName,
                         Percentage = (i.performanceData.Sum(s => s.Rating) * 100) / (i.performanceData.Count() * 3)
                     }).ToList()
                 }).OrderBy(o => o.Fname).ToList();
            return list;
        }
    }
}
