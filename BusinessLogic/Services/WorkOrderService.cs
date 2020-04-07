using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Interface;

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
        private readonly IRepo<Comments> _comments;
        private readonly IRepo<Reply> _reply;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageUploadInFile _imageuploadinfile;

        public WorkOrderService(IRepo<Issue> issueRepo, IRepo<Item> itemRepo, IRepo<UserProperty> userProperty, IRepo<Department> department, IRepo<WorkOrder> workOrder, IRepo<ApplicationRole> role, IRepo<Stage> stage, IHttpContextAccessor httpContextAccessor, IRepo<Comments> comments, IRepo<Reply> reply, IImageUploadInFile imageuploadinfile)
        {
            _issueRepo = issueRepo;
            _itemRepo = itemRepo;
            _userProperty = userProperty;
            _department = department;
            _workOrder = workOrder;
            _role = role;
            _stage = stage;
            _httpContextAccessor = httpContextAccessor;
            _comments = comments;
            _reply = reply;
            _imageuploadinfile = imageuploadinfile;
        }

        public async Task<bool> CreateWO(CreateWO createWO,IFormFile file)
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
            if (file != null)
            {
                string path = await _imageuploadinfile.UploadAsync(file);
                workOrder.AttachmentPath = path;
            }
                
            var status = await _workOrder.Add(workOrder);
            if (status > 0) return true;
            return false;
        }

        public async Task<WorkOrderDetail> GetWODetail(long id)
        {

            var workorder = await _workOrder.Get(x => x.Id == id).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Stage).Include(x => x.AssignedTo).Include(x => x.AssignedToRole).ThenInclude(x => x.Department).Select(x => new
            {
                obj = new WorkOrderDetail
                {
                    PropertyName = x.Property.PropertyName,
                    Issue = x.Issue.IssueName,
                    StageCode = x.Stage.StageCode,
                    StageDescription = x.Stage.StageDescription,
                    Item = x.Item.ItemName,
                    CreatedTime = x.CreatedTime,
                    UpdatedTime = x.UpdatedTime,
                    Department = x.AssignedToRole.Department.DepartmentName,
                    Section = x.AssignedToRole.Name,
                    AssignedToUser = x.AssignedTo.UserName,
                    Requestedby = x.RequestedBy,
                    Id = x.Id,
                    UpdatedBy = x.UpdatedByUserName,
                    Description=x.Description
                },
                Propid = x.PropertyId
            }).AsNoTracking().FirstOrDefaultAsync();
            var prop = await GetAreaLocation(workorder.Propid);
            workorder.obj.Area = prop.Area;
            workorder.obj.Location = prop.Location;
            return workorder.obj;

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

        public async Task<Pagination<List<WorkOrderAssigned>>> GetWO(int pageNumber, FilterEnumWO filter, string matchStr, FilterEnumWOStage stage, string endDate)
        {
            int iteminpage = 3;
            var query = _workOrder.GetAll();
            
            if (!string.IsNullOrWhiteSpace(matchStr)  && filter == FilterEnumWO.ByDate)
            {
                var startDate = Convert.ToDateTime(matchStr);
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    var enddate = Convert.ToDateTime(endDate);
                   query= query.Where(x => x.CreatedTime.Date >= startDate.Date && x.CreatedTime.Date <= enddate.Date);
                }
                else
                {
                    query=query.Where(x => x.CreatedTime.Date >= startDate.Date);
                }
            }
            else if(filter==FilterEnumWO.ByStatus)
            {
                string stageCode = "";
                if (stage == FilterEnumWOStage.INITWO)
                    stageCode = "INITWO";
                if (stage == FilterEnumWOStage.WOCOMPLETED)
                    stageCode = "WOCOMPLETED";
                if (stage == FilterEnumWOStage.WOPROGRESS)
                    stageCode = "WOPROGRESS";
                query= query.Include(x=>x.Stage).Where(x => x.Stage.StageCode == stageCode);

            }
            else if (filter == FilterEnumWO.ByAssigned)
            {

            }
            List<WorkOrderAssigned> workOrderAssigned = null;
            var count = query.Count();
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin")) { 
                
            }
            workOrderAssigned = await query.OrderByDescending(x => x.CreatedTime).Skip(pageNumber * iteminpage).Take(iteminpage).Select(x => new WorkOrderAssigned
                {
                    CreatedDate = x.CreatedTime.ToString("dd-MMM-yy"),
                    Description = x.Description,
                    Id = x.Id,
                    AssignedTo = x.AssignedTo.UserName,
                    AssignedToSection = x.AssignedToRole.Name
             }).AsNoTracking().ToListAsync();
            
            
            Pagination<List<WorkOrderAssigned>> pagination = new Pagination<List<WorkOrderAssigned>>
            {
                Payload = workOrderAssigned,
                CurrentPage = pageNumber,
                ItemsPerPage = count > iteminpage ? iteminpage : count,
                PageCount  =   count <= iteminpage ? 1 :  count / iteminpage + 1
            };

            return pagination;
        }

        public async Task<EditWorkOrder> GetEditWO(long id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            var editwo = await _workOrder.Get(x => x.Id == id).Include(x => x.Issue).Include(x => x.Item).Include(x => x.AssignedTo).Include(x => x.AssignedToRole).Include(x => x.AssignedToRole).Select(x => new
            {
                obj = new EditWorkOrder
                {
                    Id = x.Id,
                    PropertyName = x.Property.PropertyName,
                    Description = x.Description,
                    Issue = x.IssueId,
                    Item = x.ItemId,
                    CreatedDate = x.CreatedTime,
                    Department = x.AssignedToRole.DepartmentId.GetValueOrDefault(),
                    Section = x.AssignedToRole.Id
                },
                propId = x.PropertyId
            }).AsNoTracking().FirstOrDefaultAsync();
            editwo.obj.Items = await _itemRepo.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.ItemName
            }).AsNoTracking().ToListAsync();
            editwo.obj.Issues = await _issueRepo.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.IssueName
            }).AsNoTracking().ToListAsync();
            editwo.obj.Departments = await _department.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.DepartmentName
            }).ToListAsync();
            var propdetail = await GetAreaLocation(editwo.propId);
            editwo.obj.Area = propdetail.Area;
            editwo.obj.Location = propdetail.Location;
            editwo.obj.Sections = await _role.Get(x => x.DepartmentId == editwo.obj.Department).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.Name
            }).AsNoTracking().ToListAsync();
            return editwo.obj;
        }

        public async Task<bool> EditWO(EditWorkOrder editWorkOrder)
        {
            var wo = await _workOrder.Get(x => x.Id == editWorkOrder.Id).FirstOrDefaultAsync();
            if (wo != null)
            {
                wo.Description = editWorkOrder.Description;
                wo.AssignedToRoleId = editWorkOrder.Section;
                wo.IssueId = editWorkOrder.Issue;
                wo.ItemId = editWorkOrder.Item;
            }
            var status = await _workOrder.Update(wo);
            if (status > 0)
                return true;
            return false;
        }

        public async Task<Pagination<List<CommentDTO>>> GetPaginationComment(long workorderId,int pageNumber)
        {
            var itemsinpage = 4;
            var count = _comments.GetAll().Where(x => x.WorkOrderId == workorderId).Count();
            var obj = await _comments.GetAll().Where(x => x.WorkOrderId == workorderId).Include(x => x.Replies).OrderByDescending(x => x.CreatedTime).OrderByDescending(x=>x.CreatedTime).Select(x => new CommentDTO() { 
            CommentBy=x.CreatedByUserName,
            CommentDate=x.CreatedTime.ToString("dd-MMM-yy hh:mm:ss tt"),
            CommentString=x.Comment,
            Id=x.Id,
            Reply= x.Replies.Select(x=> new ReplyDTO { 
               Id=x.Id,
               RepliedTo=x.repliedTo,
               ReplyString=x.ReplyString,
               RepliedDate=x.CreatedTime.ToString("dd-MMM-yy hh:mm:ss tt"),
               RepliedBy=x.CreatedByUserName
            }).ToList()
            }).Skip(itemsinpage*pageNumber).Take(itemsinpage).ToListAsync();

            Pagination<List<CommentDTO>> pagedcomments = new Pagination<List<CommentDTO>>();
            pagedcomments.Payload = obj;
            pagedcomments.PageCount =  count <= itemsinpage ? 1 : count / itemsinpage + 1;
            pagedcomments.CurrentPage = pageNumber;
            
            return pagedcomments;

        }

        public async Task<bool> PostComment(Post post)
        {
            var status = false;
            if (post != null)
            {
                if (post.ParentId == 0)
                {
                    Comments comment = new Comments
                    {
                        WorkOrderId = post.WorkOrderId,
                        Comment = post.Comment,

                    };
                    var res=await _comments.Add(comment);
                    status = res > 0 ? true : false;
                }
                else
                {
                    var comm = _comments.Get(x => x.Id == post.ParentId).FirstOrDefault();
                    if (comm != null)
                    {
                        if (comm.Replies == null)
                            comm.Replies = new HashSet<Reply>();
                        comm.Replies.Add(new Reply
                        {
                            ReplyString = post.Comment,
                            repliedTo = post.RepliedTo,
                        });
                        var res=await _comments.Update(comm);
                        status = res > 0 ? true : false;
                    }

                }
            }
            return status;
        }


    }
}