using BusinessLogic.Interfaces;
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
using System.Threading.Tasks;
using Utilities.Interface;

namespace BusinessLogic.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IRepo<Issue> _issueRepo;
        private readonly UserManager<ApplicationUser> _appuser;
        private readonly IRepo<Item> _itemRepo;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<Property> _property;
        private readonly IRepo<Department> _department;
        private readonly IRepo<WorkOrder> _workOrder;
        private readonly IRepo<Stage> _stage;
        private readonly IRepo<Comment> _comments;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepo<SubLocation> _sublocation;
        private readonly IImageUploadInFile _imageuploadinfile;
        private readonly INotifier _notifier;
        public long userId;
        private readonly string _scheme;

        public WorkOrderService(IRepo<Issue> issueRepo, IRepo<Item> itemRepo, IRepo<UserProperty> userProperty, IRepo<Department> department, IRepo<WorkOrder> workOrder, IRepo<Stage> stage, IHttpContextAccessor httpContextAccessor, IRepo<Comment> comments, IImageUploadInFile imageuploadinfile, UserManager<ApplicationUser> appuser, IRepo<SubLocation> sublocation, IRepo<Property> property, INotifier notifier)
        {
            _issueRepo = issueRepo;
            _itemRepo = itemRepo;
            _userProperty = userProperty;
            _department = department;
            _workOrder = workOrder;
            _stage = stage;
            _httpContextAccessor = httpContextAccessor;
            _comments = comments;
            _imageuploadinfile = imageuploadinfile;
            _appuser = appuser;
            _sublocation = sublocation;
            _property = property;
            _notifier = notifier;
            userId = Convert.ToInt64(_httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value);
            _scheme = _httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://";
        }

        public async Task<bool> CreateWO(CreateWO createWO, List<IFormFile> File)
        {
            WorkOrder workOrder = new WorkOrder
            {
                RequestedBy = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value,
                PropertyId = createWO.PropertyId,
                IssueId = createWO.IssueId,
                ItemId = createWO.ItemId,
                Description = createWO.Description,
                DueDate = createWO.DueDate,
                LocationId = createWO.LocationId,
                SubLocationId = createWO.SubLocationId,
                Priority = createWO.Priority
            };
            if (createWO.Category.Equals("user"))
                workOrder.AssignedToId = createWO.OptionId;
            else if (createWO.Category.Equals("department"))
                workOrder.AssignedToDeptId = createWO.OptionId;

            workOrder.StageId = _stage.Get(x => x.StageCode == "NEWO").AsNoTracking().Select(x => x.Id).FirstOrDefault();
            if (File != null)
            {
                foreach (var item in File)
                {
                    string path = await _imageuploadinfile.UploadAsync(item);
                    if (path != null)
                    {
                        if (workOrder.WOAttachments == null)
                            workOrder.WOAttachments = new List<WOAttachments>();
                        workOrder.WOAttachments.Add(new WOAttachments
                        {
                            FileName = item.FileName,
                            FilePath = path
                        });
                    }
                }
            }

            var status = await _workOrder.Add(workOrder);
            if (status > 0)
            {
                //create notification
                //getting all the person whom property is assigned
                var users = await _userProperty.GetAll().Where(x => x.PropertyId == createWO.PropertyId).Select(x => x.ApplicationUserId).Distinct().ToListAsync();
                if (!users.Contains(createWO.OptionId.GetValueOrDefault()))
                    users.Add(createWO.OptionId.GetValueOrDefault());
                if (!users.Contains(userId))
                    users.Add(userId);
                await _notifier.CreateNotification("Work Order Created with WOId " + workOrder.Id, users, workOrder.Id, "WA");
                return true;
            }
            return false;
        }

        public async Task<WorkOrderDetail> GetWODetail(string id)
        {
            var iteminpage = 12;
            var workorder = await _workOrder.Get(x => x.Id.Equals(id)).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Stage).Include(x => x.WOAttachments).Include(x => x.AssignedTo).ThenInclude(x => x.Department).Include(x => x.SubLocation).Include(x => x.Location).Select(x => new
                          WorkOrderDetail
            {
                PropertyName = x.Property.PropertyName,
                Issue = x.Issue.IssueName,
                StageCode = x.Stage.StageCode,
                StageDescription = x.Stage.StageDescription,
                Item = x.Item.ItemName,
                CreatedTime = x.CreatedTime,
                DueDate = x.DueDate,
                UpdatedTime = x.UpdatedTime,
                AssignedTo = x.AssignedTo!=null?x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")":x.AssignedToDept!=null?x.AssignedToDept.DepartmentName:"Anyone",
                Requestedby = x.RequestedBy,
                Id = x.Id,
                Priority = x.Priority,
                UpdatedBy = x.UpdatedByUserName,
                Description = x.Description,
                Location = x.Location.LocationName,
                SubLocation = x.SubLocation.AreaName,
                Attachment = x.WOAttachments.Select(x => new KeyValuePair<string, string>(
                 x.FileName,
                 string.Concat(_scheme, _httpContextAccessor.HttpContext.Request.Host.Value, "/", x.FilePath)
                 )).ToList()
            }).AsNoTracking().FirstOrDefaultAsync();
            workorder.Stages = _stage.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = string.Concat(x.StageCode, "(", x.StageDescription, ")")
            }).ToList();
            var comment = await GetComment(workorder.Id, 0);
            var count = _comments.Get(x => x.WorkOrderId == workorder.Id).Count();
            Pagination<List<CommentDTO>> pagedcomments = new Pagination<List<CommentDTO>>
            {
                Payload = comment,
                ItemsPerPage = count > iteminpage ? iteminpage : count,
                PageCount = count <= iteminpage ? 1 : count % iteminpage == 0 ? count / iteminpage : count / iteminpage + 1,
                CurrentPage = 0
            };
            workorder.Comments = pagedcomments;
            return workorder;
        }

        public async Task<CreateWO> GetCreateWOModel(long userId)
        {
            var property = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).ThenInclude(x => x.Locations).ThenInclude(x => x.SubLocations).AsNoTracking().ToListAsync();
            var primaryprop = property.Where(x => x.IsPrimary == true).Select(x => x.Property).FirstOrDefault();

            CreateWO wo = new CreateWO()
            {
                Properties = property.Select(x => new SelectItem
                {
                    Id = x.Property.Id,
                    PropertyName = x.Property.PropertyName
                }).ToList(),
                PropertyId = primaryprop != null ? primaryprop.Id : 0,
                Issues = await _issueRepo.GetAll().Select(x => new SelectItem
                {
                    Id = x.Id,
                    PropertyName = x.IssueName
                }).AsNoTracking().ToListAsync(),
                Items = await _itemRepo.GetAll().Select(x => new SelectItem
                {
                    Id = x.Id,
                    PropertyName = x.ItemName
                }).AsNoTracking().ToListAsync(),
                DueDate = DateTime.Now,
            };
            if (primaryprop != null && primaryprop.Locations != null)
                wo.Locations = primaryprop.Locations.Select(x => new SelectItem { Id = x.Id, PropertyName = x.LocationName }).ToList();
            wo.OptionId = 0;
            return wo;
        }

        public async Task<List<SelectItem>> GetDataByCategory(string category)
        {
            List<SelectItem> res = null;
            if (!string.IsNullOrWhiteSpace(category))
            {
                if (category.Equals("department", StringComparison.InvariantCultureIgnoreCase))
                {
                    res = await _department.GetAll().Select(x => new SelectItem
                    {
                        Id = x.Id,
                        PropertyName = x.DepartmentName
                    }).AsNoTracking().ToListAsync();
                }
                else if (category.Equals("user", StringComparison.InvariantCultureIgnoreCase))
                {
                    var tempres = _appuser.Users.Include(x => x.Department).OrderBy(x => x.FirstName).AsEnumerable().GroupBy(x => x.Department.DepartmentName).ToList();
                    res = new List<SelectItem>();
                    if (tempres != null)
                    {
                        foreach (var item in tempres)
                        {
                            foreach (var subitem in item)
                            {
                                res.Add(new SelectItem()
                                {
                                    Id = subitem.Id,
                                    PropertyName = string.Concat("Dept: ", item.Key, " ", "User: ", subitem.FirstName + " " + subitem.LastName)
                                });
                            }
                        }
                    }
                }
            }
            return res;
        }

        public async Task<Pagination<List<WorkOrderAssigned>>> GetWO(WOFilterModel wOFilterModel)
        {
            int iteminpage = 20;
            var query = _workOrder.GetAll();
            query = await FilterWO(wOFilterModel, query);
            List<WorkOrderAssigned> workOrderAssigned = null;
            var count = query.Count();
            workOrderAssigned = await query.OrderByDescending(x => x.DueDate).Skip(wOFilterModel.PageNumber * iteminpage).Take(iteminpage).Select(x => new WorkOrderAssigned
            {
                DueDate = x.DueDate.ToString("dd-MMM-yy"),
                Description = x.Description,
                Id = x.Id,
                IsRecurring = x.IsRecurring,
                Stage = x.Stage.StageCode.ToLower(),
                AssignedTo = x.AssignedTo != null ? x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")" : x.AssignedToDept != null ? x.AssignedToDept.DepartmentName : "Anyone",
                Property = new SelectItem { Id = x.PropertyId, PropertyName = x.Property.PropertyName }
            }).AsNoTracking().ToListAsync();

            Pagination<List<WorkOrderAssigned>> pagination = new Pagination<List<WorkOrderAssigned>>
            {
                Payload = workOrderAssigned,
                CurrentPage = wOFilterModel.PageNumber,
                ItemsPerPage = count > iteminpage ? iteminpage : count,
                PageCount = count <= iteminpage ? 1 : count % iteminpage == 0 ? count / iteminpage : count / iteminpage + 1
            };

            return pagination;
        }

        public async Task<EditWorkOrder> GetEditWO(string id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            var temp = await _workOrder.Get(x => x.Id.Equals(id)).Include(x => x.WOAttachments).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Property).ThenInclude(x=>x.Locations).Include(x=>x.AssignedToDept).Include(x=>x.AssignedTo).AsNoTracking().FirstOrDefaultAsync();
            var editwo = new EditWorkOrder
            {
                Id = temp.Id,
                PropertyName = temp.Property.PropertyName,
                Locations = temp.Property.Locations.Select(x => new SelectItem { Id = x.Id, PropertyName = x.LocationName }).ToList(),
                Description = temp.Description,
                IssueId = temp.IssueId,
                ItemId = temp.ItemId,
                CreatedDate = temp.CreatedTime,
                Priority = temp.Priority,
                DueDate = temp.DueDate,
                LocationId = temp.LocationId.GetValueOrDefault(),
                SubLocationId = temp.SubLocationId.GetValueOrDefault(),
                FileAvailable = temp.WOAttachments.Select(x => new KeyValuePair<string, string>(x.FileName,
                 string.Concat(_scheme, _httpContextAccessor.HttpContext.Request.Host.Value, "/", x.FilePath))).ToList()
            };
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

            editwo.SubLocations = await _sublocation.GetAll().Where(x => x.LocationId == editwo.LocationId).Select(x => new SelectItem { Id = x.Id, PropertyName = x.AreaName }).AsNoTracking().ToListAsync();
            //get selected department and user
            if (temp.AssignedTo != null)
            {
                editwo.Category = "user";
                editwo.OptionId = (int)temp.AssignedToId;
                editwo.Options = await GetDataByCategory("user");
            }
            else if (temp.AssignedToDeptId != null)
            {
                editwo.Category = "department";
                editwo.OptionId = temp.AssignedToDeptId;
                editwo.Options = await GetDataByCategory("department");
            }
            else
                editwo.Category = "anyone";

            return editwo;
        }

        public async Task<bool> EditWO(EditWorkOrder editWorkOrder, List<IFormFile> File)
        {
            var wo = await _workOrder.Get(x => x.Id.Equals(editWorkOrder.Id)).Include(x => x.WOAttachments).Include(x => x.Comments).FirstOrDefaultAsync();
            if (wo != null)
            {
                if (File != null)
                {
                    foreach (var item in File)
                    {
                        var path = await _imageuploadinfile.UploadAsync(item);
                        if (path != null)
                        {
                            if (wo.WOAttachments == null) wo.WOAttachments = new List<WOAttachments>();
                            wo.WOAttachments.Add(new WOAttachments
                            {
                                FileName = item.FileName,
                                FilePath = path
                            });
                        }
                    }
                }
                wo.Description = editWorkOrder.Description;
                wo.IssueId = editWorkOrder.IssueId;
                wo.ItemId = editWorkOrder.ItemId;
                wo.DueDate = editWorkOrder.DueDate;
                wo.LocationId = editWorkOrder.LocationId;
                wo.Priority = editWorkOrder.Priority;
                wo.SubLocationId = editWorkOrder.SubLocationId;
                if (editWorkOrder.Category.Equals("user"))
                {
                    wo.AssignedToDeptId = null;
                    wo.AssignedToId = editWorkOrder.OptionId;
                }
                else if (editWorkOrder.Category.Equals("department"))
                {
                    wo.AssignedToId = null;
                    wo.AssignedToDeptId = editWorkOrder.OptionId;
                }
                else
                {
                    wo.AssignedToDeptId = null;
                    wo.AssignedToId = null;
                }
            }
            if (!string.IsNullOrWhiteSpace(editWorkOrder.FilesRemoved))
            {
                var remove = editWorkOrder.FilesRemoved.Contains(',') ? editWorkOrder.FilesRemoved.Split(",") : new String[] { editWorkOrder.FilesRemoved };
                foreach (var item in remove)
                {
                    var tempurl = item.Replace(_scheme + _httpContextAccessor.HttpContext.Request.Host.Value + "/", "");
                    _imageuploadinfile.Delete(tempurl);
                    var woAttch = wo.WOAttachments.Where(x => x.FilePath.Equals(tempurl)).FirstOrDefault();
                    wo.WOAttachments.Remove(woAttch);
                }
            }

            var status = await _workOrder.Update(wo);
            if (status > 0)
            {
                var users = await _userProperty.GetAll().Include(x => x.Property).Where(x => x.Property.PropertyName.Equals(editWorkOrder.PropertyName)).Select(x => x.ApplicationUserId).Distinct().ToListAsync();
                if (editWorkOrder.OptionId.HasValue && !users.Contains(editWorkOrder.OptionId.GetValueOrDefault()))
                    users.Add(editWorkOrder.OptionId.GetValueOrDefault());
                if (!users.Contains(userId))
                    users.Add(userId);
                await _notifier.CreateNotification("Some Changed are done in Workorder for WOId " + editWorkOrder.Id, users, editWorkOrder.Id, "WE");
                return true;
            }
            return false;
        }

        public async Task<List<CommentDTO>> GetComment(string workorderId, int pageNumber)
        {
            var itemsinpage = 12;
            var obj = await _comments.GetAll().Where(x => x.WorkOrderId.Equals(workorderId)).Include(x => x.ApplicationUser).Include(x => x.Replies).ThenInclude(x => x.ApplicationUser).OrderByDescending(x => x.UpdatedTime).Select(x => new CommentDTO()
            {
                CommentBy = string.Concat(x.ApplicationUser.FirstName, " ", x.ApplicationUser.LastName, "(", x.ApplicationUser.UserName, ")"),
                CommentDate = x.CreatedTime.ToString("dd-MMM-yy hh:mm:ss tt"),
                CommentString = x.CommentString,
                Id = x.Id,
                Reply = x.Replies.Select(x => new ReplyDTO
                {
                    Id = x.Id,
                    RepliedTo = string.Concat(x.Comment.ApplicationUser.FirstName, " ", x.Comment.ApplicationUser.LastName, "(", x.ApplicationUser.UserName, ")"),
                    ReplyString = x.ReplyString,
                    RepliedDate = x.CreatedTime.ToString("dd-MMM-yy hh:mm:ss tt"),
                    RepliedBy = string.Concat(x.ApplicationUser.FirstName, " ", x.ApplicationUser.LastName, "(", x.ApplicationUser.UserName, ")")
                }).ToList()
            }).Skip(itemsinpage * pageNumber).Take(itemsinpage).ToListAsync();

            return obj;
        }

        public async Task<bool> PostComment(Post post)
        {
            var status = false;
            var name = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.GivenName).Value;
            string type = String.Empty;
            string message = string.Empty;
            if (post != null)
            {
                if (post.ParentId == 0)
                {
                    Comment comment = new Comment
                    {
                        WorkOrderId = post.WorkOrderId,
                        CommentString = post.Comment,
                        CommentById = userId
                    };
                    var res = await _comments.Add(comment);
                    status = res > 0 ? true : false;
                    type = "CA";
                    message = name + "Commented on workorder " + post.WorkOrderId;
                }
                else
                {
                    var comm = _comments.Get(x => x.Id == post.ParentId).Include(x => x.ApplicationUser).FirstOrDefault();
                    if (comm != null)
                    {
                        if (comm.Replies == null)
                            comm.Replies = new HashSet<Reply>();
                        comm.Replies.Add(new Reply
                        {
                            ReplyString = post.Comment,
                            RepliedTo = post.RepliedTo,
                            ReplyById = userId
                        });

                        var res = await _comments.Update(comm);
                        status = res > 0 ? true : false;
                        type = "RA";
                        message = String.Concat(name, " replied to comment commented by ", comm.ApplicationUser.FirstName, " ", comm.ApplicationUser.LastName + " attached to " + post.WorkOrderId);
                    }
                }
            }
            if (status)
            {
                var wo = _workOrder.Get(x => x.Id == post.WorkOrderId).AsNoTracking().FirstOrDefault();
                var users = await _userProperty.GetAll().Where(x => x.PropertyId == wo.PropertyId).Select(x => x.ApplicationUserId).Distinct().ToListAsync();

                var repliedto = post.RepliedTo != null ? await _appuser.FindByNameAsync(post.RepliedTo) : null;
                if (wo.AssignedToId.HasValue && !users.Contains(wo.AssignedToId.GetValueOrDefault()))
                    users.Add(wo.AssignedToId.GetValueOrDefault());
                if (!users.Contains(userId))
                    users.Add(userId);
                if (repliedto != null && !users.Contains(repliedto.Id))
                    users.Add(repliedto.Id);
                await _notifier.CreateNotification(message, users, wo.Id, type);
            }
            return status;
        }

        public async Task<bool> WorkOrderStageChange(string id, int stageId, string comment)
        {
            var wo = await _workOrder.Get(x => x.Id.Equals(id)).Include(x => x.Stage).Include(x => x.Comments).FirstOrDefaultAsync();
            var stage = await _stage.Get(x => x.Id == stageId).FirstOrDefaultAsync();
            if (stage != null)
            {
                if (wo.Comments == null)
                    wo.Comments = new List<Comment>();
                wo.Comments.Add(new Comment
                {
                    CommentString = string.Concat("Work Order Stage Changed From ", wo.Stage.StageCode, " To ", stage.StageCode, "--Additional Comment:", comment),
                    CommentById = userId
                });
                wo.StageId = stageId;
                var status = await _workOrder.Update(wo);
                if (status > 0)
                {
                    var users = await _userProperty.GetAll().Where(x => x.PropertyId == wo.PropertyId).Select(x => x.ApplicationUserId).Distinct().ToListAsync();
                    if (!users.Contains(userId))
                        users.Add(userId);
                    await _notifier.CreateNotification("Work Order Stage Changed for WOId " + wo.Id, users, wo.Id, "WE");
                    return true;
                }
            }
            return false;
        }

        public Task<List<SelectItem>> GetLocation(long id)
        {
            var res = _property.Get(x => x.Id == id).Include(x => x.Locations).Select(x =>
                    x.Locations.Select(y => new SelectItem
                    {
                        Id = y.Id,
                        PropertyName = y.LocationName
                    }).ToList()).AsNoTracking().FirstOrDefaultAsync();
            return res;
        }

        public async Task<List<AllWOExport>> WOExport(WOFilterModel wOFilterModel)
        {
            var query = _workOrder.GetAll();
            query = await FilterWO(wOFilterModel, query);
            List<AllWOExport> workOrders = null;
            workOrders = await query.OrderByDescending(x => x.DueDate).Select(x => new AllWOExport
            {
                PropertyName = x.Property.PropertyName,
                Issue = x.Issue.IssueName,
                StageCode = x.Stage.StageCode,
                StageDescription = x.Stage.StageDescription,
                Item = x.Item.ItemName,
                CreatedTime = x.CreatedTime,
                DueDate = x.DueDate,
                UpdatedTime = x.UpdatedTime,
                AssignedTo = x.AssignedTo != null ? x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")" : x.AssignedToDept != null ? x.AssignedToDept.DepartmentName : "Anyone",
                Requestedby = x.RequestedBy,
                Id = x.Id,
                Priority = x.Priority,
                UpdatedBy = x.UpdatedByUserName,
                Description = x.Description,
                Location = x.Location.LocationName,
                SubLocation = x.SubLocation.AreaName,
                Attachment = x.WOAttachments.Select(x => x.FileName).ToList()
            }).AsNoTracking().ToListAsync();
            return workOrders;
        }

        private async Task<IQueryable<WorkOrder>> FilterWO(WOFilterModel wOFilterModel, IQueryable<WorkOrder> query)
        {
            if (!string.IsNullOrWhiteSpace(wOFilterModel.CreationStartDate))
            {
                var startDate = Convert.ToDateTime(wOFilterModel.CreationStartDate);
                query = query.Where(x => x.CreatedTime.Date >= startDate.Date);
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.CreationEndDate))
            {
                var enddate = Convert.ToDateTime(wOFilterModel.CreationEndDate);
                query = query.Where(x => x.CreatedTime.Date <= enddate.Date);
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.Status))
            {
                query = query.Where(x => x.Stage.StageCode.ToLower().Equals(wOFilterModel.Status));
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.AssignedTo))
            {
                query = query.Where(x => (x.AssignedTo != null && x.AssignedTo.FirstName.ToLower().StartsWith(wOFilterModel.AssignedTo))||
                (x.AssignedToDept != null && x.AssignedToDept.DepartmentName.ToLower().StartsWith(wOFilterModel.AssignedTo)|| (x.AssignedTo != null && x.AssignedTo.Email.ToLower().StartsWith(wOFilterModel.AssignedTo)))
                );
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.DueDate))
            {
                var dueDate = Convert.ToDateTime(wOFilterModel.DueDate);
                query = query.Where(x => x.DueDate.Date == dueDate.Date);
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.Priority))
            {
                var index = Convert.ToInt32(wOFilterModel.Priority);
                query = query.Where(x => x.Priority == index);
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.PropertyName))
            {
                query = query.Where(x => x.Property.PropertyName.ToLower().StartsWith(wOFilterModel.PropertyName));
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.Vendor))
            {
                query = query.Where(x => x.Vendor!=null && x.Vendor.VendorName.ToLower().Equals(wOFilterModel.PropertyName));
            }
            var role = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Role).Value;
            var username = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var userdept = await _appuser.FindByNameAsync(username);
            query = query.Include(x => x.AssignedTo).Include(x=>x.AssignedToDept).Include(x => x.Stage).Include(x => x.Property).Include(x=>x.Vendor);
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                var propIds = await _userProperty.GetAll().Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == userId).AsNoTracking().Select(x => x.PropertyId).Distinct().ToListAsync();
                query = query.Where(x => propIds.Contains(x.Property.Id));
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                query = query.Where(x => (x.AssignedTo!=null && x.AssignedTo.UserName.Equals(username))||(x.AssignedToDept!=null && x.AssignedToDeptId==userdept.DepartmentId)|| (x.AssignedTo==null && x.AssignedToDept==null));
            }
            return query;
        }
    }
}