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

namespace BusinessLogic.Services
{
    public class WorkOrderBL : IWorkOrderBL
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

        public WorkOrderBL(IRepo<Issue> issueRepo, IRepo<Item> itemRepo, IRepo<UserProperty> userProperty, IRepo<Department> department, IRepo<WorkOrder> workOrder, IRepo<Status> status, IHttpContextAccessor httpContextAccessor, IRepo<Comment> comments, IImageUploadInFile imageuploadinfile, UserManager<ApplicationUser> appuser, IRepo<SubLocation> sublocation, IRepo<Property> property, INotifier notifier, IRepo<Vendor> vendors, IRepo<History> history, IRecurringWorkOrderJob recurringWorkOrderJob, IRepo<RecurringWO> recuringWo, IRepo<Effort> effort)
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

        public async Task<bool> CreateWO(CreateNormalWO createWO, List<IFormFile> File)
        {
            WorkOrder workOrder = new WorkOrder
            {
                RequestedBy = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value,
                PropertyId = createWO.PropertyId,
                IssueId = createWO.IssueId==-1? (int?)null: createWO.IssueId,
                CustomIssue=createWO.CustomIssue,
                ItemId = createWO.ItemId!=0? createWO.ItemId:(int?)null,
                Description = createWO.Description,
                DueDate = createWO.DueDate,
                LocationId = createWO.LocationId,
                VendorId = createWO.VendorId,
                SubLocationId = createWO.SubLocationId,
                Priority = createWO.Priority
            };
            if (createWO.Category.Equals("user"))
                workOrder.AssignedToId = createWO.OptionId;
            else if (createWO.Category.Equals("department"))
                workOrder.AssignedToDeptId = createWO.OptionId;

            workOrder.StatusId = _status.Get(x => x.StatusCode == "NEWO").AsNoTracking().Select(x => x.Id).FirstOrDefault();
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
                var users = await GetUsersToSendNotification(workOrder);
                await _notifier.CreateNotification("Work Order Created with WOId " + workOrder.Id, users, workOrder.Id, "WA");
                return true;
            }

