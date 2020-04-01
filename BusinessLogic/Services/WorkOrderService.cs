using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IRepo<Issue> _issueRepo;
        private readonly IRepo<Item> _itemRepo;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<Department> _department;
        private readonly IRepo<WorkerType> _workertype;
        private readonly IRepo<Stage> _stage;
        private readonly IRepo<WorkOrder> _workOrder;
        public WorkOrderService(IRepo<Issue> issueRepo, IRepo<Item> itemRepo, IRepo<UserProperty> userProperty, IRepo<Department> department, IRepo<WorkerType> workertype, IRepo<Stage> stage, IRepo<WorkOrder> workOrder)
        {
            _issueRepo = issueRepo;
            _itemRepo = itemRepo;
            _userProperty = userProperty;
            _department = department;
            _workertype = workertype;
            _stage = stage;
            _workOrder = workOrder;
        }

        public async Task<WorkOrderDetail> CreateWO(CreateWO createWO)
        {
            WorkOrder workOrder = new WorkOrder
            {
                CreationTime=DateTime.UtcNow,
                UpdateTime=DateTime.UtcNow,
                ApplicationUserId=createWO.RequestedById,
                PropertyId=createWO.Property,
                IssueId=createWO.Issue,
                ItemId=createWO.Item,
                Description=createWO.Description
            };
            workOrder.Stage.StageCode = "1000";
            await _workOrder.Add(workOrder);
            var prop = await GetAreaLocation(createWO.Property);
            var workorder =await _workOrder.Get(x => x.Id == workOrder.Id).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Stage).Select(x => new WorkOrderDetail
            {
                Area=prop.Area,
                Location=prop.Location,
                PropertyName=x.Property.PropertyName,
                Issue=x.Issue.IssueName,
                StageCode=x.Stage.StageCode,
                StageDescription=x.Stage.StageDescription,
                Item=x.Item.ItemName,
                Requestedby=x.ApplicationUser.FirstName+" "+ x.ApplicationUser.LastName
            }).FirstOrDefaultAsync();
            return workorder;
        }

        public async Task<PropDetail> GetAreaLocation(long id)
        {
            var primaryprop = await _userProperty.Get(x => x.PropertyId == id).Include(x=>x.Property).AsNoTracking().FirstOrDefaultAsync();
            StringBuilder sb = new StringBuilder();
            if (primaryprop != null)
            {
                if (!string.IsNullOrWhiteSpace(primaryprop.Property.Locality))
                {
                    sb.Append(primaryprop.Property.Locality);
                    sb.Append(", ");
                }
                sb.Append(primaryprop.Property.City);

            }
            PropDetail propDetail = new PropDetail
            {
                Area = primaryprop.Property.Street,
                Location = sb.ToString()
            };
            return propDetail;
        }

        public async Task<CreateWO> GetCreateWOModel(long userId)
        {
            var property = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).AsNoTracking().ToListAsync();
            var primaryprop = property.Where(x => x.IsPrimary == true).FirstOrDefault();
            StringBuilder sb = new StringBuilder();
            if (primaryprop != null)
            {
                if (!string.IsNullOrWhiteSpace(primaryprop.Property.Locality))
                {
                    sb.Append(primaryprop.Property.Locality);
                    sb.Append(", ");
                }
                sb.Append(primaryprop.Property.City);
            }

            CreateWO wo = new CreateWO()
            {
                Properties = property.Select(x => new SelectItem
                {
                    Id = x.Property.Id,
                    PropertyName = x.Property.PropertyName
                }).ToList(),
                Property = primaryprop != null ? primaryprop.PropertyId : 0,
                Issues = await _issueRepo.GetAll().Select(x => new SelectItem
                {
                    Id = x.Id,
                    PropertyName = x.IssueName
                }).ToListAsync(),
                Items = await _itemRepo.GetAll().Select(x => new SelectItem
                {
                    Id = x.Id,
                    PropertyName = x.ItemName
                }).ToListAsync(),

                Area = primaryprop != null ? primaryprop.Property.Street : "",
                Location = sb.ToString(),
                Departments=await _department.GetAll().Select(x=> new SelectItem {Id=x.Id,PropertyName=x.DepartmentName }).ToListAsync()
            };
            
            return wo;
        }

        public Task<List<SelectItem>> GetSection(long id)
        {
            var res = _workertype.GetAll().Where(x => x.DepartmentId == id).Select(x => new SelectItem {
                Id=x.Id,
                PropertyName=x.TypeName
            }).AsNoTracking().ToListAsync();
            return res;
                
        }
    }
}
