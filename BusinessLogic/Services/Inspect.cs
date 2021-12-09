using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using DataTransferObjects.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.CheckList.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Interface;

namespace BusinessLogic.Services
{
    public class Inspect : IInspect
    {
        private readonly IRepo<Inspection> _insp;
        private readonly IRepo<InspectionQueue> _inspq;
        private readonly UserManager<ApplicationUser> _appuser;
        private readonly IRepo<CheckList> _check;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<Property> _property;
        private readonly IRepo<Department> _department;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepo<SubLocation> _sublocation;
        private readonly IRepo<Location> _location;

        private readonly IRecurringInspection _rinspect;
        private readonly INotifier _notifier;
        public long userId;
        private readonly string _scheme;
        
        private readonly TimeZoneInfo timeZone;
        private readonly bool is24HrFormat = false;

        public Inspect(IRepo<Inspection> insp, IRepo<CheckList> check, UserManager<ApplicationUser> appuser, IRepo<UserProperty> userProperty, IRepo<Property> property, IRepo<Department> department, IHttpContextAccessor httpContextAccessor, IRepo<SubLocation> sublocation, IRepo<Location> location, INotifier notifier, IRecurringInspection rinspect, IRepo<InspectionQueue> inspq)
        {


            _userProperty = userProperty;
            _department = department;
            _rinspect = rinspect;
            _insp = insp;
            _check = check;
            _httpContextAccessor = httpContextAccessor;
            _inspq = inspq;
            _appuser = appuser;
            _sublocation = sublocation;
            _property = property;
            _notifier = notifier;
            userId = Convert.ToInt64(_httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value);
            _scheme = _httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://";


            timeZone = TimeZoneInfo.FindSystemTimeZoneById(_httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TimeZone")?.Value);
            is24HrFormat = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "Clock")?.Value == "24" ? true : false;


        }



        public async Task<string> AddInspection(InspectionDTO inspect)
        {
            var ins = new Inspection() {
                Name = inspect.Name,
                CronExpression = inspect.CronExpression,
                DepartmentID = inspect.DepartmentID,
                Active = true,
                Description = inspect.Description,
                Type = inspect.Type,
                PropertyId = inspect.PropertyId,
                ChecklistItems=new List<CheckList>(),
            };

            var status = await _insp.Add(ins);
            if (status>=1)
            {
                //schedule---
                _rinspect.RunCreateInsJob(ins.CronExpression, ins.Id, timeZone);
                return ins.Id;
            }
            else return "";



        }

        public async Task<InspectionDTO> GetInspection()
        {

            var ret = new InspectionDTO();

            ret.Departments = await _department.GetAll().Select(x => new Models.SelectItem { Id = x.Id, PropertyName = x.DepartmentName }).ToListAsync();
            
            ret.Properties = await _property.Get(y => y.IsActive).Select(z => new SelectItem { Id = z.Id, PropertyName = z.PropertyName }).ToListAsync();
                
            return ret;
        }

        public  List<GroupedDTO> GetCheckList(string id)
        {
            var resp =  _check.GetAll().Where(x => x.InspectionId == id).Include(x => x.Location).ToList().GroupBy(x =>x.Location.LocationName).Select(y => new GroupedDTO
            {
                LocationName = y.Key,
                Items = y.Select(z => new ItemDTO { Description = z.Description, Id = z.Id, LocationId = z.LocationId, Order = z.Order, Status = z.Status }).OrderBy(a=>a.Order).ToList(),
                Order = y.Select(z => z.LocationOrder).FirstOrDefault(),
                LocationId = y.Select(z => z.LocationId).FirstOrDefault(),
                InspectionId = id,
                

            }).OrderBy(y=>y.Order).ToList();
            return resp;
        }

        public async Task<bool> AddList(CheckListDTO check)
        {
            CheckList chk = new CheckList(){
            InspectionId=check.InspectionId,
            LocationId=check.LocationId,
            SubLocationId=check.SubLocationId,
            Description=check.Description,
            
            };
            int? order=_check.Get(x => x.InspectionId == check.InspectionId && x.LocationId == check.LocationId).ToList().Max(x => (int?)x.Order);
            int? locorder = _check.Get(x => x.InspectionId == check.InspectionId && x.LocationId == check.LocationId).ToList().Max(y=> (int?)y.LocationOrder);
            chk.Order =order.HasValue?order.Value+1:1;
            if (locorder == null) {
                locorder = _check.Get(x => x.InspectionId == check.InspectionId).Max(y => (int?)y.LocationOrder);
                chk.LocationOrder = locorder.HasValue ? locorder.Value + 1 : 1;
             }
            chk.LocationOrder = locorder.HasValue?locorder.Value:1;
              var ret= Convert.ToBoolean(await  _check.Add(chk));
            return ret;

        }

        public async Task<List<InspectionsDTO>> GetInspections()
        {
            var res = await _inspq.GetAll().Include(y=>y.Inspection).Select(y => new InspectionsDTO {
            Description=y.Inspection.Description,
            Id=y.Inspection.Id,
            Inspector=_appuser.Users.Where(x=>x.UserName==y.Inspection.CreatedByUserName).Select(x=>x.FirstName+" "+x.LastName).FirstOrDefault(),
            Items=y.Inspection.ChecklistItems.Count(),
            Property=y.Inspection.Property.PropertyName,
            IsComplete=y.Status,
            StartDate=y.Inspection.CreatedTime,
            InspectionId=y.InspectionId,
            PropertyId=y.Inspection.PropertyId
            }).ToListAsync();
            return res;
        }

    }
}
