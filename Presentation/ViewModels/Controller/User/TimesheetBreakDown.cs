using Presentation.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class TimesheetBreakDown
    {

        [Key]
        [DisplayName("WO ID")]
        [ReadOnly(true)]
        public string WoId { get; set; }
        [ReadOnly(true)]
        public string Date { get; set; }
        public string Repair { get; set; }
        public string Service { get; set; }
        [ReadOnly(true)]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [ReadOnly(true)]
        public string Updated { get; set; }
    }
}
