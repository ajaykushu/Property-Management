using DataAccessLayer.Interfaces;
using DataAccessLayer.Repository;
using DataEntity;
using System.Threading.Tasks;
using Utilities.Interface;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using System;
using System.Collections.Generic;

namespace Utilities
{
    public class RecurringWorkOrderJob : IRecurringWorkOrderJob
    {
        private readonly IRepo<WorkOrder> _workOrder;
        private readonly IRepo<History> _history;
        private readonly IRepo<Status> _status;
        private readonly IRepo<RecurringWO> _recurringWo;


        public RecurringWorkOrderJob(IRepo<RecurringWO> recurringWo,IRepo<WorkOrder> workOrder, IRepo<History> history, IRepo<Status> status)
        {
            _workOrder = workOrder;
            _history = history;
            _status = status;
            _recurringWo = recurringWo;
        }
        public async Task CreateRecurringWO(string woId)
        {
            
            var obj =  _recurringWo.Get(x => x.Id == woId).Include(x=>x.WOAttachments).AsNoTracking().FirstOrDefault();

            if (obj.EndAfterCount.HasValue && obj.EndAfterCount.HasValue && obj.EndAfterCount.Value == 0)
            {
                RecurringJob.RemoveIfExists(woId);
            }
            else if (obj.EndAfterCount.HasValue)
                  obj.EndAfterCount-=1;


            if (obj != null)
            {
                WorkOrder workOrder = new WorkOrder()
                {
                    ParentWoId = woId,
                    AssignedToId = obj.AssignedToId,
                    AssignedToDeptId = obj.AssignedToDeptId,
                    VendorId = obj.VendorId,
                    //WOAttachments = obj.WOAttachments,
                    UpdatedByUserName = "System",
                    DueDate = DateTime.Now.AddDays(obj.DueAfterDays),
                    CreatedTime = DateTime.Now,
                    Description = obj.Description,
                    IssueId = obj.IssueId,
                    CustomIssue=obj.CustomIssue,
                    ItemId = obj.ItemId,
                    LocationId = obj.LocationId,
                    SubLocationId = obj.SubLocationId,
                    PropertyId = obj.PropertyId,
                    Priority = obj.Priority,
                    CreatedByUserName = "System",
                    RequestedBy = obj.Id,
                    StatusId = obj.StatusId

                };
                if (obj.WOAttachments != null)
                {
                    workOrder.WOAttachments = new List<WOAttachments>();
                    foreach (var item in obj.WOAttachments)
                    {
                         item.Key = 0;
                        workOrder.WOAttachments.Add(item);
                    }
                }


         
                var res=await _workOrder.Add(workOrder);
            }
        }

        

        public void RunAddScheduleJob(string cron, string id, DateTime date,TimeZoneInfo timeZone)
        {
            BackgroundJob.Schedule(() => RunCreateWoJob(cron,id,timeZone), date);
            
        }
        

        public void RunCreateWoJob(string cron, string woId,TimeZoneInfo timeZone)
        {
            var wo = _recurringWo.Get(x => x.Id.Equals(woId)).FirstOrDefault();
            if (wo != null)
                if (wo.RecurringStartDate.HasValue)
                    if (wo.RecurringStartDate.Value.Date != DateTime.Now.Date)
                        return;
            RecurringJob.AddOrUpdate(woId,() => CreateRecurringWO(woId),cron,timeZone:timeZone);
        }

        public void RunRemoveScheduleJob(DateTime datetime, string id)
        {
            BackgroundJob.Schedule(() => RemoveRecurringWo(id),datetime);
        }

        public void RunRemoveScheduleJob(string id)
        {
            RecurringJob.RemoveIfExists(id);
            
        }

        public void RemoveRecurringWo(string woId)
        {
            RecurringJob.RemoveIfExists(woId);
        }
    }
}
