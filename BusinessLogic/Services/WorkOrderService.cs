using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IRepo<ApplicationRole> _role;
        private readonly IRepo<WorkOrder> _workOrder;
        private readonly IRepo<Stage> _stage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkOrderService(IRepo<Issue> issueRepo, IRepo<Item> itemRepo, IRepo<UserProperty> userProperty, IRepo<Department> department, IRepo<WorkOrder> workOrder, IRepo<ApplicationRole> role, IRepo<Stage> stage, IHttpContextAccessor httpContextAccessor)
        {
            _issueRepo = issueRepo;
            _itemRepo = itemRepo;
            _userProperty = userProperty;
            _department = department;
            _workOrder = workOrder;
            _role = role;
            _stage = stage;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<WorkOrderDetail> CreateWO(CreateWO createWO)
        {
            WorkOrder workOrder = new WorkOrder
            {
                RequestedBy = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value,
                PropertyId = createWO.Property,
                IssueId = createWO.Issue,
                ItemId = createWO.Item,
                Description = createWO.Description,
                AssignedToRoleId = createWO.Section
            };
            workOrder.StageId = _stage.Get(x => x.StageCode == "INITWO").Select(x => x.Id).FirstOrDefault();
            await _workOrder.Add(workOrder);
            var prop = await GetAreaLocation(createWO.Property);
            var workorder = await _workOrder.Get(x => x.Id == workOrder.Id).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Stage).Include(x => x.AssignedTo).Include(x => x.AssignedToRole).ThenInclude(x => x.Department).Select(x => new WorkOrderDetail
            {
                Area = prop.Area,
                Location = prop.Location,
                PropertyName = x.Property.PropertyName,
                Issue = x.Issue.IssueName,
                StageCode = x.Stage.StageCode,
                StageDescription = x.Stage.StageDescription,
                Item = x.Item.ItemName,
                CreatedTime = x.CreatedTime,
                UpdatedTime = x.UpdatedTime,
                Department = x.AssignedToRole.Department.DepartmentName,
                Section = x.AssignedToRole.Name,
                AssignedToUser = x.AssignedTo.UserName
            }).FirstOrDefaultAsync();
            return workorder;
        }

        public async Task<PropDetail> GetAreaLocation(long id)
        {
            var primaryprop = await _userProperty.Get(x => x.PropertyId == id).Include(x => x.Property).AsNoTracking().FirstOrDefaultAsync();
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
                Departments = await _department.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.DepartmentName }).ToListAsync()
            };

            return wo;
        }

        public Task<List<SelectItem>> GetSection(long id)
        {
            var res = _role.GetAll().Where(x => x.DepartmentId == id).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.Name
            }).AsNoTracking().ToListAsync();
            return res;
        }

        public async Task<Pagination<List<WorkOrderAssigned>>> GetWO(int pageNumber, FilterEnumWO filter, string matchStr)
        {
            List<WorkOrderAssigned> workOrderAssigned = null;
            int count = 0;
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                workOrderAssigned = await _workOrder.GetAll().Select(x => new WorkOrderAssigned
                {
                    CreatedDate = x.CreatedTime.ToString("dd/MMM/YYYY"),
                    Description = x.Description,
                    Id = x.Id,
                    AssignedTo = x.AssignedTo.UserName,
                    AssignedToSection = x.AssignedToRole.Name
                }).AsNoTracking().ToListAsync();
            }
            else
            {
                workOrderAssigned = await _workOrder.GetAll().Select(x => new WorkOrderAssigned
                {
                    CreatedDate = x.CreatedTime.ToString("dd-MMM-yy"),
                    Description = x.Description,
                    Id = x.Id,
                    AssignedTo = x.AssignedTo.UserName,
                    AssignedToSection = x.AssignedToRole.Name
                }).AsNoTracking().ToListAsync();
            }
            if (workOrderAssigned != null)
                count = workOrderAssigned.Count();
            Pagination<List<WorkOrderAssigned>> pagination = new Pagination<List<WorkOrderAssigned>>
            {
                Payload = workOrderAssigned,
                CurrentPage = pageNumber,
                ItemsPerPage = count > 10 ? 10 : count,
                PageCount = count % 10 == 0 ? count / 10 : count / 10 + 1
            };

            return pagination;
        }
    }
}