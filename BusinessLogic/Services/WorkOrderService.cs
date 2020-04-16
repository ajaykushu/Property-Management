using BusinessLogic.Interfaces;
using BusinessLogic.StateMachine;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities.CustomException;
using Utilities.Interface;

namespace BusinessLogic.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IRepo<Issue> _issueRepo;
        private readonly UserManager<ApplicationUser> _appuser;
        private readonly IRepo<Item> _itemRepo;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<Department> _department;
        private readonly IRepo<ApplicationRole> _role;
        private readonly IRepo<WorkOrder> _workOrder;
        private readonly IRepo<Stage> _stage;
        private readonly IRepo<Comments> _comments;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepo<Location> _location;
        private readonly IRepo<Area> _area;
        private readonly IImageUploadInFile _imageuploadinfile;

        public WorkOrderService(IRepo<Issue> issueRepo, IRepo<Item> itemRepo, IRepo<UserProperty> userProperty, IRepo<Department> department, IRepo<WorkOrder> workOrder, IRepo<ApplicationRole> role, IRepo<Stage> stage, IHttpContextAccessor httpContextAccessor, IRepo<Comments> comments, IImageUploadInFile imageuploadinfile, UserManager<ApplicationUser> appuser, IRepo<Location> location, IRepo<Area> area)
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
            _imageuploadinfile = imageuploadinfile;
            _appuser = appuser;
            _area = area;
            _location = location;
        }

        public async Task<bool> CreateWO(CreateWO createWO)
        {
            WorkOrder workOrder = new WorkOrder
            {
                RequestedBy = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value,
                PropertyId = createWO.Property,
                IssueId = createWO.Issue,
                ItemId = createWO.Item,
                Description = createWO.Description,
                AssignedToRoleId = createWO.Section,
                DueDate=createWO.DueDate,
                LocationId=createWO.LocationId,
                AreaId=createWO.AreaId
            };

            workOrder.StageId = _stage.Get(x => x.StageCode == "INITWO").Select(x => x.Id).FirstOrDefault();
            if (createWO.File != null)
            {
                string path = await _imageuploadinfile.UploadAsync(createWO.File, "WOFiles");
                workOrder.AttachmentPath = path;
            }

            var status = await _workOrder.Add(workOrder);
            if (status > 0) return true;
            return false;
        }

        public async Task<WorkOrderDetail> GetWODetail(long id)
        {
            var workorder = await _workOrder.Get(x => x.Id == id).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Stage).Include(x => x.AssignedTo).Include(x => x.AssignedToRole).ThenInclude(x => x.Department).Include(x=>x.Area).Include(x=>x.Location).Select(x => new
                WorkOrderDetail
                {
                    PropertyName = x.Property.PropertyName,
                    Issue = x.Issue.IssueName,
                    StageCode = x.Stage.StageCode,
                    StageDescription = x.Stage.StageDescription,
                    Item = x.Item.ItemName,
                    CreatedTime = x.CreatedTime,
                    DueDate=x.DueDate,
                    UpdatedTime = x.UpdatedTime,
                    Department = x.AssignedToRole.Department.DepartmentName,
                    Section = x.AssignedToRole.Name,
                    AssignedToUser = x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")",
                    Requestedby = x.RequestedBy,
                    Id = x.Id,
                    UpdatedBy = x.UpdatedByUserName,
                    Description = x.Description,
                    Location=x.Location.LocationName,
                    Area=x.Area.AreaName,
                    Attachment =string.Concat("https://",_httpContextAccessor.HttpContext.Request.Host.Value , "/ImageFileStore/" , x.AttachmentPath)
            }).AsNoTracking().FirstOrDefaultAsync();
            var users = await _appuser.GetUsersInRoleAsync(workorder.Section);
            workorder.Users = users.Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.UserName + " (" + x.FirstName + " " + x.LastName + ")"
            }).ToList();
            return workorder;
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

                Location = await _location.GetAll().Select(x => new SelectItem {Id=x.Id,PropertyName=x.LocationName }).ToListAsync(),
                Departments = await _department.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.DepartmentName }).ToListAsync(),
                DueDate = DateTime.Now,
                
            };

            return wo;
        }

        public async Task<List<SelectItem>> GetSection(long id)
        {
            var res = await  _role.GetAll().Where(x => x.DepartmentId == id).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.Name
            }).AsNoTracking().ToListAsync();
            return res;
        }

        public async Task<Pagination<List<WorkOrderAssigned>>> GetWO(int pageNumber, FilterEnumWO filter, string matchStr, FilterEnumWOStage stage, string endDate)
        {
            int iteminpage = 8;
            var query = _workOrder.GetAll();

            if (!string.IsNullOrWhiteSpace(matchStr) && filter == FilterEnumWO.ByDate)
            {
                var startDate = Convert.ToDateTime(matchStr);
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    var enddate = Convert.ToDateTime(endDate);
                    query = query.Where(x => x.CreatedTime.Date >= startDate.Date && x.CreatedTime.Date <= enddate.Date);
                }
                else
                {
                    query = query.Where(x => x.CreatedTime.Date >= startDate.Date);
                }
            }
            else if (filter == FilterEnumWO.ByStatus)
            {
                string stageCode = "";
                if (stage == FilterEnumWOStage.INITWO)
                    stageCode = "INITWO";
                if (stage == FilterEnumWOStage.WOCOMP)
                    stageCode = "WOCOMP";
                if (stage == FilterEnumWOStage.WOPROG)
                    stageCode = "WOPROG";
                query = query.Include(x => x.Stage).Where(x => x.Stage.StageCode.ToLower().Equals(stageCode.ToLower()));
            }
            else if (filter == FilterEnumWO.ByAssigned && !string.IsNullOrWhiteSpace(matchStr))
            {
                query = query.Where(x => x.AssignedTo.UserName.ToLower().StartsWith(matchStr.ToLower()));
            }

            List<WorkOrderAssigned> workOrderAssigned = null;
            var count = query.Count();
            var role = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Role).Value;
            var username = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            query = query.Include(x => x.AssignedToRole).Include(x => x.AssignedTo);
            if (role.Equals("User"))
            {
                query = query.Where(x => x.CreatedByUserName.Equals(username));
            }
            else if (!role.Equals("Admin"))
            {
                query = query.Where(x => x.AssignedToRole.Name.ToLower().Equals(role.ToLower()));
            }
            

            workOrderAssigned = await query.Include(x => x.Stage).OrderByDescending(x => x.CreatedTime).Skip(pageNumber * iteminpage).Take(iteminpage).Select(x => new WorkOrderAssigned
            {
                CreatedDate = x.CreatedTime.ToString("dd-MMM-yy"),
                Description = x.Description,
                Id = x.Id,
                Stage = x.Stage.StageCode.ToLower(),
                AssignedTo = x.AssignedTo.UserName,
                AssignedToSection = x.AssignedToRole.Name
            }).AsNoTracking().ToListAsync();
            foreach (var item in workOrderAssigned)
                if (item.AssignedTo != null && item.AssignedTo.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                    item.AssignedTo = "Me";

            Pagination<List<WorkOrderAssigned>> pagination = new Pagination<List<WorkOrderAssigned>>
            {
                Payload = workOrderAssigned,
                CurrentPage = pageNumber,
                ItemsPerPage = count > iteminpage ? iteminpage : count,
                PageCount = count <= iteminpage ? 1 : count / iteminpage + 1
            };

            return pagination;
        }

        public async Task<EditWorkOrder> GetEditWO(long id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            var editwo = await _workOrder.Get(x => x.Id == id).Include(x => x.Issue).Include(x => x.Item).Include(x => x.AssignedTo).Include(x => x.AssignedToRole).Include(x => x.AssignedToRole).Select(x => 
                  new EditWorkOrder
                {
                    Id = x.Id,
                    PropertyName = x.Property.PropertyName,
                    Description = x.Description,
                    Issue = x.IssueId,
                    Item = x.ItemId,
                    CreatedDate = x.CreatedTime,
                    Department = x.AssignedToRole.DepartmentId.GetValueOrDefault(),
                    Section = x.AssignedToRole.Id,
                    DueDate=x.DueDate,
                    LocationId=x.LocationId.GetValueOrDefault(),
                    AreaId=x.AreaId.GetValueOrDefault()
                }
            ).AsNoTracking().FirstOrDefaultAsync();
            editwo.Items = await _itemRepo.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.ItemName
            }).AsNoTracking().ToListAsync();
            editwo.Issues = await _issueRepo.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.IssueName
            }).AsNoTracking().ToListAsync();
            editwo.Departments = await _department.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.DepartmentName
            }).ToListAsync();
            editwo.Location = await _location.GetAll()
                .Select(x => new SelectItem { Id = x.Id, PropertyName = x.LocationName })
                .ToListAsync();
            editwo.Area = await _area.GetAll().Where(x=>x.LocationId==editwo.LocationId).Select(x => new SelectItem { Id = x.Id, PropertyName = x.AreaName }).ToListAsync();
            editwo.Sections = await _role.Get(x => x.DepartmentId == editwo.Department).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.Name
            }).AsNoTracking().ToListAsync();
            return editwo;
        }

        public async Task<bool> EditWO(EditWorkOrder editWorkOrder)
        {
            var wo = await _workOrder.Get(x => x.Id == editWorkOrder.Id).FirstOrDefaultAsync();
            var prevpath = wo.AttachmentPath;
            if (wo != null)
            {
                if (editWorkOrder.File != null)
                {
                    var path = await _imageuploadinfile.UploadAsync(editWorkOrder.File, "WOFiles");
                    if (path != null)
                    {
                        wo.AttachmentPath = path;
                    }
                }
                wo.Description = editWorkOrder.Description;
                wo.AssignedToRoleId = editWorkOrder.Section;
                wo.IssueId = editWorkOrder.Issue;
                wo.ItemId = editWorkOrder.Item;
                wo.DueDate = editWorkOrder.DueDate;
                wo.LocationId = editWorkOrder.LocationId;
                wo.AreaId = editWorkOrder.AreaId;

            }

            var status = await _workOrder.Update(wo);
            if (status > 0)
            {
                if (prevpath != null)
                    _imageuploadinfile.Delete(prevpath);
                return true;
            }
            return false;
        }

        public async Task<Pagination<List<CommentDTO>>> GetPaginationComment(long workorderId, int pageNumber)
        {
            var itemsinpage = 4;
            var count = _comments.GetAll().Where(x => x.WorkOrderId == workorderId).Count();
            var obj = await _comments.GetAll().Where(x => x.WorkOrderId == workorderId).Include(x => x.Replies).OrderByDescending(x => x.CreatedTime).OrderByDescending(x => x.CreatedTime).Select(x => new CommentDTO()
            {
                CommentBy = x.CreatedByUserName,
                CommentDate = x.CreatedTime.ToString("dd-MMM-yy hh:mm:ss tt"),
                CommentString = x.Comment,
                Id = x.Id,
                Reply = x.Replies.Select(x => new ReplyDTO
                {
                    Id = x.Id,
                    RepliedTo = x.RepliedTo,
                    ReplyString = x.ReplyString,
                    RepliedDate = x.CreatedTime.ToString("dd-MMM-yy hh:mm:ss tt"),
                    RepliedBy = x.CreatedByUserName
                }).ToList()
            }).Skip(itemsinpage * pageNumber).Take(itemsinpage).ToListAsync();

            Pagination<List<CommentDTO>> pagedcomments = new Pagination<List<CommentDTO>>
            {
                Payload = obj,
                PageCount = count <= itemsinpage ? 1 : count / itemsinpage + 1,
                CurrentPage = pageNumber
            };

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
                    var res = await _comments.Add(comment);
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
                            RepliedTo = post.RepliedTo,
                        });
                        var res = await _comments.Update(comm);
                        status = res > 0 ? true : false;
                    }
                }
            }
            return status;
        }

        public async Task<bool> WorkOrderOperation(long workOrderId, ProcessEnumWOStage command)
        {
            var wo = await _workOrder.Get(x => x.Id == workOrderId).Include(x => x.Stage).Include(x => x.Comments).FirstOrDefaultAsync();

            var nextstate = WorkOrderState.GetNextState((FilterEnumWOStage)wo.Stage.Id, command);

            if (nextstate == (FilterEnumWOStage)wo.StageId && command == ProcessEnumWOStage.TrackIn)
                throw new BadRequestException("WO Completed");

            if (nextstate == (FilterEnumWOStage)wo.StageId && command == ProcessEnumWOStage.TrackOut)
                throw new BadRequestException("WO Reached Start Stage");

            wo.Comments.Add(new Comments
            {
                Comment = string.Concat("Work Order Stage Changed From " , wo.Stage.StageCode , " To " , nextstate)
            });
            wo.StageId = (int)nextstate;
            if (wo.Comments == null)
                wo.Comments = new List<Comments>();

            var status = await _workOrder.Update(wo);
            if (status > 0)
                return true;
            return false;
        }

        public async Task<bool> AssignToUser(long userId, long workOrderId)
        {
            var wo = _workOrder.Get(x => x.Id == workOrderId).Include(x => x.AssignedToRole).FirstOrDefault();
            if (wo != null)
            {
                var user = await _appuser.FindByIdAsync(userId + "");
                //check userid is in role
                var status = await _appuser.IsInRoleAsync(user, wo.AssignedToRole.Name);
                if (status)
                {
                    wo.AssignedToId = userId;
                    if (wo.Comments == null) wo.Comments = new List<Comments>();
                    wo.Comments.Add(
                        new Comments
                        {
                            Comment = string.Concat("Assigned To User: " , user.UserName , " (" , user.FirstName , " " + user.LastName , ")")
                        }
                    );
                    var updateStatus = await _workOrder.Update(wo);
                    if (updateStatus > 0)
                        return true;
                }
            }
            return false;
        }

        public async  Task<List<SelectItem>> GetArea(long id)
        {
            var res =await  _area.GetAll().Where(x => x.LocationId == id).Select(x => new SelectItem
            {
                Id=x.Id,
                PropertyName=x.AreaName
            }).ToListAsync();
            return res;
        }
    }
}