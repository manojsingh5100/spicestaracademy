using SJModel;
using SJModel.PerformanceModel;
using SJData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJService
{
    public class ParameterTypeService
    {
        SJStarERPEntities _context = null;
        public ParameterTypeService()
        {
            _context = new SJStarERPEntities();
        }
        public DataTableFilterModel GetParemeterTypeList(DataTableFilterModel filter)
        {
            var data = _context.tblParameterTypes.AsQueryable();
            var info = data.Select(model => new ParameterTypeViewModel
            {
                Id = model.Id,
                Name = model.Name,

                IsActive = model.IsActive
            }).AsEnumerable();
            var totalCount = info.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                info = info.Where(d => !string.IsNullOrEmpty(d.Name) && d.Name.ToLower().Contains(filter.search.value.ToLower()));
            }
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


        public string AddUpadteParameterType(ParameterTypeViewModel Model)
        {
            string Message = "";
            try
            {
                tblParameterType data = null;
                data = _context.tblParameterTypes.Where(t => t.Id == Model.Id).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = Model.IsActive;
                    data.Name = Model.Name;
                    Message = "Parameter updated successfully!";
                }
                else
                {
                    data = new tblParameterType
                    {
                        IsActive = Model.IsActive,
                        Name = Model.Name
                    };
                    _context.tblParameterTypes.Add(data);
                    Message = "Parameter added successfully!";
                }
                _context.SaveChanges();
                return Message;
            }
            catch (Exception ex)
            {
                return ex.Message + " " + ex.InnerException;
            }
        }

        public ParameterTypeViewModel GetParameterTyeInfoById(int Id)
        {
            return _context.tblParameterTypes.Where(t => t.Id == Id).Select(item => new ParameterTypeViewModel
            {
                Id = item.Id,
                IsActive = item.IsActive,
                Name = item.Name
            }).FirstOrDefault();
        }

        public string ParameterTypeNameById(int Id)
        {
            return _context.tblParameterTypes.Where(t => t.Id == Id).FirstOrDefault().Name;
        }

        public ParameterListViewModel ParameterSaveUpdate(ParameterListViewModel Model)
        {
            tblParameterList list = _context.tblParameterLists.Where(t => t.tblParameterTypeId == Model.tblParameterTypeId
               && t.Id == Model.ParameterId).FirstOrDefault();
            if (list == null)
                list = new tblParameterList();
            list.IsActive = Model.IsActive;
            list.Name = Model.Name;
            list.ReviewId = Model.ReviewId > 0 ? Model.ReviewId : (int?)null;
            if (Model.GenderId > 0)
            {
                if (Model.GenderId == 1)
                    list.Gender = "F";
                else
                    list.Gender = "M";
            }
            else
                list.Gender = null;
            list.tblParameterTypeId = Model.tblParameterTypeId;
            if (list.Id == 0)
            {
                list.KeyId = Model.KeyId;
                Model.Message = "Sub-Parameter saved successfully!";
                list.Id = Model.ParameterId;
                _context.tblParameterLists.Add(list);
            }
            else
                Model.Message = "Sub-Parameter update successfully!";
            _context.SaveChanges();
            return Model;
        }

        public DataTableFilterModel GetParemeterList(DataTableFilterModel filter , int ParameterTypeId)
        {
            var data = _context.tblParameterLists.Where(p => p.tblParameterTypeId == ParameterTypeId).AsQueryable();
            var info = data.Select(model => new ParameterListViewModel
            {
                ParameterId = model.Id,
                KeyId = model.KeyId,
                Name = model.Name,
                Gender = model.Gender,
                ReviewId = model.ReviewId,
                IsActive = model.IsActive,
            }).AsEnumerable();
            var totalCount = info.Count();
            if (!string.IsNullOrWhiteSpace(filter.search.value))
            {
                info = info.Where(d => !string.IsNullOrEmpty(d.Name) && d.Name.ToLower().Contains(filter.search.value.ToLower()));
            }
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

        public ParameterListViewModel GetParameterInfoById(int Id)
        {
            return _context.tblParameterLists.Where(t => t.Id == Id).Select(item => new ParameterListViewModel
            {
                ParameterId = item.Id,
                IsActive = item.IsActive,
                Name = item.Name,
                tblParameterTypeId = item.tblParameterTypeId,
                KeyId = item.KeyId
            }).FirstOrDefault();
        }

        public int GetParameterCount(int ParameterTypeId)
        {
            return _context.tblParameterLists.Where(t => t.tblParameterTypeId == ParameterTypeId).Count();
        }
    }
}
