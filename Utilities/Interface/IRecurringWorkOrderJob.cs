
using System;
using System.Threading.Tasks;

namespace Utilities.Interface
{
    public interface IRecurringWorkOrderJob
    {
        void RunCreateWoJob(string cron, string woId, TimeZoneInfo timeZone);
        void RunRemoveScheduleJob(DateTime dateTime, string id);
        void RunRemoveScheduleJob(string id);
        void RunAddScheduleJob(string cron, string id,DateTime datetime, TimeZoneInfo timeZone);
        
    }
}