            return false;
        }



        public async Task<WorkOrderDetail> GetWODetail(string id)
        {

            var iteminpage = 12;
            var workorder = await _workOrder.Get(x => x.Id.Equals(id)).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Status).Include(x => x.WOAttachments).Include(x => x.AssignedTo).ThenInclude(x => x.Department).Include(x => x.SubLocation).Include(x => x.Location).Include(x => x.Vendor).Select(x => new
                            WorkOrderDetail
            {
                PropertyName = x.Property.PropertyName,
                Issue = x.Issue!=null?x.Issue.IssueName:x.CustomIssue,
                StatusDescription = x.Status.StatusDescription,
                Item = x.Item.ItemName,
                CreatedTime = x.CreatedTime,
                ParentWOId = x.ParentWoId,
                StatusId=x.StatusId,
                Vendor = x.Vendor != null ? x.Vendor.VendorName : null,
                DueDate = x.DueDate.ToString("dd-MMM-yyyy"),
                UpdatedTime = x.UpdatedTime,
                AssignedTo = x.AssignedTo != null ? x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")" : x.AssignedToDept != null ? x.AssignedToDept.DepartmentName : "Anyone",
                Requestedby = x.RequestedBy,
                Id = x.Id,
                Priority = x.Priority,
                UpdatedBy = x.UpdatedByUserName,
                Description = x.Description,
                Location = x.Location.LocationName,
                SubLocation = x.SubLocation.AreaName,
                Attachment = x.WOAttachments.Select(x => new KeyValuePair<string, string>(
                 x.FileName,
                 string.Concat(_scheme, _httpContextAccessor.HttpContext.Request.Host.Value, "/api/", x.FilePath)
                 )).ToList()
            }).AsNoTracking().FirstOrDefaultAsync();
            workorder.Statuses = _status.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.StatusDescription
            }).AsNoTracking().ToList();

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

        public async Task<CreateNormalWO> GetCreateWOModel(long userId)
        {
            CreateNormalWO wo = new CreateNormalWO();
            /*wo.Items = await _itemRepo.Get(x=>x.Active).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.ItemName
            }).ToListAsync();*/
            if (_httpContextAccessor.HttpContext.User.IsInRole("Master Admin"))
            {
                var prop = await _property.GetAll().AsNoTracking().ToListAsync();
                wo.Properties = prop.Select(x => new SelectItem
                {
                    Id = x.Id,
                    PropertyName = x.PropertyName
                }).ToList();
            }
            else
            {
                var property = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).ThenInclude(x => x.Locations).AsNoTracking().ToListAsync();
               
                var primaryprop = property.Where(x => x.IsPrimary == true).Select(x => x.Property).FirstOrDefault();
                wo.Properties = property.Select(x => new SelectItem
                {
                    Id = x.Property.Id,
                    PropertyName = x.Property.PropertyName
                }).ToList();
                if (primaryprop != null && primaryprop.Locations != null)
                    wo.Locations = primaryprop.Locations.Where(y=>y.Active).Select(x => new SelectItem { Id = x.Id, PropertyName = x.LocationName }).ToList();
                wo.PropertyId = primaryprop != null ? primaryprop.Id : 0;
            }

            wo.Vendors = await _vendors.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.VendorName
            }).AsNoTracking().ToListAsync();
            wo.DueDate = DateTime.Now;

            wo.OptionId = 0;
            return wo;
        }

        public async Task<Dictionary<string, List<SelectItem>>> GetDataByCategory(string category)
        {
            Dictionary<string, List<SelectItem>> res = new Dictionary<string, List<SelectItem>>();
            if (!string.IsNullOrWhiteSpace(category))
            {
                if (category.Equals("department", StringComparison.InvariantCultureIgnoreCase))
                {
                    var tempres = await _department.GetAll().Select(item => new SelectItem
                    {
                        Id = item.Id,
                        PropertyName = item.DepartmentName
                    }).AsNoTracking().ToListAsync();
                    //mapping to dict
                    res.Add("", tempres);

                }
                else if (category.Equals("user", StringComparison.InvariantCultureIgnoreCase))
                {
                    var tempres = _appuser.Users.Include(x => x.Department).OrderBy(x => x.FirstName).AsEnumerable().GroupBy(x => x.Department.DepartmentName).ToList();

                    if (tempres != null)
                    {
                        foreach (var item in tempres)
                        {
                            var list = new List<SelectItem>();
                            foreach (var subitem in item)
                            {
                                list.Add(new SelectItem()
                                {
                                    Id = subitem.Id,
                                    PropertyName = string.Concat(subitem.FirstName + " " + subitem.LastName)
                                });
                            }
                            res.Add(item.Key, list);
                        }
                    }
                }
            }
            return res;
        }

        public async Task<Pagination<List<WorkOrderAssigned>>> GetWO(WOFilterDTO wOFilterModel)
        {
            int iteminpage = 20;
            var query = _workOrder.GetAll();
            int count = 0;
            query = await FilterWO(wOFilterModel, query);
            List<WorkOrderAssigned> workOrderAssigned = null;
            if (wOFilterModel.IsActive.HasValue)
                count = query.Where(x => x.IsActive == wOFilterModel.IsActive).Count();
            else
                count = query.Count();

            workOrderAssigned = await query.Skip(wOFilterModel.PageNumber * iteminpage).Take(iteminpage).Select(x => new WorkOrderAssigned
                {
                    DueDate = x.DueDate.ToString("dd-MMM-yy"),
                    Description = x.Description,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    ParentId = x.ParentWoId,
                    Property = new SelectItem { Id = x.PropertyId, PropertyName = x.Property.PropertyName },
                    Status = x.Status.StatusDescription,
                    AssignedTo = x.AssignedTo != null ? x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")" : x.AssignedToDept != null ? x.AssignedToDept.DepartmentName : "Anyone"
                }).ToListAsync();
               
            

            Pagination<List<WorkOrderAssigned>> pagination = new Pagination<List<WorkOrderAssigned>>
            {
                Payload = workOrderAssigned,
                CurrentPage = wOFilterModel.PageNumber,
                ItemsPerPage = count > iteminpage ? iteminpage : count,
                PageCount = count <= iteminpage ? 1 : count % iteminpage == 0 ? count / iteminpage : count / iteminpage + 1
            };

            return pagination;
        }

        public async Task<EditNormalWorkOrder> GetEditWO(string id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            var temp = await _workOrder.Get(x => x.Id.Equals(id)).Include(x => x.WOAttachments).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Property).ThenInclude(x => x.Locations).Include(x => x.AssignedToDept).Include(x => x.AssignedTo).AsNoTracking().FirstOrDefaultAsync();

            var editwo = new EditNormalWorkOrder();
            editwo.Id = temp.Id;
            editwo.PropertyName = temp.Property.PropertyName;
            editwo.Locations = temp.Property.Locations.Select(x => new SelectItem { Id = x.Id, PropertyName = x.LocationName }).ToList();
            editwo.Description = temp.Description;
            editwo.IssueId = temp.IssueId.HasValue? temp.IssueId.Value:-1;
            editwo.ItemId = temp.ItemId.GetValueOrDefault();
            editwo.CustomIssue = temp.CustomIssue;
            editwo.CreatedDate = temp.CreatedTime;
            editwo.VendorId = temp.VendorId;
            editwo.Priority = temp.Priority;
            editwo.DueDate = temp.DueDate;
            editwo.LocationId = temp.LocationId.GetValueOrDefault();
            editwo.SubLocationId = temp.SubLocationId.GetValueOrDefault();
            editwo.FileAvailable = temp.WOAttachments.Select(x => new KeyValuePair<string, string>(x.FileName,
             string.Concat(_scheme, _httpContextAccessor.HttpContext.Request.Host.Value, "/api/", x.FilePath))).ToList();

            editwo.Items = await _itemRepo.GetAll().Where(x=>x.LocationId==editwo.LocationId).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.ItemName
            }).AsNoTracking().ToListAsync();
            editwo.Vendors = await _vendors.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.VendorName
            }).AsNoTracking().ToListAsync();
            editwo.Issues = await _issueRepo.GetAll().Where(x=>x.ItemId==editwo.ItemId).Select(x => new SelectItem
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

        public async Task<bool> EditWO(EditNormalWorkOrder editWorkOrder, List<IFormFile> File)
        {
            History history = new History();
            var userObj = await _appuser.FindByIdAsync(userId.ToString());
            var wo = await _workOrder.Get(x => x.Id.Equals(editWorkOrder.Id) && x.Status.StatusCode != "COMP").Include(x => x.WOAttachments).Include(x => x.AssignedToDept).Include(x => x.AssignedTo).FirstOrDefaultAsync();

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
                wo.IssueId = editWorkOrder.IssueId!=-1? editWorkOrder.IssueId:(int?)null;
                wo.ItemId = editWorkOrder.ItemId != 0 ? editWorkOrder.ItemId : (int?)null;
                wo.DueDate = editWorkOrder.DueDate;
                wo.CustomIssue = editWorkOrder.CustomIssue;
                wo.LocationId = editWorkOrder.LocationId;
                wo.VendorId = editWorkOrder.VendorId;
                wo.Priority = editWorkOrder.Priority;
                wo.SubLocationId = editWorkOrder.SubLocationId;

                //adding history
                history.Entity = "WorkOrder";
                history.PropertyName = "Assigned To";
                history.RowId = wo.Id;
                history.OldValue = wo.AssignedTo != null ? string.Concat(wo.AssignedTo.FirstName, " ", wo.AssignedTo.LastName) : wo.AssignedToDept != null ? wo.AssignedToDept.DepartmentName : "NA";
                if (editWorkOrder.Category.Equals("user"))
                {

                    wo.AssignedToDeptId = null;
                    wo.AssignedToId = editWorkOrder.OptionId;
                    history.NewValue = _appuser.Users.Where(x => x.Id == editWorkOrder.OptionId).Select(x => string.Concat(x.FirstName, " ", x.LastName)).FirstOrDefault();
                }
                else if (editWorkOrder.Category.Equals("department"))
                {
                    wo.AssignedToId = null;
                    wo.AssignedToDeptId = editWorkOrder.OptionId;
                    history.NewValue = _department.Get(x => x.Id == editWorkOrder.OptionId).Select(x => x.DepartmentName).FirstOrDefault();
                }
                else
                {
                    wo.AssignedToDeptId = null;
                    wo.AssignedToId = null;
                    history.NewValue = "";
                }


                if (!string.IsNullOrWhiteSpace(editWorkOrder.FilesRemoved))
                {
                    var remove = editWorkOrder.FilesRemoved.Contains(',') ? editWorkOrder.FilesRemoved.Split(",") : new String[] { editWorkOrder.FilesRemoved };
                    foreach (var item in remove)
                    {
                        var path = item.Contains("/api") ? "/api/" : "/";
                        var tempurl = item.Replace(_scheme + _httpContextAccessor.HttpContext.Request.Host.Value + path, "");
                        _imageuploadinfile.Delete(tempurl);
                        var woAttch = wo.WOAttachments.Where(x => x.FilePath.Equals(tempurl)).FirstOrDefault();
                        wo.WOAttachments.Remove(woAttch);
                    }
                }





                var status = await _workOrder.Update(wo);

                await _history.Add(history);
                if (status > 0)
                {
                    var users = await GetUsersToSendNotification(wo);
                    await _notifier.CreateNotification(editWorkOrder.Id + " has been updated by " + userObj.FirstName + " " + userObj.LastName, users, editWorkOrder.Id, "WE");
                    return true;
                }
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

        public async Task<bool> PostComment(PostCommentDTO post)
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

                if (!post.WorkOrderId.Contains("R"))
                {
                    var wo = _workOrder.Get(x => x.Id == post.WorkOrderId).AsNoTracking().FirstOrDefault();
                    var users = await GetUsersToSendNotification(wo);
                    var repliedto = post.RepliedTo != null ? await _appuser.FindByNameAsync(post.RepliedTo) : null;
                    if (repliedto != null && !users.Contains(repliedto.Id))
                        users.Add(repliedto.Id);
                    await _notifier.CreateNotification(message, users, wo.Id, type);
                }
            }
            return status;
        }

        public async Task<bool> WorkOrderStatusChange(WorkOrderDetail workOrderDetail, IList<IFormFile> files)
        {
            var wo = await _workOrder.Get(x => x.Id.Equals(workOrderDetail.Id)).Include(x => x.Status).Include(x=>x.WOAttachments).FirstOrDefaultAsync();
            var status = await _status.Get(x => x.Id == workOrderDetail.StatusId).FirstOrDefaultAsync();
            if (status != null)
            {

                #region history Add
                History history = new History();
                history.PropertyName = "Status";
                history.Entity = "WorkOrder";
                history.Comment = workOrderDetail.StatusChangeComment;
                history.OldValue = wo.Status.StatusDescription;
                history.NewValue = status.StatusDescription;
                history.RowId = wo.Id;
                #endregion
                foreach (var item in files)
                {
                    string path = await _imageuploadinfile.UploadAsync(item);
                    if (path != null)
                    {
                        if (wo.WOAttachments == null)
                            wo.WOAttachments = new List<WOAttachments>();
                        wo.WOAttachments.Add(new WOAttachments
                        {
                            FileName = item.FileName,
                            FilePath = path
                        });
                    }
                }

                wo.StatusId = workOrderDetail.StatusId;
                if(status.StatusCode=="COMP" || status.StatusCode == "COBQ" || status.StatusCode == "CONI")
                    wo.IsActive = false;
                else
                    wo.IsActive = true;

                var updatestatus = await _workOrder.Update(wo);
                await _history.Add(history);
                if (updatestatus > 0)
                {
                    if (!string.IsNullOrWhiteSpace(workOrderDetail.FilesRemoved))
                    {
                        var remove = workOrderDetail.FilesRemoved.Contains(',') ? workOrderDetail.FilesRemoved.Split(",") : new String[] { workOrderDetail.FilesRemoved };
                        foreach (var item in remove)
                        {
                            var path = item.Contains("/api") ? "/api/" : "/";
                            var tempurl = item.Replace(_scheme + _httpContextAccessor.HttpContext.Request.Host.Value +path, "");
                            _imageuploadinfile.Delete(tempurl);
                            var woAttch = wo.WOAttachments.Where(x => x.FilePath.Equals(tempurl)).FirstOrDefault();
                            wo.WOAttachments.Remove(woAttch);
                        }
                    }

                    var users = await GetUsersToSendNotification(wo);
                    await _notifier.CreateNotification("Work Order Status Changed for WOId " + wo.Id, users, wo.Id, "WE");

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


        public async Task<List<AllWOExport>> WOExport(WOFilterDTO wOFilterModel)
        {
            List<AllWOExport> workOrders = null;
            var query = _workOrder.GetAll();
            var count = 0;
            query = await FilterWO(wOFilterModel, query);
            var skip = wOFilterModel.PageNumber > 0 && wOFilterModel.IsCurrent ? wOFilterModel.PageNumber * 20 : 0;
            if(wOFilterModel.IsActive.HasValue)
             count = query.Where(x => x.IsActive == wOFilterModel.IsActive).Count();
            else
                count = query.Count();
            workOrders = await query.Select(x => new AllWOExport
            {
                PropertyName = x.Property.PropertyName,
                Issue = x.Issue!=null? x.Issue.IssueName:x.CustomIssue,
                StatusDescription = x.Status.StatusDescription,
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
            }).Skip(skip).Take(wOFilterModel.IsCurrent ? 20 : count).AsNoTracking().ToListAsync();

            return workOrders;
        }


        private async Task<IQueryable<WorkOrder>> FilterWO(WOFilterDTO wOFilterModel, IQueryable<WorkOrder> query)
        {

            #region init
            var role = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Role).Value;
            var username = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var userdept = await _appuser.FindByNameAsync(username);
            var propIds = await _userProperty.GetAll().Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == userId).AsNoTracking().Select(x => x.PropertyId).Distinct().ToListAsync();
            #endregion

            #region filter based on filtermodel
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
                query = query.Where(x => x.Status.StatusCode.Equals(wOFilterModel.Status));
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.WOId))
            {
                query = query.Where(x => x.Id.Contains(wOFilterModel.WOId));
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.AssignedTo))
            {
                query = query.Where(x => (x.AssignedTo != null && x.AssignedTo.FirstName.StartsWith(wOFilterModel.AssignedTo)) ||
                (x.AssignedToDept != null && x.AssignedToDept.DepartmentName.StartsWith(wOFilterModel.AssignedTo) || (x.AssignedTo != null && x.AssignedTo.Email.StartsWith(wOFilterModel.AssignedTo)))
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
                query = query.Where(x => x.Property.PropertyName.Contains(wOFilterModel.PropertyName));
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.Vendor))
            {
                query = query.Where(x => x.Vendor != null && x.Vendor.VendorName.Contains(wOFilterModel.Vendor));
            }
            #endregion
            //joins we need
            query = query.Include(x => x.AssignedTo).Include(x => x.AssignedToDept).Include(x => x.Status).Include(x => x.Property).Include(x => x.Vendor);

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                query = query.Where(x => propIds.Contains(x.Property.Id));
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                query = query.Where(x => (x.AssignedTo != null && x.AssignedTo.UserName.Equals(username)) || (x.AssignedToDept != null && x.AssignedToDeptId == userdept.DepartmentId) || (x.AssignedTo == null && x.AssignedToDept == null && propIds.Contains(x.Property.Id)) || x.CreatedByUserName.Equals(username));
            }
            if (wOFilterModel.IsActive.HasValue)
            {
                query=query.Where(x => x.IsActive==wOFilterModel.IsActive);
            }
            if (!String.IsNullOrEmpty(wOFilterModel.TermSearch))
            {
                query=query.Include(x => x.Issue).ThenInclude(x => x.Item).Where(x => x.Item.ItemName.Contains(wOFilterModel.TermSearch) || x.Issue.IssueName.Contains(wOFilterModel.TermSearch) || x.Property.PropertyName.Contains(wOFilterModel.TermSearch) || x.Id.Contains(wOFilterModel.TermSearch) || x.Description.Contains(wOFilterModel.TermSearch) || x.Status.StatusDescription.Contains(wOFilterModel.TermSearch) || x.Location.LocationName.Contains(wOFilterModel.TermSearch)|| (x.AssignedTo != null && x.AssignedTo.FirstName.Contains(wOFilterModel.TermSearch)|| x.AssignedTo.LastName.Contains(wOFilterModel.TermSearch)) ||
                (x.AssignedToDept != null && x.AssignedToDept.DepartmentName.Contains(wOFilterModel.TermSearch)));
            }
            if (wOFilterModel.SortedByDate)
            {
                query = query.OrderByDescending(x => x.CreatedTime);
            }
            else
                query = query.OrderBy(x => x.Priority);
            return query;
        }
        private async Task<IQueryable<RecurringWO>> FilterWO(WOFilterDTO wOFilterModel, IQueryable<RecurringWO> query)
        {

            #region init
            var role = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Role).Value;
            var username = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var userdept = await _appuser.FindByNameAsync(username);
            var propIds = await _userProperty.GetAll().Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == userId).AsNoTracking().Select(x => x.PropertyId).Distinct().ToListAsync();
            #endregion
            
            #region filter based on filtermodel
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
                query = query.Where(x => x.Status.StatusCode.Equals(wOFilterModel.Status));
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.WOId))
            {
                query = query.Where(x => x.Id.Contains(wOFilterModel.WOId));
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.AssignedTo))
            {
                query = query.Where(x => (x.AssignedTo != null && x.AssignedTo.FirstName.StartsWith(wOFilterModel.AssignedTo)) ||
                (x.AssignedToDept != null && x.AssignedToDept.DepartmentName.StartsWith(wOFilterModel.AssignedTo) || (x.AssignedTo != null && x.AssignedTo.Email.StartsWith(wOFilterModel.AssignedTo)))
                );
            }

            if (!string.IsNullOrWhiteSpace(wOFilterModel.Priority))
            {
                var index = Convert.ToInt32(wOFilterModel.Priority);
                query = query.Where(x => x.Priority == index);
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.PropertyName))
            {
                query = query.Where(x => x.Property.PropertyName.Contains(wOFilterModel.PropertyName));
            }
            if (!string.IsNullOrWhiteSpace(wOFilterModel.Vendor))
            {
                query = query.Where(x => x.Vendor != null && x.Vendor.VendorName.Contains(wOFilterModel.Vendor));
            }
            #endregion
            //joins we need
            query = query.Include(x => x.AssignedTo).Include(x => x.AssignedToDept).Include(x => x.Status).Include(x => x.Property).Include(x => x.Vendor);

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                query = query.Where(x => propIds.Contains(x.Property.Id));
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                query = query.Where(x => (x.AssignedTo != null && x.AssignedTo.UserName.Equals(username)) || (x.AssignedToDept != null && x.AssignedToDeptId == userdept.DepartmentId) || (x.AssignedTo == null && x.AssignedToDept == null && propIds.Contains(x.Property.Id)) || x.CreatedByUserName.Equals(username));
            }
            if (!String.IsNullOrEmpty(wOFilterModel.TermSearch))
            {
                query = query.Include(x => x.Issue).ThenInclude(x => x.Item).Where(x => x.Item.ItemName.Contains(wOFilterModel.TermSearch) || x.Issue.IssueName.Contains(wOFilterModel.TermSearch) || x.Property.PropertyName.Contains(wOFilterModel.TermSearch) || x.Id.Contains(wOFilterModel.TermSearch) || x.Description.Contains(wOFilterModel.TermSearch) || x.Status.StatusDescription.Contains(wOFilterModel.TermSearch) || x.Location.LocationName.Contains(wOFilterModel.TermSearch) || (x.AssignedTo != null && x.AssignedTo.FirstName.Contains(wOFilterModel.TermSearch) || x.AssignedTo.LastName.Contains(wOFilterModel.TermSearch)) ||
                  (x.AssignedToDept != null && x.AssignedToDept.DepartmentName.Contains(wOFilterModel.TermSearch)));
            }
            if (wOFilterModel.SortedByDate)
            {
               query= query.OrderByDescending(x => x.CreatedTime);
            }
            else
                query = query.OrderBy(x => x.Priority);
            return query;
        }

        public async Task<List<long>> GetUsersToSendNotification(WorkOrder woId)
        {
            var appuser = await _appuser.FindByNameAsync(woId.CreatedByUserName);

            HashSet<long> users = new HashSet<long>();
            users.Add(appuser.Id);
            if (woId.AssignedToId.HasValue)
                users.Add(woId.AssignedToId.GetValueOrDefault());
           
            else if (woId.AssignedToDeptId.HasValue)
            {
                var tempuserId = await _appuser.Users.Where(x => x.DepartmentId == woId.AssignedToDeptId).Select(x => x.Id).ToListAsync();
                if (tempuserId != null)
                    foreach (var item in tempuserId)
                        users.Add(item);
            }
            return users.ToList();
        }

        public async Task<List<HistoryDetail>> GetHistory(string entity, string rowId)
        {
            var data = await _history.Get(x => x.Entity.ToLower() == entity.ToLower() && x.RowId.Equals(rowId)).Select(x => new HistoryDetail
            {
                Comment = x.Comment,
                NewValue = x.NewValue,
                OldValue = x.OldValue,
                PropertyName = x.PropertyName,
                UpdatedBy = x.CreatedByUserName,
                UpdateTime = x.CreatedTime.ToString("dd-MMM-yyyy")
            }).AsNoTracking().ToListAsync();
            if (data != null)
                return data;
            return null;
        }

        public async Task<bool> CreateRecurringWO(CreateRecurringWODTO createWO, List<IFormFile> file)
        {
            RecurringWO workOrder = new RecurringWO
            {
                RequestedBy = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value,
                PropertyId = createWO.PropertyId,
                IssueId = createWO.IssueId !=-1 ? createWO.IssueId : (int?)null,
                ItemId = createWO.ItemId != 0 ? createWO.ItemId : (int?)null,
                Description = createWO.Description,
                DueAfterDays = createWO.DueAfterDays,
                LocationId = createWO.LocationId,
                VendorId = createWO.VendorId,
                SubLocationId = createWO.SubLocationId,
                Priority = createWO.Priority
            };
            if (createWO.Category.Equals("user"))
                workOrder.AssignedToId = createWO.OptionId;
            else if (createWO.Category.Equals("department"))
                workOrder.AssignedToDeptId = createWO.OptionId;

            workOrder.StatusId = _status.Get(x => x.StatusCode == "NEWO").AsNoTracking().Select(x => x.Id).FirstOrDefault();
            if (file != null)
            {
                foreach (var item in file)
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


            workOrder.EndAfterCount = createWO.EndAfterCount;
            workOrder.RecurringEndDate = createWO.RecurringEndDate;
            workOrder.CronExpression = createWO.CronExpression;
            var status = await _recuringWo.Add(workOrder);
            if (status > 0)
            {
                if (createWO.RecurringStartDate.HasValue)
                {
                    _recurringWorkOrderJob.RunAddScheduleJob(workOrder.CronExpression, workOrder.Id, createWO.RecurringStartDate.Value, timeZone);
                }
                else
                {
                    _recurringWorkOrderJob.RunCreateWoJob(workOrder.CronExpression, workOrder.Id, timeZone);
                }

                if (createWO.RecurringEndDate.HasValue)
                {
                    _recurringWorkOrderJob.RunRemoveScheduleJob(createWO.RecurringEndDate.Value, workOrder.Id);
                }

                return true;
            }

            return false;
        }

        public async Task<EditRecurringWorkOrderDTO> GetEditRecurringWO(string id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            var temp = await _recuringWo.Get(x => x.Id.Equals(id)).Include(x => x.WOAttachments).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Property).ThenInclude(x => x.Locations).Include(x => x.AssignedToDept).Include(x => x.AssignedTo).AsNoTracking().FirstOrDefaultAsync();

            var editwo = new EditRecurringWorkOrderDTO();
            editwo.Id = temp.Id;
            editwo.PropertyName = temp.Property.PropertyName;
            editwo.Locations = temp.Property.Locations.Select(x => new SelectItem { Id = x.Id, PropertyName = x.LocationName }).ToList();
            editwo.Description = temp.Description;
            editwo.IssueId = temp.IssueId.HasValue? temp.IssueId.Value:-1;
            editwo.CustomIssue = temp.CustomIssue;
            editwo.RecurringEndDate = temp.RecurringEndDate;
            editwo.RecurringStartDate = temp.RecurringStartDate;
            editwo.EndAfterCount = temp.EndAfterCount;
            editwo.ItemId = temp.ItemId;
            editwo.CreatedDate = temp.CreatedTime;
            editwo.VendorId = temp.VendorId;
            editwo.Priority = temp.Priority;
            editwo.CronExpression = !string.IsNullOrEmpty(temp.CronExpression) ? new ExpressionDescriptor(temp.CronExpression, new Options
            {
                DayOfWeekStartIndexZero = true,
                Use24HourTimeFormat = is24HrFormat
            }).GetDescription(DescriptionTypeEnum.FULL) : "";
            editwo.DueAfterDays = temp.DueAfterDays;
            editwo.LocationId = temp.LocationId.GetValueOrDefault();
            editwo.SubLocationId = temp.SubLocationId.GetValueOrDefault();
            editwo.FileAvailable = temp.WOAttachments.Select(x => new KeyValuePair<string, string>(x.FileName,
             string.Concat(_scheme, _httpContextAccessor.HttpContext.Request.Host.Value, "/api/", x.FilePath))).ToList();

            editwo.Items = await _itemRepo.GetAll().Where(x => x.LocationId == editwo.LocationId).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.ItemName
            }).AsNoTracking().ToListAsync();
            editwo.Vendors = await _vendors.GetAll().Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.VendorName
            }).AsNoTracking().ToListAsync();
            editwo.Issues = await _issueRepo.GetAll().Where(x=>x.ItemId==editwo.ItemId).Select(x => new SelectItem
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

        public async Task<bool> EditRecurringWO(EditRecurringWorkOrderDTO editWorkOrder, List<IFormFile> File)
        {
            History history = new History();
            var userObj = await _appuser.FindByIdAsync(userId.ToString());
            var wo = await _recuringWo.Get(x => x.Id.Equals(editWorkOrder.Id)).Include(x => x.WOAttachments).Include(x => x.Comments).Include(x => x.AssignedToDept).Include(x => x.AssignedTo).FirstOrDefaultAsync();
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
                wo.IssueId = editWorkOrder.IssueId!=-1? editWorkOrder.IssueId:(int?)null;
                wo.ItemId = editWorkOrder.ItemId != 0 ? editWorkOrder.ItemId : (int?)null; 
                wo.CustomIssue = editWorkOrder.CustomIssue;
                wo.DueAfterDays = editWorkOrder.DueAfterDays;
                wo.LocationId = editWorkOrder.LocationId;
                wo.VendorId = editWorkOrder.VendorId;
                wo.Priority = editWorkOrder.Priority;
                wo.CronExpression = editWorkOrder.CronExpression;
                wo.EndAfterCount = editWorkOrder.EndAfterCount;
                wo.RecurringEndDate = editWorkOrder.RecurringEndDate;
                wo.RecurringStartDate = editWorkOrder.RecurringStartDate;
                wo.SubLocationId = editWorkOrder.SubLocationId;

                //adding history
                history.Entity = "WorkOrder";
                history.PropertyName = "Assigned To";
                history.RowId = wo.Id;
                history.OldValue = wo.AssignedTo != null ? string.Concat(wo.AssignedTo.FirstName, " ", wo.AssignedTo.LastName) : wo.AssignedToDept != null ? wo.AssignedToDept.DepartmentName : "NA";
                if (editWorkOrder.Category.Equals("user"))
                {

                    wo.AssignedToDeptId = null;
                    wo.AssignedToId = editWorkOrder.OptionId;
                    history.NewValue = _appuser.Users.Where(x => x.Id == editWorkOrder.OptionId).Select(x => string.Concat(x.FirstName, " ", x.LastName)).FirstOrDefault();
                }
                else if (editWorkOrder.Category.Equals("department"))
                {
                    wo.AssignedToId = null;
                    wo.AssignedToDeptId = editWorkOrder.OptionId;
                    history.NewValue = _department.Get(x => x.Id == editWorkOrder.OptionId).Select(x => x.DepartmentName).FirstOrDefault();
                }
                else
                {
                    wo.AssignedToDeptId = null;
                    wo.AssignedToId = null;
                    history.NewValue = "";
                }

            }
            if (!string.IsNullOrWhiteSpace(editWorkOrder.FilesRemoved))
            {
                var remove = editWorkOrder.FilesRemoved.Contains(',') ? editWorkOrder.FilesRemoved.Split(",") : new String[] { editWorkOrder.FilesRemoved };
                foreach (var item in remove)
                {
                    string path = item.Contains("/api") ? "/api" : "/";
                    var tempurl = item.Replace(_scheme + _httpContextAccessor.HttpContext.Request.Host.Value + path, "");
                    _imageuploadinfile.Delete(tempurl);
                    var woAttch = wo.WOAttachments.Where(x => x.FilePath.Equals(tempurl)).FirstOrDefault();
                    wo.WOAttachments.Remove(woAttch);
                }
            }



            var status = await _recuringWo.Update(wo);

            await _history.Add(history);
            if (status > 0)
            {

                //cancel all jobs
                _recurringWorkOrderJob.RunRemoveScheduleJob(wo.Id);
                //create new jobs
                if (wo.RecurringStartDate.HasValue)
                {
                    _recurringWorkOrderJob.RunAddScheduleJob(wo.CronExpression, wo.Id, wo.RecurringStartDate.Value, timeZone);
                }
                else
                    _recurringWorkOrderJob.RunCreateWoJob(wo.CronExpression, wo.Id, timeZone);

                if (wo.RecurringEndDate.HasValue)
                {
                    _recurringWorkOrderJob.RunRemoveScheduleJob(wo.RecurringEndDate.Value, wo.Id);
                }

                return true;
            }
            return false;
        }

        public async Task<Pagination<List<RecurringWOs>>> GetRecurringWO(WOFilterDTO wOFilterDTO)
        {
            int iteminpage = 20;
            var query = _recuringWo.GetAll();
            query = await FilterWO(wOFilterDTO, query);
            List<RecurringWOs> recWorkOrder = null;
            var count = query.Count();
          
             recWorkOrder = await query.Skip(wOFilterDTO.PageNumber * iteminpage).Take(iteminpage).Select(x => new RecurringWOs
            {
                DueAfterDays = "After " + x.DueAfterDays + " Days",
                Description = x.Description,
                Id = x.Id,
                Property = new SelectItem { Id = x.Property.Id, PropertyName = x.Property.PropertyName },
                ScheduleAt = !string.IsNullOrEmpty(x.CronExpression) ? new ExpressionDescriptor(x.CronExpression, new Options
                {
                    DayOfWeekStartIndexZero = true,
                    Use24HourTimeFormat = is24HrFormat
                }).GetDescription(DescriptionTypeEnum.FULL) : null,
                Status = x.Status.StatusDescription,
                AssignedTo = x.AssignedTo != null ? x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")" : x.AssignedToDept != null ? x.AssignedToDept.DepartmentName : "Anyone",
            }).AsNoTracking().ToListAsync();
            wOFilterDTO.TermSearch = wOFilterDTO.TermSearch == null ? "" : wOFilterDTO.TermSearch;

            Pagination<List<RecurringWOs>> pagination = new Pagination<List<RecurringWOs>>
            {
                Payload = recWorkOrder,
                CurrentPage = wOFilterDTO.PageNumber,
                ItemsPerPage = count > iteminpage ? iteminpage : count,
                PageCount = count <= iteminpage ? 1 : count % iteminpage == 0 ? count / iteminpage : count / iteminpage + 1
            };

            return pagination;
        }

        public async Task<Pagination<List<ChildWo>>> GetChildWO(int pageNumber, string search, string rwoId)
        {
            int iteminpage = 20;
            var query = _workOrder.GetAll().Where(x => x != null && x.ParentWoId.Equals(rwoId));
            List<ChildWo> recWorkOrder = null;
            var count = query.Count();
            recWorkOrder = await query.OrderByDescending(x => x.Priority).Skip(pageNumber * iteminpage).Take(iteminpage).Select(x => new ChildWo
            {
                DueDate = x.DueDate.ToString("dd-MMM-yy"),
                Description = x.Description,
                Id = x.Id,
                Status = x.Status.StatusDescription,
                AssignedTo = x.AssignedTo != null ? x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")" : x.AssignedToDept != null ? x.AssignedToDept.DepartmentName : "Anyone",
                Property = new SelectItem { Id = x.PropertyId, PropertyName = x.Property.PropertyName }
            }).AsNoTracking().ToListAsync();


            Pagination<List<ChildWo>> pagination = new Pagination<List<ChildWo>>
            {
                Payload = recWorkOrder,
                CurrentPage = pageNumber,
                ItemsPerPage = count > iteminpage ? iteminpage : count,
                PageCount = count <= iteminpage ? 1 : count % iteminpage == 0 ? count / iteminpage : count / iteminpage + 1
            };

            return pagination;
        }

        public async Task<WorkOrderDetail> GetRecurringWODetail(string rwoId)
        {
            var iteminpage = 12;
            var workorder = await _recuringWo.Get(x => x.Id.Equals(rwoId)).Include(x => x.Issue).Include(x => x.Item).Include(x => x.Status).Include(x => x.WOAttachments).Include(x => x.AssignedTo).ThenInclude(x => x.Department).Include(x => x.SubLocation).Include(x => x.Location).Include(x => x.Vendor).Select(x => new
                             WorkOrderDetail
            {
                PropertyName = x.Property.PropertyName,
                Issue = x.Issue!=null?x.Issue.IssueName:x.CustomIssue,
                StatusDescription = x.Status.StatusDescription,
                Item = x.Item.ItemName,
                CreatedTime = x.CreatedTime,
                Vendor = x.Vendor != null ? x.Vendor.VendorName : null,
                DueDate = "After " + x.DueAfterDays + " Days",
                UpdatedTime = x.UpdatedTime,
                CronExpression = !string.IsNullOrEmpty(x.CronExpression) ? new ExpressionDescriptor(x.CronExpression, new Options
                {
                    DayOfWeekStartIndexZero = true,
                    Use24HourTimeFormat = is24HrFormat
                }).GetDescription(DescriptionTypeEnum.FULL) : null,
                EndAfterCount = x.EndAfterCount,
                RecurringEndDate = x.RecurringEndDate,
                RecurringStartDate = x.RecurringStartDate,
                AssignedTo = x.AssignedTo != null ? x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")" : x.AssignedToDept != null ? x.AssignedToDept.DepartmentName : "Anyone",
                Requestedby = x.RequestedBy,
                Id = x.Id,
                Priority = x.Priority,
                Recurring = true,
                UpdatedBy = x.UpdatedByUserName,
                Description = x.Description,
                Location = x.Location.LocationName,
                SubLocation = x.SubLocation.AreaName,
                Attachment = x.WOAttachments.Select(x => new KeyValuePair<string, string>(
                 x.FileName,
                 string.Concat(_scheme, _httpContextAccessor.HttpContext.Request.Host.Value, "/api/", x.FilePath)
                 )).ToList()
            }).AsNoTracking().FirstOrDefaultAsync();
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

        public async Task<List<AllWOExportRecurring>> WOExportRecurring(WOFilterDTO wOFilterModel)
        {
            var skip = wOFilterModel.PageNumber > 0 && wOFilterModel.IsCurrent ? wOFilterModel.PageNumber * 20 : 0;
            List<AllWOExportRecurring> workOrders = null;
            var query = _recuringWo.GetAll();
            query = await FilterWO(wOFilterModel, query);

            var count = await query.CountAsync();
            workOrders = await query.OrderBy(x => x.Priority).Select(x => new AllWOExportRecurring
            {
                PropertyName = x.Property.PropertyName,
                Issue = x.Issue!=null?x.Issue.IssueName:x.CustomIssue,
                StatusDescription = x.Status.StatusDescription,
                Item = x.Item.ItemName,
                CreatedTime = x.CreatedTime,
                DueAfterDays = "After " + x.DueAfterDays + " Days",
                UpdatedTime = x.UpdatedTime,
                AssignedTo = x.AssignedTo != null ? x.AssignedTo.UserName + "(" + x.AssignedTo.FirstName + " " + x.AssignedTo.LastName + ")" : x.AssignedToDept != null ? x.AssignedToDept.DepartmentName : "Anyone",
                Requestedby = x.RequestedBy,
                Id = x.Id,
                ScheduledAt = !string.IsNullOrEmpty(x.CronExpression) ? new ExpressionDescriptor(x.CronExpression, new Options
                {
                    DayOfWeekStartIndexZero = true,
                    Use24HourTimeFormat = is24HrFormat
                }).GetDescription(DescriptionTypeEnum.FULL) : "",
                ChildWO = x.ChildWorkOrders.Select(x => x.Id).ToList<string>(),
                Priority = x.Priority,
                UpdatedBy = x.UpdatedByUserName,
                Description = x.Description,
                Location = x.Location.LocationName,
                SubLocation = x.SubLocation.AreaName,
                Attachment = x.WOAttachments.Select(x => x.FileName).ToList()
            }).Skip(skip).Take(wOFilterModel.IsCurrent ? 20 : count).AsNoTracking().ToListAsync();

            return workOrders;
        }

        public async Task<bool> AddEffort(EffortPagination effortDTOs, string id)
        {
            var temp = effortDTOs.EffortDTOs.OrderBy(x => x.DateTime).ToList();
            var startdate = temp.First().DateTime;
            var enddate = temp.Last().DateTime;
            var eff =await  _workOrder.Get(x=>x.Id==id).Include(x=>x.Efforts).FirstOrDefaultAsync();
            if (eff == null) eff.Efforts = new List<Effort>();
            //add effort
            if (effortDTOs != null)//post request fired we have  to add the effort
            {
                List<Effort> effort = new List<Effort>();
                foreach (var item in effortDTOs.EffortDTOs)
                {
                    if (!item.Iseditable && (item.Repair!=0||item.Service!=0))
                    {

                        if (eff != null)
                        {
                            var x = eff.Efforts.Where(x => x.Date == item.DateTime).FirstOrDefault();
                            if (x != null)
                            {
                                x.Repair = item.Repair;
                                x.Service = item.Service;
                            }
                            else
                                eff.Efforts.Add(new Effort
                                {
                                    Repair = item.Repair,
                                    Service = item.Service,
                                    WOId = id,
                                    Date = item.DateTime,
                                    UserId = userId
                                });
                        }
                        else {
                            
                            eff.Efforts.Add(new Effort
                            {
                                Repair = item.Repair,
                                Service = item.Service,
                                WOId = id,
                                Date = item.DateTime,
                                UserId = userId
                            });
                        }
                    }
                }
                
                 var status= await _workOrder.Update(eff);
                if (status >= 1)
                    return true;
                
            }
            return false;

           
        }

        public async Task<EffortPagination> GetEffort(string id,bool prev)
        {
            var wo = _workOrder.Get(x => x.Id == id).First();
            var date = DateTime.Now;
            //no of days in week =7;
            var currentday = date.DayOfWeek; //sunday=0;
            
            var startdate = date.AddDays(-1*(int)currentday);
            if (wo.CreatedTime > startdate)
                startdate = wo.CreatedTime;
            var enddate= date.AddDays(7-(int)currentday-1);
            if (prev)
            {
                var temp = startdate;
                startdate = startdate.AddDays(-7);
                if (wo.CreatedTime > startdate)
                    startdate = wo.CreatedTime;
                enddate = temp.AddDays(-1);
            }
            var obj= await _effort.GetAll().Where(x => x.Date.Date >= startdate.Date && x.Date.Date <= enddate.Date && x.WOId == id).ToListAsync();
            var EffortDto = new List<EffortDTO>() ;
            
            if (obj == null)
            {
               
                foreach (var datetime in EachDay(startdate, enddate))
                {
                    var effort = new EffortDTO();
                    effort.Repair = 0;
                    effort.Service = 0;
                    effort.DateTime = datetime;
                    effort.Iseditable = true;
                    EffortDto.Add(effort);
                }
               
            }
            else
            {
                foreach (var datetime in EachDay(startdate, enddate))
                {
                    var effort = new EffortDTO();
                    var x = obj.Find(x => x.Date == datetime);
                    effort.Repair =x!=null?x.Repair:0;
                    effort.Iseditable = true;
                    effort.DateTime = datetime;
                    effort.Service = x != null ? x.Service : 0;
                    EffortDto.Add(effort);
                }
            }

            return new EffortPagination
            { Lastday = enddate,
                FistDay = startdate,
                EffortDTOs = EffortDto

            };
        }

        public async  Task<List<SelectItem>> GetIssues(long id)
        {
          var res=  await _issueRepo.GetAll().Where(x => x.ItemId == id).Select(x => new SelectItem
            {
              Id=x.Id,
              PropertyName=x.IssueName
            }).AsNoTracking().ToListAsync();

            return res;
        }

        public async Task<List<SelectItem>> GetItem(long id)
        {
            var res = await _itemRepo.Get(x=>x.Active).Select(x => new SelectItem
            {
                Id=x.Id,
                PropertyName=x.ItemName
            }).ToListAsync();

            return res;
        }
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public async Task<List<string>> GetWoList(long id)
        {
            var rest = await _sublocation.Get(x => x.Id == id).Include(x => x.WorkOrders).Select(y => y.WorkOrders.Select(z => z.Id).ToList()).FirstOrDefaultAsync();
            return rest;
        }
    }
}