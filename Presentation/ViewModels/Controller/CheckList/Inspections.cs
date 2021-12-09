using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class Inspections
    {
       
            public string Id { get; set; }
        public string InspectionId { get; set; }
        public long PropertyId { get; set; }
        public string Description { get; set; }
            public string Property { get; set; }
            public string Inspector { get; set; }
            [DisplayName("Start Date")]

            public DateTime StartDate { get; set; }
            public int Items { get; set; }
            
            public int IsComplete { get; set; }
        
    }
}
