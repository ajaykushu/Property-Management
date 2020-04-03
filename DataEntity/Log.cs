using System;

namespace DataEntity
{
    public class Log
    {
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
    }
}