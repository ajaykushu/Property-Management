using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WorkOrder.RequestModels
{
    public class EffortDTO
    {

        public DateTime DateTime { get; set; }
        public int Effort { get; set; }
        public bool Iseditable { get; set; }

    }
}
