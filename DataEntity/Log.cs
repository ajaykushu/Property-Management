using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Log
    {
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CreatedByUserName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string UpdatedByUserName { get; set; }
    }
}