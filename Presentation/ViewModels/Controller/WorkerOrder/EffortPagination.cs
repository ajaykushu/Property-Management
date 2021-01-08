using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Presentation.ViewModels
{
    public class EffortPagination
    {
        public List<EffortDTO> EffortDTOs { get; set; }
        
        public DateTime IncidentCreatedDate { get; set; }
        public DateTime LastEffortFilledDate { get; set; }
        public int TotalEffort { get; set; }


    }
}
