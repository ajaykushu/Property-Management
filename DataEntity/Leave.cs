using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntity
{
    public class Leave:Log
    {
        [Key]
        public string Id { get; set; }
        public char LeaveType { get; set; }//vocation holiday''
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Reason { get; set; }

        [ForeignKey("ApplicationUser")]
        public long UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
