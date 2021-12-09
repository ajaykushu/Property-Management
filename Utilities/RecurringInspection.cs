using DataAccessLayer.Interfaces;
using DataEntity;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Interface;

namespace Utilities
{
    public class RecurringInspection : IRecurringInspection
    {

        private readonly IRepo<InspectionQueue> _insp ;
        private readonly IRepo<CheckListQueue>  _cheklistqueue;
        private readonly IRepo<CheckList> _checklist;


        public RecurringInspection(  IRepo<InspectionQueue> insp,
        IRepo<CheckListQueue> cheklistqueue,
         IRepo<CheckList> checklist)
        {
            _insp = insp;
            _checklist = checklist;
            _cheklistqueue = cheklistqueue;
        }

       

       

        

        public void RunRemoveScheduleJob(string id)
        {
             RecurringJob.RemoveIfExists(id);
        }
        public void RunCreateInsJob(string cron, string insId, TimeZoneInfo timeZone)
        {
            RecurringJob.AddOrUpdate(insId, () => CreateRecurringInspection(insId), cron, timeZone: timeZone);
        }

        public void CreateRecurringInspection(string insId)
        {

            InspectionQueue inspectionQueue = new InspectionQueue() { 
            InspectionId=insId,
            Status=1,
          
            };
            inspectionQueue.checkListQueues = new List<CheckListQueue>();
            var items = _checklist.Get(x => x.InspectionId == insId).ToList();
            foreach(var item in items)
            {
                inspectionQueue.checkListQueues.Add(new CheckListQueue
                {
                    CheckListId=item.Id,
                    InspectionQueueId=insId,
                    Status=1
                });
            }
            _insp.Add(inspectionQueue);
        }
    }
}
