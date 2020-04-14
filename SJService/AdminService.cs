using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJData;
using SJModel;

namespace SJService
{
    public class AdminService
    {
        SJStarERPEntities _context = null;
        LogActivityService logActivityService = null;
        public AdminService()
        {
            _context = new SJStarERPEntities();
            logActivityService = new LogActivityService();
        }

        public DataTableFilterModel GetAdmissionBatchListinfo(DataTableFilterModel filter)
        {
            var data = _context.BatchMasters.Where(a => a.IsActive)
                .Select(a => new RoleViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    ActiveStr = a.IsActive ? "Yes" : "No",
                    BatchStartDate = a.DateOfStart,
                    BatchEndDate = a.DateOfEnd
                }).AsEnumerable();
            var totalCount = data.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
                data = data.Where(d => d.Name.ToString().ToLower().Contains(filter.search.value.ToLower()) || d.ActiveStr.ToLower().Contains(filter.search.value.ToLower()));
            var o = filter.order[0];
            var name = filter.columns[filter.order[0].column].data;
            if (o.dir == "asc")
                data = data.OrderBy(x => x.GetType().GetProperty(name).GetValue(x));
            else
                data = data.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x));
            var filteredCount = data.Count();
            filter.recordsTotal = totalCount;
            filter.recordsFiltered = filteredCount;
            filter.data = data.Skip(filter.start).Take(filter.length).ToList();
            return filter;
        }

        public RoleViewModel GetBatchDataById(int Id)
        {
            RoleViewModel model = new RoleViewModel();
            model.IsActive = true;
            var data = _context.BatchMasters.Where(b => b.Id == Id).Select(item => new
            {
                Id = item.Id,
                Name = item.Name,
                IsActive = item.IsActive,
                StartDate = item.DateOfStart,
                EndDate = item.DateOfEnd
            }).FirstOrDefault();
            if (data != null)
            {
                model.Id = data.Id;
                model.Name = data.Name;
                model.IsActive = data.IsActive;
                model.BatchStartDateStr = string.Format("{0}", (data.StartDate.HasValue ? data.StartDate.Value.Day.ToString().PadLeft(2, '0') + "/" + data.StartDate.Value.Month.ToString().PadLeft(2, '0') + "/" + data.StartDate.Value.Year : ""));
                model.BatchEndDateStr = string.Format("{0}", (data.EndDate.HasValue ? data.EndDate.Value.Day.ToString().PadLeft(2, '0') + "/" + data.EndDate.Value.Month.ToString().PadLeft(2, '0') + "/" + data.EndDate.Value.Year : ""));
            }
            return model;
        }

        public string AddUpdateBatchMaster(RoleViewModel Model)
        {
            try
            {
                string status = "";
                BatchMaster role = null;

                LogActivityViewModel log = new LogActivityViewModel();
                log.EnteredBy = Model.EnteredBy;
                log.EnteredDate = DateTime.Now;
                log.ActioName = "AddUpdateBatchMaster";
                log.ModuleName = "Control Panel";
                log.ControllerName = "Admin";
                if (Model.Id > 0)
                {
                    role = _context.BatchMasters.Where(b => b.Id == Model.Id).FirstOrDefault();
                    status = "Update Successfully!";
                    log.Activity = "Update";
                    log.ActivityMessage = "'" + Model.Name + "' is updated.";
                }
                if (role == null)
                    role = new BatchMaster();
                role.IsActive = Model.IsActive;
                role.Name = Model.Name;
                role.DateOfStart = !string.IsNullOrEmpty(Model.BatchStartDateStr) ? (DateTime.ParseExact(Model.BatchStartDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                role.DateOfEnd = !string.IsNullOrEmpty(Model.BatchEndDateStr) ? (DateTime.ParseExact(Model.BatchEndDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                if (Model.Id == 0)
                {
                    status = "Add Successfully!";
                    log.Activity = "Create";
                    log.ActivityMessage = "'" + Model.Name + "' is added.";
                    _context.BatchMasters.Add(role);
                }
                _context.SaveChanges();
                logActivityService.CreateLogActivity(log);
                return status;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
