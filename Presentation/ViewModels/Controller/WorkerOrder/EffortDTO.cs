using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.ViewModels
{
    public class EffortDTO
    {
        public int BOTax { get; set; }
        public int Repair { get; set; }
        public DateTime DateTime { get; set; }
        public DayOfWeek Day
        {
            get; set;
        }
        public int TotalForDay
        {
            get; set;
        }
        public bool IsEditable
        {
            get; set;

        }

    }
}
