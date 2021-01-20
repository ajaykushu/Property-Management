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
        public string WoId { get; set; }
        public string Date { get; set; }
        public string Repair { get; set; }
        public string Service { get; set; }
        public string Vacation { get; set; }
        public string Holiday { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        public string Updated { get; set; }
    }
}
