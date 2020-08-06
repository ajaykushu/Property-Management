using DataAccessLayer.Interfaces;
using DataAccessLayer.Repository;
using DataEntity;
using System.Threading.Tasks;
using Utilities.Interface;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using System;

namespace Utilities
{
    public class RecurringWorkOrderJob : IRecurringWorkOrderJob
    {
        private readonly IRepo<WorkOrder> _workOrder;
        private readonly IRepo<History> _history;
        private readonly IRepo<Status> _status;


        public RecurringWorkOrderJob(IRepo<WorkOrder> workOrder, IRepo<History> history, IRepo<Status> status)
        {
            _workOrder = workOrder;
            _history = history;
            _status = status;
        }
        public async Task CreateRecurringWO(string woId)
        {
            
            var obj =  _workOrder.Get(x => x.Id == woId).Include(x=>x.WOAttachments).FirstOrDefault();
            string status = await _history.Get(x => x.Entity == "WorkOrder" && x.RowId == woId && x.PropertyName=="Status").OrderBy(x => x.CreatedTime).Take(1).Select(x=>x.OldValue).AsNoTracking().FirstOrDefaultAsync();
            if (obj.EndAfterCount.HasValue && obj.EndAfterCount.HasValue && obj.EndAfterCount.Value == 0)
            {
                RecurringJob.RemoveIfExists(woId);
            }
            else if (obj.EndAfterCount.HasValue)
                  obj.EndAfterCount-=1;

            if (obj != null)
            {
                obj.Id = null;
                if (status != null)
                {
                    obj.StatusId= await _status.Get(x => x.StatusDescription.Equals(status)).Select(x => x.Id).FirstAsync();
                }
                var res=await _workOrder.Add(obj);
            }
        }

        

        public void RunAddScheduleJob(string cron, string id, DateTime date)
        {
            BackgroundJob.Schedule(() => RunCreateWoJob(cron,id), date);
            
        }
        

        public void RunCreateWoJob(string cron, string woId)
        {
            var wo = _workOrder.Get(x => x.Id.Equals(woId)).FirstOrDefault();
            if (wo != null)
                if (wo.RecurringStartDate.HasValue)
                    if (wo.RecurringStartDate.Value.Date != DateTime.Now.Date)
                        return;
            RecurringJob.AddOrUpdate(woId,() => CreateRecurringWO(woId),cron);
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
