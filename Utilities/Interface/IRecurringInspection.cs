using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Interface
{
    public interface  IRecurringInspection
    {
        void RunCreateInsJob(string cron, string inspId, TimeZoneInfo timeZone);
        //void RunRemoveScheduleJob(DateTime dateTime, string Id);
        void RunRemoveScheduleJob(string id);
      //  void RunAddScheduleJob(string cron, string id, DateTime datetime, TimeZoneInfo timeZone);
    }
}
