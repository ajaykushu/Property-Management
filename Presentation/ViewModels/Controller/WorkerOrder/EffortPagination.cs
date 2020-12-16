using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Presentation.ViewModels
{
    public class EffortPagination
    {
        public List<EffortDTO> EffortDTOs { get; set; }
        public int TotalEffortOfWeek {
            get; set;
        }

        public int TotalBOTaxEffortOfWeek
        {
            get;set;
        }
        public int TotalRepairEffortOfWeek
        {
            get; set;
        }
        public DateTime? LastDay
        {
            get; set;
        }

        public DateTime? FirstDay { get; set; }


    }
}
