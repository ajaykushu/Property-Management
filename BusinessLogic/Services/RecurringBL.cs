using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Interface;

namespace BusinessLogic.Services
{
    public class RecurringBL : IRecurringBL
    {
        private readonly IRepo<History> _history;
        private readonly IRepo<RecurringWO> _recuringWo;
        private readonly IRepo<Status> _status;
        private readonly IRepo<Comment> _comments;
        private readonly IImageUploadInFile _imageuploadinfile;
        private readonly INotifier _notifier;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _appuser;
        private readonly IRepo<Item> _itemRepo;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<Property> _property;
        private readonly string _scheme;
        public RecurringBL(IRepo<History> history, IRepo<RecurringWO> recuringWo, IRepo<Comment> comments, IImageUploadInFile imageuploadinfile, INotifier notifier,
         IHttpContextAccessor httpContextAccessor, IRepo<Status> status,
         UserManager<ApplicationUser> appuser,
         IRepo<Item> itemRepo,
         IRepo<UserProperty> userProperty,
         IRepo<Property> property)
        {
            _history = history;
            _recuringWo = recuringWo;
            _httpContextAccessor = httpContextAccessor;
            _comments = comments;
            _status = status;
            _imageuploadinfile = imageuploadinfile;
            _notifier = notifier;
            _appuser = appuser;
            _userProperty = userProperty;
            _itemRepo = itemRepo;
            _property = property;
            _scheme= _httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://";
        }
        public async Task<bool> WorkOrderStatusChange(WorkOrderDetail workOrderDetail, IList<IFormFile> files)
        {
            var wo = await _recuringWo.Get(x => x.Id.Equals(workOrderDetail.Id)).Include(x => x.Status).FirstOrDefaultAsync();
            var status = await _status.Get(x => x.Id == workOrderDetail.StatusId).FirstOrDefaultAsync();
            History history = new History();
            if (status != null)
            {

                #region history Add
                
                history.PropertyName = "Status";
                history.Entity = "WorkOrder";
                history.Comment = workOrderDetail.StatusChangeComment;
                history.OldValue = wo.Status.StatusDescription;
                history.NewValue = status.StatusDescription;
                history.RowId = wo.Id;
                #endregion
                wo.StatusId = workOrderDetail.StatusId;
            }
            if (files != null && files.Count > 0) {
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

            }
               
                var updatestatus = await _recuringWo.Update(wo);
                
                if (updatestatus > 0)
                {
                await _history.Add(history);
                if (!string.IsNullOrWhiteSpace(workOrderDetail.FilesRemoved))
                    {
                        var remove = workOrderDetail.FilesRemoved.Contains(',') ? workOrderDetail.FilesRemoved.Split(",") : new String[] { workOrderDetail.FilesRemoved };
                   
                    foreach (var item in remove)
                        {
                        var path = item.Contains("/api") ? "/api/" : "/";
                        var tempurl = item.Replace(_scheme + _httpContextAccessor.HttpContext.Request.Host.Value + path, "");
                        _imageuploadinfile.Delete(tempurl);
                            var woAttch = wo.WOAttachments.Where(x => x.FilePath.Equals(tempurl)).FirstOrDefault();
                            wo.WOAttachments.Remove(woAttch);
                        }
                    }

                   // var users = await GetUsersToSendNotification(wo);
                    //await _notifier.CreateNotification("Recurring Work Order Status Changed for WOId " + wo.Id, users, wo.Id, "WE");

                    return true;
                }
            
            return false;
        }

        private async Task<List<long>> GetUsersToSendNotification(RecurringWO wo)
        {
            var users = _userProperty.GetAll().Where(x => x.PropertyId == wo.PropertyId).Select(x => x.ApplicationUserId).Distinct().ToHashSet();
            if (wo.AssignedToId.HasValue)
                users.Add(wo.AssignedToId.GetValueOrDefault());
            else if (wo.AssignedToDeptId.HasValue)
            {
                var tempuserId = await _appuser.Users.Where(x => x.DepartmentId == wo.AssignedToDeptId).Select(x => x.Id).ToListAsync();
                if (tempuserId != null)
                    foreach (var item in tempuserId)
                        users.Add(item);
            }
            return users.ToList();
        }
    }
}
