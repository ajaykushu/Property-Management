using BusinessLogic.Interfaces;
using AutoMapper;
using BusinessLogic.Interfaces;
using CronExpressionDescriptor;
using DataAccessLayer.Interfaces;
using DataEntity;
using DataTransferObjects.ResponseModels;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ResponseModels;
using Models.WorkOrder.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.Interface;
using DataTransferObjects.RequestModels;

namespace BusinessLogic.Services
{
    public class HomeBL:IHomeBL
    {
        private readonly IRepo<Issue> _issueRepo;
        private readonly UserManager<ApplicationUser> _appuser;
        private readonly IRepo<Item> _itemRepo;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<Property> _property;
        private readonly IRepo<Department> _department;
        private readonly IRepo<WorkOrder> _workOrder;
        private readonly IRepo<Status> _status;
        private readonly IRepo<Comment> _comments;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepo<SubLocation> _sublocation;
        private readonly IImageUploadInFile _imageuploadinfile;
        private readonly INotifier _notifier;
        private readonly IRepo<Vendor> _vendors;
        private readonly IRepo<History> _history;
        private readonly IRepo<RecurringWO> _recuringWo;
        public long userId;
        private readonly string _scheme;
        private readonly IRecurringWorkOrderJob _recurringWorkOrderJob;
        private readonly TimeZoneInfo timeZone;
        private readonly bool is24HrFormat = false;
        private readonly IRepo<Effort> _effort;

        public HomeBL(IRepo<Issue> issueRepo, IRepo<Item> itemRepo, IRepo<UserProperty> userProperty, IRepo<Department> department, IRepo<WorkOrder> workOrder, IRepo<Status> status, IHttpContextAccessor httpContextAccessor, IRepo<Comment> comments, IImageUploadInFile imageuploadinfile, UserManager<ApplicationUser> appuser, IRepo<SubLocation> sublocation, IRepo<Property> property, INotifier notifier, IRepo<Vendor> vendors, IRepo<History> history, IRecurringWorkOrderJob recurringWorkOrderJob, IRepo<RecurringWO> recuringWo, IRepo<Effort> effort)
        {
            _issueRepo = issueRepo;
            _itemRepo = itemRepo;
            _userProperty = userProperty;
            _department = department;
            _workOrder = workOrder;
            _history = history;
            _effort = effort;
            _status = status;
            _httpContextAccessor = httpContextAccessor;
            _comments = comments;
            _recuringWo = recuringWo;
            _imageuploadinfile = imageuploadinfile;
            _appuser = appuser;
            _sublocation = sublocation;
            _property = property;
            _notifier = notifier;
            userId = Convert.ToInt64(_httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value);
            _scheme = _httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://";
            _vendors = vendors;
            _recurringWorkOrderJob = recurringWorkOrderJob;
            timeZone = TimeZoneInfo.FindSystemTimeZoneById(_httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TimeZone")?.Value);
            is24HrFormat = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "Clock")?.Value == "24" ? true : false;


        }

        public async Task<DashBoard> GetDashboard()
        {
            
            var obj = new DashBoard();
           var ret= await  _property.GetAll().Where(x => x.IsActive).Include(x=>x.UserProperties).Include(x=>x.Locations).ThenInclude(x => x.SubLocations).ThenInclude(x=>x.WorkOrders).ToListAsync();
            obj.Properties = ret.Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyName,Selected=x.UserProperties.Where(y=>y.ApplicationUserId==userId && y.IsPrimary).FirstOrDefault()?.Id.ToString()}).ToList();

            var prim = obj.Properties.Where(x => string.IsNullOrEmpty(x.Selected)).First();
            var sub = ret.Where(x=>x.Id==prim.Id).SelectMany(x=>x.Locations).ToList();
            obj.Locations = new List<LoctionDetail>();
            obj.Locations = sub.Select(x => new LoctionDetail {Id=x.Id, RoomNumber = x.LocationName, HasPendingWorkorder = x.WorkOrders != null?x.WorkOrders.Any(x => x.IsActive):false,WorkOrderCount = x.WorkOrders!=null? x.WorkOrders.Count:0}).ToList();
            return obj;
        }

        public async Task<List<LoctionDetail>> LocationView(long Id)
        {
            var ret = await _property.GetAll().Where(x => x.IsActive && Id==x.Id).Include(x => x.Locations).ThenInclude(x => x.WorkOrders).FirstOrDefaultAsync();
            var obj = new List<LoctionDetail>();
            var data = ret.Locations.ToList();
            obj = data.Select(x => new LoctionDetail {Id=x.Id, RoomNumber = x.LocationName, HasPendingWorkorder = x.WorkOrders != null?x.WorkOrders.Any(x => x.IsActive):false, WorkOrderCount = x.WorkOrders != null ? x.WorkOrders.Count : 0 }).ToList();

            return obj;

        }
        public async Task<List<SubLocationModel>> Sublocation(long loc)
        {
            var dc = await _sublocation.GetAll().Where(x => x.LocationId == loc).Include(x => x.WorkOrders).Select(y => new SubLocationModel { HasPendingWorkder = y.WorkOrders != null ? y.WorkOrders.Any(x => x.IsActive) : false, Id = y.Id, SublocationName = y.AreaName,Workorder=y.WorkOrders.Select(x=>x.Id).ToList() }).ToListAsync();
            return dc;

        }
    }
}
