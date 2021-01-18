using Presentation.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class TimesheetBreakDown
    {
        [Key]
        [SkipProperty]
        public string Id { get; set; }
        public string Repair { get; set; }
        public string Service { get; set; }
        public string Vacation { get; set; }
        public string Holiday { get; set; }
    }
}
