
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ResponseModels.User
{
    public class TimeSheet
    {
        
        [DisplayName("WOId")]
        public string WoId { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("Total Effort")]
        public string TotalHours { get; set; }
        [DisplayName("Updated")]
        public string Updated { get; set; }
    }
}
