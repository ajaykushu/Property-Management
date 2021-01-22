
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ResponseModels.User
{
    public class TimesheetBreakDown
    {
       
        
        public string WoId { get; set; }
        public string Date { get; set; }
        public string Repair { get; set; }
        public string Service { get; set; }
        public string UserName { get; set; }
        public string Updated { get; set; }
    }
}
